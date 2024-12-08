using UnityEngine;
using UnityEngine.Events;
public class PressButton : MonoBehaviour
{
    public GameObject pulsador;
    public GameObject jugador;
    public GameObject puerta;
    //public DoorOpener puerta; //A cada boton hay que asociarle una puerta en el Inspector


    public UnityEvent onPress;
    public UnityEvent onRelease;



    AudioSource efectosonido;

    bool isPressed = false;
    public bool opendoor = false;
    private Animator doorAnimator;

    Vector3 initialPosition = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        efectosonido = GetComponent<AudioSource>();
        isPressed = false; 
        initialPosition = pulsador.transform.localPosition;
        doorAnimator = puerta.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPressed == false && (other.gameObject == jugador)) { //Chequea si no esta presionado antes: ispressed!=true
            pulsador.transform.localPosition = initialPosition - new Vector3(0, 0.1f, 0);
            efectosonido.Play();
            Debug.Log("Cambio posicion");

            CheckPlayerSize(jugador, pulsador);
            isPressed = true;
            onPress.Invoke(); //Funcion Check Size
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == jugador)
        {
            pulsador.transform.localPosition = initialPosition;
            //onRelease.Invoke();
            //isPressed = false; //TODO cuando salte el personaje activar!
        } 
    }

    public void OpenDoor() {
        if (opendoor==true)
        {
            doorAnimator.SetTrigger("Door_Open");
            //doorAnimator.SetTrigger("Door_Open");
            Debug.Log("Puerta Abierta");
        }
        else
        {
            Debug.Log(opendoor);
            Debug.Log("Puerta Cerrada!");
        }
    }

    public void CheckPlayerSize(GameObject playerobj, GameObject pulsobj)
    {
        Debug.Log("Chequeando Tamaño...");
        float sizeplayer = playerobj.GetComponent<SphereCollider>().radius;
        float sizepulsador = pulsobj.GetComponent<BoxCollider>().size.x;
        Debug.Log(sizeplayer);
        Debug.Log(sizepulsador);
        if ((sizeplayer*2) == sizepulsador) {
            Debug.Log("Son del mismo tamaño!");
            opendoor = true;
        }else
        {
            Debug.Log("No son del mismo tamaño...");
            opendoor = false;
         }       
    }
}
