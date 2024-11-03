using UnityEngine;
using UnityEngine.UI;

public class StatisticsPanel : MonoBehaviour
{
    public Button backButton;
    public Text winsText;
    public Text lossesText;
    public Text coinsEarnedText;

    [Header("UI Manager")]
    public UIManager uiManager; // Ссылка на UIManager через Inspector

    private void Start()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager не найден в сцене.");
                return;
            }
        }

        backButton.onClick.AddListener(OnBackButtonClicked);
        LoadStatistics();
    }

    private void OnBackButtonClicked()
    {
        if (uiManager != null)
        uiManager.OpenMainMenu();
    }

    private void LoadStatistics()
    {
        // Загрузка статистики из PlayerPrefs или DataManager
        int wins = PlayerPrefs.GetInt("Wins", 0);
        int losses = PlayerPrefs.GetInt("Losses", 0);
        int coinsEarned = PlayerPrefs.GetInt("CoinsEarned", 0);

        winsText.text = "Wins: " + wins;
        lossesText.text = "Losses: " + losses;
        coinsEarnedText.text = "Coins Earned: " + coinsEarned;
    }
}
