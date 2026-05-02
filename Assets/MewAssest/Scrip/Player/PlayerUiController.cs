using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerUiController : MonoBehaviour
{
    public bool isMenuOpen;
    public bool isUpgradeOpen;
    private PlayerController playerController;
    private GameStateManger gameStateManger;
    private MenuManager menuManager;
    private UpgradeManager upgradeManager;
    private InputAction upgradeAction;
    private InputAction menuAction;
    private static PlayerUiController StaticInstance = null;
    public static PlayerUiController GetStatic()
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


        StaticInstance = this;
    }
    void Start()
    {
        upgradeAction = InputSystem.actions.FindAction("Upgrade");
        menuAction = InputSystem.actions.FindAction("Menu");
        
        upgradeManager = UpgradeManager.GetStatic();
        gameStateManger = GameStateManger.GetStatic();
        menuManager = MenuManager.GetStatic();
    }

    void Update()
    {
        if (upgradeAction.WasReleasedThisFrame())
        {
            isUpgradeOpen = !isUpgradeOpen;

            if(isMenuOpen == true)
            {
                isMenuOpen = false;
            }
            
            if(isUpgradeOpen == true)
            {
                gameStateManger.isPaused = false;
                gameStateManger.SetAllActiveUIoff();
                gameStateManger.PauseForGame(upgradeManager.upgradeUI);
            }
            else
            {
                gameStateManger.isPaused = true;
                gameStateManger.ResumeForGame(upgradeManager.upgradeUI);
            }
        }

        if(menuAction.WasReleasedThisFrame())
        {
            isMenuOpen = !isMenuOpen;

            if(isUpgradeOpen == true)
            {
                isUpgradeOpen = false;
            }

            if(isMenuOpen == true)
            {
                gameStateManger.isPaused = false;
                gameStateManger.SetAllActiveUIoff();
                gameStateManger.PauseForGame(menuManager.MenuUI);
            }
            else
            {
                gameStateManger.isPaused = true;
                gameStateManger.ResumeForGame(menuManager.MenuUI);
            }

        }
    }
}
