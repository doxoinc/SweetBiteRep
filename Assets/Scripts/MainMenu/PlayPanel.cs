// PlayPanel.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayPanel : MonoBehaviour
{
    [Header("Buttons")]
    public Button backButton;
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;
    public Button playButton;

    [Header("Difficulty Sprites")]
    public Sprite easySelectedSprite;       // Спрайт для выбранной кнопки Easy
    public Sprite normalSelectedSprite;     // Спрайт для выбранной кнопки Normal
    public Sprite hardSelectedSprite;       // Спрайт для выбранной кнопки Hard

    [Header("Button Images")]
    public Image easyButtonImage;
    public Image normalButtonImage;
    public Image hardButtonImage;

    private DifficultyLevel selectedDifficulty = DifficultyLevel.Normal; // Значение по умолчанию

    [Header("UI Manager")]
    public UIManager uiManager; // Ссылка на UIManager через Inspector

    // Сохранение исходных спрайтов для возврата к ним
    private Sprite easyDefaultSprite;
    private Sprite normalDefaultSprite;
    private Sprite hardDefaultSprite;

    private void Start()
    {
        // Поиск UIManager, если ссылка не установлена
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager не найден в сцене.");
                return;
            }
        }

        // Сохранение исходных спрайтов
        if (easyButtonImage != null)
            easyDefaultSprite = easyButtonImage.sprite;
        else
            Debug.LogWarning("easyButtonImage не назначен в PlayPanel.");

        if (normalButtonImage != null)
            normalDefaultSprite = normalButtonImage.sprite;
        else
            Debug.LogWarning("normalButtonImage не назначен в PlayPanel.");

        if (hardButtonImage != null)
            hardDefaultSprite = hardButtonImage.sprite;
        else
            Debug.LogWarning("hardButtonImage не назначен в PlayPanel.");

        // Привязка методов к событиям кнопок
        backButton.onClick.AddListener(OnBackButtonClicked);
        easyButton.onClick.AddListener(() => OnDifficultySelected(DifficultyLevel.Easy));
        normalButton.onClick.AddListener(() => OnDifficultySelected(DifficultyLevel.Normal));
        hardButton.onClick.AddListener(() => OnDifficultySelected(DifficultyLevel.Hard));
        playButton.onClick.AddListener(OnPlayButtonClicked);

        // Обновление UI в соответствии с текущей сложностью
        UpdateDifficultyUI();
    }

    /// <summary>
    /// Обработчик нажатия кнопки "Назад".
    /// </summary>
    private void OnBackButtonClicked()
    {
        if (uiManager != null)
            uiManager.OpenMainMenu();
    }

    /// <summary>
    /// Обработчик выбора сложности.
    /// </summary>
    /// <param name="difficulty">Выбранный уровень сложности.</param>
    private void OnDifficultySelected(DifficultyLevel difficulty)
    {
        selectedDifficulty = difficulty;
        // Сохранение выбранной сложности в DataManager
        DataManager.Instance.SetSelectedDifficulty(selectedDifficulty);
        UpdateDifficultyUI();
        Debug.Log("Selected Difficulty: " + selectedDifficulty);
    }

    /// <summary>
    /// Обновляет спрайты кнопок в соответствии с выбранной сложностью.
    /// </summary>
    private void UpdateDifficultyUI()
    {
        // Обновление спрайта кнопки Easy
        if (easyButtonImage != null)
        {
            easyButtonImage.sprite = selectedDifficulty == DifficultyLevel.Easy ? easySelectedSprite : easyDefaultSprite;
        }
        else
        {
            Debug.LogWarning("easyButtonImage не назначен в PlayPanel.");
        }

        // Обновление спрайта кнопки Normal
        if (normalButtonImage != null)
        {
            normalButtonImage.sprite = selectedDifficulty == DifficultyLevel.Normal ? normalSelectedSprite : normalDefaultSprite;
        }
        else
        {
            Debug.LogWarning("normalButtonImage не назначен в PlayPanel.");
        }

        // Обновление спрайта кнопки Hard
        if (hardButtonImage != null)
        {
            hardButtonImage.sprite = selectedDifficulty == DifficultyLevel.Hard ? hardSelectedSprite : hardDefaultSprite;
        }
        else
        {
            Debug.LogWarning("hardButtonImage не назначен в PlayPanel.");
        }
    }

    /// <summary>
    /// Обработчик нажатия кнопки "Play".
    /// </summary>
    private void OnPlayButtonClicked()
    {
        // Передача выбранной сложности через DataManager
        DataManager.Instance.SetSelectedDifficulty(selectedDifficulty);
        SceneManager.LoadScene("BasicGame");
    }
}
