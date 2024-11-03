using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения полоски вниз

    private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
        {
            MoveDown();
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Найти GameManager и вызвать метод для обработки окончания игры
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.OnFinishLineReached();
            }
            else
            {
                Debug.LogError("GameManager не найден в сцене.");
            }

            // Отключить финишную полоску
            gameObject.SetActive(false);
        }
    }
}
