using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI lastScoreText; // Текст для отображения последнего счёта
    public TextMeshProUGUI lastCoinsText; // Текст для отображения последних монет
    public TextMeshProUGUI bestScoreText; // Текст для отображения лучшего счёта
    public TextMeshProUGUI bestCoinsText; // Текст для отображения лучших монет

    private void Start()
    {
        LoadResults();
    }

    // Метод для загрузки результатов из PlayerPrefs
    private void LoadResults()
    {
        // Загрузка последнего счёта
        if (PlayerPrefs.HasKey("LastScore"))
        {
            int lastScore = PlayerPrefs.GetInt("LastScore");
            lastScoreText.text = $"Последний счёт: {lastScore}";
        }
        else
        {
            lastScoreText.text = "Последний счёт: 0";
        }

        // Загрузка последних монет
        if (PlayerPrefs.HasKey("LastCoins"))
        {
            int lastCoins = PlayerPrefs.GetInt("LastCoins");
            lastCoinsText.text = $"Последние монеты: {lastCoins}";
        }
        else
        {
            lastCoinsText.text = "Последние монеты: 0";
        }

        // Загрузка лучшего счёта
        if (PlayerPrefs.HasKey("BestScore"))
        {
            int bestScore = PlayerPrefs.GetInt("BestScore");
            bestScoreText.text = $"Лучший счёт: {bestScore}";
        }
        else
        {
            bestScoreText.text = "Лучший счёт: 0";
        }

        // Загрузка лучших монет
        if (PlayerPrefs.HasKey("BestCoins"))
        {
            int bestCoins = PlayerPrefs.GetInt("BestCoins");
            bestCoinsText.text = $"Лучшие монеты: {bestCoins}";
        }
        else
        {
            bestCoinsText.text = "Лучшие монеты: 0";
        }
    }

    // Метод для начала новой игры
    public void StartNewGame()
    {
        // Загрузить игровую сцену (предполагается, что она называется "GameScene")
        SceneManager.LoadScene("GameScene");
    }

    // Метод для выхода из игры
    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
