using UnityEngine;

public class LanzaProyectilTrampa : MonoBehaviour
{
    public GameObject projectile;         // Prefab del proyectil
    public Transform spawnPoint;          // Punto desde donde se lanza el proyectil
    public float damage = 10f;
    public float projectileSpeed = 30f;
    public float rangoActivacion = 10f;   // Distancia mínima para disparar
    public float tiempoEntreDisparos = 0.5f;

    private Transform jugador;
    private float tiempoProximoDisparo;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'Player'");
        }
    }

    private void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);
        if (distancia <= rangoActivacion && Time.time >= tiempoProximoDisparo)
        {
            Disparar(jugador);
            tiempoProximoDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    private void Disparar(Transform objetivo)
    {
        GameObject proyectil = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);

        Rigidbody rb = proyectil.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direccion = (objetivo.position - spawnPoint.position).normalized;
            rb.linearVelocity = direccion * projectileSpeed;
        }

        Bala balaScript = proyectil.GetComponent<Bala>();
        if (balaScript != null)
        {
            balaScript.SetDamage(damage);
        }

        Debug.Log("¡Trampa disparó al jugador!");
    }
}
