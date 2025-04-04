using UnityEngine;

public class Pistola : MonoBehaviour, IArma
{
    [Header("Configuraci�n de la Pistola")]
    public GameObject proyectil; // Prefab de la bala
    public Transform spawn; // Punto donde aparece la bala
    public Transform handTransform; // Transform de la mano del personaje
    public float fuerza = 100f; // Fuerza de disparo
    public float damage = 10f; // Da�o de la pistola
    public float rate2 = 1f; // 1 bala por segundo
    private float shotRate2; // Temporizador para controlar los disparos
    public float speed = 2f;

    public Animator animator; // Animator para controlar las animaciones
    public LayerMask aimLayerMask; // Capas con las que debe colisionar el raycast
    public float range = 300f; // Alcance m�ximo del disparo
    public GameObject pistola; // Pistola para cambiar m�s tarde
    public GameObject knife; // Cuchillo
    public GameObject rifle; // Cuchillo
    void Start()
    {
        // Anclar la pistola a la mano
        if (handTransform != null && gameObject != null)
        {
            pistola.transform.SetParent(handTransform);
            pistola.transform.localPosition = new Vector3(-0.044f, 0.167f, 0.055f); // Ajusta seg�n el modelo
            pistola.transform.localRotation = Quaternion.Euler(35.5f, -26.7f, 90.7f); // Ajusta seg�n el modelo

            Debug.Log("Pistola anclada correctamente a la mano.");
        }

        // Activar la pistola al iniciar
        pistola.SetActive(true);
        knife.SetActive(false);
        rifle.SetActive(false);
        // Obtener el Animator
        animator = GameObject.Find("Swat").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontr� un Animator en el personaje.");
        }
    }

    void Update()
    {
        // L�gica de caminar (animaci�n)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (animator != null)
            {
                animator.SetBool("AndarPistola", true); // Activar animaci�n de caminar
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("AndarPistola", false); // Desactivar animaci�n de caminar
            }
        }

        // L�gica de disparo
        if (Input.GetButton("Fire2")) // Bot�n derecho para apuntar
        {
            if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && Time.time >= shotRate2))
            {
                shotRate2 = Time.time + rate2; // Actualizar el temporizador
                Shoot(); // Llamar al m�todo de disparo
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("DispararPistola", false); // Desactivar la animaci�n de disparo
            }
        }
    }

    public void Shoot()
    {
        if (animator != null)
        {
            animator.SetBool("DispararPistola", true); // Activar animaci�n de disparo
        }

        // Raycast desde el centro de la c�mara
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Usar la c�mara principal
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, range, aimLayerMask))
        {
            targetPoint = hit.point; // Punto de impacto del raycast
        }
        else
        {
            targetPoint = ray.GetPoint(range); // Punto lejano si no impacta nada
        }

        // Crear la bala
        Vector3 direction = (targetPoint - spawn.position).normalized;
        GameObject bala = Instantiate(proyectil, spawn.position, Quaternion.LookRotation(direction));

        // Configurar el da�o de la bala
        Bala1 balaScript = bala.GetComponent<Bala1>();
        if (balaScript != null)
        {
            balaScript.SetDamage(damage);
            Debug.Log("Da�o configurado en la bala: " + damage);
        }
        else
        {
            Debug.LogError("El script Bala1 no est� asignado al prefab de la bala.");
        }

        Debug.Log("�Disparo realizado!");
    }
}
