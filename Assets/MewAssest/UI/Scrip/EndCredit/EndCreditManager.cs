using TMPro;
using UnityEngine;

public class EndCreditManager : MonoBehaviour
{
    public GameObject endCreditUI;
    private GameStateManger gameStateManger;
    public TMP_Text endCreditText;
    public bool isGameEnd;
    void Awake()
    {
        
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
    }
}
