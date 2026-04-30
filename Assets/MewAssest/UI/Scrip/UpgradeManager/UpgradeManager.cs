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
    public TMP_Text moneyAmount;
    private int costUp = 2;
    private int currentDamageCost = 30;
    private int currentSpeedCost = 15;
    public bool isOpen;
    private PlayerController playerController;
    private PlayerUiController playerUiController;
    private GameStateManger gameStateManger;
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
        moneyAmount.text = $"${playerController.playerMoney}";
    }
    void Start()
    {
        gameStateManger = GameStateManger.GetStatic();
        playerController = PlayerController.GetStatic();
        playerUiController = PlayerUiController.GetStatic();

        upgradeUI.SetActive(false);
        
        damageCost.text = $"${currentDamageCost}";
        speedCost.text = $"${currentSpeedCost}";
    }
    public void onDamageUp()
    {
        if(playerController.playerMoney >= currentDamageCost)
        {
            playerController.playerMoney -= currentDamageCost;
            playerController.playerDamage++;

            currentDamageCost *= costUp;
            damageCost.text = $"${currentDamageCost}";
        }
    }

    public void onSpeedUp()
    {
        if(playerController.playerMoney >= currentSpeedCost)
        {
            playerController.playerMoney -= currentSpeedCost;
            playerController.playerMoveSpeed++;

            currentSpeedCost *= costUp;
            speedCost.text = $"${currentSpeedCost}";
        }
    }

    public void onExit()
    {
        playerUiController.isUpgradeOpen = false;
        gameStateManger.ResumeForGame(upgradeUI);
    }


}
