using UnityEngine;
using UnityEngine.UI;

public class HealBarManager : MonoBehaviour
{
    public Image healReqBar;
    private PlayerController playerController;
    private static HealBarManager StaticInstance = null;
    public static HealBarManager GetStatic()
    {
        return StaticInstance;
    }

    void Awake()
    {
        if(StaticInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(transform.parent.gameObject);

        StaticInstance = this;
    }

    void Start()
    {
        playerController = PlayerController.GetStatic();

    }

    public void SetHealReqBar(float current, float max)
    {
        healReqBar.fillAmount = current / max;
    }
}
