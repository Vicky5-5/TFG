using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingTutorial : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows = 3;
    public float throwCooldown = 5f;

    [Header("Throwing")]
    public float throwForce = 10f;
    public float throwUpwardForce = 5f;

    private bool readyToThrow = true;

    private void Update()
    {
        // Lanza la granada solo si presiona "3" y hay disponibles
        if (Input.GetKeyDown(KeyCode.Alpha3) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        // Instanciar la granada en el punto de ataque
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // Verificar si la granada tiene el script CaracterGranada
        CaracterGranada granadaScript = projectile.GetComponent<CaracterGranada>();
        if (granadaScript != null)
        {
            granadaScript.StartExplosionTimer();
        }
        else
        {
            Debug.LogError("La granada instanciada no tiene el script CaracterGranada asignado.");
        }

        // Obtener Rigidbody de la granada
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            // Calcular dirección del lanzamiento
            Vector3 forceDirection = cam.transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
            {
                forceDirection = (hit.point - attackPoint.position).normalized;
            }

            // Aplicar fuerza al lanzamiento
            Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("El objeto lanzado no tiene un Rigidbody.");
        }
        

        totalThrows--;
        Invoke(nameof(ResetThrow), throwCooldown);
    }



    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
