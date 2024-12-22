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
        // Manejar el estado de pausa en base al estado del menú o la bandera isPaused
        if (gamemanager.IsMenu() || isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        if (Input.GetKeyDown(KeyCode.Escape) && !gamemanager.IsMenu())
        {
            if (!isPaused)
            {
                // Activar pausa
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                isPaused = true;

                // Hacer visible y desbloquear el cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public bool IsPaused() { return isPaused; }

    public void QuitPauseMenu()
    {
        // Desactivar pausa
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;

        // Ocultar y bloquear el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
