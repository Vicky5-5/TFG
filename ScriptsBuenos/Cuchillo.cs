using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuchillo : MonoBehaviour
{
    public Transform handTransform; // Transform de la mano del personaje
    public float damage = 5f; // Da�o del cuchillo
    public Animator animator; // Animator para controlar las animaciones
    public GameObject rifle; // Rifle configurado como arma inicial
    public GameObject pistola; // Pistola para cambiar m�s tarde
    public GameObject knife; // Cuchillo
    public GameObject enemigoG;
    private bool canAttack = true; // Control para evitar m�ltiples activaciones

    void Start()
    {
        // Anclar el cuchillo a la mano
        if (handTransform != null && knife != null)
        {
            knife.transform.SetParent(handTransform);
            knife.transform.localPosition = new Vector3(-0.265f, 0.052f, 0.103f); // Ajusta seg�n el modelo
            knife.transform.localRotation = Quaternion.Euler(-8.1f, -170.04f, 0f); // Ajusta seg�n el modelo

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
            Debug.LogError("No se encontr� un Animator en el personaje.");
        }
    }

    void Update()
    {
        // Control de la animaci�n de caminar
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (animator != null)
            {
                animator.SetBool("AndarKnife", true); // Activar animaci�n de caminar
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("AndarKnife", false); // Desactivar animaci�n de caminar (deber�a regresar a "idle")
            }
        }

        // Control de la animaci�n de ataque
        if (Input.GetButtonDown("Fire1") && canAttack) // Bot�n izquierdo para atacar
        {
            if (animator != null)
            {
                StartCoroutine(Attack()); // Iniciar el ataque
            }
        }
    }

    // Interfaz que se utiliza para definir m�todos que pueden ser interrumpidos y reanudados, conocidos como corutinas
    private IEnumerator Attack()
    {
        canAttack = false; // Bloquear el ataque durante la ejecuci�n
        animator.SetTrigger("KnifeAtaque"); // Activar la animaci�n de ataque

        // Esperar la duraci�n de la animaci�n antes de continuar
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo seg�n la animaci�n

        // Obtener origen y direcci�n del Raycast desde la c�mara
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        Debug.DrawRay(origin, direction * 4f, Color.red, 2f); // L�nea de depuraci�n para visualizar

        // Realizar el Raycast
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, 4f)) // Cambiar el rango seg�n sea necesario
        {

            // Verificar si el objeto golpeado es un enemigo
            Enemigo enemigo = hit.collider.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.TakeDamage(damage); // Aplicar da�o al enemigo
                Debug.Log("Da�o aplicado al enemigo.");
            }
        }
        else
        {
            Debug.Log("El Raycast no detect� ning�n objetivo.");
        }

        canAttack = true; // Permitir el pr�ximo ataque
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Aseg�rate de que el enemigo tenga la etiqueta "Enemigo"
        {
            Debug.Log("El cuchillo golpe� al enemigo: " + other.name);
            Enemigo enemigo = other.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.TakeDamage(damage); // Aplicar da�o al enemigo
            }
        }
    }
}