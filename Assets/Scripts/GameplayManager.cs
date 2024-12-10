using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager instance;
    public static GameplayManager GetInstance() {  return instance; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
