using UnityEditor;
using UnityEditor.Actions;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManger : MonoBehaviour
{
    public int currentEnemyDead;
    public GameObject key;
    private static GameStateManger StaticInstance = null;
    public static GameStateManger GetStatic()
    {
        return StaticInstance;
    }
    public bool isPaused = false;
    public GameObject[] UIs;
    private PlayerController playerController;

    void Awake()
    {
        if(StaticInstance != null)
        {
            Destroy(this.gameObject);
        }
        StaticInstance = this;
        currentEnemyDead = 0;
    }

    void Start()
    {
        playerController = PlayerController.GetStatic();
    }    
    void Update()
    {
        if(currentEnemyDead >= 5)
        {
            OnGetEnemy();
        }
    }

    public void SetAllActiveUIoff()
    {
        foreach (var ui in UIs)
        {
            ui.SetActive(false);
        }
    }

    public void PauseForGame(GameObject targetUI)
    {
        if(isPaused == false)
        {
            isPaused = true;
            playerController.enabled = false;
            targetUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ResumeForGame(GameObject targetUI)
    {
        if(isPaused == true)
        {
            targetUI.SetActive(false);
            isPaused = false;
            playerController.enabled = true;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void MainMenu()
    {
        // SceneManager.LoadScene(0);
        EditorApplication.isPlaying = false;
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void OnGetEnemy()
    {
        key.SetActive(true);
    }
}
