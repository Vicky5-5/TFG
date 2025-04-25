using UnityEngine;

public class Trampilla : MonoBehaviour
{
    public Transform puntoDeRotacion; // GameObject vacío como pivote
    public float anguloCerrado = 0f; // Ángulo de cierre
    public float anguloAbierto = -90f; // Ángulo de apertura
    public float velocidad = 2f; // Velocidad de movimiento
    private bool abrir = false; // Control de apertura/cierre

    void Update()
    {
        // Determinar el ángulo objetivo basado en el estado de la trampilla
        float anguloObjetivo = abrir ? anguloAbierto : anguloCerrado;

        // Rotar suavemente el punto de rotación (eje Z)
        Vector3 rotacionActual = puntoDeRotacion.localEulerAngles;
        float nuevoAnguloZ = Mathf.LerpAngle(rotacionActual.z, anguloObjetivo, velocidad * Time.deltaTime);
        puntoDeRotacion.localEulerAngles = new Vector3(rotacionActual.x, rotacionActual.y, nuevoAnguloZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Abrir la trampilla al detectar al jugador
        if (other.CompareTag("Player"))
        {
            abrir = true;
            Debug.Log("Jugador detectado, abriendo trampilla.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cerrar la trampilla cuando el jugador salga
        if (other.CompareTag("Player"))
        {
            abrir = false;
            Debug.Log("Jugador salió, cerrando trampilla.");
        }
    }
}
