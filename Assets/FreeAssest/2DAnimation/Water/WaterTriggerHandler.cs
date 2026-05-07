using UnityEngine;

public class WaterTriggerHandler : MonoBehaviour
{
    public LayerMask waterMask; // กำหนด Layer ของวัตถุที่จะทำให้เกิดคลื่น (เช่น Player)
    private WaterInteractable water;

    private void Awake()
    {
        // อ้างอิงสคริปต์หลัก InteractableWater ที่อยู่ใน Object เดียวกัน
        water = GetComponent<WaterInteractable>();
    }

    // เรียกเมื่อมีวัตถุ (ที่มี Collider และ Rigidbody) เข้ามาใน Box Collider (Is Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่า Layer ของวัตถุตรงกับที่ตั้งไว้หรือไม่
        if (((1 << collision.gameObject.layer) & waterMask) != 0)
        {
            // พยายามหา Rigidbody (ในตัววัตถุหรือตัวแม่)
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            if (rb != null)
            {
                // คำนวณแรง Splash จากความเร็วแกน Y คูณกับ Force Multiplier
                float force = rb.velocity.y * water.forceMultiplier;

                // เรียกฟังก์ชัน Splash ของสปริง
                water.Splash(collision.transform.position.x, force);
            }
        }
    }
}
