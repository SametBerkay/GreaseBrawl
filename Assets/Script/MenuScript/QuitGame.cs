using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Bu metod UI Button'dan çağrılabilir
    public void QuitGameNow()
    {
        Debug.Log("Oyundan çıkılıyor...");
        Application.Quit();

        // Not: Bu satır sadece Editor'da çalışır (test için)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
