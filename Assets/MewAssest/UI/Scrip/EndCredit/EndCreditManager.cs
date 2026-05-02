using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCreditManager : MonoBehaviour
{
    public GameObject endCreditUI;
    private GameStateManger gameStateManger;
    public TMP_Text endCreditText;
    public bool isGameEnd;
    private static EndCreditManager StaticInstance = null;
    public static EndCreditManager GetStatic()
    {
        return StaticInstance;
    }
    void Awake()
    {
        if(StaticInstance != null)
        {
            Destroy(this.gameObject);
        }
        StaticInstance = this;
    }
    void Start()
    {
        gameStateManger = GameStateManger.GetStatic();
        endCreditUI.SetActive(false);
    }
    void Update()
    {
        if(isGameEnd == true) // ต้องเปลี่ยน
        {
            endCreditUI.SetActive(true);
            endCreditText.transform.position = new Vector2(endCreditText.transform.position.x, endCreditText.transform.position.y + 100 * Time.deltaTime);
        }
        if(endCreditText.transform.position.y >= 2000)
        {
            SceneManager.LoadScene(0);
        }
    }
}
