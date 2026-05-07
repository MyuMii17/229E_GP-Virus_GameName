using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject enemyPrefeb;
    private EnemyController enemySet;
    private int enemyCount;
    public EnemyController enemyrset;
    public bool isHasEnemy;
    void Awake()
    {
        enemyCount = 0;
        spawnPoint = transform.GetChild(0).transform;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyController enemy))
        {
            enemyrset = enemy;
        }
    }
    void Update()
    {
        if(enemyrset == null && enemyCount <= 0)
        {
            enemyCount++;
            StartCoroutine(spawnEnemy());
        }
    }
    IEnumerator spawnEnemy()
    {
        Instantiate(enemyPrefeb,spawnPoint.position,Quaternion.identity);
        yield return new WaitForSeconds(1);
    }
}
