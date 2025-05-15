using UnityEngine;
using UnityEngine.UI;

public class Apuntado : MonoBehaviour
{
    public Animator animator;    // Animator del personaje para controlar animaciones
    public Image cruz;           // Retícula en el UI (asignada desde el Canvas)
    public GameObject proyectil; // Prefab del proyectil
    public Transform spawn;      // Punto de aparición del proyectil
    public Camera jugadorCamera;  // Cámara del jugador

    void Start()
    {

        //// Ocultar retícula al inicio
        if (cruz != null)
        {
            cruz.enabled = false; // La retícula comienza oculta
        }
        else
        {
            Debug.LogError("No se ha asignado la retícula (cruz) en el Inspector.");
        }
    }

    public void Update()
    {
        

        // Control del apuntado (Fire2)
        if (Input.GetButton("Fire2")) // Botón derecho para apuntar
        {

            // Mostrar la retícula
            if (cruz != null)
            {
                cruz.enabled = true;
                cruz.rectTransform.sizeDelta = new Vector2(32, 32); // Tamaño fijo de la retícula
            }

            // Reducir FOV para zoom
            if (jugadorCamera != null)
            {
                jugadorCamera.fieldOfView = Mathf.Lerp(jugadorCamera.fieldOfView, 45f, Time.deltaTime * 5f); // Reducir FOV
            }
        }
        else
        {
            // Ocultar la retícula
            if (cruz != null)
            {
                cruz.enabled = false;
            }

            // Restaurar FOV al valor normal
            if (jugadorCamera != null)
            {
                jugadorCamera.fieldOfView = Mathf.Lerp(jugadorCamera.fieldOfView, 60f, Time.deltaTime * 5f);
            }
        }
    }

}
