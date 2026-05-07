using System.Collections.Generic;
using UnityEngine;

// บังคับให้ Game Object ต้องมี Component เหล่านี้
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(EdgeCollider2D))]
public class WaterInteractable : MonoBehaviour
{
    [Header("Spring Settings")]
    public float springConstant = 0.02f; // ความแข็งของสปริง
    public float damping = 0.04f;        // การหน่วง (เพื่อให้คลื่นค่อยๆ หยุด)
    public float spread = 0.05f;         // การกระจายแรงไปจุดข้างๆ
    [Range(0.1f, 10f)]
    public float speedMultiplier = 1f;   // ตัวคูณความเร็วจำลอง
    public int iterations = 8;           // จำนวนรอบการคำนวณการกระจายคลื่น

    [Header("Mesh Settings")]
    public int widthSegments = 20;       // จำนวนช่องผิวน้ำ (ยิ่งเยอะยิ่งละเอียด)
    public float width = 10f;            // ความกว้างรวม
    public float height = 5f;            // ความสูงรวม
    public Material waterMaterial;       // Material ที่ใช้ Shader Graph

    [Header("Collision Settings")]
    public float forceMultiplier = 0.1f;  // คูณแรงที่มากระแทก
    public float maxForce = 0.5f;         // จำกัดแรงสูงสุด
    public float radiusMultiplier = 0.5f; // รัศมีที่จะได้รับแรงกระแทก

    // ตัวแปรภายในสำหรับจัดการ Mesh
    private List<WaterPoint> waterPoints = new List<WaterPoint>();
    private Mesh mesh;
    private Vector3[] vertices;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private EdgeCollider2D edgeCollider;

    // คลาสภายในสำหรับเก็บข้อมูลสปริงแต่ละจุด
    private class WaterPoint
    {
        public float velocity;
        public float position;
        public float targetHeight;
    }

    private void Awake()
    {
        // อ้างอิง Component
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        GenerateMesh();
        CreateWaterPoints();
    }

    // ฟังก์ชันสร้าง Mesh ของน้ำ (Quads)
    public void GenerateMesh()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        int numVertices = (widthSegments + 1) * 2;
        vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[widthSegments * 6];

        // สร้าง Vertices และ UVs
        for (int x = 0; x <= widthSegments; x++)
        {
            float xPos = (float)x / widthSegments * width - (width / 2f);

            // 0 ถึง widthSegments = Vertices แถวล่าง
            vertices[x] = new Vector3(xPos, 0, 0);

            // widthSegments + 1 ถึง numVertices = Vertices แถวบน (ผิวน้ำ)
            vertices[x + widthSegments + 1] = new Vector3(xPos, height, 0);

            // กำหนด UV (0..1) สำหรับ Shader
            uvs[x] = new Vector2((float)x / widthSegments, 0);
            uvs[x + widthSegments + 1] = new Vector2((float)x / widthSegments, 1);
        }

        // เชื่อม Vertices เป็น Triangles (Quads)
        for (int i = 0; i < widthSegments; i++)
        {
            int t = i * 6;
            // Triangle 1
            triangles[t] = i;
            triangles[t + 1] = i + widthSegments + 1;
            triangles[t + 2] = i + 1;

            // Triangle 2
            triangles[t + 3] = i + 1;
            triangles[t + 4] = i + widthSegments + 1;
            triangles[t + 5] = i + widthSegments + 2;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // คำนวณแสง
        mesh.RecalculateBounds();  // คำนวณขอบเขตวัตถุ

        meshFilter.mesh = mesh;
        meshRenderer.material = waterMaterial;
    }

    // สร้างข้อมูลจุดสปริงตามความละเอียด Mesh
    private void CreateWaterPoints()
    {
        waterPoints.Clear();
        for (int i = 0; i <= widthSegments; i++)
        {
            waterPoints.Add(new WaterPoint
            {
                position = height,
                targetHeight = height,
                velocity = 0
            });
        }
    }

    private void FixedUpdate()
    {
        // 1. คำนวณฟิสิกส์สปริงสำหรับแต่ละจุด (ยกเว้นจุดขอบสุดสองข้าง)
        for (int i = 1; i < waterPoints.Count - 1; i++)
        {
            // F = -kx (Force = -SpringConstant * Displacement)
            float x = waterPoints[i].position - waterPoints[i].targetHeight;
            float acceleration = -springConstant * x - damping * waterPoints[i].velocity;

            // อัปเดต Velocity และตำแหน่งตำแหน่ง
            waterPoints[i].position += waterPoints[i].velocity * speedMultiplier;
            waterPoints[i].velocity += acceleration * speedMultiplier;

            // อัปเดต y ของ Vertices แถวบนใน Mesh
            vertices[i + widthSegments + 1].y = waterPoints[i].position;
        }

        // 2. คำนวณการกระจายคลื่น (Spread) ไปยังจุดข้างเคียง
        for (int j = 0; j < iterations; j++)
        {
            for (int i = 1; i < waterPoints.Count - 1; i++)
            {
                // ส่งแรงไปซ้าย
                float leftDelta = spread * (waterPoints[i].position - waterPoints[i - 1].position);
                waterPoints[i - 1].velocity += leftDelta;

                // ส่งแรงไปขวา
                float rightDelta = spread * (waterPoints[i].position - waterPoints[i + 1].position);
                waterPoints[i + 1].velocity += rightDelta;
            }
        }

        // แอพพลายตำแหน่ง Vertices ใหม่ลง Mesh
        mesh.vertices = vertices;
    }

    // ฟังก์ชันสั่ง Splash (ถูกเรียกจาก WaterTriggerHandler)
    public void Splash(float xPosition, float force)
    {
        // แปลงตำแหน่ง World X เป็นตำแหน่ง Local X ภายในน้ำ
        float relativeX = xPosition - transform.position.x + (width / 2f);

        // หาจุด Vertices ที่อยู่ในรัศมีและแอพพลายแรง Velocity
        for (int i = 0; i < waterPoints.Count; i++)
        {
            float vertexX = vertices[i + widthSegments + 1].x + (width / 2f);
            if (Mathf.Abs(vertexX - relativeX) < radiusMultiplier)
            {
                // จำกัดแรงและใส่ลงใน Velocity ของสปริง
                waterPoints[i].velocity = Mathf.Clamp(force, -maxForce, maxForce);
            }
        }
    }
}