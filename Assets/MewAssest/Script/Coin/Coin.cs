using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coin = 2;
    private float pushForce;
    private float pushAccel = 6;
    private ConstantForce2D ct;
    private Rigidbody2D rb;
    public void Start()
    {
        ct = GetComponent<ConstantForce2D>();
        rb = GetComponent<Rigidbody2D>();
        pushForce = rb.mass * pushAccel;

        float forceX = Random.Range(-0.5f,0.5f);
        rb.AddForce(Vector2.up * pushForce, ForceMode2D.Impulse);
        ct.force = new Vector2(forceX, 0f);
        

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            player.playerMoney += coin;
            Destroy(this.gameObject);
        }
    }
}
