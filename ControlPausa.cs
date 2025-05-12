using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    EventSystem Syst;
    public GameObject EmergentePausa;
    public Button Reanudar;
    public Button VolverAlMenu;
    public Button Salir;
    private bool Pausado = false;
    public GameObject jugador;
    void Start()
    {
        Syst = EventSystem.current;       

        // Asegurar que el panel está desactivado al inicio
        EmergentePausa.SetActive(false);

        // Asignar funciones a los botones
        Reanudar.onClick.AddListener(PanelPausa);
        VolverAlMenu.onClick.AddListener(VolverMenu);
        Salir.onClick.AddListener(CerrarJuego);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; // Desbloquear el cursor
            Cursor.visible = true; // Hacerlo visible
            PanelPausa();
        }
    }

    public void PanelPausa()
    {
        Pausado = !Pausado;
        EmergentePausa.SetActive(Pausado);
        if (Pausado)
        {
            Time.timeScale = 0; // Pausar el juego
            
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Desbloquear el cursor
            Cursor.visible = false; // Hacerlo invisible
            Time.timeScale = 1; // Reanudar el juego
        }
    }

    public void VolverMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
         Destroy(jugador); // Destruir el personaje sin afectar la cámara    
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }
}
