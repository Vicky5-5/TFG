using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Caida : MonoBehaviour
{
    public static bool nivel1;
    public static bool nivel2;


    void OnTriggerEnter(Collider other)
    {

        if (SceneManager.GetActiveScene().name.Equals("Nivel1"))
        {
            nivel1 = true;
            nivel2 = false;
        }
        else if (SceneManager.GetActiveScene().name.Equals("Nivel2"))
        {
            nivel2 = true;
            nivel1 = false;
        } else
        {
            nivel2 = false;
            nivel1 = false;
        }

        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Scenes/Muerte");
            Destroy(other.gameObject);
        }
    }
}
