using UnityEngine;

public class PlayButton : MonoBehaviour
{
    GameManager gm;
    [SerializeField] UIManager ui;
    [SerializeField] GameplayManager gameplayManager;
    private void Start()
    {
        gm = GameManager.GetInstance();
    }
    public void Play() //Si le doy al boton de Play
    {
        //Quito el menu de la ui
        gm.SetMenu(false);
       
        if (gm.IsTutorial()) //Si es el tutorial
        {
            //Mostrar texto de tutorial
            ui.ShowTargetObject();
        }
        else
        {
            gameplayManager.LoadLevel("EscenaNiveles");
        }
    }
}
