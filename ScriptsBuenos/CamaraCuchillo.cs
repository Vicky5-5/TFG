using UnityEngine;

public class CamaraCuchillo : MonoBehaviour
{
    public Transform camara2; // ?? Asigna "Camara2" desde el Inspector
    public Transform objetivo; // ?? El jugador que sigue la cámara
    public float smoothSpeed = 10f;
    public Vector3 offset;

    private GameObject cuchillo;

    private void Start()
    {
        cuchillo = GameObject.Find("Cuchillo");

        // ?? Verifica que la referencia `Camara2` existe
        if (camara2 == null)
        {
            Debug.LogError("La referencia a 'Camara2' no está asignada.");
        }
    }

    private void LateUpdate()
    {
        if (cuchillo != null && cuchillo.activeSelf && camara2 != null) // ?? Solo si el cuchillo está activo y `Camara2` asignado
        {
            Vector3 posicionDeseada = objetivo.position + offset;
            camara2.position = Vector3.Lerp(camara2.position, posicionDeseada, Time.deltaTime * smoothSpeed);

            // ?? **Asegurar que la cámara de 'Camara2' mire al frente**
            camara2.rotation = Quaternion.Euler(0f, objetivo.eulerAngles.y, 0f);
        }
    }
}
