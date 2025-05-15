using System.Collections;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    public GameObject proyectil; // Prefab del proyectil
    public Transform spawn;      // Punto donde aparece el proyectil
    public float damage = 20f;   // Daño del rifle
    public float cadencia = 0.1f;   // 10 balas por segundo
    private float cadencia2;     // Temporizador para controlar los disparos
    public Animator animator;    // Animator para controlar las animaciones
    public Camera jugadorCamera;  // Cámara del jugador (para calcular la dirección del disparo)
    public float alcance = 300f;   // Alcance máximo del disparo
    public Transform manoTransform;
    public GameObject rifle; // Pistola para cambiar más tarde
    public GameObject knife; // Cuchillo
    public GameObject pistola; // Cuchillo

    public float speed = 2f;
    public Transform manoIzqIKTarget;
    void Start()
    {
        // Anclar la pistola a la mano
        if (manoTransform != null && gameObject != null)
        {
            rifle.transform.SetParent(manoTransform);
            rifle.transform.localPosition = new Vector3(0.16f, 0.298f, -0.119f); // Ajusta según el modelo
            rifle.transform.localRotation = Quaternion.Euler(7.026f, 49.306f, 92.113f); // Ajusta según el modelo

            Debug.Log("Rifle anclada correctamente a la mano.");
        }
        if (rifle != null)
        {
            if (manoIzqIKTarget == null)
            {
                Debug.LogError("No se encontró 'mixamorig:LeftHand' dentro del rifle.");
            }
        }
        // Activar la pistola al iniciar
        pistola.SetActive(false);
        knife.SetActive(false);
        rifle.SetActive(true);
        // Obtener el Animator
        animator = GameObject.Find("Jugador").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el personaje.");
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (animator != null)
            {
                animator.SetBool("AndaRifle", true); // Activar animación de caminar
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("AndaRifle", false); // Desactivar animación de caminar (debería regresar a "idle")
            }
        }

        if (Input.GetButton("LeftShift"))
        {
            if (speed != 5f)
            {
                speed = 5f; // Ajustar velocidad al correr
                animator.SetBool("RifleRun", true);
            }
        }
        else
        {
            if (speed != 2f)
            {
                speed = 2f; // Ajustar velocidad al andar
                animator.SetBool("RifleRun", false);
            }
        }
        // Verifica si está apuntando (Fire2)
        if (Input.GetButton("Fire2")) // Botón derecho para apuntar
        {
            // Permitir disparar solo si Fire2 está mantenido y Fire1 se pulsa o mantiene
            if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && Time.time >= cadencia2)
            {
                cadencia2 = Time.time + cadencia; // Actualizar el temporizador según la cadencia
                Disparar(); // Disparar proyectil
            }
            else
            {
                // Si suelta Fire1 mientras mantiene Fire2, no dispara
                if (animator != null)
                {
                    animator.SetBool("DispararRifle", false); // Detener animación de disparo
                }
            }
        }
        else
        {
            // Apagar la animación de disparo si se suelta Fire2 (opcional, según diseño)
            animator.SetBool("DispararRifle", false);
        }
    }

    public void Disparar()
    {
        if (animator != null)
        {
            animator.SetBool("DispararRifle", true);

            // Raycast desde el centro de la cámara
            Ray ray = jugadorCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, alcance))
            {
                targetPoint = hit.point; // Punto de impacto del raycast
            }
            else
            {
                targetPoint = ray.GetPoint(alcance); // Punto lejano si no impacta nada
            }

            // Crear la bala
            Vector3 direction = (targetPoint - spawn.position).normalized;
            GameObject bala = Instantiate(proyectil, spawn.position, Quaternion.LookRotation(direction));

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
    void OnAnimatorIK(int layerIndex)
    {
        if (animator != null)
        {
            // Configurar IK para la mano derecha
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, manoTransform.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, manoTransform.rotation);

            // Configurar IK para la mano izquierda
            if (manoIzqIKTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, manoIzqIKTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, manoIzqIKTarget.rotation);
            }
            else
            {
                Debug.LogError("leftHandIKTarget es nulo. Verifica la asignación.");
            }
        }
    }
}
