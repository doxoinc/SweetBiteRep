using UnityEngine;

/// <summary>
/// Управляет сеткой игры, обеспечивая преобразование координат ячеек в мировые позиции и наоборот.
/// </summary>
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    private Grid grid;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            grid = GetComponent<Grid>();
            if (grid == null)
            {
                Debug.LogError("GridManager: Компонент Grid не найден на объекте GameGrid.");
            }
            else
            {
                Debug.Log("GridManager успешно инициализирован.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Преобразует координаты ячейки в мировую позицию (центр ячейки).
    /// </summary>
    /// <param name="cellPosition">Координаты ячейки.</param>
    /// <returns>Мировая позиция центра ячейки.</returns>
    public Vector3 CellToWorldPosition(Vector2Int cellPosition)
    {
        if (grid == null)
        {
            Debug.LogError("GridManager: Компонент Grid отсутствует.");
            return Vector3.zero;
        }
        Vector3Int cell = new Vector3Int(cellPosition.x, cellPosition.y, 0);
        return grid.CellToWorld(cell) + grid.cellSize / 2f; // Центр ячейки
    }

    /// <summary>
    /// Преобразует мировую позицию в координаты ячейки.
    /// </summary>
    /// <param name="worldPosition">Мировая позиция.</param>
    /// <returns>Координаты ячейки.</returns>
    public Vector2Int WorldToCellPosition(Vector3 worldPosition)
    {
        if (grid == null)
        {
            Debug.LogError("GridManager: Компонент Grid отсутствует.");
            return Vector2Int.zero;
        }
        Vector3Int cell = grid.WorldToCell(worldPosition);
        return new Vector2Int(cell.x, cell.y);
    }
}
