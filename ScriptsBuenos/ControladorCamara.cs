using UnityEngine;

public class ControladorCamara : MonoBehaviour
{
    [Header("Configuración General")]
    public float smoothSpeed = 15f; // Velocidad de transición suave
    public Vector3 offset; // Offset adicional para ajustar la posición de la cámara

    [Header("Spawns de Cámara")]
    public Transform defaultSpawn; // Posición predeterminada de la cámara
    public Transform currentCameraSpawn; // Spawn actual de la cámara (se ajustará dinámicamente)

    private void Start()
    {
        // Configurar el spawn inicial como el predeterminado
        if (defaultSpawn != null)
        {
            currentCameraSpawn = defaultSpawn;
            UpdateCameraPositionInstant(); // Posicionar instantáneamente al inicio
        }
        else
        {
            Debug.LogError("El spawn predeterminado no está configurado.");
        }
    }

    private void Update()
    {
        if (currentCameraSpawn != null)
        {
            // Transición suave de posición
            Vector3 desiredPosition = currentCameraSpawn.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
            transform.position = smoothedPosition;

            // Transición suave de rotación
            Quaternion desiredRotation = currentCameraSpawn.rotation;
            Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * smoothSpeed);
            transform.rotation = smoothedRotation;
        }
        else
        {
            Debug.LogError("El spawn actual de la cámara es nulo.");
        }
    }

    /// <summary>
    /// Cambiar dinámicamente el spawn de la cámara.
    /// </summary>
    /// <param name="newSpawn">El Transform del nuevo punto de spawn de la cámara.</param>
    public void AdjustCameraForWeapon(Transform newSpawn)
    {
        if (newSpawn != null)
        {
            currentCameraSpawn = newSpawn; // Actualizar el spawn actual
            Debug.Log($"Cambiando spawn de cámara a: {newSpawn.name}");
        }
        else
        {
            Debug.LogError("El nuevo spawn de la cámara es nulo.");
        }
    }

    /// <summary>
    /// Actualiza la posición de la cámara instantáneamente sin transición suave.
    /// </summary>
    public void UpdateCameraPositionInstant()
    {
        if (currentCameraSpawn != null)
        {
            transform.position = currentCameraSpawn.position + offset;
            transform.rotation = currentCameraSpawn.rotation;
            Debug.Log($"Cámara actualizada instantáneamente al spawn: {currentCameraSpawn.name}");
        }
        else
        {
            Debug.LogError("El spawn actual de la cámara es nulo.");
        }
    }

    /// <summary>
    /// Restablece la cámara a su posición predeterminada.
    /// </summary>
    public void ResetToDefaultView()
    {
        if (defaultSpawn != null)
        {
            AdjustCameraForWeapon(defaultSpawn);
            Debug.Log("Cámara restablecida al spawn predeterminado.");
        }
        else
        {
            Debug.LogError("El spawn predeterminado no está configurado.");
        }
    }
}
