using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("UI Manager")]
    public UIManager uiManager;

    [Header("Buttons")]
    public Button soundButton;
    public Button musicButton;
    public Button exitButton;

    [Header("Sprites")]
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

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
        if (soundButton == null || musicButton == null || exitButton == null)
        {
            Debug.LogError("Одна или несколько кнопок не назначены в инспекторе.");
            return;
        }

        if (soundOnSprite == null || soundOffSprite == null || musicOnSprite == null || musicOffSprite == null)
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
        exitButton.onClick.AddListener(OnExitButtonClicked);
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
        // Переключение состояния музыки главного меню
        bool newMusicState = !DataManager.Instance.IsMainMenuMusicOn;
        AudioManager.Instance.ToggleMainMenuMusic(newMusicState);
        UpdateMusicButton();

        Debug.Log("Музыка главного меню переключена: " + (newMusicState ? "Включена" : "Отключена"));
    }

    private void OnExitButtonClicked()
    {
        // Логика выхода из панели настроек
        if (uiManager != null)
            uiManager.OpenMainMenu();
    }

    private void UpdateSoundButton()
    {
        // Обновление спрайта кнопки звука
        soundButton.image.sprite = DataManager.Instance.IsSoundOn ? soundOnSprite : soundOffSprite;
    }

    private void UpdateMusicButton()
    {
        // Обновление спрайта кнопки музыки
        musicButton.image.sprite = DataManager.Instance.IsMainMenuMusicOn ? musicOnSprite : musicOffSprite;
    }
}
