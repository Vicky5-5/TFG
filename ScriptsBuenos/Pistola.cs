using UnityEngine;

public class Pistola : MonoBehaviour
{
    public GameObject proyectil;
    public Transform spawn;
    public float damage = 20f;
    public float cadencia = 0.3f;
    private float cadencia2;
    public Animator animator;
    public Camera jugadorCamera;
    public float rango = 300f;
    public float fuerza = 50f;
    public float retrocesoCamaraIntensidad = 0.05f; // Intensidad del retroceso

    public AudioClip sonidoDisparo;
    private AudioSource audioSource;

    public GameObject rifle;
    public GameObject knife;
    public GameObject pistola;

    void Start()
    {
        animator = GameObject.Find("Jugador").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en la pistola.");
        }

        rifle.SetActive(false);
        knife.SetActive(false);

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && Time.time >= cadencia2))
            {
                cadencia2 = Time.time + cadencia; // Asignar el tiempo de espera correcto antes del siguiente disparo
                Disparar();
            }
            else
            {
                animator.SetBool("DispararPistola", false);
            }
        }
        else
        {
            animator.SetBool("DispararPistola", false);
        }
    }

    public void Disparar()
    {
        if (animator != null)
        {
            animator.SetBool("DispararPistola", true);

            if (sonidoDisparo != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoDisparo);
            }

            Ray ray = jugadorCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint = Physics.Raycast(ray, out hit, rango) ? hit.point : ray.GetPoint(rango);

            Vector3 direction = (targetPoint - spawn.position).normalized;
            GameObject bala = Instantiate(proyectil, spawn.position, Quaternion.LookRotation(direction));

            Rigidbody balaRigidbody = bala.GetComponent<Rigidbody>();
            if (balaRigidbody != null)
            {
                balaRigidbody.AddForce(direction * fuerza, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("El proyectil no tiene un Rigidbody asignado.");
            }

            Bala balaScript = bala.GetComponent<Bala>();
            if (balaScript != null)
            {
                balaScript.SetDamage(damage);
            }
            else
            {
                Debug.LogError("El script Bala no está asignado al prefab de la bala.");
            }

            Debug.Log("¡Disparo realizado!");

            // **Aplicar el retroceso de la cámara**
            Object.FindFirstObjectByType<ControladorCamara>().AplicarRetroceso(retrocesoCamaraIntensidad);
        }
        else
        {
            Debug.LogError("El Animator no está asignado.");
        }
    }
}