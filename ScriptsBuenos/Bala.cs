using UnityEngine;

public class Bala : MonoBehaviour
{
    public float speed = 50f; // Velocidad de la bala
    public float damage; // Daño que inflige la bala
    private Vector3 moveDirection;
    public GameObject enemigo;
    void Start()
    {
        moveDirection = transform.forward; // Dirección inicial de la bala
        float maxDistance = 500f; // Alcance máximo
        float lifeTime = maxDistance / speed; // Calcula el tiempo necesario para recorrer la distancia
        Destroy(gameObject, lifeTime); // Destruir tras alcanzar el alcance máximo
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;

        // Verificar colisiones en el camino
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDirection, out hit, moveDistance))
        {
            // Procesar impacto si se detecta una colisión
            OnImpact(hit.collider);
        }
        else
        {
            // Movimiento de la bala si no hay colisiones
            transform.position += moveDirection * moveDistance;
        }
    }

    public void SetDamage(float damageValue)
    {
        damage = damageValue; // Establecer el daño desde el rifle o cualquier fuente externa
    }

    private void OnImpact(Collider other)
    {
        // Detectar si la bala golpea a un enemigo
        Enemigo enemigo = other.GetComponent<Enemigo>();
        EnemigoBoss boss = other.GetComponent<EnemigoBoss>();

        if (enemigo != null)
        {
            enemigo.TakeDamage(damage); // Aplicar daño al enemigo
        }

        if (boss != null)
        {
            boss.TakeDamage(damage); // Aplicar daño al enemigo
        }

        // Detectar si la bala golpea al jugador
        VidaPersonaje playerHealth = other.GetComponent<VidaPersonaje>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamagePlayer(damage); // Aplicar daño al jugador
        }

        // Destruir la bala después del impacto
        Destroy(gameObject);
    }


}
