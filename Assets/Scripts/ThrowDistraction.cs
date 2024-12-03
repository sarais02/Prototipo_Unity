using System.Collections;
using UnityEngine;

public class ThrowDistraction : MonoBehaviour
{
    [SerializeField] private GameObject distractionPrefab;
    [SerializeField] private float throwForce = 10f;

    private Vector3 initialScale;
    private Coroutine resetCoroutine;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Throw();
        }
    }

    private void Throw()
    {
        // Instancia el objeto
        GameObject distraction = Instantiate(distractionPrefab, transform.position + transform.forward, Quaternion.identity);

        // Aplica fuerza
        Rigidbody rb = distraction.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }
    }
}
