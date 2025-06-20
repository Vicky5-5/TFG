using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Importar el namespace de TextMeshPro

public class VidaPersonaje : MonoBehaviour
{
    // Variables p�blicas
    public float maxHealth = 100f; // Vida m�xima
    public float currentHealth;   // Vida actual
    public Slider healthSlider;   // Barra de vida en el UI
    public Image fillImage;       // Imagen de relleno (Fill Area)
    public Image vida;       // Imagen de relleno (Fill Area)
    public TextMeshProUGUI healthText; // Texto para mostrar la vida actual con TextMeshPro
    public Animator animator;     // Animador para las animaciones
    public static bool nivel1;
    public static bool nivel2;


    void Start()
    {
        // Inicializar valores de vida
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth; // Barra comienza llena
        }        

        // Actualizar el texto de vida
        UpdateHealthUI();
    }

    public void TakeDamagePlayer(float damage)
    {
        if (damage <= 0)
        {
            Debug.LogWarning("El da�o debe ser mayor que 0.");
            return;
        }

        // Reducir vida instant�neamente
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Limitar entre 0 y vida m�xima

        Debug.Log($"Jugador recibi� {damage} de da�o. Vida actual: {currentHealth}");

        // Actualizar la barra de vida, el color y el texto
        UpdateHealthUI();

        // Verificar si la vida lleg� a 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Actualizar la barra de vida
        }

        vida.fillAmount = currentHealth / maxHealth;

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}"; // Mostrar vida actual como texto
        }
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto.");

        if (animator != null)
        {
            animator.SetTrigger("Muerte"); // Activar animaci�n de muerte
        }
        if (SceneManager.GetActiveScene().name.Equals("Nivel1"))
        {
            nivel1 = true;
            nivel2 = false;
        }
        else if (SceneManager.GetActiveScene().name.Equals("Nivel2"))
        {
            nivel2 = true;
            nivel1 = false;
        }
        else
        {
            nivel2 = false;
            nivel1 = false;
        }

        SceneManager.LoadScene("Scenes/Muerte");
        Destroy(gameObject); // Eliminar al jugador
    }
}
