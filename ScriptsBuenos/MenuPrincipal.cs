using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MenuPrincipal : MonoBehaviour
{
    EventSystem Syst;
    public Button Tutorial; 
    public Button Nivel1;
    public Button Nivel2; 
    public Button CerrarSesion;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Syst = EventSystem.current;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;        

        // Agregar listeners a los botones
        Tutorial.onClick.AddListener(InicioTutorial);
        Nivel1.onClick.AddListener(InicioNivel1);
        Nivel2.onClick.AddListener(InicioNivel2);
        CerrarSesion.onClick.AddListener(Login);

    }

    void InicioTutorial()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1; // Restablecer el tiempo por si estaba en pausa
        }

        SceneManager.LoadScene("Scenes/Tuto"); // Cambia el nombre de la escena según corresponda

    }

    void InicioNivel1()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1; // Restablecer el tiempo por si estaba en pausa
        }
        
        SceneManager.LoadScene("Scenes/Nivel1"); // Cambia el nombre de la escena según corresponda

    }

    void InicioNivel2()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1; // Restablecer el tiempo por si estaba en pausa
        }
        SceneManager.LoadScene("Scenes/Nivel2"); // Cambia el nombre de la escena según corresponda

    }

    void Login()
    {
        SceneManager.LoadScene("Scenes/Login"); // Cambia el nombre de la escena según corresponda
    }
}
