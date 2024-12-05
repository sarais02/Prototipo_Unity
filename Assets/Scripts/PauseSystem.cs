using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject pauseMenu = null;
    bool isPaused;
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (isPaused == false)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                isPaused = true;
                
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                isPaused = false;
                
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
