using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackArea : MonoBehaviour
{
    private static AttackArea StaticInstance = null;
    public static AttackArea GetStatic()
    {
        return StaticInstance;
    }
    void Awake()
    {
        StaticInstance = this;
    }
    public void OnAttack()
    {
        if (AttackAreaList.GetStatic().enemiesInRange != null)
        {
            foreach (var enemy in AttackAreaList.GetStatic().enemiesInRange)
            {
                enemy.OnHit(PlayerController.GetStatic().playerDamage);
            }
        }
    }
}
