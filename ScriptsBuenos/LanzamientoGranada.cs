using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzamientoGranada : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cam;
    public Transform puntoAtaque;
    public GameObject objetoLanzar;

    [Header("Opciones")]
    public int totalLanzamientos = 3;
    public float enfriamientoLanzamiento = 5f;

    [Header("Lanzamiento")]
    public float fuerzaLanazamiento = 10f;
    public float fuerzaArriba = 5f;

    private bool preparado = true;
   
    private void Update()
    {
        // Lanza la granada solo si presiona "3" y hay disponibles
        if (Input.GetKeyDown(KeyCode.Alpha3) && preparado && totalLanzamientos > 0)
        {
            Lanzar();
        }
    }

    private void Lanzar()
    {
        preparado = false;

        // Desactivar pistola, rifle y cuchillo antes de lanzar la granada
        GestorArmas.armaActual.SetActive(false);


        GameObject projectile = Instantiate(objetoLanzar, puntoAtaque.position, cam.rotation);

        CaracterGranada granadaScript = projectile.GetComponent<CaracterGranada>();
        if (granadaScript != null)
        {
            granadaScript.TemporizadorExplosion();
        }
        else
        {
            Debug.LogError("La granada instanciada no tiene el script CaracterGranada asignado.");
        }

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            Vector3 forceDirection = cam.transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
            {
                forceDirection = (hit.point - puntoAtaque.position).normalized;
            }

            Vector3 forceToAdd = forceDirection * fuerzaLanazamiento + transform.up * fuerzaArriba;
            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("El objeto lanzado no tiene un Rigidbody.");
        }

        totalLanzamientos--;
        Invoke(nameof(ResetThrow), enfriamientoLanzamiento);
    }



    private void ResetThrow()
    {
        preparado = true;

        GestorArmas.armaActual.SetActive(true);
    }
}
