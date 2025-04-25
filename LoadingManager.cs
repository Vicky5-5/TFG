using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar; // Barra de progreso
    public TextMeshProUGUI loadingText; // Texto de carga
    private float fakeLoadDelay = 2f; // Retraso inicial simulado

    void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        yield return new WaitForSeconds(fakeLoadDelay); // Retraso inicial antes de la carga

        AsyncOperation operation = SceneManager.LoadSceneAsync("Scenes/Nueva Escena");
        operation.allowSceneActivation = false; // Impedir activación automática de la escena

        float simulatedProgress = 0f; // Progreso simulado inicial

        while (simulatedProgress < 1f)
        {
            // Si el progreso real es menor al 90%, usa este como límite superior
            if (operation.progress < 0.9f)
            {
                simulatedProgress += Time.deltaTime * 0.2f; // Incrementa gradualmente
                simulatedProgress = Mathf.Min(simulatedProgress, operation.progress / 0.9f); // Alinea con el progreso real
            }
            else
            {
                // Si el progreso real ya llegó al 90%, completa el progreso simulado al 100%
                simulatedProgress += Time.deltaTime * 0.2f; // Incrementa gradualmente hasta 1.0f
                simulatedProgress = Mathf.Clamp(simulatedProgress, 0f, 1f); // Asegura que no pase de 1.0f
            }

            // Actualización de la barra de progreso y del texto
            progressBar.value = simulatedProgress;
            loadingText.text = $"Cargando... {Mathf.Floor(simulatedProgress * 100)}%";

            yield return null; // Esperar al siguiente fotograma
        }

        // Cuando el progreso alcanza el 100%, permite activar la escena
        operation.allowSceneActivation = true;
    }
}
