using UnityEngine;
using UnityEngine.SceneManagement;

public class Caida : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Player"))
            {
                // Opci�n 1: destruir al jugador
                Destroy(other.gameObject);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        
    }
}
