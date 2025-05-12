using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions; 

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

    private string connectionString;
    private MySqlConnection Conexion;
    private MySqlCommand comando;

    void Start()
    {
        Syst = EventSystem.current;

        CamposVacios.SetActive(false);
        UsuarioIncorrecto.SetActive(false);

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
    }


    public void ComprobarDatos()
    {
        if (string.IsNullOrWhiteSpace(usuarioIntroducido.text) || string.IsNullOrWhiteSpace(contrasenia.text))
        {

            CamposVacios.SetActive(true);

        }
        else
        {
                // Establecer la conexion a la base de datos
                connectionString = "Server=172.18.35.6;Database=bd_prueba;User=TFG;Password=09052025;Charset=utf8;";
                using (MySqlConnection Conexion = new MySqlConnection(connectionString))
                {
                    Conexion.Open();

                    Debug.Log("Conexi�n a la base de datos establecida.");

                    // Consulta para verificar usuario y contrase�a
                    string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario=@usuario AND Contrasenia=@Contrasenia";
                    using (MySqlCommand comando = new MySqlCommand(query, Conexion))
                    {
                        // Usamos par�metros para evitar inyecciones SQL
                        comando.Parameters.AddWithValue("@usuario", usuarioIntroducido.text);
                        comando.Parameters.AddWithValue("@Contrasenia", contrasenia.text);

                        // Ejecutamos la consulta
                        int count = Convert.ToInt32(comando.ExecuteScalar());

                        if (count > 0)
                        {
                            // Cargar nueva escena
                            SceneManager.LoadScene("Scenes/BarraCarga"); // Cambia el nombre de la escena segun corresponda
                        }
                        else
                        {
                            // Mostrar mensaje de error
                            UsuarioIncorrecto.SetActive(true);
                        }
                    }
                }
            }
        }
    }
