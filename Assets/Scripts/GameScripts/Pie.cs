using UnityEngine;

public enum PieFlavor
{
    Strawberry,
    Kiwi,
    Blueberry
}

public class Pie : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f; // Скорость падения пирога

    [Header("Flavor Settings")]
    public PieFlavor flavor; // Вкус пирога

    private GameManager gameManager; // Ссылка на GameManager
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Поиск GameManager на сцене
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError($"GameManager не найден на сцене для объекта {gameObject.name}.");
        }

        // Кэширование SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer не найден на объекте {gameObject.name}.");
        }
    }

    private void Update()
    {
        MoveDown();
    }

    /// <summary>
    /// Движение пирога вниз.
    /// </summary>
    private void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Деактивация пирога, если он вышел за нижнюю границу экрана
        if (transform.position.y < -Camera.main.orthographicSize - 1f)
        {
            gameObject.SetActive(false);
            Debug.Log($"Пирог {flavor} деактивирован, вышел за пределы экрана.");
        }
    }

    /// <summary>
    /// Обработка столкновений с игроком.
    /// </summary>
    /// <param name="collision">Столкновение с другим объектом.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.CollectPie(flavor);
                // Вызов звука сбора пирога перемещен в GameManager
                // AudioManager.Instance.PlaySound(SoundType.PieCollect); // Удалено
            }
            else
            {
                Debug.LogError($"GameManager не назначен для объекта {gameObject.name}.");
            }
            gameObject.SetActive(false);
            Debug.Log($"Пирог {flavor} собран игроком и деактивирован.");
        }
    }

    /// <summary>
    /// Устанавливает вкус пирога и соответствующий спрайт.
    /// </summary>
    /// <param name="newFlavor">Новый вкус пирога.</param>
    public void SetFlavor(PieFlavor newFlavor)
    {
        flavor = newFlavor;

        if (spriteRenderer != null && gameManager != null && gameManager.objectPool != null)
        {
            switch (flavor)
            {
                case PieFlavor.Strawberry:
                    spriteRenderer.sprite = gameManager.objectPool.pieSprites[0];
                    break;
                case PieFlavor.Kiwi:
                    spriteRenderer.sprite = gameManager.objectPool.pieSprites[1];
                    break;
                case PieFlavor.Blueberry:
                    spriteRenderer.sprite = gameManager.objectPool.pieSprites[2];
                    break;
                default:
                    spriteRenderer.sprite = gameManager.objectPool.pieSprites[0];
                    break;
            }
            Debug.Log($"Пирог установлен на вкус {flavor} с соответствующим спрайтом.");
        }
        else
        {
            Debug.LogError($"Недостаточно данных для установки спрайта пирога на объекте {gameObject.name}.");
        }
    }
}
