using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

public class Logica : MonoBehaviour
{
    EventSystem Syst;
    public GameObject CamposVacios;
    public Button ReintentarCampos;
    public Button ReintentarUsuario;
    public GameObject UsuarioIncorrecto;
    public Selectable usuario;
    public Button borrar;
    public TMP_InputField[] inputFields; 
    public TMP_InputField contrasenia;
    public TMP_InputField usuarioIntroducido; 
    public Button enviar;
    public GameObject ServidorNoDisponible;
    public Button ReintentarServidor;


    private string servidor = "172.18.35.6";

    private string connectionString;
    private MySqlConnection Conexion;
    private MySqlCommand comando;


    void Start()
    {
        Syst = EventSystem.current;

        CamposVacios.SetActive(false);
        UsuarioIncorrecto.SetActive(false);
        ServidorNoDisponible.SetActive(false);

        usuario.Select();


        if (contrasenia.text != null)
        {
            contrasenia.contentType = TMP_InputField.ContentType.Password;
            contrasenia.ForceLabelUpdate(); // Fuerza el formato al instante
        }

        // Detecta en tiempo real lo que introduce el usuario y se lo transmite a la funcion ValidarEntrada
        usuarioIntroducido.onValueChanged.AddListener(delegate { ValidarEntrada(usuarioIntroducido); });
        contrasenia.onValueChanged.AddListener(delegate { ValidarEntrada(contrasenia); });



        // Agregamos funcionalidad a los botones
        borrar.onClick.AddListener(LimpiarCampos);
        enviar.onClick.AddListener(ComprobarDatos);
        ReintentarCampos.onClick.AddListener(VolverLogin);
        ReintentarUsuario.onClick.AddListener(VolverLogin);
        ReintentarServidor.onClick.AddListener(VolverLogin);


    }

    // Función para que solo se puedan escribir letras y números en los campos de texto, con el fin de evitar inyecciones SQL
    void ValidarEntrada(TMP_InputField inputField)
    {
        string CaracteresValidos = @"^[a-zA-Z0-9]*$"; // Expresion regular de letras y números

        if (!Regex.IsMatch(inputField.text, CaracteresValidos))
        {
            inputField.text = Regex.Replace(inputField.text, "[^a-zA-Z0-9]", ""); // No escribe caracteres distintos a legras o numeros
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Selectable anterior = Syst.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (anterior != null)
            {
                anterior.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Selectable siguiente = Syst.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (siguiente != null)
            {
                siguiente.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            enviar.onClick.Invoke();
        }
    }

    void LimpiarCampos()
    {
        foreach (var inputField in inputFields)
        {
            inputField.text = ""; // Limpia el contenido del campo
        }
    }

    void VolverLogin()
    {
        if (CamposVacios.activeSelf.Equals(true))
        {
            CamposVacios.SetActive(false);

        }
        else if (UsuarioIncorrecto.activeSelf.Equals(true))
        {
            UsuarioIncorrecto.SetActive(false);
        }
        else if (ServidorNoDisponible.activeSelf.Equals(true))
        {
            ServidorNoDisponible.SetActive(false);
        }
    }


    bool ServidorDisponible()
    {
        try
        {
            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
            PingReply reply = ping.Send(servidor, 20);
            return reply.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }

    public void ComprobarDatos()
    {
        if (string.IsNullOrWhiteSpace(usuarioIntroducido.text) || string.IsNullOrWhiteSpace(contrasenia.text))
        {
            CamposVacios.SetActive(true);
        }
        else if (!ServidorDisponible()) // Verificamos el estado antes de conectar
        {
            ServidorNoDisponible.SetActive(true);
        }
        else
        {
            // Intentar la conexión
            connectionString = $"Server={servidor};Database=bd_prueba;User=TFG;Password=09052025;Charset=utf8;";
            using (MySqlConnection Conexion = new MySqlConnection(connectionString))
            {
                try
                {
                    Conexion.Open();
                    Debug.Log("Conexión establecida.");

                    string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario=@usuario AND Contrasenia=@Contrasenia";
                    using (MySqlCommand comando = new MySqlCommand(query, Conexion))
                    {
                        comando.Parameters.AddWithValue("@usuario", usuarioIntroducido.text);
                        comando.Parameters.AddWithValue("@Contrasenia", contrasenia.text);

                        int count = Convert.ToInt32(comando.ExecuteScalar());

                        if (count > 0)
                        {
                            SceneManager.LoadScene("Scenes/BarraCarga");
                        }
                        else
                        {
                            UsuarioIncorrecto.SetActive(true);
                        }
                    }
                }
                catch (Exception e)
                {
                    ServidorNoDisponible.SetActive(true);
                    Debug.Log("Error al conectar: " + e.Message);
                }
            }
        }
    }
}

