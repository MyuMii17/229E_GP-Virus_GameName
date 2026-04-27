using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor
{
    SerializedProperty maxHP;
    SerializedProperty damage;
    SerializedProperty moveSpeed;
    SerializedProperty maxDashCount;
    SerializedProperty pushAcceleration;
    SerializedProperty dashAcceleration;
    SerializedProperty jumpAcceleration;
    SerializedProperty attackCooldownTime;
    SerializedProperty dashCooldownTime;
    SerializedProperty damageReduction;
    SerializedProperty healRequirement;

    void OnEnable()
    {
        maxHP = serializedObject.FindProperty("maxHP");
        damage = serializedObject.FindProperty("damage");
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        maxDashCount = serializedObject.FindProperty("maxDashCount");
        pushAcceleration = serializedObject.FindProperty("pushAcceleration");
        dashAcceleration = serializedObject.FindProperty("dashAcceleration");
        jumpAcceleration = serializedObject.FindProperty("jumpAcceleration");
        attackCooldownTime = serializedObject.FindProperty("attackCooldownTime");
        dashCooldownTime = serializedObject.FindProperty("dashCooldownTime");
        damageReduction = serializedObject.FindProperty("damageReduction");
        healRequirement = serializedObject.FindProperty("healRequirement");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        PlayerData data = (PlayerData)target;

        DrawPropertiesExcluding(serializedObject, 
            "maxHP", "damage", "moveSpeed", "maxDashCount", 
            "pushAcceleration", "dashAcceleration", "jumpAcceleration", 
            "attackCooldownTime", "dashCooldownTime", "damageReduction",
            "healRequirement");

        EditorGUILayout.PropertyField(maxHP);
        EditorGUILayout.PropertyField(damage);
        EditorGUILayout.PropertyField(moveSpeed);
        EditorGUILayout.PropertyField(maxDashCount);
        EditorGUILayout.PropertyField(pushAcceleration);
        EditorGUILayout.PropertyField(dashAcceleration);
        EditorGUILayout.PropertyField(jumpAcceleration);
        EditorGUILayout.PropertyField(attackCooldownTime);
        EditorGUILayout.PropertyField(dashCooldownTime);
        EditorGUILayout.PropertyField(damageReduction);
        EditorGUILayout.PropertyField(healRequirement);
        
        if(GUILayout.Button("Rename Assest"))
        {
            AssetDatabase.RenameAsset(
                AssetDatabase.GetAssetPath(data),
                data.ObjectName
            );

            AssetDatabase.SaveAssets();
        }

        serializedObject.ApplyModifiedProperties();
    }        
}
