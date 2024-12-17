using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    [SerializeField] PressButton doorButton;

    [SerializeField] GameplayManager gameplayManager;

    GameManager gm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //gameplayManager = GameplayManager.GetInstance();
        gm = GameManager.GetInstance();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() && doorButton.IsDoorOpen())
        {
            Debug.Log("CAMBIO ESCENA");
            gm.SetTutorial(false);
            
            gameplayManager.LoadLevel("EscenaNiveles");
            //gameplayManager.LoadLevel("Diego");
        }
    }
}
