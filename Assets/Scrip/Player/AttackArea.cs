using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackArea : MonoBehaviour
{
    private PlayerController player;
    private AttackAreaList attackAreaList;
    private static AttackArea StaticInstance = null;
    public static AttackArea GetStatic()
    {
        return StaticInstance;
    }
    void Awake()
    {
        StaticInstance = this;
    }
    void Start()
    {
        player = PlayerController.GetStatic();
        attackAreaList = AttackAreaList.GetStatic();
        gameObject.SetActive(false);
    }
    public void OnAttack()
    {
        foreach (var enemy in attackAreaList.enemiesInRange)
        {
            enemy.isHasHit = true;
        }
    }
}
