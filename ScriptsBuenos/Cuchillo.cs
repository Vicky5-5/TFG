using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuchillo : MonoBehaviour
{
    public Transform handTransform; // Transform de la mano del personaje
    public float damage = 5f; // Daño del cuchillo
    public Animator animator; // Animator para controlar las animaciones
    public GameObject rifle; // Rifle configurado como arma inicial
    public GameObject pistola; // Pistola para cambiar más tarde
    public GameObject knife; // Cuchillo
    public GameObject enemigoG;
    private bool canAttack = true; // Control para evitar múltiples activaciones

    void Start()
    {
        // Anclar el cuchillo a la mano
        if (handTransform != null && knife != null)
        {
            knife.transform.SetParent(handTransform);
            knife.transform.localPosition = new Vector3(-0.265f, 0.052f, 0.103f); // Ajusta según el modelo
            knife.transform.localRotation = Quaternion.Euler(-8.1f, -170.04f, 0f); // Ajusta según el modelo

            Debug.Log("Cuchillo anclado correctamente.");
        }

        // Activar el cuchillo y desactivar el rifle y pistola
        rifle.SetActive(false);
        pistola.SetActive(false);
        knife.SetActive(true);

        // Obtener el Animator
        animator = GameObject.Find("Jugador").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el personaje.");
        }
    }

    void Update()
    {
        // Control de la animación de caminar
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (animator != null)
            {
                animator.SetBool("AndarKnife", true); // Activar animación de caminar
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("AndarKnife", false); // Desactivar animación de caminar (debería regresar a "idle")
            }
        }

        // Control de la animación de ataque
        if (Input.GetButtonDown("Fire1") && canAttack) // Botón izquierdo para atacar
        {
            if (animator != null)
            {
                StartCoroutine(Attack()); // Iniciar el ataque
            }
        }
    }

    // Interfaz que se utiliza para definir métodos que pueden ser interrumpidos y reanudados, conocidos como corutinas
    private IEnumerator Attack()
    {
        canAttack = false; // Bloquear el ataque durante la ejecución
        animator.SetTrigger("KnifeAtaque"); // Activar la animación de ataque

        // Esperar la duración de la animación antes de continuar
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo según la animación

        // Obtener origen y dirección del Raycast desde la cámara
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        Debug.DrawRay(origin, direction * 4f, Color.red, 2f); // Línea de depuración para visualizar

        // Realizar el Raycast
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, 4f)) // Cambiar el rango según sea necesario
        {

            // Verificar si el objeto golpeado es un enemigo
            Enemigo enemigo = hit.collider.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.TakeDamage(damage); // Aplicar daño al enemigo
                Debug.Log("Daño aplicado al enemigo.");
            }
        }
        else
        {
            Debug.Log("El Raycast no detectó ningún objetivo.");
        }

        canAttack = true; // Permitir el próximo ataque
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Asegúrate de que el enemigo tenga la etiqueta "Enemigo"
        {
            Debug.Log("El cuchillo golpeó al enemigo: " + other.name);
            Enemigo enemigo = other.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.TakeDamage(damage); // Aplicar daño al enemigo
            }
        }
    }
}