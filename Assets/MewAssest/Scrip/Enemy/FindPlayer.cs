using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    public HashSet<PlayerController> playerInRange = new HashSet<PlayerController>();

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            playerInRange.Add(player);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            playerInRange.Remove(player);
        }
    }
}
