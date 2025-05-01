using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaracterGranada : MonoBehaviour
{
    [Header("Explosion Settings")]
    public int damage = 50;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public GameObject explosionEffect; // Prefab del efecto de explosión

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("La granada no tiene un Rigidbody.");
        }
    }

    public void StartExplosionTimer()
    {
        Invoke(nameof(Explode), 3f); // La granada explotará solo después de ser lanzada
    }

    public void Explode()
    {
        // Instanciar el efecto de partículas en la explosión
        if (explosionEffect != null)
        {
            GameObject effectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Reproducir el ParticleSystem manualmente
            ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
            else
            {
                // Si el ParticleSystem está en un hijo, buscarlo
                ps = effectInstance.GetComponentInChildren<ParticleSystem>();
                if (ps != null) ps.Play();
            }

            Destroy(effectInstance, 2f); // Elimina el efecto tras 2 segundos
        }

        // Detectar enemigos en el radio de explosión
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.GetComponent<Enemigo>())
            {
                hit.GetComponent<Enemigo>().TakeDamage(damage);
            }
        }

        // Destruir la granada tras explotar
        Destroy(gameObject);
    }



}
