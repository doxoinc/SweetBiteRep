using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniGamePanel : MonoBehaviour
{
    public Button backButton;
    public Button playButton;

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
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        if (uiManager != null)
        uiManager.OpenMainMenu();
    }

    private void OnPlayButtonClicked()
    {
        if (DataManager.Instance.SpendCoins(5))
        {
            // Сохранение изменения монет
            SceneManager.LoadScene("SnakeGame");
        }
        else
        {
            // Показать сообщение о нехватке монет
            Debug.Log("Not enough coins to play Mini Game");
        }
    }
}
