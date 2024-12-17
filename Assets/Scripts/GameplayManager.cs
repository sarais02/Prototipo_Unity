using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    GameManager gm;
    private static GameplayManager instance;
    public static GameplayManager GetInstance() {  return instance; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        gm = GameManager.GetInstance();
    }
    public void LoadLevel(string sceneName)
    {
        if (sceneName == "Garcia") gm.SetMenu(true);
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
