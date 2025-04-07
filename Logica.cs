using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System; // Para manejar escenas

public class Logica : MonoBehaviour
{
    EventSystem Syst;
    public Selectable usuario;
    public Button borrar;
    public TMP_InputField[] inputFields; // Usamos TMP_InputField para TextMeshPro
    public TMP_InputField contrase�a;
    public TMP_InputField usuarioIntroducido; // Campo de texto para el usuario
    public Button enviar; // Bot�n de enviar

    public GameObject messageBoxPanel; // Panel del MessageBox
    public TextMeshProUGUI messageText; // Texto del MessageBox

    private string connectionString;
    private MySqlConnection Conexion;
    private MySqlCommand comando;

    void Start()
    {
        Syst = EventSystem.current;

        usuario.Select();

        if (contrase�a != null)
        {
            contrase�a.contentType = TMP_InputField.ContentType.Password;
            contrase�a.ForceLabelUpdate(); // Aseg�rate de que la configuraci�n se aplique de inmediato
        }

        // Agregar listeners a los botones
        borrar.onClick.AddListener(LimpiarCampos);
        enviar.onClick.AddListener(ComprobarDatos);

        // Aseg�rate de que el MessageBox est� inicialmente inactivo
        if (messageBoxPanel != null)
            messageBoxPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            Selectable anterior = Syst.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (anterior != null)
            {
                anterior.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable siguiente = Syst.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (siguiente != null)
            {
                siguiente.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            borrar.onClick.Invoke();
        }
    }

    void LimpiarCampos()
    {
        foreach (var inputField in inputFields)
        {
            inputField.text = ""; // Limpia el contenido del campo
        }
    }

    public void ComprobarDatos()
    {
        try
        {
            // Establecer la conexi�n a la base de datos
            //connectionString = "Server=bmkbnmjuonepujmxpt8k-mysql.services.clever-cloud.com;Database=bmkbnmjuonepujmxpt8k;User=ugroq2fn9qpuagqi;Password=yeEc6IDn7ftrLznrBDlS;";
            connectionString = "Server=192.168.1.133;Database=bd_prueba;User=Unity;Password=TFG2025;";

            using (MySqlConnection Conexion = new MySqlConnection(connectionString))
            {
                Conexion.Open();

                Debug.Log("Conexi�n a la base de datos establecida.");

                // Consulta para verificar usuario y contrase�a
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario=@usuario AND Contrase�a=@contrase�a";
                using (MySqlCommand comando = new MySqlCommand(query, Conexion))
                {
                    // Usamos par�metros para evitar inyecciones SQL
                    comando.Parameters.AddWithValue("@usuario", usuarioIntroducido.text);
                    comando.Parameters.AddWithValue("@contrase�a", contrase�a.text);

                    // Ejecutamos la consulta
                    int count = Convert.ToInt32(comando.ExecuteScalar());

                    if (count > 0)
                    {
                        // Mostrar MessageBox de �xito
                        Debug.Log("�Inicio de sesi�n exitoso!");

                        // Cargar nueva escena
                        // SceneManager.LoadScene("BarraCarga"); // Cambia "BarraCarga" por el nombre de tu escena
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        Debug.Log("Usuario o contrase�a incorrectos.");
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError($"Error al conectar con la base de datos: {ex.Message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error general: {ex.Message}");
        }
    }
}



