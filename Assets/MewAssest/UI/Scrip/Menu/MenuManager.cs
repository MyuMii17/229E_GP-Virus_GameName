using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuUI;
    public bool isOpen;
    private GameStateManger gameStateManger;
    private PlayerController playerController;
    private PlayerUiController playerUiController;
    private static MenuManager StaticInstance = null;
    public static MenuManager GetStatic()
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
        playerController = PlayerController.GetStatic();
        gameStateManger = GameStateManger.GetStatic();
        playerUiController = PlayerUiController.GetStatic();
        MenuUI.SetActive(false);
    }

    public void Resume()
    {
        playerUiController.isMenuOpen = false;
        gameStateManger.ResumeForGame(MenuUI);
    }
    public void MainMenu()
    {
        gameStateManger.MainMenu();
    }
}
