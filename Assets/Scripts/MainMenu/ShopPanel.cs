using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    public Button backButton;
    public Button moveLeftButton;
    public Button moveRightButton;
    public Transform gridContainer;
    public GameObject skinItemPrefab;

    [Header("Skin Data")]
    public List<SkinDataSO> allSkins = new List<SkinDataSO>(); // Список всех скинов, назначаемый через инспектор

    [Header("UI Elements")]
    public Text coinsText; // Текст для отображения монет

    private int currentPage = 0;
    private int totalPages = 2; // Страница 0: Скины для змейки, Страница 1: Скины для тарелки

    [Header("UI Manager")]
    public UIManager uiManager; // Ссылка на UIManager через инспектор

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
        moveLeftButton.onClick.AddListener(OnMoveLeft);
        moveRightButton.onClick.AddListener(OnMoveRight);

        // Подписка на событие изменения монет
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnCoinCountChanged += UpdateCoinsUI;
            UpdateCoinsUI(DataManager.Instance.Coins);
        }

        DisplayCurrentPage();
    }

    private void OnDestroy()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnCoinCountChanged -= UpdateCoinsUI;
        }
    }

    private void OnBackButtonClicked()
    {
        if (uiManager != null)
            uiManager.OpenMainMenu();
    }

    private void OnMoveLeft()
    {
        if (currentPage > 0)
        {
            currentPage--;
            DisplayCurrentPage();
        }
    }

    private void OnMoveRight()
    {
        if (currentPage < totalPages - 1)
        {
            currentPage++;
            DisplayCurrentPage();
        }
    }

    private void DisplayCurrentPage()
    {
        // Очистка текущих элементов
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        // Фильтрация скинов по текущей странице
        List<SkinDataSO> skinsToDisplay = new List<SkinDataSO>();
        if (currentPage == 0)
        {
            // Скины для змейки
            skinsToDisplay = allSkins.FindAll(s => s.Type == SkinType.Snake);
        }
        else if (currentPage == 1)
        {
            // Скины для тарелки
            skinsToDisplay = allSkins.FindAll(s => s.Type == SkinType.Plate);
        }

        // Создание элементов скинов
        foreach (var skin in skinsToDisplay)
        {
            GameObject skinItem = Instantiate(skinItemPrefab, gridContainer);
            SkinItemUI skinUI = skinItem.GetComponent<SkinItemUI>();
            if (skinUI != null)
            {
                skinUI.Setup(skin);
            }
            else
            {
                Debug.LogError("SkinItemUI компонент не найден на skinItemPrefab.");
            }
        }
    }

    private void UpdateCoinsUI(int newCoinCount)
    {
        if (coinsText != null)
        {
            coinsText.text = newCoinCount.ToString();
        }
        else
        {
            Debug.LogError("CoinsText не назначен в ShopPanel.");
        }
    }
}
