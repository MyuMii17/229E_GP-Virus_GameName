using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    SerializedProperty enemyType;
    SerializedProperty enemyHPs;
    SerializedProperty enemyHPm;
    SerializedProperty enemyHPb;

    void OnEnable()
    {
        enemyType = serializedObject.FindProperty("enemyType");
        enemyHPs = serializedObject.FindProperty("enemyHPs");
        enemyHPm = serializedObject.FindProperty("enemyHPm");
        enemyHPb = serializedObject.FindProperty("enemyHPb");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DrawPropertiesExcluding(serializedObject, 
            "enemyType", "enemyHPs", "enemyHPm", "enemyHPb");
        
        EditorGUILayout.PropertyField(enemyType);
        var type = (EnemyType)enemyType.enumValueIndex;

        switch (type)
        {
            case EnemyType.Small:
                EditorGUILayout.PropertyField(enemyHPs);
                break;
            case EnemyType.Medium:
                EditorGUILayout.PropertyField(enemyHPb);
                break;
            case EnemyType.Big:
                EditorGUILayout.PropertyField(enemyHPb);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }       
}
