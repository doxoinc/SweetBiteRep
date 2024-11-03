using UnityEngine;
using TMPro;

public class LostPanelController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI finalScoreText; // Текст для отображения финального счёта

    private void OnEnable()
    {
    }

    /// <summary>
    /// Устанавливает текст с финальным счётом.
    /// </summary>
    /// <param name="finalScore">Финальный счёт игрока.</param>
    public void SetFinalScore(int finalScore)
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {finalScore}";
            Debug.Log($"LostPanel: Установлен финальный счёт: {finalScore}");
        }
        else
        {
            Debug.LogWarning("finalScoreText не назначен в LostPanelController.");
        }
    }
}
