using UnityEngine;

public class LanzaProyectilTrampa : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject projectile; // Prefab del proyectil
    public Transform spawnPoint; // Punto de origen del proyectil
    public float damage = 10f; // Daño infligido por la trampa
    public float projectileSpeed = 30f; // Velocidad del proyectil

    private void OnTriggerEnter(Collider other)
    {
        // Detectar si el jugador est� dentro del rango de la trampa
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador activ� la trampa.");
            Disparar(other.transform); // Disparar hacia el jugador
        }
    }

    private void Disparar(Transform objetivo)
    {
        // Crear el proyectil en el punto de spawn
        GameObject proyectil = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);

        // Configurar dirección del proyectil hacia el jugador
        Rigidbody rb = proyectil.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direccion = (objetivo.position - spawnPoint.position).normalized; // Direcci�n hacia el jugador
            rb.linearVelocity = direccion * projectileSpeed; // Configurar velocidad
        }

        // Configurar el daño del proyectil (si tiene un script de bala)
        Bala balaScript = proyectil.GetComponent<Bala>();
        if (balaScript != null)
        {
            balaScript.SetDamage(damage); // Pasar el da�o al proyectil
        }
    }
}
