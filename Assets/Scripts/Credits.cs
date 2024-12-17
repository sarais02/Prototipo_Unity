using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject creditsCanvas = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            creditsCanvas.SetActive(true);
        }
    }
}
