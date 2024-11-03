using UnityEngine;
using TMPro;

/// <summary>
/// Контроллер панели результатов змейки, отображающий монеты.
/// </summary>
public class SnakeVictoryPanelController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI coinsText; // Текст для отображения монет

    private void OnEnable()
    {
        UpdatePanel();
    }

    /// <summary>
    /// Обновляет панель с текущими данными из DataManager.
    /// </summary>
    private void UpdatePanel()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("SnakeVictoryPanelController: DataManager не найден.");
            return;
        }

        int coins = DataManager.Instance.Coins;
        coinsText.text = $"Монет заработано: {coins}";
    }
}
