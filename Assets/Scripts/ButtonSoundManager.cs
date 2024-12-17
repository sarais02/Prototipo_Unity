using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioSource audioSource; // Arrastra aqu� el AudioSource
    public AudioClip hoverSound;    // Sonido al pasar el mouse
    public AudioClip clickSound;    // Sonido al hacer click

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // M�todo para reproducir el sonido de hover
    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    // M�todo para reproducir el sonido de click
    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
