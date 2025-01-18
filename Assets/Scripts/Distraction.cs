using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    [SerializeField] private float distractionRadius;
    [SerializeField] private float investigationTime;
    [SerializeField] private Transform auxTransform;

    [SerializeField] private Material material;
    private float dissolveStrength = 0f;
    public float speed = 1f; // Velocidad de disolución

    private MaquinaDeEstados maquinaDeEstados;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            TriggerDistraction();
        }
    }
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        StartCoroutine(StartDissolveAfterDelay(2.5f));
        if (renderer != null)
        {
            material = renderer.material; 
        }
    }

    IEnumerator StartDissolveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(maquinaDeEstados != null) maquinaDeEstados.VolverAPatrullar();
        Destroy(gameObject, 1f);
        while (dissolveStrength < 1)
        {
            dissolveStrength = Mathf.Lerp(dissolveStrength, 1, Time.deltaTime * speed);
            material.SetFloat("_DissolveStrength", dissolveStrength);
            yield return null;
        }

        material.SetFloat("_DissolveStrength", 1); 
    }

    void TriggerDistraction()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, distractionRadius);

        foreach (Collider col in nearbyEnemies)
        {
            if (col.CompareTag("Enemy"))
            {
                EnemyAI enemy = col.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.Investigate(auxTransform.position);
                }
            }
            if (col.CompareTag("BadGranny"))
            {
                maquinaDeEstados = col.GetComponent<MaquinaDeEstados>();
                if (maquinaDeEstados != null)
                {
                    maquinaDeEstados.ComprobarPan(auxTransform);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distractionRadius);
    }
}