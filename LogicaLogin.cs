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
    public TMP_InputField contraseña;
    public TMP_InputField usuarioIntroducido; // Campo de texto para el usuario
    public Button enviar; // Botón de enviar

    // Slider para la barra de vida
    public Slider barraDeVida;
    public float vidaActual;
    public float vidaMaxima = 100f;

    private string connectionString;
    private MySqlConnection Conexion;
    private MySqlCommand comando;

    void Start()
    {
        Syst = EventSystem.current;

        usuario.Select();

        if (contraseña != null)
        {
            contraseña.contentType = TMP_InputField.ContentType.Password;
            contraseña.ForceLabelUpdate(); // Asegúrate de que la configuración se aplique de inmediato
        }

        // Configurar la barra de vida
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaActual; // Establecer el valor inicial
        }

        // Agregar listeners a los botones
        borrar.onClick.AddListener(LimpiarCampos);
        enviar.onClick.AddListener(ComprobarDatos);
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

    // Método para actualizar la barra de vida
    public void ActualizarVida(float nuevaVida)
    {
        vidaActual = Mathf.Clamp(nuevaVida, 0, vidaMaxima); // Limitar el valor entre 0 y vida máxima
        if (barraDeVida != null)
        {
            barraDeVida.value = vidaActual;
        }
        Debug.Log($"Vida actualizada: {vidaActual}");
    }

    public void ComprobarDatos()
    {
        try
        {
            // Establecer la conexión a la base de datos
            connectionString = "Server=sql8.freesqldatabase.com;Database=sql8773958;User=sql8773958;Password=Cs3e4bAvrl;Charset=utf8;";
            using (MySqlConnection Conexion = new MySqlConnection(connectionString))
            {
                Conexion.Open();

                Debug.Log("Conexión a la base de datos establecida.");

                // Consulta para verificar usuario y contraseña
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario=@usuario AND Contraseña=@contraseña";
                using (MySqlCommand comando = new MySqlCommand(query, Conexion))
                {
                    // Usamos parámetros para evitar inyecciones SQL
                    comando.Parameters.AddWithValue("@usuario", usuarioIntroducido.text);
                    comando.Parameters.AddWithValue("@contraseña", contraseña.text);

                    // Ejecutamos la consulta
                    int count = Convert.ToInt32(comando.ExecuteScalar());

                    if (count > 0)
                    {
                        // Mostrar MessageBox de éxito
                        Debug.Log("¡Inicio de sesión exitoso!");

                        // Simular daño para probar la barra de vida
                        ActualizarVida(vidaActual - 20f); // Por ejemplo, restamos 20 puntos de vida

                        // Cargar nueva escena
                        SceneManager.LoadScene("Scenes/BarraCarga"); // Cambia el nombre de la escena según corresponda
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        Debug.Log("Usuario o contraseña incorrectos.");
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
