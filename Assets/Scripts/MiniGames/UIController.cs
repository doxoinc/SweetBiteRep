using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    private SnakeController snakeController;

    private void Start()
    {
        // Найти SnakeController в сцене
        snakeController = FindObjectOfType<SnakeController>();
        if (snakeController == null)
        {
            Debug.LogError("SnakeController не найден в сцене.");
            return;
        }

        // Назначить методы к кнопкам
        if (upButton != null)
            upButton.onClick.AddListener(() => snakeController.SetDirection(Vector2Int.up));
        else
            Debug.LogError("UpButton не назначен в UIController.");

        if (downButton != null)
            downButton.onClick.AddListener(() => snakeController.SetDirection(Vector2Int.down));
        else
            Debug.LogError("DownButton не назначен в UIController.");

        if (leftButton != null)
            leftButton.onClick.AddListener(() => snakeController.SetDirection(Vector2Int.left));
        else
            Debug.LogError("LeftButton не назначен в UIController.");

        if (rightButton != null)
            rightButton.onClick.AddListener(() => snakeController.SetDirection(Vector2Int.right));
        else
            Debug.LogError("RightButton не назначен в UIController.");
    }
}
