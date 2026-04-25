using UnityEngine;

public enum EnemyType{ Small, Medium, Big }

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/BaseData/EnemyData")]
public class EnemyData : BaseData
{
    [Header("EnemyData Setting")]
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int enemyHPs;
    [SerializeField] private int enemyHPm;
    [SerializeField] private int enemyHPb;
    public int EnemyHPs => enemyHPs;

}
