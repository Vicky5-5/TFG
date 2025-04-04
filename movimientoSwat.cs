using UnityEngine;
using UnityEngine.UI;

public class movimientoSwat : MonoBehaviour
{
    public float speed = 2f;
    public float jumpSpeed = 4f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 300f;

    public VidaPersonaje healthManager; // Referencia al script de vida

    public Image cruz; // Ret�cula de apuntado
    public GameObject rifle; // Rifle configurado como arma inicial
    public Transform rightHand; // Mano derecha
    public Transform leftHandIKTarget; // Objetivo IK para la mano izquierda

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

        // Configurar el rifle como arma inicial
        if (rifle != null && rightHand != null)
        {
            rifle.SetActive(true); // Activar el rifle
            rifle.transform.SetParent(rightHand); // Anclar el rifle a la mano derecha
            rifle.transform.localPosition = new Vector3(0.167f, 0.312f, -0.023f); // Ajusta seg�n sea necesario
            rifle.transform.localRotation = Quaternion.Euler(9.92f, 20.9f, 92.24f); // Ajusta seg�n sea necesario
        }
        else
        {
            Debug.LogError("Rifle o mano derecha no asignados en el Inspector.");
        }

      
        // Asegurar que la ret�cula est� oculta al inicio
        if (cruz != null)
        {
            cruz.enabled = false;
        }
        else
        {
            Debug.LogError("No se ha asignado la ret�cula en el Inspector.");
        }

        if (healthManager == null)
        {
            Debug.LogError("HealthManager no est� asignado en el Inspector.");
        }
    }

    void Update()
    {

        // Movimiento y rotaci�n
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        // Calcula la direcci�n basada en la c�mara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Elimina cualquier movimiento vertical (en el eje Y)
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Direcci�n del movimiento basada en la c�mara
        Vector3 moveDirection = (forward * verticalInput) + (right * horizontalInput);

        // Aplicar movimiento usando la velocidad
        Vector3 velocity = moveDirection * speed;
        velocity.y = ySpeed; // Mantener la gravedad y salto
        characterController.Move(velocity * Time.deltaTime);

        // Se ejecutan los movimientos del personaje
        //Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        if (characterController.isGrounded)
        {
            ySpeed = 0;
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;
                animator.SetBool("Salto", true);
            }
            else
            {
                animator.SetBool("Salto", false);
            }
        }
        else
        {
            ySpeed += gravity * Time.deltaTime;
        }

        //Vector3 velocity = moveDirection * speed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        // Animaciones de caminar y correr
        animator.SetBool("isWalking", moveDirection.magnitude > 0);

        if (Input.GetButton("LeftShift"))
        {
            speed = 10f;
            animator.SetBool("Correr", true);
        }
        else
        {
            
            animator.SetBool("Correr", false);
        }
    }

    public void TakeDamageFromEnemy(float damage)
    {
        // Llamar al m�todo TakeDamage del HealthManager
        if (healthManager != null)
        {
            healthManager.TakeDamage(damage);
        }
    }
    void OnAnimatorIK(int layerIndex)
    {
        // IK para la mano derecha (rifle)
        if (rightHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
        }

        float leftHandWeight = 1f; // Aseg�rate de que este valor est� entre 0 y 1

        if (leftHandIKTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        }
    }
}
