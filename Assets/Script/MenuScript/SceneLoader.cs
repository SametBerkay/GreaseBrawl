using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
   [SerializeField]
    private string sceneName; // Inspector üzerinden sahne adını gir.

    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Sahneyi yükler
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Sahne adı boş. Lütfen geçerli bir sahne adı girin!");
        }
    }
}
