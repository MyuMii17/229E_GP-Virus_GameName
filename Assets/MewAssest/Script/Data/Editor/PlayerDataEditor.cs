using UnityEditor;
    
[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor
{
    SerializedProperty maxHP;
    SerializedProperty damage;
    SerializedProperty moveSpeed;
    SerializedProperty maxDashCount;
    SerializedProperty dashAcceleration;
    SerializedProperty jumpAcceleration;
    SerializedProperty attackCooldownTime;
    SerializedProperty dashCooldownTime;

    void OnEnable()
    {
        maxHP = serializedObject.FindProperty("maxHP");
        damage = serializedObject.FindProperty("damage");
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        maxDashCount = serializedObject.FindProperty("maxDashCount");
        dashAcceleration = serializedObject.FindProperty("dashAcceleration");
        jumpAcceleration = serializedObject.FindProperty("jumpAcceleration");
        attackCooldownTime = serializedObject.FindProperty("attackCooldownTime");
        dashCooldownTime = serializedObject.FindProperty("dashCooldownTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DrawPropertiesExcluding(serializedObject, 
            "maxHP", "damage", "moveSpeed", "maxDashCount", "dashAcceleration", "jumpAcceleration", "attackCooldownTime", "dashCooldownTime");

        EditorGUILayout.PropertyField(maxHP);
        EditorGUILayout.PropertyField(damage);
        EditorGUILayout.PropertyField(moveSpeed);
        EditorGUILayout.PropertyField(maxDashCount);
        EditorGUILayout.PropertyField(dashAcceleration);
        EditorGUILayout.PropertyField(jumpAcceleration);
        EditorGUILayout.PropertyField(attackCooldownTime);
        EditorGUILayout.PropertyField(dashCooldownTime);

        serializedObject.ApplyModifiedProperties();
    }        
}
