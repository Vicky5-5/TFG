using UnityEngine;
using UnityEngine.SceneManagement;

public class Muerte : MonoBehaviour
{
    public static Muerte instance; // Instancia singleton
    public Animator animator;      // Animator asignado al jugador


    private void Awake()
    {
        // Configurar singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject); // Eliminar duplicados
        }
    }

    // Método para registrar la muerte del jugador
    public void PlayerDied(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        movimientoSwat playerController = player.GetComponent<movimientoSwat>();
        Animator anim = player.GetComponent<Animator>();

        if (anim != null)
        {
            Debug.Log("Antes de activar Trigger");
            anim.SetTrigger("Muerte");

            // Detener el movimiento físico (desactivamos la gravedad y la velocidad)
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Congelar el Rigidbody (no solo kinematic, sino todo el movimiento)
            rb.constraints = RigidbodyConstraints.FreezeAll;

            // Desactivar la gravedad para que no caiga
            rb.useGravity = false;

            // Desactivar el control del jugador inmediatamente
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            Debug.Log("Después de activar Trigger");
        }
        else
        {
            Debug.LogError("No se encontró un Animator en el jugador.");
        }

        Debug.Log("Estado del jugador: muerto.");       
    }
}
