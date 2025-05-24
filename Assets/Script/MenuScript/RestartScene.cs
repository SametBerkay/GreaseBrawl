using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager i�in gerekli

public class RestartScene : MonoBehaviour
{
    void Update()
    {
        // E�er "U" tu�una bas�lm��sa
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Mevcut sahneyi yeniden y�kle
            RestartCurrentScene();
        }
    }

    void RestartCurrentScene()
    {
        // Mevcut sahneyi al
        string currentScene = SceneManager.GetActiveScene().name;

        // Sahneyi yeniden y�kle
        SceneManager.LoadScene(currentScene);
    }
}
