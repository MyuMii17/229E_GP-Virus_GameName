using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Player()
    {
        SceneManager.LoadScene(1);
    }
}
