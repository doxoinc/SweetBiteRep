using UnityEngine;
using TMPro;

public class VictoryPanelController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI coinsText; // Текст для отображения монет
    public TextMeshProUGUI statusText; // Текст для отображения статуса (Победа/Поражение)

    /// <summary>
    /// Устанавливает текст с количеством монет и статусом игры.
    /// </summary>
    /// <param name="coins">Количество монет, выигранных игроком.</param>
    /// <param name="isVictory">Флаг победы.</param>
    public void SetResults(int coins, bool isVictory)
    {
        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
        else
        {
            Debug.LogWarning("coinsText не назначен в VictoryPanelController.");
        }

        if (statusText != null)
        {
            statusText.text = isVictory ? "Победа!" : "Поражение!";
            Debug.Log($"VictoryPanel: Установлен статус игры: {(isVictory ? "Победа" : "Поражение")}");
        }
        else
        {
            Debug.LogWarning("statusText не назначен в VictoryPanelController.");
        }
    }
}
