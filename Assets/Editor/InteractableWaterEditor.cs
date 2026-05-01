#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(WaterInteractable))]
public class InteractableWaterEditor : Editor
{
    private BoxBoundsHandle _boundsHandle = new BoxBoundsHandle();

    // ส่วนนี้จะทำให้มีปุ่มปรากฏในหน้า Inspector
    public override void OnInspectorGUI()
    {
        // วาด UI พื้นฐาน (ตัวแปรต่างๆ เช่น Spring Constant, Damping)
        base.OnInspectorGUI();

        WaterInteractable water = (WaterInteractable)target;

        GUILayout.Space(10); // เว้นวรรค

        // สร้างปุ่ม Generate Mesh
        if (GUILayout.Button("Generate Mesh"))
        {
            Undo.RecordObject(water, "Generate Water Mesh");
            water.GenerateMesh();
        }

        // ปุ่มแถม: สำหรับรีเซ็ตค่าสปริงในกรณีที่น้ำเด้งจนหลุดขอบ
        if (GUILayout.Button("Reset Water Points"))
        {
            // เราเข้าถึงฟังก์ชัน private ไม่ได้ถ้าไม่แก้เป็น public 
            // แต่ในที่นี้ถ้ากด Generate Mesh มันจะเริ่มใหม่ให้อยู่แล้วครับ
            water.GenerateMesh();
        }
    }

    // ส่วนที่ใช้ลากในหน้า Scene (เหมือนเดิม)
    protected virtual void OnSceneGUI()
    {
        WaterInteractable water = (WaterInteractable)target;

        _boundsHandle.center = water.transform.position + new Vector3(0, water.height / 2, 0);
        _boundsHandle.size = new Vector3(water.width, water.height, 0.1f);

        EditorGUI.BeginChangeCheck();
        _boundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(water, "Modify Water Dimensions");
            water.width = Mathf.Max(0.1f, _boundsHandle.size.x);
            water.height = Mathf.Max(0.1f, _boundsHandle.size.y);
            water.GenerateMesh();
        }
    }
}
#endif