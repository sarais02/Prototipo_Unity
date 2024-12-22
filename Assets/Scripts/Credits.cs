using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject creditsCanvas = null;
    public GameObject musicadeFondo;
    private AudioSource audioSource;
    private AudioClip clip;

    private void OnTriggerEnter(Collider other)
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.1f;

        if (other.CompareTag("Player"))
        {
            creditsCanvas.SetActive(true);
            musicadeFondo.GetComponent<AudioSource>().Stop();
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
