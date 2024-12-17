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
        tutorial = true;
        menu = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (menu) Time.timeScale = 0;
        else Time.timeScale = 1;

        //Debug.Log("Menu " + menu);
    }
    public bool IsTutorial() { return tutorial; }
    public void SetTutorial(bool t) { tutorial = t; }

    public bool IsMenu() { return menu; }
    public void SetMenu(bool t) { menu = t; }
}
