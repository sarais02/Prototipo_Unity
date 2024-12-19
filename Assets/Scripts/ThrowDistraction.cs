using System.Collections;
using UnityEngine;

public class ThrowDistraction : MonoBehaviour
{
    [SerializeField] private GameObject distractionPrefab;
    [SerializeField] private float throwForce = 10f;

    private bool isThrowing = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isThrowing)
        {
            StartCoroutine(ThrowWithDelay());
        }
    }

    private IEnumerator ThrowWithDelay()
    {
        isThrowing = true;
        yield return new WaitForSeconds(0.8f); 
        GameObject distraction = Instantiate(distractionPrefab, transform.position + transform.forward, Quaternion.identity);

        // Aplica fuerza
        Rigidbody rb = distraction.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1.35f); //Espera a que la animación acabe
        isThrowing = false;
    }
}
