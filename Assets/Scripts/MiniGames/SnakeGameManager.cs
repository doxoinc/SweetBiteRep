using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Менеджер игры "Змейка", управляющий инициализацией, логикой игры и завершением.
/// </summary>
public class SnakeGameManager : MonoBehaviour
{
    [Header("Snake Settings")]
    public GameObject snakeHeadPrefab;
    public GameObject snakeBodySegmentPrefab;
    public GameObject snakeTailPrefab; // Префаб хвоста
    public float moveInterval = 0.5f; // Интервал движения змейки
    private bool isGameOver = false;

    [Header("Food Settings")]
    public GameObject foodPrefab;
    public int gridSize = 6; // Размер сетки (6x6)

    [Header("UI Elements")]
    public PanelType victoryPanelType = PanelType.SnakeVictory; // Тип панели для завершения игры

    [Header("Sprites")]
    public Sprite bodySprite; // Спрайт тела змейки
    public Sprite tailSprite; // Спрайт хвоста змейки

    private SnakeController snakeController;
    private FoodSpawner foodSpawner;
    private float moveTimer;

    private UIManager uiManager; // Ссылка на UIManager

    private void Start()
    {
        InitializeGame();
    }

    /// <summary>
    /// Инициализирует игру, создавая змейку и спавнер еды.
    /// </summary>
    private void InitializeGame()
    {
        // Проверка на наличие GridManager
        if (GridManager.Instance == null)
        {
            Debug.LogError("SnakeGameManager: GridManager.Instance равен null. Убедитесь, что GridManager присутствует в сцене.");
            return;
        }

        // Создание головы змейки
        Vector3 headPosition = GridManager.Instance.CellToWorldPosition(Vector2Int.zero);
        GameObject head = Instantiate(snakeHeadPrefab, headPosition, Quaternion.identity);
        snakeController = head.GetComponent<SnakeController>();
        if (snakeController == null)
        {
            Debug.LogError("SnakeGameManager: SnakeController не найден на SnakeHead.");
            return;
        }
        else
        {
            Debug.Log("SnakeGameManager: SnakeController успешно инициализирован.");
        }

        // Настройка контроллера змейки
        snakeController.Initialize(this, snakeBodySegmentPrefab, snakeTailPrefab, gridSize);

        // Создание FoodSpawner
        GameObject spawnerObject = new GameObject("FoodSpawner");
        foodSpawner = spawnerObject.AddComponent<FoodSpawner>();
        foodSpawner.Initialize(foodPrefab, gridSize, snakeController);

        // Спавн начальной еды, избегая позиций змейки
        foodSpawner.SpawnFood();
        Debug.Log("SnakeGameManager: Начальная еда спавнена.");

        // Инициализация таймера движения
        moveTimer = moveInterval;

        // Найти UIManager в сцене
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("SnakeGameManager: UIManager не найден в сцене.");
        }
        else
        {
            Debug.Log("SnakeGameManager: UIManager найден.");
        }

        // Проверка наличия DataManager
        if (DataManager.Instance == null)
        {
            Debug.LogError("SnakeGameManager: DataManager.Instance равен null. Убедитесь, что DataManager присутствует в сцене.");
        }
        else
        {
            Debug.Log("SnakeGameManager: DataManager.Instance доступен.");
        }
    }

    private void Update()
    {
        if (isGameOver)
            return; // Игра окончена, не выполняем дальнейшие действия

        if (snakeController == null)
        {
            Debug.LogError("SnakeGameManager: snakeController равен null в Update().");
            return;
        }

        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            snakeController.Move();
            moveTimer = moveInterval;
        }
    }

    /// <summary>
    /// Обрабатывает сбор еды змейкой.
    /// </summary>
    /// <param name="foodPosition">Позиция собранной еды.</param>
    public void OnFoodCollected(Vector2Int foodPosition)
    {
        Debug.Log("SnakeGameManager: OnFoodCollected вызван.");

        // Проверка на null для snakeController
        if (snakeController == null)
        {
            Debug.LogError("SnakeGameManager: snakeController равен null.");
            return;
        }

        // Проверка на null для foodSpawner
        if (foodSpawner == null)
        {
            Debug.LogError("SnakeGameManager: foodSpawner равен null.");
            return;
        }

        // Проверка на null для DataManager.Instance
        if (DataManager.Instance == null)
        {
            Debug.LogError("SnakeGameManager: DataManager.Instance равен null.");
            return;
        }

        // Увеличение длины змейки
        snakeController.Grow();
        Debug.Log("SnakeGameManager: Змейка выросла.");

        // Спавн новой еды, избегая позиций змейки
        foodSpawner.SpawnFood();
        Debug.Log("SnakeGameManager: Еда спавнена.");

        // Добавление монет
        DataManager.Instance.AddCoins(1); // Добавляем 1 монету за сбор еды

        // Добавление очков
        SnakePlayerController spc = snakeController.GetComponent<SnakePlayerController>();
        if (spc != null)
        {
            spc.AddScore(1);
            Debug.Log($"SnakeGameManager: Очки добавлены. Текущий счёт: {spc.GetScore()}");
        }
        else
        {
            Debug.LogWarning("SnakeGameManager: SnakePlayerController не найден на SnakeController.");
        }

        // Добавление звука сбора еды
        AudioManager.Instance?.PlaySound(SoundType.Eat);
    }

    /// <summary>
    /// Обрабатывает завершение игры при столкновении с препятствием или собой.
    /// </summary>
    public void OnGameOver()
    {
        if (isGameOver)
            return; // Предотвращаем повторный вызов

        isGameOver = true; // Устанавливаем флаг окончания игры

        Debug.Log("SnakeGameManager: Игра окончена.");

        // Открытие панели завершения через UIManager
        if (uiManager != null)
        {
            uiManager.OpenPanel(victoryPanelType);
            Debug.Log("SnakeGameManager: Вызван UIManager.OpenPanel для SnakeVictory.");
        }
        else
        {
            Debug.LogError("SnakeGameManager: uiManager равен null.");
        }

        // Вызов звука поражения
        AudioManager.Instance?.PlaySound(SoundType.Lose);
    }

    /// <summary>
    /// Завершает игру и возвращает в главное меню.
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Замените на название вашей главной сцены
    }

    /// <summary>
    /// Перезапускает игру.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("SnakeGame"); // Замените на название вашей игровой сцены
    }

    /// <summary>
    /// Сохраняет результаты игры.
    /// </summary>
    /// <param name="finalScore">Финальный счёт.</param>
    /// <param name="finalCoins">Количество собранных монет.</param>
    public void SaveResults(int finalScore, int finalCoins)
    {
        // Реализуйте сохранение результатов, например, через PlayerPrefs или систему сохранений
        PlayerPrefs.SetInt("FinalScore", finalScore);
        PlayerPrefs.SetInt("FinalCoins", finalCoins);
        PlayerPrefs.Save();
        Debug.Log("SnakeGameManager: Результаты сохранены.");
    }
}
