using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject jugador;

    private void Update()
    {
        VerificarEnemigos();
    }

    private void VerificarEnemigos()
    {
        int enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        Debug.Log($"Enemigos restantes: {enemigosRestantes}");

        if (enemigosRestantes == 0)
        {
            CambiarNivel();
        }
    }

    private void CambiarNivel()
    {
        if (SceneManager.GetActiveScene().name.Equals("Nivel1"))
        {           
            SceneManager.LoadScene("Scenes/Nivel2");
            Destroy(jugador);
        }
        else if (SceneManager.GetActiveScene().name.Equals("Nivel2")) { 
            
          SceneManager.LoadScene("Scenes/Victoria");
            Destroy(jugador);
        }
    }
}
