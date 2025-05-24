using UnityEngine;

public class Bala : MonoBehaviour
{
    public float speed = 50f; // Velocidad de la bala
    public float damage; // Da�o que inflige la bala
    private Vector3 moveDirection;
    public GameObject enemigo;
    void Start()
    {
        moveDirection = transform.forward; // Direcci�n inicial de la bala
        float maxDistance = 500f; // Alcance m�ximo
        float lifeTime = maxDistance / speed; // Calcula el tiempo necesario para recorrer la distancia
        Destroy(gameObject, lifeTime); // Destruir tras alcanzar el alcance m�ximo
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;

        // Verificar colisiones en el camino
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDirection, out hit, moveDistance))
        {
            // Procesar impacto si se detecta una colisi�n
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
        damage = damageValue; // Establecer el da�o desde el rifle o cualquier fuente externa
    }

    private void OnImpact(Collider other)
    {
        // Detectar si la bala golpea a un enemigo
        Enemigo enemigo = other.GetComponent<Enemigo>();
        EnemigoBoss boss = other.GetComponent<EnemigoBoss>();

        if (enemigo != null)
        {
            enemigo.TakeDamage(damage); // Aplicar da�o al enemigo
        }

        if (boss != null)
        {
            boss.TakeDamage(damage); // Aplicar da�o al enemigo
        }

        // Detectar si la bala golpea al jugador
        VidaPersonaje playerHealth = other.GetComponent<VidaPersonaje>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamagePlayer(damage); // Aplicar da�o al jugador
        }

        // Destruir la bala despu�s del impacto
        Destroy(gameObject);
    }


}
