using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [Header("UI Manager")]
    public UIManager uiManager;

    [Header("Buttons")]
    public Button musicButton;      // Кнопка Music
    public Image musicButtonImage;  // Изображение кнопки Music

    public Button soundButton;      // Кнопка Sound
    public Image soundButtonImage;  // Изображение кнопки Sound

    public Button restartButton;    // Кнопка Restart
    public Button exitButton;       // Кнопка Exit
    public Button finishGameButton; // Кнопка FinishGame
    public Button continueButton;   // Кнопка Continue

    [Header("Button Sprites")]
    public Sprite musicOnSprite;    // Изображение для включенной музыки
    public Sprite musicOffSprite;   // Изображение для выключенной музыки

    public Sprite soundOnSprite;    // Изображение для включенных звуков
    public Sprite soundOffSprite;   // Изображение для выключенных звуков

    private void Start()
    {
        // Проверка и инициализация UIManager
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager не найден в сцене.");
                return;
            }
        }

        // Проверка наличия кнопок и спрайтов
        if (musicButton == null || musicButtonImage == null ||
            soundButton == null || soundButtonImage == null ||
            restartButton == null || exitButton == null ||
            finishGameButton == null || continueButton == null)
        {
            Debug.LogError("Одна или несколько кнопок/изображений не назначены в инспекторе.");
            return;
        }

        if (musicOnSprite == null || musicOffSprite == null ||
            soundOnSprite == null || soundOffSprite == null)
        {
            Debug.LogError("Одна или несколько спрайтов не назначены в инспекторе.");
            return;
        }

        // Инициализация спрайтов в зависимости от текущих настроек
        UpdateSoundButton();
        UpdateMusicButton();

        // Привязка методов к событиям кнопок
        soundButton.onClick.AddListener(OnSoundButtonClicked);
        musicButton.onClick.AddListener(OnMusicButtonClicked);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitToMainMenu);
        finishGameButton.onClick.AddListener(FinishGame);
        continueButton.onClick.AddListener(ContinueGame);
    }

    private void OnSoundButtonClicked()
    {
        // Переключение состояния звуковых эффектов
        bool newSoundState = !DataManager.Instance.IsSoundOn;
        AudioManager.Instance.ToggleSound(newSoundState);
        UpdateSoundButton();

        Debug.Log("Звуковые эффекты переключены: " + (newSoundState ? "Включены" : "Отключены"));
    }

    private void OnMusicButtonClicked()
    {
        // Переключение состояния музыки игровой сцены
        bool newMusicState = !DataManager.Instance.IsGameSceneMusicOn;
        AudioManager.Instance.ToggleGameSceneMusic(newMusicState);
        UpdateMusicButton();

        Debug.Log("Музыка игровой сцены переключена: " + (newMusicState ? "Включена" : "Отключена"));
    }

    private void RestartGame()
    {
        Debug.Log("Нажата кнопка Restart.");
        AudioManager.Instance.PlaySound(SoundType.ButtonClick);
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ClosePanel(PanelType.SettingsGame);
            // Рестарт игры через GameManager
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.RestartGame();
            }
            else
            {
                Debug.LogError("GameManager не найден в сцене.");
            }
        }
        else
        {
            Debug.LogError("UIManager не найден в сцене.");
        }
    }

    private void ExitToMainMenu()
    {
        Debug.Log("Нажата кнопка Exit.");
        AudioManager.Instance.PlaySound(SoundType.ButtonClick);
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ClosePanel(PanelType.SettingsGame);
            // Выход в главное меню через GameManager
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.ExitGame();
            }
            else
            {
                Debug.LogError("GameManager не найден в сцене.");
            }
        }
        else
        {
            Debug.LogError("UIManager не найден в сцене.");
        }
    }

    private void FinishGame()
    {
        Debug.Log("Нажата кнопка FinishGame. Логика не реализована.");
        AudioManager.Instance.PlaySound(SoundType.ButtonClick);
        // Добавьте необходимую логику, если требуется
    }

    private void ContinueGame()
    {
        Debug.Log("Нажата кнопка Continue. Игра возобновляется.");
        AudioManager.Instance.PlaySound(SoundType.ButtonClick);
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ClosePanel(PanelType.SettingsGame);
        }
        else
        {
            Debug.LogError("UIManager не найден в сцене.");
        }
    }

    private void UpdateSoundButton()
    {
        // Обновление спрайта кнопки звука
        soundButtonImage.sprite = DataManager.Instance.IsSoundOn ? soundOnSprite : soundOffSprite;
    }

    private void UpdateMusicButton()
    {
        // Обновление спрайта кнопки музыки
        musicButtonImage.sprite = DataManager.Instance.IsGameSceneMusicOn ? musicOnSprite : musicOffSprite;
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        Debug.Log("Игра поставлена на паузу.");
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        Debug.Log("Игра возобновлена.");
    }
}
