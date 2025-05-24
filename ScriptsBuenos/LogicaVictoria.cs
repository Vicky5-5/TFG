using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicaVictoria : MonoBehaviour 
{
    EventSystem Syst;
    public Button Salir;
    public Button VolverMenu;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Syst = EventSystem.current;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Salir.onClick.AddListener(CerrarApp);
        VolverMenu.onClick.AddListener(VueltaMenu);
    }


    private void CerrarApp() {
        Application.Quit();
    }

private void VueltaMenu()
{
    SceneManager.LoadScene("Scenes/Menu"); // Carga el menú principal
}
}
