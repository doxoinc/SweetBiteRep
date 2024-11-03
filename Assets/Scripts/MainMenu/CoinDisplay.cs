using UnityEngine;
using UnityEngine.UI; // Для использования Text
// Если вы используете TextMeshPro, раскомментируйте следующую строку и измените тип на TextMeshProUGUI
// using TMPro;

public class CoinDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Text coinText; // Для использования Text
    // Для TextMeshPro:
    // public TextMeshProUGUI coinText;

    private void Start()
    {
        if (coinText == null)
        {
            // Попытка найти компонент Text на том же объекте
            coinText = GetComponent<Text>();
            // Для TextMeshPro:
            // coinText = GetComponent<TextMeshProUGUI>();

            if (coinText == null)
            {
                Debug.LogError("CoinDisplay: Text компонент не назначен.");
                return;
            }
        }

        if (DataManager.Instance != null)
        {
            UpdateCoinDisplay(DataManager.Instance.Coins); // Инициализация текста при старте
            DataManager.Instance.OnCoinCountChanged += UpdateCoinDisplay; // Подписка на событие
        }
        else
        {
            Debug.LogError("CoinDisplay: DataManager.Instance не найден.");
        }
    }

    private void OnDestroy()
    {
        // Отписка от события при уничтожении объекта
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnCoinCountChanged -= UpdateCoinDisplay;
        }
    }

    /// <summary>
    /// Обновляет отображение количества монет.
    /// </summary>
    /// <param name="newCoinCount">Новое количество монет.</param>
    public void UpdateCoinDisplay(int newCoinCount)
    {
        if (coinText != null)
        {
            coinText.text = newCoinCount.ToString();
        }
        else
        {
            Debug.LogError("CoinDisplay: Text компонент не назначен.");
        }
    }
}
