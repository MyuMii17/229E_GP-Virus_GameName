using UnityEngine;

public class WallCheck : MonoBehaviour
{
    private PlayerController playerController;
    void Start()
    {
        playerController = PlayerController.GetStatic();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.rb.gravityScale = playerController.playerGravityScale;;
            playerController.isCanMove = true;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.rb.gravityScale = playerController.playerGravityScale * 0.75f;;
            playerController.isCanMove = false;
        }
    }
}
