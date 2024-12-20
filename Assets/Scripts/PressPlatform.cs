using UnityEngine;
using UnityEngine.Events;
public class PressPlatform : MonoBehaviour
{
    public GameObject pulsador;
    public GameObject jugador;
    public GameObject puerta; //A cada boton hay que asociarle una puerta en el Inspector
    public Material correctMaterial;

    AudioSource efectosonido;
    AudioSource sonidopuerta; 

    bool isPressed = false;
    bool opendoor = false;
    bool doorSoundPlayed = false;
    bool inTrigger = false;
    private Animator doorAnimator;
    private Renderer objRenderer;
    Collider colliderobj;
    Vector3 initialPosition = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        efectosonido = GetComponent<AudioSource>();
        colliderobj = this.GetComponent<Collider>();
        initialPosition = pulsador.transform.localPosition;
        doorAnimator = puerta.GetComponent<Animator>();
        sonidopuerta = puerta.GetComponent<AudioSource>();
        objRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed && other.gameObject == jugador) { //Chequea si no esta presionado antes: ispressed!=true isPressed == false && (
            inTrigger = true;
            efectosonido.Play();
            CheckPlayerSize(jugador, pulsador);
            isPressed = true;            
        }               
    }

    private void OnTriggerStay(Collider other) {
        if (isPressed && other.gameObject == jugador) {
            CheckPlayerSize(jugador, pulsador); 
        } 
    }

    private void OnTriggerExit(Collider other) {
        if (inTrigger && other.gameObject==jugador)
        {
            inTrigger = false;                    
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
            objRenderer.material = correctMaterial;
            doorAnimator.SetTrigger("Door_Open");
            if (!doorSoundPlayed)
            {                
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
