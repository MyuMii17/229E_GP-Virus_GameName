using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]private DataHolder dataHolder;
    private float enemyMass;
    public float enemyHP;
    void Start()
    {
        dataHolder = GetComponent<DataHolder>();
        if(dataHolder.baseData is EnemyData enemyData)
        {
            enemyHP = enemyData.EnemyHPs;
        }
    }

    public void OnHit(int damage)
    {
        enemyHP -= damage;
        if(enemyHP <= 0)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
