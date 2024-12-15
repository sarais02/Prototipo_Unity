using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    [SerializeField] PressButton doorButton;

    [SerializeField] GameplayManager gameplayManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //gameplayManager = GameplayManager.GetInstance();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() && doorButton.IsDoorOpen())
        {
            Debug.Log("CAMBIO ESCENA");
            gameplayManager.LoadLevel("EscenaNiveles");
        }
    }
}
