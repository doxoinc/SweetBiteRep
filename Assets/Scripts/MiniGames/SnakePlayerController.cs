using UnityEngine;

/// <summary>
/// Контроллер игрока змейки, отвечающий за управление счётом.
/// </summary>
public class SnakePlayerController : MonoBehaviour
{
    private int score = 0;

    /// <summary>
    /// Добавляет очки к текущему счёту.
    /// </summary>
    /// <param name="amount">Количество добавляемых очков.</param>
    public void AddScore(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("SnakePlayerController: Попытка добавить отрицательное количество очков.");
            return;
        }

        score += amount;
        Debug.Log($"SnakePlayerController: Очки добавлены: {amount}. Текущий счёт: {score}");
    }

    /// <summary>
    /// Возвращает текущий счёт.
    /// </summary>
    /// <returns>Текущий счёт.</returns>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// Сбрасывает счёт до нуля.
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        Debug.Log("SnakePlayerController: Счёт сброшен до 0.");
    }
}
