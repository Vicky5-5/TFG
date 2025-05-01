using UnityEngine;

public class TrampaPinchos : MonoBehaviour
{
    public Transform pinchos; // El objeto que se mueve (el mesh de pinchos)
    public float arriba = 1.5f; // Posición de los pinchos cuando están arriba
    public float abajo = 0f; // Posición de los pinchos cuando están abajo
    public float speed = 2f; // Velocidad de movimiento de los pinchos
    public float interval = 4f; // Intervalo entre movimientos

    private bool isUp = false; // Estado de los pinchos (arriba o abajo)
    private float timer = 0f; // Temporizador para alternar estados
    private Vector3 startPos; // Posición inicial de los pinchos

    public GameObject player;
    void Start()
    {
        // Guardar la posición inicial de los pinchos
        startPos = pinchos.localPosition;
    }

    void Update()
    {
        // Temporizador para alternar la posición de los pinchos
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            isUp = !isUp; // Alternar entre arriba y abajo
            timer = 0f; // Reiniciar el temporizador
        }

        // Mover los pinchos suavemente hacia la posición objetivo
        Vector3 target = startPos + Vector3.up * (isUp ? arriba : abajo);
        pinchos.localPosition = Vector3.MoveTowards(pinchos.localPosition, target, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar colisión con el jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Colisión con trampa!");

            // Registrar la muerte del jugador en GameManager
            if (Muerte.instance != null)
            {
                Muerte.instance.PlayerDied(player);
            }

        }
    }

}
