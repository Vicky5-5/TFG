using UnityEngine;
using UnityEngine.UI;

public class Apuntado : MonoBehaviour
{
    public Animator animator;    // Animator del personaje para controlar animaciones
    public Image cruz;           // Ret�cula en el UI (asignada desde el Canvas)
    public GameObject proyectil; // Prefab del proyectil
    public Transform spawn;      // Punto de aparici�n del proyectil
    public Camera playerCamera;  // C�mara del jugador
    public float rate = 0.5f;    // Cadencia de disparo
    private float shotRate;      // Temporizador para controlar los disparos
    public float damage = 20f;   // Da�o del arma
    public LayerMask aimLayerMask; // Capas con las que debe colisionar el raycast
    public Transform cameraAimPoint; // Punto en el rifle hacia donde la c�mara debe apuntar

    void Start()
    {

        //// Ocultar ret�cula al inicio
        if (cruz != null)
        {
            cruz.enabled = false; // La ret�cula comienza oculta
        }
        else
        {
            Debug.LogError("No se ha asignado la ret�cula (cruz) en el Inspector.");
        }
    }

    public void Update()
    {
        if (cameraAimPoint != null && playerCamera != null)
        {
            // Ajuste de la posici�n y rotaci�n de la c�mara
            Vector3 offset = new Vector3(0, 0.3f, -0.5f); // Ajuste para evitar colisiones con objetos cercanos
            playerCamera.transform.position = cameraAimPoint.position + cameraAimPoint.forward * offset.z + cameraAimPoint.up * offset.y;

            Quaternion targetRotation = cameraAimPoint.rotation;
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, 0f);
            playerCamera.transform.rotation = targetRotation;
        }

        // Control del apuntado (Fire2)
        if (Input.GetButton("Fire2")) // Bot�n derecho para apuntar
        {

            // Mostrar la ret�cula
            if (cruz != null)
            {
                cruz.enabled = true;
                cruz.rectTransform.sizeDelta = new Vector2(32, 32); // Tama�o fijo de la ret�cula
            }

            // Reducir FOV para zoom
            if (playerCamera != null)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 45f, Time.deltaTime * 5f); // Reducir FOV
            }
        }
        else
        {
            // Ocultar la ret�cula
            if (cruz != null)
            {
                cruz.enabled = false;
            }

            // Restaurar FOV al valor normal
            if (playerCamera != null)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 60f, Time.deltaTime * 5f);
            }
        }
    }

}


