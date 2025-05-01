using UnityEngine;
using UnityEngine.SceneManagement;

public class Caida : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Player"))
            {
                // Opción 1: destruir al jugador
                Destroy(other.gameObject);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        
    }
}
