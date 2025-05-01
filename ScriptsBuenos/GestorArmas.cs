using UnityEngine;

public class GestorArmas : MonoBehaviour
{
    [Header("Configuraci�n de Armas")]
    public GameObject pistola; // GameObject de la pistola
    public GameObject rifle; // GameObject del rifle
    public GameObject knife; // GameObject del cuchillo

    [Header("Spawns de C�mara")]
    public Transform pistolCameraSpawn; // Punto de spawn de la c�mara para la pistola
    public Transform rifleCameraSpawn; // Punto de spawn de la c�mara para el rifle
    public Transform knifeCameraSpawn; // Punto de spawn de la c�mara para el cuchillo

    [Header("Animaciones")]
    public Animator animator; // Controlador de animaciones

    [Header("Controlador de C�mara")]
    public ControladorCamara cameraController; // Referencia al script del controlador de c�mara

    [Header("No tocar, gracias :)")]
    public GameObject armaActual; // Arma actualmente seleccionada

    public static GestorArmas Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("M�s de un GestorArmas encontrado en la escena.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (pistola != null)
        {
            CambiarArma(pistola); // Pistola como arma inicial
            Debug.Log("Pistola configurada como arma inicial.");
        }
        else
        {
            Debug.LogError("La pistola no est� asignada en el Inspector.");
        }
    }

    void Update()
    {
        // Cambiar entre armas usando teclas
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CambiarArma(pistola);
            animator?.SetBool("Knife", false);
            animator?.SetBool("Rifle", false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CambiarArma(rifle);
            animator?.SetBool("Knife", false);
            animator?.SetBool("Rifle", true);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (knife != null)
            {
                CambiarArma(knife);
                animator?.SetBool("Knife", true);
                animator?.SetBool("Rifle", false);

            }

        } 
    }

    /// <summary>
    /// Cambia el arma activa y ajusta la c�mara.
    /// </summary>
    /// <param name="nuevaArma">El GameObject del arma que se activar�.</param>
    public void CambiarArma(GameObject nuevaArma)
    {
        if (armaActual != null)
        {
            armaActual.SetActive(false); // Desactivar arma anterior
            Debug.Log($"Arma anterior desactivada: {armaActual.name}");
        }

        if (nuevaArma != null)
        {
            nuevaArma.SetActive(true); // Activar nueva arma
            armaActual = nuevaArma;
            Debug.Log($"Nueva arma activada: {armaActual.name}");

            // Cambiar la posici�n de la c�mara seg�n el arma
            if (nuevaArma == pistola && pistolCameraSpawn != null)
            {
                cameraController.AdjustCameraForWeapon(pistolCameraSpawn);
            }
            else if (nuevaArma == rifle && rifleCameraSpawn != null)
            {
                cameraController.AdjustCameraForWeapon(rifleCameraSpawn);
            }
            else if (nuevaArma == knife && knifeCameraSpawn != null)
            {
                Debug.Log("Cambiando la c�mara al spawn del cuchillo.");
                cameraController.AdjustCameraForWeapon(knifeCameraSpawn);
            }
            
            else
            {
                Debug.LogWarning("No se encontr� un spawn de c�mara para esta arma.");
            }
        }
        else
        {
            Debug.LogError("El objeto del arma es nulo.");
        }
    }
    

}
