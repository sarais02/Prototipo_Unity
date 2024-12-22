using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu = null;
    public bool isPaused;
    GameManager gamemanager;
    void Start()
    {
        isPaused = false;
        gamemanager = GameManager.GetInstance();
    }


    void Update()
    {
        if (gamemanager.IsMenu() || isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == false)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                isPaused = true;
            }
        }
    }
    public void QuitPauseMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
}
