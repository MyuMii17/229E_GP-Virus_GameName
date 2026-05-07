using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackAreaList : MonoBehaviour
{
    public HashSet<EnemyController> enemiesInRange = new HashSet<EnemyController>();
    private static AttackAreaList StaticInstance = null;
    public static AttackAreaList GetStatic()
    {
        return StaticInstance;
    }
    void Awake()
    {
        StaticInstance = this;
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out EnemyController enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out EnemyController enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }
}
