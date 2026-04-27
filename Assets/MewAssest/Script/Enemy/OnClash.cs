using System.Collections.Generic;
using UnityEngine;

public class OnClash : MonoBehaviour
{
    [SerializeField]private EnemyController enemyController;
    [SerializeField]private PlayerController playerInRange;
    [SerializeField]private PlayerController playerHit;
    private PlayerController playerController;
    void Start()
    {
        playerController = PlayerController.GetStatic();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PlayerController player) && player.isImmune != true)
        {
            playerInRange = player;
            var dir = transform.position -  player.transform.position;
            dir.Normalize();
            if(playerHit != player)
            {
                enemyController.isClash = true;
                playerHit = player;
                player.isHasHit = true;
                player.rb.linearVelocity = Vector2.zero;
                enemyController.ClashToPlayer(playerController.playerPushForce);
                player.OnHit(enemyController.enemyDamage, dir, enemyController.enemyPushForce);
            }

        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PlayerController player) && player.isImmune != true)
        {
            playerInRange = null;
            playerHit = null;
            if(player.isGameOverl == false && Time.timeScale != 0)
            {
                StartCoroutine(player.HasHit());
            }
        }
    }

}
