using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public TMP_Text damageCost;
    public TMP_Text speedCost;
    public Button damageUpgradButtton;
    public Button SpeedUpgradButtton;
    public Button exitButtton;
    public GameObject upgradeUI;
    private int costUp = 10;
    private int currentDamageCost = 10;
    private int currentSpeedCost = 10;
    public bool isOpen;
    private PlayerController playerController;
    private static UpgradeManager StaticInstance = null;
    public static UpgradeManager GetStatic()
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
    void Update()
    {
        if (isOpen)
        {
            playerController.enabled = false;
        }
    }
    void Start()
    {
        playerController = PlayerController.GetStatic();
        upgradeUI.SetActive(false);
        
        damageCost.text = $"${currentDamageCost}";
        speedCost.text = $"${currentSpeedCost}";
    }
    public void onDamageUp()
    {
        playerController.playerMoney -= currentDamageCost;
        playerController.playerDamage++;

        currentDamageCost += costUp;
        damageCost.text = $"${currentDamageCost}";
    }

    public void onSpeedUp()
    {
        playerController.playerMoney -= currentSpeedCost;
        playerController.playerMoveSpeed++;

        currentSpeedCost += costUp;
        speedCost.text = $"${currentSpeedCost}";
    }

    public void onExit()
    {
        isOpen = false;
        Time.timeScale = 1;
        playerController.enabled = true;
        upgradeUI.SetActive(false);
    }


}
