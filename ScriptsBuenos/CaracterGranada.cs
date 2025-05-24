using System.Collections;
using UnityEngine;

public class CaracterGranada : MonoBehaviour
{
    [Header("Configuraci�n de Explosi�n")]
    public int damage = 50;
    public float radioExplosion = 10f;
    public float fuerzaExplosion = 50f;
    public GameObject efectoExplosion; // Prefab del efecto de explosi�n

    private Rigidbody rb;
    private bool haExplotado = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("La granada no tiene un Rigidbody.");
        }
    }

    public void TemporizadorExplosion()
    {
        Invoke(nameof(Explosion), 3f); // La granada explotar� despu�s de 3 segundos
    }

    public void Explosion()
    {
        if (haExplotado) return; // Evitar explosiones m�ltiples
        haExplotado = true;

        //Instanciar el efecto de part�culas de explosi�n
        if (efectoExplosion != null)
        {
            GameObject effectInstance = Instantiate(efectoExplosion, transform.position, Quaternion.identity);

            // Reproducir el ParticleSystem correctamente
            ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
            else
            {
                ps = effectInstance.GetComponentInChildren<ParticleSystem>();
                if (ps != null) ps.Play();
            }

            Destroy(effectInstance, 2f); // Eliminar el efecto tras 2 segundos
        }
        else
        {
            Debug.LogError("No se ha asignado un prefab para el efecto de explosi�n.");
        }

        // ?? **Aplicar da�o y fuerza de explosi�n**
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radioExplosion);
        foreach (Collider hit in hitColliders)
        {
            if (hit.GetComponent<Enemigo>())
            {
                hit.GetComponent<Enemigo>().TakeDamage(damage);
            }

            if (hit.GetComponent<EnemigoBoss>())
            {
                hit.GetComponent<EnemigoBoss>().TakeDamage(damage);
            }

            Rigidbody rbHit = hit.GetComponent<Rigidbody>();
            if (rbHit != null)
            {
                rbHit.AddExplosionForce(fuerzaExplosion, transform.position, radioExplosion);
            }
        }

        // Destruir la granada tras explotar
        Destroy(gameObject, 0.1f); // Peque�o retraso para visualizar la explosi�n antes de eliminar
    }
}