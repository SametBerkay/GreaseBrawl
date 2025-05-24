using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIFloat : MonoBehaviour
{
    [Header("Aşağı Yukarı Hareket Ayarları")]
    public float floatDistance = 20f; // Ne kadar yukarı-aşağı gidecek
    public float duration = 1f;       // Bir gidiş süresi

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        Vector3 originalPos = rect.anchoredPosition;

        rect.DOAnchorPosY(originalPos.y + floatDistance, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
