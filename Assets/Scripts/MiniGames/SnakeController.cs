using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Контроллер змейки, управляющий её движением, ростом и взаимодействием с объектами.
/// </summary>
public class SnakeController : MonoBehaviour
{
    public Vector2Int direction = Vector2Int.up; // Начальное направление движения

    // Публичное свойство для доступа к сегментам змейки
    public ReadOnlyCollection<Transform> BodySegments => bodySegments.AsReadOnly();

    private List<Transform> bodySegments = new List<Transform>();
    private GameObject bodySegmentPrefab;
    private GameObject tailSegmentPrefab; // Префаб хвоста
    private int gridSize;
    private SnakeGameManager gameManager;

    // Текущая позиция головы змейки в координатах ячеек
    private Vector2Int headCellPosition;

    /// <summary>
    /// Инициализация змейки.
    /// </summary>
    /// <param name="manager">Ссылка на менеджер игры.</param>
    /// <param name="bodyPrefab">Префаб сегмента тела.</param>
    /// <param name="tailPrefab">Префаб сегмента хвоста.</param>
    /// <param name="grid">Размер сетки.</param>
    public void Initialize(SnakeGameManager manager, GameObject bodyPrefab, GameObject tailPrefab, int grid)
    {
        gameManager = manager;
        bodySegmentPrefab = bodyPrefab;
        tailSegmentPrefab = tailPrefab;
        gridSize = grid;
        headCellPosition = Vector2Int.zero; // Начальная позиция в центре сетки
        UpdateWorldPosition();
        RotateHead();
        Debug.Log("SnakeController инициализирован с gridSize: " + gridSize);

        // Удалите добавление начальных сегментов тела
        // int initialSegments = 2; // Количество начальных сегментов тела
        // for (int i = 0; i < initialSegments; i++)
        // {
        //     Grow();
        // }
    }

    /// <summary>
    /// Перемещает змейку на одну ячейку в текущем направлении.
    /// </summary>
    public void Move()
    {
        // Вычисление новой позиции головы
        Vector2Int newHeadPosition = headCellPosition + direction;

        // Проверка выхода за границы сетки
        if (Mathf.Abs(newHeadPosition.x) > gridSize / 2 || Mathf.Abs(newHeadPosition.y) > gridSize / 2)
        {
            gameManager.OnGameOver();
            return;
        }

        // Проверка столкновения с телом
        foreach (Transform segment in BodySegments)
        {
            Vector2Int segmentPos = GridManager.Instance.WorldToCellPosition(segment.position);
            if (segmentPos == newHeadPosition)
            {
                gameManager.OnGameOver();
                return;
            }
        }

        // Сохранение предыдущей позиции головы
        Vector3 previousHeadPosition = transform.position;

        // Обновление позиции головы
        headCellPosition = newHeadPosition;
        UpdateWorldPosition();

        // Поворот головы
        RotateHead();

        // Перемещение сегментов тела
        MoveBodySegments(previousHeadPosition);

        // Отладочный лог
        Debug.Log("После Move():");
        for (int i = 0; i < bodySegments.Count; i++)
        {
            Debug.Log($"Сегмент {i}: Позиция = {bodySegments[i].position}");
        }
    }

    /// <summary>
    /// Перемещает сегменты тела змейки, следуя за головой.
    /// </summary>
    /// <param name="previousHeadPosition">Предыдущая позиция головы.</param>
    private void MoveBodySegments(Vector3 previousHeadPosition)
    {
        if (bodySegments.Count > 0)
        {
            // Перемещаем первый сегмент к предыдущей позиции головы
            Transform firstSegment = bodySegments[0];
            Vector3 temp = firstSegment.position;
            firstSegment.position = previousHeadPosition;

            // Поворот первого сегмента на основе направления движения
            Vector2Int segmentDirection = new Vector2Int(
                Mathf.RoundToInt(firstSegment.position.x - temp.x),
                Mathf.RoundToInt(firstSegment.position.y - temp.y)
            );
            RotateSegment(firstSegment, segmentDirection);

            // Перемещаем остальные сегменты
            for (int i = 1; i < bodySegments.Count; i++)
            {
                Transform currentSegment = bodySegments[i];
                Vector3 previousSegmentPosition = temp;
                temp = currentSegment.position;
                currentSegment.position = previousSegmentPosition;

                // Поворот текущего сегмента на основе направления движения
                Vector2Int currentDirection = new Vector2Int(
                    Mathf.RoundToInt(currentSegment.position.x - previousSegmentPosition.x),
                    Mathf.RoundToInt(currentSegment.position.y - previousSegmentPosition.y)
                );
                RotateSegment(currentSegment, currentDirection);
            }
        }
    }

    /// <summary>
    /// Обновляет мировую позицию головы змейки.
    /// </summary>
    private void UpdateWorldPosition()
    {
        Vector3 newWorldPos = GridManager.Instance.CellToWorldPosition(headCellPosition);
        transform.position = newWorldPos;
        Debug.Log($"Змейка перемещена в ячейку: {headCellPosition}, мировая позиция: {newWorldPos}");
    }

    /// <summary>
    /// Изменяет направление движения змейки.
    /// </summary>
    /// <param name="newDirection">Новое направление движения.</param>
    public void SetDirection(Vector2Int newDirection)
    {
        // Запрет на смену направления на противоположное
        if (newDirection + direction != Vector2Int.zero)
        {
            direction = newDirection;
            Debug.Log("Направление изменено на: " + direction);
        }
    }

    /// <summary>
    /// Возвращает угол поворота на основе направления.
    /// </summary>
    /// <param name="dir">Направление.</param>
    /// <returns>Угол поворота в градусах.</returns>
    private float GetAngleFromDirection(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
            return 0f;
        if (dir == Vector2Int.right)
            return 90f;
        if (dir == Vector2Int.down)
            return 180f;
        if (dir == Vector2Int.left)
            return 270f;
        return 0f;
    }

    /// <summary>
    /// Поворачивает голову змейки в направлении движения.
    /// </summary>
    private void RotateHead()
    {
        float angle = GetAngleFromDirection(direction);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        Debug.Log($"SnakeController: Голова повернута на {angle} градусов в направлении {direction}.");
    }

    /// <summary>
    /// Поворачивает сегмент тела в заданном направлении.
    /// </summary>
    /// <param name="segment">Сегмент тела.</param>
    /// <param name="segmentDirection">Направление сегмента.</param>
    private void RotateSegment(Transform segment, Vector2Int segmentDirection)
    {
        float angle = GetAngleFromDirection(segmentDirection);
        segment.rotation = Quaternion.Euler(0f, 0f, angle);
        Debug.Log($"SnakeController: Сегмент {segment.name} повернут на {angle} градусов в направлении {segmentDirection}.");
    }

    /// <summary>
    /// Увеличивает длину змейки, добавляя новый сегмент.
    /// </summary>
    public void Grow()
    {
        Vector3 newSegmentPosition;

        if (bodySegments.Count == 0)
        {
            // Если нет сегментов тела, добавляем за головой
            newSegmentPosition = transform.position + new Vector3(-direction.x, -direction.y, 0);
        }
        else
        {
            // Добавляем за последним сегментом (хвостом) на основе направления движения хвоста
            Transform lastSegment = bodySegments[bodySegments.Count - 1];
            Vector3 tailDirectionVector;

            if (bodySegments.Count >= 2)
            {
                Transform secondLastSegment = bodySegments[bodySegments.Count - 2];
                tailDirectionVector = lastSegment.position - secondLastSegment.position;
            }
            else
            {
                tailDirectionVector = lastSegment.position - transform.position;
            }

            // Нормализуем направление и округляем до целых чисел
            Vector2Int tailDirection = new Vector2Int(
                Mathf.RoundToInt(tailDirectionVector.x),
                Mathf.RoundToInt(tailDirectionVector.y)
            );

            // Добавляем направление хвоста без отрицания
            newSegmentPosition = lastSegment.position + new Vector3(tailDirection.x, tailDirection.y, 0);
        }

        // Создаём новый сегмент тела
        GameObject newSegment = Instantiate(bodySegmentPrefab, newSegmentPosition, Quaternion.identity);
        bodySegments.Add(newSegment.transform);

        // Определяем направление для поворота нового сегмента
        Vector2Int segmentDirection;

        if (bodySegments.Count >= 2)
        {
            Transform lastSegment = bodySegments[bodySegments.Count - 1];
            Transform secondLastSegment = bodySegments[bodySegments.Count - 2];
            Vector3 tailDirectionVector = lastSegment.position - secondLastSegment.position;
            segmentDirection = new Vector2Int(
                Mathf.RoundToInt(tailDirectionVector.x),
                Mathf.RoundToInt(tailDirectionVector.y)
            );
        }
        else
        {
            segmentDirection = new Vector2Int(-direction.x, -direction.y);
        }

        // Поворот нового сегмента
        RotateSegment(newSegment.transform, segmentDirection);

        Debug.Log("Snake grew. Total segments: " + bodySegments.Count + " | New Segment Position: " + newSegmentPosition);

        // Обновление спрайтов для хвоста и тела
        UpdateTailSprite();
    }

    /// <summary>
    /// Обрабатывает столкновения с объектами.
    /// </summary>
    /// <param name="collision">Объект столкновения.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            Vector2Int foodPos = GridManager.Instance.WorldToCellPosition(collision.transform.position);
            Debug.Log("Food collected at position: " + foodPos);
            Destroy(collision.gameObject); // Удаляем еду

            // Вызываем метод GameManager для обработки сбора еды
            gameManager.OnFoodCollected(foodPos);

            // Воспроизводим звук сбора еды
            AudioManager.Instance?.PlaySound(SoundType.Eat);
        }
        else
        {
            // Обработка столкновений с другими объектами (например, стенами или собой)
            Debug.Log("Snake collided with object: " + collision.gameObject.name);
            gameManager.OnGameOver();
        }
    }

    /// <summary>
    /// Обновляет спрайты для хвоста и тела змейки.
    /// </summary>
    private void UpdateTailSprite()
    {
        if (bodySegments.Count == 0)
            return;

        // Устанавливаем спрайт хвоста для последнего сегмента
        Transform tail = bodySegments[bodySegments.Count - 1];
        SpriteRenderer tailRenderer = tail.GetComponent<SpriteRenderer>();
        if (tailRenderer != null)
        {
            tailRenderer.sprite = gameManager.tailSprite;
            Debug.Log($"SnakeController: Сегмент {tail.name} установлен как хвост.");
        }

        // Устанавливаем спрайт тела для всех остальных сегментов
        for (int i = 0; i < bodySegments.Count - 1; i++)
        {
            Transform segment = bodySegments[i];
            SpriteRenderer segmentRenderer = segment.GetComponent<SpriteRenderer>();
            if (segmentRenderer != null)
            {
                segmentRenderer.sprite = gameManager.bodySprite;
                Debug.Log($"SnakeController: Сегмент {segment.name} установлен как тело.");
            }
        }
    }

    /// <summary>
    /// Возвращает список занятых позиций змейки.
    /// </summary>
    public List<Vector2Int> GetOccupiedPositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>
        {
            headCellPosition
        };
        foreach (Transform segment in BodySegments)
        {
            Vector2Int pos = GridManager.Instance.WorldToCellPosition(segment.position);
            positions.Add(pos);
        }
        return positions;
    }
}
