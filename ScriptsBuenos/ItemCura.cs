using UnityEngine;

public class ItemCura : MonoBehaviour
{
    VidaPersonaje vida;
    Collider col;
    public float cura = 20f;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            VidaPersonaje vida = col.GetComponent<VidaPersonaje>();
            if (vida != null && vida.currentHealth < vida.maxHealth)
            {
                float vidaActual = vida.currentHealth += cura;
                if (vida.maxHealth < vidaActual)
                {
                    vida.currentHealth = 100;
                }
                else
                {
                    vida.currentHealth = vidaActual;
                }
                vida.UpdateHealthUI();
                Destroy(gameObject);
            }
        }
    }
}