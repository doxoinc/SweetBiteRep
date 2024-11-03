using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Спавнер еды, отвечающий за генерацию еды на свободных ячейках сетки.
/// </summary>
public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public int gridSize = 6;
    private SnakeController snakeController;

    /// <summary>
    /// Инициализация спавнера еды.
    /// </summary>
    /// <param name="food">Префаб еды.</param>
    /// <param name="grid">Размер сетки.</param>
    /// <param name="controller">Ссылка на контроллер змейки.</param>
    public void Initialize(GameObject food, int grid, SnakeController controller)
    {
        foodPrefab = food;
        gridSize = grid;
        snakeController = controller;
    }

    /// <summary>
    /// Спавнит еду в случайной свободной ячейке.
    /// </summary>
    public void SpawnFood()
    {
        Vector2Int spawnPosition = GetRandomFreeCell();
        if (spawnPosition != Vector2Int.one * -999) // Проверка на валидность позиции
        {
            Vector3 worldPos = GridManager.Instance.CellToWorldPosition(spawnPosition);
            Instantiate(foodPrefab, worldPos, Quaternion.identity, transform);
            Debug.Log($"FoodSpawner: Еда спавнена в позиции: {spawnPosition}, мировая позиция: {worldPos}");
        }
        else
        {
            Debug.LogWarning("FoodSpawner: Не удалось найти свободную позицию для еды.");
        }
    }

    /// <summary>
    /// Возвращает случайную свободную ячейку внутри сетки.
    /// </summary>
    /// <returns>Координаты свободной ячейки или (-999, -999) при отсутствии.</returns>
    private Vector2Int GetRandomFreeCell()
    {
        List<Vector2Int> freeCells = new List<Vector2Int>();

        for (int x = -gridSize / 2; x <= gridSize / 2; x++)
        {
            for (int y = -gridSize / 2; y <= gridSize / 2; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);
                bool isOccupied = false;

                // Проверка на занятость сегментов тела
                foreach (Transform segment in snakeController.BodySegments)
                {
                    Vector2Int segmentPos = GridManager.Instance.WorldToCellPosition(segment.position);
                    if (segmentPos == cell)
                    {
                        isOccupied = true;
                        break;
                    }
                }

                // Проверка на занятость головы змейки
                Vector2Int headPos = GridManager.Instance.WorldToCellPosition(snakeController.transform.position);
                if (headPos == cell)
                {
                    isOccupied = true;
                }

                if (!isOccupied)
                {
                    freeCells.Add(cell);
                }
            }
        }

        if (freeCells.Count > 0)
        {
            int randomIndex = Random.Range(0, freeCells.Count);
            return freeCells[randomIndex];
        }
        else
        {
            Debug.LogWarning("FoodSpawner: Все ячейки заняты. Еда не может быть спавнена.");
            return new Vector2Int(-999, -999); // Некорректная позиция для обозначения ошибки
        }
    }
}
