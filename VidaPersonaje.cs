using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Importar el namespace de TextMeshPro

public class VidaPersonaje : MonoBehaviour
{
    // Variables públicas
    public float maxHealth = 100f; // Vida máxima
    public float currentHealth;   // Vida actual
    public Slider healthSlider;   // Barra de vida en el UI
    public Image fillImage;       // Imagen de relleno (Fill Area)
    public Gradient healthGradient; // Gradiente de color para la vida
    public TextMeshProUGUI healthText; // Texto para mostrar la vida actual con TextMeshPro
    public Animator animator;     // Animador para las animaciones
    public static bool nivel1;

    void Start()
    {
        // Inicializar valores de vida
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth; // Barra comienza llena
        }

        // Configurar el color inicial del relleno
        if (fillImage != null && healthGradient != null)
        {
            fillImage.color = healthGradient.Evaluate(1f); // Vida completa inicial
        }

        // Actualizar el texto de vida
        UpdateHealthUI();
    }

    public void TakeDamagePlayer(float damage)
    {
        if (damage <= 0)
        {
            Debug.LogWarning("El daño debe ser mayor que 0.");
            return;
        }

        // Reducir vida instantáneamente
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Limitar entre 0 y vida máxima

        Debug.Log($"Jugador recibió {damage} de daño. Vida actual: {currentHealth}");

        // Actualizar la barra de vida, el color y el texto
        UpdateHealthUI();

        // Verificar si la vida llegó a 0
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

        if (fillImage != null && healthGradient != null)
        {
            float normalizedHealth = currentHealth / maxHealth; // Escalar entre 0 y 1
            fillImage.color = healthGradient.Evaluate(normalizedHealth); // Actualizar color según vida
        }

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
            animator.SetTrigger("Muerte"); // Activar animación de muerte
        }

        if (SceneManager.GetActiveScene().name.Equals("Nivel1"))
        {
            nivel1 = true;
        }
        else
        {
            nivel1 = false;
        }

        SceneManager.LoadScene("Scenes/Muerte");
        Destroy(gameObject); // Eliminar al jugador
    }
}
