using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{
    EventSystem Syst;
    public Text Explicacion; // Referencia al texto en la UI
    public GameObject Inicio;
    public GameObject Reticula;
    public Button Continuar;
    public GameObject jugador;

    private void Start()
    {

        Syst = EventSystem.current;

        Cursor.lockState = CursorLockMode.None; // Desbloquear el cursor
        Cursor.visible = true; // Hacerlo visible

        Reticula.SetActive(false);
        Inicio.SetActive(true);

        Continuar.onClick.AddListener(ContinuarTutorial);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Continuar.onClick.Invoke();
        }
    }
        void ContinuarTutorial()
    {
        Reticula.SetActive(true);
        Inicio.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked; // Desbloquear el cursor
        Cursor.visible = false; // Hacerlo invisible
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que entra es el jugador
        {
            switch (gameObject.name) // Identifica el Mesh Collider por su nombre
            {
                case "Point1":
                    Explicacion.text = "Hay una roca en el camino. Corre con la tecla Shift y saltala con el Espacio";
                    break;
                case "Point2":
                    Explicacion.text = "Oh no, un enemigo delante. Apunta con el click derecho y disparale con el izquierdo";
                    break;
                case "Point3":
                    Explicacion.text = "Ya conoces la pistola, pero si prefieres el rifle pulsa 2. Si prefieres un cuchillo pulsa 4 y para recuperar la pistola pulsa 1";
                    break;
                case "Point4":
                    Explicacion.text = "Hay dos momias en la entrada. Lanzales una granada pulsando 3. Cuando tengas via libre, entra a la piramide";
                    break;

                case "Piramide":

                    if (GameObject.FindGameObjectsWithTag("Enemigo").Length == 0)
                    {
                        Destroy(jugador);
                        SceneManager.LoadScene("Scenes/Menu");
                        
                    } else
                    {
                        Explicacion.text = "Elimina a todos los enemigos antes de entrar";
                    }

                        break;
            }
        }
    }      
}

