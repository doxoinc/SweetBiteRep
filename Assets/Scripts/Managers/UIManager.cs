using UnityEngine;
using System; // Для использования Action<>
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Для использования Button

public class UIManager : MonoBehaviour
{
    [Header("Non-Animated Panels")]
    public GameObject mainMenuPanel;          // Панель главного меню
    public GameObject flavorSelectionPanel;   // Панель выбора вкуса пирога

    [Header("Animated Panels")]
    public List<PanelController> animatedPanels; // Список всех анимированных панелей в текущей сцене

    [Header("Main Menu Buttons Group")]
    public CanvasGroup mainMenuButtonsGroup; // CanvasGroup для группы кнопок главного меню

    public event Action<PanelType> OnOpenPanel;
    public event Action<PanelType> OnClosePanel;

    private void Awake()
    {
        InitializeAnimatedPanels();
        CloseAllPanels();
    }

    private void OnEnable()
    {
        // Подписка на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Отписка от события загрузки сцены
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Инициализирует и проверяет список анимированных панелей.
    /// </summary>
    private void InitializeAnimatedPanels()
    {
        if (animatedPanels == null)
        {
            animatedPanels = new List<PanelController>();
        }

        foreach (var panel in animatedPanels)
        {
            if (panel == null)
            {
                Debug.LogError("One of the animatedPanels references is null in UIManager.");
            }
            else
            {
                // Подписка на события, если PanelController поддерживает события
                if (panel.OnPanelOpened != null && panel.OnPanelClosed != null)
                {
                    panel.OnPanelOpened += () => OnOpenPanel?.Invoke(panel.panelType);
                    panel.OnPanelClosed += () => OnClosePanel?.Invoke(panel.panelType);
                }
            }
        }
    }

    /// <summary>
    /// Методы для открытия панелей по PanelType
    /// </summary>
    public void OpenMainMenu()
    {
        if (mainMenuPanel != null)
        {
            CloseAllPanels();
            Debug.Log("Opening MainMenuPanel.");
            mainMenuPanel.SetActive(true);

            // Управление музыкой
            AudioManager.Instance.ToggleMainMenuMusic(DataManager.Instance.IsMainMenuMusicOn);
            AudioManager.Instance.ToggleGameSceneMusic(false);

            // Включаем кнопки главного меню
            SetMainMenuButtonsInteractable(true);
        }
        else
        {
            Debug.LogWarning("MainMenuPanel is not assigned in UIManager.");
        }
    }

    public void OpenPanel(PanelType panelType)
    {
        PanelController targetPanel = animatedPanels.Find(p => p.panelType == panelType);

        if (targetPanel == null)
        {
            Debug.LogError($"Panel of type {panelType} not found in animatedPanels list.");
            return;
        }

        StartCoroutine(OpenPanelCoroutine(targetPanel));

        // Управление музыкой для определённых панелей
        if (panelType == PanelType.FlavorSelection || panelType == PanelType.SnakeVictory)
        {
            // Включаем музыку игровой сцены, если настройка позволяет
            AudioManager.Instance.ToggleGameSceneMusic(DataManager.Instance.IsGameSceneMusicOn);
            // Отключаем музыку главного меню
            AudioManager.Instance.ToggleMainMenuMusic(false);
        }

        // Отключаем кнопки главного меню при открытии панели
        SetMainMenuButtonsInteractable(false);
    }

    /// <summary>
    /// Открывает указанную анимированную панель с задержкой, чтобы гарантировать активацию перед запуском анимации.
    /// </summary>
    /// <param name="panel">Панель для открытия.</param>
    private IEnumerator OpenPanelCoroutine(PanelController panel)
    {
        Debug.Log($"Closing all animated panels before opening {panel.gameObject.name}.");
        CloseAllAnimatedPanels();

        // Активировать целевую панель
        Debug.Log($"Activating panel: {panel.gameObject.name}");
        panel.gameObject.SetActive(true);

        // Ждать до конца текущего кадра, чтобы гарантировать активацию GameObject
        yield return new WaitForEndOfFrame();

        // Проверить, активен ли GameObject
        Debug.Log($"After WaitForEndOfFrame, {panel.gameObject.name} activeInHierarchy: {panel.gameObject.activeInHierarchy}");

        if (panel.gameObject.activeInHierarchy)
        {
            Debug.Log($"Opening panel: {panel.gameObject.name}");
            // Открыть панель с анимацией
            panel.OpenPanel();
        }
        else
        {
            Debug.LogError($"{panel.gameObject.name} is still inactive after SetActive(true).");
        }
    }

    /// <summary>
    /// Закрывает все анимированные панели.
    /// </summary>
    private void CloseAllAnimatedPanels()
    {
        foreach (var panel in animatedPanels)
        {
            if (panel != null && panel.gameObject.activeSelf)
            {
                Debug.Log($"Closing panel: {panel.gameObject.name}");
                panel.ClosePanel();
            }
        }
    }

    /// <summary>
    /// Закрывает все панели (анимированные и неанимированные).
    /// </summary>
    private void CloseAllPanels()
    {
        // Закрыть все анимированные панели
        CloseAllAnimatedPanels();

        // Закрыть неанимированные панели
        if (mainMenuPanel != null)
        {
            Debug.Log("Deactivating MainMenuPanel.");
            mainMenuPanel.SetActive(false);
        }

        if (flavorSelectionPanel != null)
        {
            Debug.Log("Deactivating FlavorSelectionPanel.");
            flavorSelectionPanel.SetActive(false);
        }

        // Включаем кнопки главного меню после закрытия панелей
        SetMainMenuButtonsInteractable(true);
    }

    /// <summary>
    /// Обработчик события загрузки сцены.
    /// </summary>
    /// <param name="scene">Загруженная сцена.</param>
    /// <param name="mode">Режим загрузки.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} loaded.");

        // Закрыть все панели перед открытием новых
        CloseAllPanels();

        // В зависимости от сцены, открыть нужные панели и управлять музыкой
        if (scene.name == "MainMenu")
        {
            OpenMainMenu();
        }
        else if (scene.name == "BasicGame") // Замените "BasicGame" на точное имя вашей игровой сцены
        {
            // Активировать FlavorSelectionPanel
            if (flavorSelectionPanel != null)
            {
                flavorSelectionPanel.SetActive(true);
                Debug.Log("FlavorSelectionPanel activated.");

                // Управление музыкой
                AudioManager.Instance.ToggleGameSceneMusic(DataManager.Instance.IsGameSceneMusicOn);
                AudioManager.Instance.ToggleMainMenuMusic(false);
            }
            else
            {
                Debug.LogError("FlavorSelectionPanel is not assigned in UIManager.");
            }
        }
        else if (scene.name == "SnakeGame") // Добавьте условие для мини-игры "Змейка"
        {
            // Панель результатов открывается через SnakeGameManager
            // Никаких дополнительных действий не требуется здесь
            Debug.Log("SnakeGame сцена загружена. Панель результатов будет открыта из SnakeGameManager.");
        }
        // Добавьте дополнительные условия для других сцен по необходимости
    }

    /// <summary>
    /// Закрывает указанную панель по типу.
    /// </summary>
    /// <param name="panelType">Тип панели для закрытия.</param>
    public void ClosePanel(PanelType panelType)
    {
        PanelController targetPanel = animatedPanels.Find(p => p.panelType == panelType);

        if (targetPanel == null)
        {
            Debug.LogError($"Panel of type {panelType} not found in animatedPanels list.");
            return;
        }

        if (targetPanel.gameObject.activeInHierarchy)
        {
            targetPanel.ClosePanel();
        }
        else
        {
            Debug.LogWarning($"Panel {panelType} is not active and cannot be closed.");
        }
    }

    /// <summary>
    /// Устанавливает доступность кнопок главного меню.
    /// </summary>
    /// <param name="isInteractable">Если true, кнопки будут доступны для взаимодействия.</param>
    public void SetMainMenuButtonsInteractable(bool isInteractable)
    {
        if (mainMenuButtonsGroup == null)
        {
            Debug.LogWarning("MainMenuButtonsGroup не назначен в UIManager.");
            return;
        }

        mainMenuButtonsGroup.interactable = isInteractable;
        mainMenuButtonsGroup.blocksRaycasts = isInteractable;

        // Дополнительно можно изменить прозрачность, если требуется
        mainMenuButtonsGroup.alpha = isInteractable ? 1f : 0.5f;

        Debug.Log($"MainMenuButtonsGroup.interactable установлено на {isInteractable}");
    }
}
