using UnityEngine;

public class FlavorSelectionPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager; // Ссылка на GameManager

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError($"GameManager не найден в сцене для объекта {gameObject.name}.");
            }
        }
    }

    // Метод, вызываемый при выборе вкуса
    public void OnFlavorSelected(string flavor)
    {
        if (gameManager != null)
        {
            gameManager.SelectFlavor(flavor);
            Hide(); // Скрыть панель после выбора вкуса
        }
        else
        {
            Debug.LogError($"GameManager не назначен для объекта {gameObject.name}.");
        }
    }

    // Метод для показа панели
    public void Show()
    {
        gameManager = gameManager ?? FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameObject.SetActive(true);
            Debug.Log("FlavorSelectionPanel показан.");
        }
        else
        {
            Debug.LogError($"GameManager не найден для показа FlavorSelectionPanel на объекте {gameObject.name}.");
        }
    }

    // Метод для скрытия панели
    public void Hide()
    {
        gameObject.SetActive(false);
        Debug.Log("FlavorSelectionPanel скрыт.");
    }
}
