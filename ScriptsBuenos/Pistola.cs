using Unity.Burst.Intrinsics;
using UnityEngine;

public class Pistola : MonoBehaviour, IArma
{
    public GameObject proyectil; // Prefab del proyectil
    public Transform spawn;      // Punto donde aparece el proyectil
    public float damage = 20f;   // Daño del rifle
    public float rate2 = 0.1f;   // 10 balas por segundo
    private float shotRate2;     // Temporizador para controlar los disparos
    public Animator animator;    // Animator para controlar las animaciones
    public Camera playerCamera;  // Cámara del jugador (para calcular la dirección del disparo)
    public LayerMask aimLayerMask; // Capas con las que debe colisionar el raycast
    public float range = 300f;   // Alcance máximo del disparo
    public float fuerza = 100f; // Fuerza de disparo

    public GameObject rifle; // Pistola para cambiar más tarde
    public GameObject knife; // Cuchillo
    public GameObject pistola; // Cuchillo
    void Start()
    {
        // Obtener el componente Animator desde el personaje
        animator = GameObject.Find("Jugador").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el pistola.");
        }
        rifle.SetActive(false);
        knife.SetActive(false);
    }

    void Update()
    {

        // Verifica si está apuntando (Fire2)
        if (Input.GetButton("Fire2")) // Botón derecho para apuntar
        {
            // Permitir disparar si Fire1 está mantenido o pulsado
            if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && Time.time >= shotRate2))
            {
                shotRate2 = Time.time + rate2; // Actualizar el temporizador según la cadencia
                Shoot(); // Disparar proyectil
            }
        }
        else
        {
            // Apagar la animación de disparo si se suelta Fire2 (opcional, según diseño)
            animator.SetBool("DispararPistola", false);
        }
    }

    public void Shoot()
    {
        if (animator != null)
        {
            animator.SetBool("DispararPistola", true);

            // Raycast desde el centro de la cámara
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

            // Aplicar fuerza al Rigidbody de la bala
            Rigidbody balaRigidbody = bala.GetComponent<Rigidbody>();
            if (balaRigidbody != null)
            {
                balaRigidbody.AddForce(direction * fuerza, ForceMode.Impulse); // Usar fuerza de tipo "Impulse"
                Debug.Log("Fuerza aplicada a la bala: " + fuerza);
            }
            else
            {
                Debug.LogError("El proyectil no tiene un Rigidbody asignado.");
            }

            // Configurar el daño de la bala
            Bala balaScript = bala.GetComponent<Bala>();
            if (balaScript != null)
            {
                balaScript.SetDamage(damage);
                Debug.Log("Daño configurado en la bala: " + damage);
            }
            else
            {
                Debug.LogError("El script Bala no está asignado al prefab de la bala.");
            }

            Debug.Log("¡Disparo realizado!");
        }
        else
        {
            Debug.LogError("El Animator no está asignado.");
        }
    }

}
