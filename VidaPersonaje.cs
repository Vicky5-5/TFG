using UnityEngine;
using UnityEngine.UI;

public class VidaPersonaje : MonoBehaviour
{
    public float maxHealth = 100f; // Vida m�xima
    public float currentHealth;   // Vida actual
    public Slider healthSlider;   // Barra de vida en el UI. Parte de Nati Natasha :3

    void Start()
    {
        // Inicializar vida
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        // Reducir salud
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Actualizar barra de vida
        }

        Debug.Log($"Jugador recibi� {damage} de da�o. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        // Incrementar salud
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Actualizar barra de vida
        }

        Debug.Log($"Jugador se cur� {amount} puntos. Vida actual: {currentHealth}");
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"El jugador ha tocado: {other.name}");

        if (other.CompareTag("Enemy")) // Verifica si es un enemigo
        {
            Debug.Log("El jugador fue golpeado por un enemigo.");
            TakeDamage(10f); // Aplicar 10 de da�o (ajusta seg�n sea necesario)
        }
    }
    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        Destroy(gameObject);
        // Aqu� puedes a�adir l�gica para reiniciar el nivel o mostrar un men� de muerte
    }
}
