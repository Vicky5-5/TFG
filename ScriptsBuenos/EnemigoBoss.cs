using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemigoBoss : MonoBehaviour
{
    public Image barraVida;
    public float VidaMaxima = 100f; // Vida del enemigo
    public float health = 100f; // Vida del enemigo
    public NavMeshAgent agent;
    public Transform player; // Referencia al jugador
    public LayerMask whatIsGround, whatIsPlayer;

    // Patrullaje
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    // Ataque
    public float damage = 10f; // Daño que hace el enemigo
    public float timeBetweenAttacks = 2f;
    private bool alreadyAttacked;
    public Transform spawnPoint; // Punto de spawn del proyectil

    // Estados
    public float rangoAlcance = 10f;
    public float rangoAtaque = 5f;
    private bool jugadorEnRangoDeAlcance, jugadorEnRangoDeAtaque;

    public Animator animator;

    private void Start()
    {
        health = VidaMaxima;
        UpdateBarraVida(VidaMaxima, health);
    }

    private void Awake()
    {
        player = GameObject.Find("Jugador").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Verificar si el jugador está dentro de los rangos
        jugadorEnRangoDeAlcance = Physics.CheckSphere(transform.position, rangoAlcance, whatIsPlayer);
        jugadorEnRangoDeAtaque = Physics.CheckSphere(transform.position, rangoAtaque, whatIsPlayer);

        if (!jugadorEnRangoDeAlcance && !jugadorEnRangoDeAtaque)
            Patrolling();
        else if (jugadorEnRangoDeAlcance && !jugadorEnRangoDeAtaque)
            ChasePlayer();
        else if (jugadorEnRangoDeAlcance && jugadorEnRangoDeAtaque)
        {
            if (!alreadyAttacked)
                AttackPlayer();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateBarraVida(VidaMaxima, health);
        Debug.Log($"El enemigo ha recibido {damage} de daño. Vida restante: {health}");

        if (health <= 0)
            Die();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Punto de patrullaje alcanzado
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Verificar si el punto está sobre el suelo
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
        animator.SetBool("WalkZombie", true);
        animator.SetBool("ZombieRun", false);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("ZombieRun", true);
        animator.SetBool("WalkZombie", false);

    }

    private void AttackPlayer()
    {

        // Detiene el movimiento
        agent.SetDestination(transform.position);

        // Mira al jugador (sin inclinarse hacia arriba o abajo)
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // Animación de ataque
        animator.SetTrigger("ZombieAtaque");

        // Aplicar daño al jugador (si está cerca y tiene el script correcto)
        VidaPersonaje playerHealth = player.GetComponent<VidaPersonaje>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamagePlayer(damage);
        }

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);


    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Die()
    {
        Debug.Log("El enemigo ha muerto.");
        animator.SetBool("MuerteZombie", true);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoAlcance);
    }

    void UpdateBarraVida(float vidaMaxima, float vidaActual)
    {
        barraVida.fillAmount = vidaActual / vidaMaxima;
    }
}
