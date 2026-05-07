using UnityEngine;

[CreateAssetMenu(fileName = "BaseData", menuName = "Scriptable Objects/BaseData")]
public class BaseData : ScriptableObject
{
    [Header("Main Settings")]
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject prefeb;
    [SerializeField] private string objectName;
    [SerializeField, TextArea(3, 10)] private string description;
    [SerializeField] private float mass;
    [SerializeField] private float linearDamp;
    [SerializeField] private float angularDamp;
    [SerializeField] private float gravityScale;
    [SerializeField] private bool isCanDestroy;
    public Sprite Icon => icon;
    public GameObject Prefeb => prefeb;
    public string ObjectName => objectName;
    public string Description => description;
    public float Mass => mass;
    public float LinearDamp => linearDamp;
    public float AngularDamp => angularDamp;
    public float GravityScale => gravityScale;
    public bool IsCanDestroy => isCanDestroy;
}
