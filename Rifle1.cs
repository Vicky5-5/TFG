using System.Collections;
using UnityEngine;

public class Rifle1 : MonoBehaviour, IArma
{
    public GameObject proyectil; // Prefab del proyectil
    public Transform spawn;      // Punto donde aparece el proyectil
    public float damage = 20f;   // Da�o del rifle
    public float rate2 = 0.1f;   // 10 balas por segundo
    private float shotRate2;     // Temporizador para controlar los disparos
    public Animator animator;    // Animator para controlar las animaciones
    public Camera playerCamera;  // C�mara del jugador (para calcular la direcci�n del disparo)
    public LayerMask aimLayerMask; // Capas con las que debe colisionar el raycast
    public float range = 300f;   // Alcance m�ximo del disparo

    public GameObject pistola; // Pistola para cambiar m�s tarde
    public GameObject knife; // Cuchillo
    void Start()
    {
        // Obtener el componente Animator desde el personaje
        animator = GameObject.Find("Swat").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontr� un Animator en el rifle.");
        }
        pistola.SetActive(false);
        knife.SetActive(false);
    }

    void Update()
    {
        
        // Verifica si est� apuntando (Fire2)
        if (Input.GetButton("Fire2")) // Bot�n derecho para apuntar
        {
            // Permitir disparar si Fire1 est� mantenido o pulsado
            if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && Time.time >= shotRate2))
            {
                shotRate2 = Time.time + rate2; // Actualizar el temporizador seg�n la cadencia
                Shoot(); // Disparar proyectil
            }
        }
        else
        {
            // Apagar la animaci�n de disparo si se suelta Fire2 (opcional, seg�n dise�o)
            animator.SetBool("Disparar", false);
        }
    }

    public void Shoot()
    {
        if (animator != null)
        {
            animator.SetBool("Disparar", true);

            // Raycast desde el centro de la c�mara
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, range, aimLayerMask))
            {
                targetPoint = hit.point; // Punto de impacto del raycast
            }
            else
            {
                targetPoint = ray.GetPoint(range); // Punto lejano si no impacta nada
            }

            // Crear la bala
            Vector3 direction = (targetPoint - spawn.position).normalized;
            GameObject bala = Instantiate(proyectil, spawn.position, Quaternion.LookRotation(direction));

            // Configurar el da�o de la bala
            Bala1 balaScript = bala.GetComponent<Bala1>();
            if (balaScript != null)
            {
                balaScript.SetDamage(damage);
                Debug.Log("Da�o configurado en la bala: " + damage);
            }
            else
            {
                Debug.LogError("El script Bala1 no est� asignado al prefab de la bala.");
            }

            Debug.Log("�Disparo realizado!");
        }
        else
        {
            Debug.LogError("El Animator no est� asignado.");
        }
    }
}
