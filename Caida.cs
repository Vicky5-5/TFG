using UnityEngine;
using UnityEngine.SceneManagement;

public class Caida : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Player"))
            {
                // Destruye el player
                Destroy(other.gameObject);
                //Reinicia la escena
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        
    }
}
