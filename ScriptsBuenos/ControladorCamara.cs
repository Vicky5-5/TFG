using UnityEngine;
using System.Collections;

public class ControladorCamara : MonoBehaviour
{
    [Header("Configuración General")]
    public float smoothSpeed = 15f;
    public Vector3 offset;

    [Header("Spawns de Cámara")]
    public Transform defaultSpawn;
    public Transform currentCameraSpawn;

    private bool retrocesoActivo = false;

    private void Start()
    {
        if (defaultSpawn != null)
        {
            currentCameraSpawn = defaultSpawn;
            UpdateCameraPositionInstant();
        }
        else
        {
            Debug.LogError("El spawn predeterminado no está configurado.");
        }
    }

    private void LateUpdate()
    {
        if (currentCameraSpawn != null && !retrocesoActivo)
        {
            Vector3 targetPosition = currentCameraSpawn.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

            Quaternion rotacionObjetivo = Quaternion.Euler(0f, currentCameraSpawn.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotacionObjetivo, Time.deltaTime * smoothSpeed);
        }
    }

    public void AplicarRetroceso(float intensidad)
    {
        if (!retrocesoActivo)
        {
            StartCoroutine(RetrocesoSuave(intensidad));
        }
    }

    private IEnumerator RetrocesoSuave(float intensidad)
    {
        retrocesoActivo = true;

        Vector3 originalPosition = transform.position;
        Vector3 retrocesoPosition = originalPosition - transform.forward * intensidad;

        float duracion = 0.1f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.position = Vector3.Lerp(originalPosition, retrocesoPosition, tiempo / duracion);
            yield return null;
        }

        tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.position = Vector3.Lerp(retrocesoPosition, originalPosition, tiempo / duracion);
            yield return null;
        }

        retrocesoActivo = false;
    }

    public void AdjustCameraForWeapon(Transform newSpawn)
    {
        if (newSpawn != null)
        {
            currentCameraSpawn = newSpawn;
        }
        else
        {
            Debug.LogError("El nuevo spawn de la cámara es nulo.");
        }
    }

    public void UpdateCameraPositionInstant()
    {
        if (currentCameraSpawn != null)
        {
            transform.position = currentCameraSpawn.position + offset;
            transform.rotation = Quaternion.Euler(0f, currentCameraSpawn.eulerAngles.y, 0f);
        }
    }

    public void ResetToDefaultView()
    {
        if (defaultSpawn != null)
        {
            AdjustCameraForWeapon(defaultSpawn);
        }
        else
        {
            Debug.LogError("El spawn predeterminado no está configurado.");
        }
    }
}
