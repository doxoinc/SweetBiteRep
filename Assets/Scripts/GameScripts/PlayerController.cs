using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxMoveSpeed = 5f;      // Максимальная скорость движения
    public float acceleration = 10f;     // Ускорение
    public float deceleration = 10f;     // Замедление

    [Header("UI Elements")]
    public TextMeshPro scoreText; // Используем TextMeshPro для отображения счёта

    private int score = 0;

    // Флаги для управления движением через кнопки
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    // Текущая скорость по оси X
    private float currentVelocityX = 0f;

    private void Update()
    {
        HandleMovement();
        UpdateUI();
    }

    /// <summary>
    /// Управление движением игрока по горизонтали с инерцией.
    /// </summary>
    private void HandleMovement()
    {
        float targetVelocityX = 0f;

        if (isMovingLeft)
        {
            targetVelocityX = -maxMoveSpeed;
        }
        else if (isMovingRight)
        {
            targetVelocityX = maxMoveSpeed;
        }

        if (targetVelocityX != 0f)
        {
            // Плавное ускорение к целевой скорости
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX, acceleration * Time.deltaTime);
        }
        else
        {
            // Плавное замедление до остановки
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, 0f, deceleration * Time.deltaTime);
        }

        // Применение движения
        transform.Translate(currentVelocityX * Time.deltaTime, 0f, 0f);

        // Ограничение позиции игрока внутри экрана
        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float clampedX = Mathf.Clamp(transform.position.x, 
                                      -screenHalfWidth + 1f,
                                       screenHalfWidth - 1f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// Добавляет очки к счёту игрока.
    /// </summary>
    /// <param name="amount">Количество добавляемых очков.</param>
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Очков добавлено: {amount}. Текущий счёт: {score}");
    }

    /// <summary>
    /// Сбрасывает счёт игрока до 0.
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        Debug.Log("Счёт сброшен до 0.");
    }

    /// <summary>
    /// Возвращает текущий счёт игрока.
    /// </summary>
    /// <returns>Текущий счёт.</returns>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// Обновляет UI для отображения текущего счёта.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
        else
            Debug.LogWarning("scoreText не назначен в PlayerController.");
    }

    /// <summary>
    /// Метод, вызываемый при нажатии кнопки MoveLeft.
    /// </summary>
    public void OnMoveLeftPressed()
    {
        isMovingLeft = true;
        Debug.Log("MoveLeft pressed.");
    }

    /// <summary>
    /// Метод, вызываемый при отпускании кнопки MoveLeft.
    /// </summary>
    public void OnMoveLeftReleased()
    {
        isMovingLeft = false;
        Debug.Log("MoveLeft released.");
    }

    /// <summary>
    /// Метод, вызываемый при нажатии кнопки MoveRight.
    /// </summary>
    public void OnMoveRightPressed()
    {
        isMovingRight = true;
        Debug.Log("MoveRight pressed.");
    }

    /// <summary>
    /// Метод, вызываемый при отпускании кнопки MoveRight.
    /// </summary>
    public void OnMoveRightReleased()
    {
        isMovingRight = false;
        Debug.Log("MoveRight released.");
    }
}
