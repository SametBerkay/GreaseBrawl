using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;      // Normal pause panel (Restart & Continue olan)
    public GameObject infoPanel;       // Yeni eklenen Info/Tutorial paneli

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);  // Başta kapalı
        infoPanel.SetActive(false);   // Info da başta kapalı
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;             // Oyunu dondur
        pausePanel.SetActive(true);      // Normal pause paneli aç
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;             // Oyunu devam ettir
        pausePanel.SetActive(false);     // Pause paneli kapat
        infoPanel.SetActive(false);      // Info paneli de garanti kapatalım
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;  // Yoksa sahne donmuş gelir
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Yeni eklenen metod - Info Panelini açar
    public void ShowInfoPanel()
    {
        Time.timeScale = 0f;             // Oyunu dondur
        infoPanel.SetActive(true);       // Info/Tutorial paneli aç
        isPaused = true;                  // Pause gibi davransın
    }

    // Info paneldeki kapat (Resume) butonu için
    public void CloseInfoPanel()
    {
        Time.timeScale = 1f;             // Oyunu devam ettir
        infoPanel.SetActive(false);      // Info paneli kapat
        isPaused = false;                 // Pause modundan çık
    }
}
