using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager GetInstance() { return instance; }

    [SerializeField] bool tutorial = true;
    [SerializeField] bool menu = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (!PlayerPrefs.HasKey("IsTutorial")) // Si no existe, establece un valor predeterminado
        { 
            PlayerPrefs.SetInt("IsTutorial", 1); // 1 para true, 0 para false
            PlayerPrefs.Save(); // Guarda los cambios
        }

        // Obtener el valor de "IsTutorial"
        int i = PlayerPrefs.GetInt("IsTutorial");
        if (i == 1) tutorial = true;
        else tutorial = false;
        menu = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (menu) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
    public bool IsTutorial() { return tutorial; }
    public void SetTutorial(bool t) { tutorial = t; }

    public bool IsMenu() { return menu; }
    public void SetMenu(bool t) 
    { 
        menu = t; 
        //PlayerPrefs.SetInt("IsMenu", menu ? 1 : 0);
        //PlayerPrefs.Save();
    }

    public void LoadLevel(string sceneName)
    {
        if (sceneName == "Garcia") SetMenu(true);
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
