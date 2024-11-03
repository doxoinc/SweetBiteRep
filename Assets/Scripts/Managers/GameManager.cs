using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Object Pool")]
    public ObjectPool objectPool; // Ссылка на объектный пул
    public float spawnInterval = 1f; // Интервал спавна пирогов

    [Header("UI Elements")]
    public GameObject flavorSelectionPanel; // Панель выбора вкуса
    public GameObject victoryPanel;          // Панель Victory
    public GameObject lostPanel;             // Панель Lost

    [Header("Game Settings")]
    public float gameDuration = 30f; // Длительность игры в секундах
    public int requiredScore = 10;    // Необходимое количество пирогов для победы

    [Header("Finish Line")]
    public GameObject finishLinePrefab; // Префаб финишной полоски
    private GameObject finishLineInstance;

    private PieFlavor selectedFlavor;
    private float timer;
    private bool isGameActive = false;

    private UIManager uiManager; // Ссылка на UIManager текущей сцены

    private PlayerController playerController; // Ссылка на PlayerController

    // Ссылки на контроллеры панелей
    private VictoryPanelController victoryPanelController;
    private LostPanelController lostPanelController;

    private void Start()
    {
        // Найти UIManager в текущей сцене
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager не найден в текущей сцене.");
            return;
        }

        // Найти PlayerController в сцене
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController не найден в сцене.");
            return;
        }

        // Найти контроллеры панелей через UIManager
        if (victoryPanel != null)
        {
            victoryPanelController = victoryPanel.GetComponent<VictoryPanelController>();
            if (victoryPanelController == null)
            {
                Debug.LogError("VictoryPanelController не найден на VictoryPanel.");
            }
        }
        else
        {
            Debug.LogWarning("VictoryPanel не назначен в GameManager.");
        }

        if (lostPanel != null)
        {
            lostPanelController = lostPanel.GetComponent<LostPanelController>();
            if (lostPanelController == null)
            {
                Debug.LogError("LostPanelController не найден на LostPanel.");
            }
        }
        else
        {
            Debug.LogWarning("LostPanel не назначен в GameManager.");
        }

        ShowFlavorSelection();
    }

    // Показать панель выбора вкуса
    private void ShowFlavorSelection()
    {
        if (flavorSelectionPanel != null)
        {
            flavorSelectionPanel.SetActive(true);
            Debug.Log("Панель выбора вкуса активирована.");
        }
        else
        {
            Debug.LogWarning("FlavorSelectionPanel не назначен в GameManager.");
        }
    }

    // Метод, вызываемый кнопками выбора вкуса
    public void SelectFlavor(string flavor)
    {
        if (System.Enum.TryParse(flavor, out PieFlavor parsedFlavor))
        {
            selectedFlavor = parsedFlavor;
            flavorSelectionPanel.SetActive(false);
            Debug.Log($"Выбран вкус: {selectedFlavor}");
            StartGame();
        }
        else
        {
            Debug.LogError($"Неизвестный вкус пирога: {flavor}");
        }
    }

    // Начать игру
    private void StartGame()
    {
        isGameActive = true;
        timer = gameDuration;
        Debug.Log("Игра началась.");

        // Сбросить счёт через PlayerController
        if (playerController != null)
        {
            playerController.ResetScore();
            Debug.Log("Счёт игрока сброшен.");
        }
        else
        {
            Debug.LogWarning("PlayerController не найден в GameManager.");
        }

        // **Удалите или закомментируйте этот блок, чтобы не сбрасывать монеты при начале игры**
        /*
        if (DataManager.Instance != null)
        {
            DataManager.Instance.ResetCoins();
            Debug.Log("Монеты сброшены через DataManager.");
        }
        else
        {
            Debug.LogError("DataManager не найден в сцене.");
        }
        */

        StartCoroutine(SpawnPies());
        StartCoroutine(GameTimer());

        // Создать финишную полоску
        if (finishLinePrefab != null)
        {
            // Разместить полоску выше экрана, чтобы она потом двигалась вниз
            finishLineInstance = Instantiate(finishLinePrefab, new Vector3(0f, Camera.main.orthographicSize + 10f, 0f), Quaternion.identity);
            Debug.Log("Финишная полоска создана.");
        }
        else
        {
            Debug.LogWarning("FinishLinePrefab не назначен в GameManager.");
        }
    }

    // Корутина для спавна пирогов
    private IEnumerator SpawnPies()
    {
        while (isGameActive)
        {
            SpawnPie();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Спавн одного пирога
    private void SpawnPie()
    {
        if (objectPool == null)
        {
            Debug.LogError("ObjectPool не назначен в GameManager.");
            return;
        }

        GameObject pieObject = objectPool.GetPooledPie();
        if (pieObject == null)
        {
            Debug.LogError("Пул объектов не вернул пирог.");
            return;
        }

        Pie pie = pieObject.GetComponent<Pie>();
        if (pie != null)
        {
            // Рандомное расположение по X
            float xPos = Random.Range(-Camera.main.orthographicSize * Camera.main.aspect + 1f,
                                       Camera.main.orthographicSize * Camera.main.aspect - 1f);
            Vector3 spawnPosition = new Vector3(xPos, Camera.main.orthographicSize + 1f, 0f);
            pieObject.transform.position = spawnPosition;
            pieObject.SetActive(true);

            // Выбор случайного вкуса
            PieFlavor randomFlavor = (PieFlavor)Random.Range(0, System.Enum.GetValues(typeof(PieFlavor)).Length);
            pie.SetFlavor(randomFlavor);
            Debug.Log($"Спавн пирога: {randomFlavor} в позиции {spawnPosition}");
        }
        else
        {
            Debug.LogError("Pie компонент не найден на пироге из ObjectPool.");
        }
    }

    // Корутина для таймера игры
    private IEnumerator GameTimer()
    {
        while (timer > 0f)
        {
            timer -= 1f;
            UpdateTimerText();
            yield return new WaitForSeconds(1f);
        }

        EndGame();
    }

    // Обновить текст таймера (будет скрыт)
    private void UpdateTimerText()
    {
        // Поскольку таймер удалён из UI, этот метод можно оставить пустым или удалить
    }

    // Завершить игру
    private void EndGame()
    {
        isGameActive = false;
        Debug.Log("Игра завершена.");

        // Отключить спавн пирогов
        StopAllCoroutines();
        Debug.Log("Все корутины остановлены.");

        // Запустить движение финишной полоски
        if (finishLineInstance != null)
        {
            FinishLine finishLineScript = finishLineInstance.GetComponent<FinishLine>();
            if (finishLineScript != null)
            {
                finishLineScript.StartMoving();
                Debug.Log("Финишная полоска начала движение.");
            }
            else
            {
                Debug.LogWarning("FinishLine скрипт не найден на FinishLineInstance.");
                // Если финишная полоска не имеет скрипта FinishLine, сразу определить результат
                DetermineGameResult();
            }
        }
        else
        {
            Debug.LogWarning("finishLineInstance равен null.");
            // Если финишная полоска не была создана, сразу определить результат
            DetermineGameResult();
        }
    }

    // Метод для определения результата игры и отображения панели
    public void DetermineGameResult()
    {
        if (playerController != null)
        {
            int finalScore = playerController.GetScore();
            // Вычисляем монеты как половину очков (2 очка = 1 монета)
            int coinsWon = finalScore / 2;

            // Обновляем DataManager с новым количеством монет
            if (DataManager.Instance != null)
            {
                DataManager.Instance.AddCoins(coinsWon);
                Debug.Log($"Монет добавлено: {coinsWon}. Всего монет: {DataManager.Instance.Coins}");
            }
            else
            {
                Debug.LogError("DataManager не найден в сцене.");
            }

            // Сохранить результаты
            SaveResults(finalScore, coinsWon);

            if (finalScore >= requiredScore)
            {
                // Победа
                if (uiManager != null)
                {
                    if (victoryPanelController != null)
                    {
                        victoryPanelController.SetResults(coinsWon, isVictory: true);
                    }
                    else
                    {
                        Debug.LogWarning("VictoryPanelController не найден.");
                    }

                    // Вызов звука победы
                    AudioManager.Instance.PlaySound(SoundType.Win);

                    uiManager.OpenPanel(PanelType.Victory);
                    Debug.Log("Открываем VictoryPanel через UIManager.");
                }
                else
                {
                    Debug.LogError("UIManager не найден в текущей сцене.");
                }
            }
            else
            {
                // Поражение
                if (uiManager != null)
                {
                    if (victoryPanelController != null)
                    {
                        victoryPanelController.SetResults(coinsWon, isVictory: false);
                    }
                    else
                    {
                        Debug.LogWarning("VictoryPanelController не найден.");
                    }

                    // Вызов звука поражения
                    AudioManager.Instance.PlaySound(SoundType.Lose);

                    uiManager.OpenPanel(PanelType.Victory); // Используем VictoryPanel для отображения поражения
                    Debug.Log("Открываем VictoryPanel через UIManager.");
                }
                else
                {
                    Debug.LogError("UIManager не найден в текущей сцене.");
                }
            }
        }
        else
        {
            Debug.LogWarning("PlayerController не найден. Результат игры не может быть определён.");
        }
    }

    // Метод, вызываемый FinishLine.cs после достижения финиша
    public void OnFinishLineReached()
    {
        DetermineGameResult();
    }

    // Метод, вызываемый при сборе пирога
    public void CollectPie(PieFlavor flavor)
    {
        Debug.Log($"Собран пирог: {flavor}");
        if (flavor == selectedFlavor)
        {
            if (playerController != null)
            {
                playerController.AddScore(1);
                Debug.Log($"Счёт увеличен до: {playerController.GetScore()}");
            }
            else
            {
                Debug.LogWarning("PlayerController не найден.");
            }

            // Воспроизведение звука сбора пирога только для нужного пирога
            AudioManager.Instance.PlaySound(SoundType.PieCollect);
        }
    }

    // Метод для сохранения результатов с использованием PlayerPrefs
    public void SaveResults(int finalScore, int finalCoins)
    {
        Debug.Log("Начало сохранения результатов.");

        // Сохраняем счёт
        PlayerPrefs.SetInt("LastScore", finalScore);
        Debug.Log($"LastScore сохранен: {finalScore}");

        // Сохраняем монеты через DataManager
        // Предполагается, что DataManager уже сохраняет монеты при добавлении через AddCoins

        // Сохраняем лучший счёт
        if (PlayerPrefs.HasKey("BestScore"))
        {
            if (finalScore > PlayerPrefs.GetInt("BestScore"))
            {
                PlayerPrefs.SetInt("BestScore", finalScore);
                Debug.Log($"BestScore обновлен: {finalScore}");
            }
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", finalScore);
            Debug.Log($"BestScore установлен: {finalScore}");
        }

        // Сохраняем лучшие монеты
        if (PlayerPrefs.HasKey("BestCoins"))
        {
            if (finalCoins > PlayerPrefs.GetInt("BestCoins"))
            {
                PlayerPrefs.SetInt("BestCoins", finalCoins);
                Debug.Log($"BestCoins обновлены: {finalCoins}");
            }
        }
        else
        {
            PlayerPrefs.SetInt("BestCoins", finalCoins);
            Debug.Log($"BestCoins установлены: {finalCoins}");
        }

        PlayerPrefs.Save();
        Debug.Log("Результаты сохранены с помощью PlayerPrefs.");
    }

    // Метод для перезапуска игры
    public void RestartGame()
    {
        // Перезагрузить текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Игра перезапущена.");
    }

    // Метод для выхода в главное меню
    public void ExitGame()
    {
        // Загрузить сцену главного меню (замените "MainMenu" на имя вашей сцены главного меню)
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Переход в главное меню.");
    }
}
