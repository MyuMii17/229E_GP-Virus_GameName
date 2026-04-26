using System.Collections.Generic;
using UnityEngine;

public class OnClash : MonoBehaviour
{
    [SerializeField]private EnemyController enemyController;
    [SerializeField]private PlayerController playerInRange;
    [SerializeField]private PlayerController playerHit;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PlayerController player))
        {
            playerInRange = player;
            var dir = transform.position -  player.transform.position;
            dir.Normalize();
            if(playerHit != player)
            {
                playerHit = player;
                player.isHasHit = true;
                player.rb.linearVelocity = Vector2.zero;
                player.OnHit(enemyController.enemyDamage, dir, enemyController.enemyPushForce);
            }

        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PlayerController player))
        {
            playerInRange = null;
            playerHit = null;
            if(player.isGameOverl == false)
            {
                StartCoroutine(player.HasHit());
            }
        }
    }

}
