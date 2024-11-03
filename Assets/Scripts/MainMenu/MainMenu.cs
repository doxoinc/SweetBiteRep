using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("UI Manager")]
    public UIManager uiManager; // Назначьте через Inspector

    // Метод вызывается автоматически при запуске сцены
    private void Start()
    {
        // Если uiManager не назначен через Inspector, попробуем найти его
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager не найден в сцене.");
                return;
            }
        }
    }

    // Методы, вызываемые при нажатии кнопок
    public void OnPlayButtonClicked()
    {
        PlayButtonClickSound();
        if (uiManager != null)
            uiManager.OpenPanel(PanelType.Play);
        else
            Debug.LogError("UIManager не назначен в MainMenu.");
    }

    public void OnSettingsButtonClicked()
    {
        PlayButtonClickSound();
        if (uiManager != null)
            uiManager.OpenPanel(PanelType.SettingsMainMenu);
        else
            Debug.LogError("UIManager не назначен в MainMenu.");
    }

    public void OnRecordsButtonClicked()
    {
        PlayButtonClickSound();
        if (uiManager != null)
            uiManager.OpenPanel(PanelType.Records);
        else
            Debug.LogError("UIManager не назначен в MainMenu.");
    }

    public void OnStatisticsButtonClicked()
    {
        PlayButtonClickSound();
        if (uiManager != null)
            uiManager.OpenPanel(PanelType.Statistics);
        else
            Debug.LogError("UIManager не назначен в MainMenu.");
    }

    public void OnMiniGameButtonClicked()
    {
        PlayButtonClickSound();
        if (uiManager != null)
            uiManager.OpenPanel(PanelType.MiniGame);
        else
            Debug.LogError("UIManager не назначен в MainMenu.");
    }

    public void OnShopButtonClicked()
    {
        PlayButtonClickSound();
        if (uiManager != null)
            uiManager.OpenPanel(PanelType.Shop);
        else
            Debug.LogError("UIManager не назначен в MainMenu.");
    }

    public void OpenSettingsPanel()
    {
        PlayButtonClickSound();
        uiManager.OpenPanel(PanelType.SettingsGame);
    }

    /// <summary>
    /// Воспроизводит звук нажатия кнопки.
    /// </summary>
    private void PlayButtonClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(SoundType.ButtonClick);
        }
        else
        {
            Debug.LogError("AudioManager не найден в сцене. Убедитесь, что AudioManager присутствует и инициализирован.");
        }
    }
}
