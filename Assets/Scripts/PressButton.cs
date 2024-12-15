using UnityEngine;
using UnityEngine.Events;
public class PressButton : MonoBehaviour
{
    public GameObject jugador;
    public GameObject puerta; //A cada boton hay que asociarle una puerta en el Inspector
    
    public UnityEvent onPress;
    public UnityEvent onRelease;

    AudioSource efectosonido;
    AudioSource sonidopuerta; 

    bool isPressed = false;
    bool opendoor = false;
    bool doorSoundPlayed = false;
    bool inTrigger = false;
    private Animator doorAnimator;
    Collider colliderobj;
    Vector3 initialPosition = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        efectosonido = GetComponent<AudioSource>();
        colliderobj = this.GetComponent<Collider>();
        initialPosition = transform.localPosition;
        doorAnimator = puerta.GetComponent<Animator>();
        sonidopuerta = puerta.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed && other.gameObject == jugador && !opendoor) { //Chequea si no esta presionado antes: ispressed!=true isPressed == false && (
            inTrigger = true;
            jugador.transform.position = new Vector3(
                transform.position.x, 
                transform.position.y + (transform.lossyScale.y/2 + jugador.GetComponent<PlayerController>().size.y/2), 
                transform.position.z);
            jugador.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | 
                RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            efectosonido.Play();

            CheckPlayerSize(jugador, this.gameObject);
            //onPress.Invoke(); //Funcion Check Size
            isPressed = true;            
        }               
    }

    private void OnTriggerStay(Collider other) {
        if (isPressed && other.gameObject == jugador && !opendoor) {
            CheckPlayerSize(jugador, this.gameObject); 
        } 
    }

    private void OnTriggerExit(Collider other) {
        if (inTrigger && other.gameObject==jugador)
        {
            inTrigger = false;
            transform.localPosition = initialPosition;
            jugador.GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezeRotationZ | 
                RigidbodyConstraints.FreezeRotationX; ;           
            isPressed = false;
            doorSoundPlayed = false;                       
        } 
    }

    
    public void CheckPlayerSize(GameObject playerobj, GameObject pulsobj)
    {
        Debug.Log("Chequeando Tamaño...");
        float sizeplayer = playerobj.transform.lossyScale.x; 
        float sizepulsador = pulsobj.transform.lossyScale.x;
        Debug.Log(sizeplayer);
        Debug.Log(sizepulsador);
        if ((Mathf.Abs(sizeplayer - sizepulsador) < 0.1f) && !opendoor) {
            doorAnimator.SetTrigger("Door_Open");
            if (!doorSoundPlayed)
            {
                transform.localPosition = initialPosition - new Vector3(0, 0.1f, 0);
                sonidopuerta.Play();
                doorSoundPlayed = true; //Evita que se repita el sonido
            }
            Debug.Log("Puerta Abierta");
            opendoor = true;
        }
        else if (!opendoor) {
            // Si no coinciden los tamaños y la puerta no está abierta, permanece cerrada
            Debug.Log("No son del mismo tamaño...");
            Debug.Log("Puerta Cerrada!");
            opendoor = false;
        }       
    }

    public bool IsDoorOpen()
    {
        return opendoor;
    }
}
