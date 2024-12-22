using UnityEngine;

public class PlayButton : MonoBehaviour
{
    GameManager gm;
    [SerializeField] UIManager ui;

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
            Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor al inicio
            Cursor.visible = false;                  // Ocultar el cursor al inicio
            ui.ShowTargetObject();
        }
        else
        {
            gm.LoadLevel("Cabaña");
        }
    }
}
