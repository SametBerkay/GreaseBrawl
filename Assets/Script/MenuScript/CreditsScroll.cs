using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 50f; // Yazının kayma hızı, Inspector'dan ayarlanabilir.
    
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // İlk pozisyonu ekranın altından başlat.
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -Screen.height / 2);
        
        // 60 saniye sonra "MenuScene" sahnesine geçiş yap.
        Invoke("LoadMenuScene", 60f);
    }

    private void Update()
    {
        // Yukarı doğru kaydır.
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }

    private void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene"); // "MenuScene" sahnesine geçiş yap.
    }
}
