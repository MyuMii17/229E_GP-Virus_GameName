using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    SerializedProperty enemyType;
    SerializedProperty enemyHP;
    SerializedProperty damage;
    SerializedProperty moveSpeed;
    SerializedProperty pushAcceleration;
    SerializedProperty attackCooldownTime;
    SerializedProperty arrowPrefeb;

    void OnEnable()
    {
        enemyType = serializedObject.FindProperty("enemyType");
        enemyHP= serializedObject.FindProperty("enemyHPs");
        damage = serializedObject.FindProperty("damage");
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        pushAcceleration = serializedObject.FindProperty("pushAcceleration");
        attackCooldownTime = serializedObject.FindProperty("attackCooldownTime");
        arrowPrefeb = serializedObject.FindProperty("arrowPrefeb");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EnemyData data = (EnemyData)target;
        
        DrawPropertiesExcluding(serializedObject, "arrowPrefeb");

        var type = (EnemyType)enemyType.enumValueIndex;

        switch (type)
        {
            case EnemyType.Swordman:
                break;
            case EnemyType.Archer:
                EditorGUILayout.PropertyField(arrowPrefeb);
                break;
        }

        if (GUILayout.Button("Rename Asset"))
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
