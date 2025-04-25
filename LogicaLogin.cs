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

        if (contrase�a != null)
        {
            contrase�a.contentType = TMP_InputField.ContentType.Password;
            contrase�a.ForceLabelUpdate(); // Aseg�rate de que la configuraci�n se aplique de inmediato
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

    // M�todo para actualizar la barra de vida
    public void ActualizarVida(float nuevaVida)
    {
        vidaActual = Mathf.Clamp(nuevaVida, 0, vidaMaxima); // Limitar el valor entre 0 y vida m�xima
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
            // Establecer la conexi�n a la base de datos
            connectionString = "Server=sql8.freesqldatabase.com;Database=sql8773958;User=sql8773958;Password=Cs3e4bAvrl;Charset=utf8;";
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

                        // Simular da�o para probar la barra de vida
                        ActualizarVida(vidaActual - 20f); // Por ejemplo, restamos 20 puntos de vida

                        // Cargar nueva escena
                        SceneManager.LoadScene("Scenes/BarraCarga"); // Cambia el nombre de la escena seg�n corresponda
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
