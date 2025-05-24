using UnityEngine;
using UnityEngine.UI;

public class Apuntado : MonoBehaviour
{
    public Animator animator;    // Animator del personaje para controlar animaciones
    public Image cruz;           // Ret�cula en el UI (asignada desde el Canvas)
    public GameObject proyectil; // Prefab del proyectil
    public Transform spawn;      // Punto de aparici�n del proyectil
    public Camera jugadorCamera;  // C�mara del jugador

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
            if (jugadorCamera != null)
            {
                jugadorCamera.fieldOfView = Mathf.Lerp(jugadorCamera.fieldOfView, 45f, Time.deltaTime * 5f); // Reducir FOV
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
            if (jugadorCamera != null)
            {
                jugadorCamera.fieldOfView = Mathf.Lerp(jugadorCamera.fieldOfView, 60f, Time.deltaTime * 5f);
            }
        }
    }

}