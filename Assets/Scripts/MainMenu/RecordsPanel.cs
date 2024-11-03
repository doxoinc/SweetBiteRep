using UnityEngine;
using UnityEngine.UI;

public class RecordsPanel : MonoBehaviour
{
    [Header("UI Manager")]
    public UIManager uiManager; // Ссылка на UIManager через Inspector

    public Button backButton;
    // Добавь ссылки на подпанели, если необходимо

    private void Start()
    {
        // Проверка наличия UIManager
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
        LoadRecords();
    }

    private void OnBackButtonClicked()
    {
        if (uiManager != null)
            uiManager.OpenMainMenu();
    }

    private void LoadRecords()
    {
        // Реализуй загрузку и отображение рекордов
        // Например, загрузка из PlayerPrefs
    }
}
