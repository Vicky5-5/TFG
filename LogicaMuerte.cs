using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicaMuerte : MonoBehaviour 
{
    EventSystem Syst;
    public Button Reaparecer;
    public Button VolverAlInicio;
    private bool N1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Syst = EventSystem.current;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        if (Caida.nivel1 == true || VidaPersonaje.nivel1 == true)
        {
            N1 = true;
        }

        Reaparecer.onClick.AddListener(Revivir);
        VolverAlInicio.onClick.AddListener(VueltaMenu);
    }


    private void Revivir()
{
        if (N1 == true)
        {
            SceneManager.LoadScene("Scenes/Nivel1");
        } else
        {
            SceneManager.LoadScene("Scenes/Nivel2");
        }
}

private void VueltaMenu()
{
    SceneManager.LoadScene("Scenes/Menu"); // Carga el menú principal
}
}
