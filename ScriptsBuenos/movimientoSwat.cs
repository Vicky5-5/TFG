using UnityEngine;
using UnityEngine.UI;

public class movimientoSwat : MonoBehaviour
{
    public float velocidad = 2f;
    public float velocidadSalto = 4f;
    public float gravedad = -9.81f;
    public float sensibilidadRaton = 300f;

    public VidaPersonaje barraVida; // Referencia al script de vida

    public Image cruz; // Retícula de apuntado
    public GameObject pistola; // Rifle configurado como arma inicial
    public GameObject cuchillo; // Rifle configurado como arma inicial
    public GameObject rifle; // Rifle configurado como arma inicial
    public Transform manoDerecha; // Mano derecha
    public Transform manoIzquierdaIKTarget; // Objetivo IK para la mano izquierda

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    public Transform cameraTransform;
    void Start()
    {
        // Configurar referencias
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Desactivar el cuchillo al inicio
        if (cuchillo != null)
        {
            cuchillo.SetActive(false);
        }
        if (rifle != null)
        {
            rifle.SetActive(false);
        }
        // Configurar la pistola como arma inicial correctamente
        if (pistola != null && manoDerecha != null)
        {
            pistola.SetActive(true);
            pistola.transform.SetParent(manoDerecha);
            pistola.transform.localPosition = new Vector3(-0.04162508f, 0.1772844f, 0.08919427f);
            pistola.transform.localRotation = Quaternion.Euler(166.204f, 181.868f, 271.425f);
        }
        else
        {
            Debug.LogError("Pistola o mano derecha no asignados en el Inspector.");
        }

        // Configurar referencia al Gestor de Armas SOLO si no lo maneja ya
        GestorArmas gestorArmas = Object.FindFirstObjectByType<GestorArmas>(); // Corrección de método
        if (gestorArmas != null)
        {
            gestorArmas.CambiarArma(pistola); // Establecer la pistola
            Debug.Log("GestorArmas activando la pistola correctamente.");
        }
        else
        {
            Debug.LogError("No se encontró el GestorArmas en la escena.");
        }

        // Asegurar que la retícula esté oculta al inicio
        if (cruz != null)
        {
            cruz.enabled = false;
        }
        else
        {
            Debug.LogError("No se ha asignado la retícula en el Inspector.");
        }

        if (barraVida == null)
        {
            Debug.LogError("HealthManager no está asignado en el Inspector.");
        }
    }


    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadRaton * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        Vector3 moveDirection = cameraTransform.forward * verticalInput + cameraTransform.right * horizontalInput;
        moveDirection.y = 0; // Evita que se modifique la altura por error
        moveDirection.Normalize();

        Vector3 velocity = moveDirection * velocidad;

        // Manejo del salto
        if (characterController.isGrounded)
        {
            ySpeed = -0.5f; // Pequeño valor negativo para asegurar que esté en el suelo

            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = velocidadSalto;
                animator.SetBool("Salto", true);
            }
            else
            {
                animator.SetBool("Salto", false);
            }
        }
        else
        {
            ySpeed += gravedad * Time.deltaTime; // Aplica gravedad gradual
        }

        velocity.y = ySpeed; // Mantiene la velocidad en el eje Y
        characterController.Move(velocity * Time.deltaTime); // Aplica movimiento final

        // **Animaciones de caminar y correr**
        bool isWalking = moveDirection.magnitude > 0;
        animator.SetBool("isWalking", isWalking);

        if (Input.GetKey(KeyCode.LeftShift) && isWalking)
        {
            velocidad = 5f; // Aumenta la velocidad al correr
            animator.SetBool("Correr", true);
        }
        else
        {
            velocidad = 2f; // Vuelve a caminar si no está corriendo
            animator.SetBool("Correr", false);
        }
    }




    public void TakeDamageFromEnemy(float damage)
    {
        // Llamar al método TakeDamage del HealthManager
        if (barraVida != null)
        {
            barraVida.TakeDamagePlayer(damage);
        }
    }
    void OnAnimatorIK(int layerIndex)
    {
        // IK para la mano derecha (pistola)
        if (manoDerecha != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, manoDerecha.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, manoDerecha.rotation);
        }

        float leftHandWeight = 1f; // Asegúrate de que este valor esté entre 0 y 1

        if (manoIzquierdaIKTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, manoIzquierdaIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, manoIzquierdaIKTarget.rotation);
        }
    }
}
