using UnityEngine;
using TMPro;

/// <summary>
/// Контроллер панели, отвечающий за открытие, закрытие и обновление UI элементов.
/// </summary>
public class PanelController : MonoBehaviour
{
    [Header("Panel Settings")]
    public PanelType panelType; // Назначьте тип панели в инспекторе

    private Animator animator;
    private CanvasGroup canvasGroup;
    private bool isOpen = false;

    // Событие, вызываемое после завершения открытия панели
    public System.Action OnPanelOpened;
    // Событие, вызываемое после завершения закрытия панели
    public System.Action OnPanelClosed;

    [Header("UI Elements (SnakeVictory Panel Only)")]
    public TextMeshProUGUI coinsText; // Текст для отображения монет

    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on " + gameObject.name);
        }

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup component is missing on " + gameObject.name);
        }

        // Убедитесь, что панель изначально закрыта
        CloseImmediate();
    }

    /// <summary>
    /// Открывает панель с анимацией.
    /// </summary>
    public void OpenPanel()
    {
        if (!isOpen)
        {
            isOpen = true;
            gameObject.SetActive(true);
            animator.SetTrigger("Open");
            Debug.Log($"{gameObject.name} is opening.");
            // Управление через анимационные события
        }
    }

    /// <summary>
    /// Закрывает панель с анимацией.
    /// </summary>
    public void ClosePanel()
    {
        if (isOpen)
        {
            isOpen = false;
            animator.SetTrigger("Close");
            Debug.Log($"{gameObject.name} is closing.");
            // Управление через анимационные события
        }
    }

    /// <summary>
    /// Немедленно закрывает панель без анимации.
    /// </summary>
    public void CloseImmediate()
    {
        if (animator != null)
        {
            animator.Play("Closed"); // Убедитесь, что у вас есть состояние "Closed" в Animator
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} is now inactive.");
    }

    /// <summary>
    /// Вызывается аниматором по завершении анимации открытия.
    /// </summary>
    public void OnOpenAnimationComplete()
    {
        OnPanelOpened?.Invoke();
        Debug.Log($"{gameObject.name} has been opened.");

        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        // Специфическая логика для определённых панелей
        if (panelType == PanelType.SnakeVictory)
        {
            UpdateSnakeVictoryPanel();
        }
    }

    /// <summary>
    /// Вызывается аниматором по завершении анимации закрытия.
    /// </summary>
    public void OnCloseAnimationComplete()
    {
        OnPanelClosed?.Invoke();
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} has been closed.");

        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// Обновляет UI элементы панели SnakeVictory.
    /// </summary>
    private void UpdateSnakeVictoryPanel()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("PanelController: DataManager не найден.");
            return;
        }

        // Обновление количества монет
        if (coinsText != null)
        {
            int coins = DataManager.Instance.Coins;
            coinsText.text = coins.ToString();
            Debug.Log($"PanelController: coinsText обновлен на {coins}");
        }
        else
        {
            Debug.LogWarning("PanelController: coinsText не назначен для SnakeVictoryPanel.");
        }
    }
}
