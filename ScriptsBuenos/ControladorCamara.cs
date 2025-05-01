using UnityEngine;

public class ControladorCamara : MonoBehaviour
{
    [Header("Configuraci�n General")]
    public float smoothSpeed = 15f; // Velocidad de transici�n suave
    public Vector3 offset; // Offset adicional para ajustar la posici�n de la c�mara

    [Header("Spawns de C�mara")]
    public Transform defaultSpawn; // Posici�n predeterminada de la c�mara
    public Transform currentCameraSpawn; // Spawn actual de la c�mara (se ajustar� din�micamente)

    private void Start()
    {
        // Configurar el spawn inicial como el predeterminado
        if (defaultSpawn != null)
        {
            currentCameraSpawn = defaultSpawn;
            UpdateCameraPositionInstant(); // Posicionar instant�neamente al inicio
        }
        else
        {
            Debug.LogError("El spawn predeterminado no est� configurado.");
        }
    }

    private void Update()
    {
        if (currentCameraSpawn != null)
        {
            // Transici�n suave de posici�n
            Vector3 desiredPosition = currentCameraSpawn.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
            transform.position = smoothedPosition;

            // Transici�n suave de rotaci�n
            Quaternion desiredRotation = currentCameraSpawn.rotation;
            Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * smoothSpeed);
            transform.rotation = smoothedRotation;
        }
        else
        {
            Debug.LogError("El spawn actual de la c�mara es nulo.");
        }
    }

    /// <summary>
    /// Cambiar din�micamente el spawn de la c�mara.
    /// </summary>
    /// <param name="newSpawn">El Transform del nuevo punto de spawn de la c�mara.</param>
    public void AdjustCameraForWeapon(Transform newSpawn)
    {
        if (newSpawn != null)
        {
            currentCameraSpawn = newSpawn; // Actualizar el spawn actual
            Debug.Log($"Cambiando spawn de c�mara a: {newSpawn.name}");
        }
        else
        {
            Debug.LogError("El nuevo spawn de la c�mara es nulo.");
        }
    }

    /// <summary>
    /// Actualiza la posici�n de la c�mara instant�neamente sin transici�n suave.
    /// </summary>
    public void UpdateCameraPositionInstant()
    {
        if (currentCameraSpawn != null)
        {
            transform.position = currentCameraSpawn.position + offset;
            transform.rotation = currentCameraSpawn.rotation;
            Debug.Log($"C�mara actualizada instant�neamente al spawn: {currentCameraSpawn.name}");
        }
        else
        {
            Debug.LogError("El spawn actual de la c�mara es nulo.");
        }
    }

    /// <summary>
    /// Restablece la c�mara a su posici�n predeterminada.
    /// </summary>
    public void ResetToDefaultView()
    {
        if (defaultSpawn != null)
        {
            AdjustCameraForWeapon(defaultSpawn);
            Debug.Log("C�mara restablecida al spawn predeterminado.");
        }
        else
        {
            Debug.LogError("El spawn predeterminado no est� configurado.");
        }
    }
}
