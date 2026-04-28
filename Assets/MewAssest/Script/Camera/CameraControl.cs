using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform playerTransform;
    private Camera cam;

     void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GetComponent<Camera>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);

        if(transform.position.y < 2.1f)
        {
            transform.position = new Vector3(playerTransform.position.x, 2.1f, -10f);
        }
    }
}
