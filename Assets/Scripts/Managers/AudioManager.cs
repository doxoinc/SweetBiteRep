using UnityEngine;

public enum SoundType
{
    ButtonClick,
    PieCollect,
    Win,
    Lose,
    Eat,
    Move,
    Purchase // Новый тип звука для покупки
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource sfxSource;            // Для звуковых эффектов
    public AudioSource mainMenuMusicSource;  // Для музыки главного меню
    public AudioSource gameSceneMusicSource; // Для музыки игровой сцены

    [Header("Audio Clips")]
    public AudioClip buttonClickClip;
    public AudioClip pieCollectClip;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip eatClip; // Аудиоклип для звука еды
    public AudioClip moveClip; // Аудиоклип для звука движения
    public AudioClip purchaseClip; // Новый аудиоклип для покупки
    public AudioClip mainMenuMusicClip;       // Музыка главного меню
    public AudioClip gameSceneMusicClip;      // Музыка игровой сцены

    private void Awake()
    {
        // Реализация Singleton паттерна
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Инициализирует аудио настройки при старте игры.
    /// </summary>
    private void InitializeAudioSettings()
    {
        // Проверка наличия DataManager
        if (DataManager.Instance != null)
        {
            bool isSoundOn = DataManager.Instance.IsSoundOn;
            bool isMainMenuMusicOn = DataManager.Instance.IsMainMenuMusicOn;
            bool isGameSceneMusicOn = DataManager.Instance.IsGameSceneMusicOn;

            // Установка состояний звуковых эффектов
            sfxSource.mute = !isSoundOn;

            // Установка состояний музыки главного меню
            if (mainMenuMusicClip != null)
            {
                mainMenuMusicSource.clip = mainMenuMusicClip;
                mainMenuMusicSource.loop = true;
                if (isMainMenuMusicOn)
                {
                    mainMenuMusicSource.Play();
                }
            }
            else
            {
                Debug.LogWarning("MainMenuMusicClip не назначен в AudioManager.");
            }

            // Установка состояний музыки игровой сцены
            if (gameSceneMusicClip != null)
            {
                gameSceneMusicSource.clip = gameSceneMusicClip;
                gameSceneMusicSource.loop = true;
                if (isGameSceneMusicOn)
                {
                    gameSceneMusicSource.Play();
                }
            }
            else
            {
                Debug.LogWarning("GameSceneMusicClip не назначен в AudioManager.");
            }
        }
        else
        {
            Debug.LogError("DataManager не найден. Убедитесь, что DataManager присутствует в сцене.");
        }
    }

    /// <summary>
    /// Воспроизводит звуковой эффект по типу.
    /// </summary>
    /// <param name="soundType">Тип звука для воспроизведения.</param>
    public void PlaySound(SoundType soundType)
    {
        if (DataManager.Instance != null && DataManager.Instance.IsSoundOn)
        {
            if (sfxSource != null)
            {
                switch (soundType)
                {
                    case SoundType.ButtonClick:
                        if (buttonClickClip != null)
                            sfxSource.PlayOneShot(buttonClickClip);
                        else
                            Debug.LogWarning("AudioManager: buttonClickClip не назначен.");
                        break;
                    case SoundType.PieCollect:
                        if (pieCollectClip != null)
                            sfxSource.PlayOneShot(pieCollectClip);
                        else
                            Debug.LogWarning("AudioManager: pieCollectClip не назначен.");
                        break;
                    case SoundType.Win:
                        if (winClip != null)
                            sfxSource.PlayOneShot(winClip);
                        else
                            Debug.LogWarning("AudioManager: winClip не назначен.");
                        break;
                    case SoundType.Lose:
                        if (loseClip != null)
                            sfxSource.PlayOneShot(loseClip);
                        else
                            Debug.LogWarning("AudioManager: loseClip не назначен.");
                        break;
                    case SoundType.Eat:
                        if (eatClip != null)
                            sfxSource.PlayOneShot(eatClip);
                        else
                            Debug.LogWarning("AudioManager: eatClip не назначен.");
                        break;
                    case SoundType.Move:
                        if (moveClip != null)
                            sfxSource.PlayOneShot(moveClip);
                        else
                            Debug.LogWarning("AudioManager: moveClip не назначен.");
                        break;
                    case SoundType.Purchase: // Обработка звука покупки
                        if (purchaseClip != null)
                            sfxSource.PlayOneShot(purchaseClip);
                        else
                            Debug.LogWarning("AudioManager: purchaseClip не назначен.");
                        break;
                    default:
                        Debug.LogWarning($"Неизвестный тип звука: {soundType}");
                        break;
                }
            }
            else
            {
                Debug.LogWarning("SFXSource не назначен в AudioManager.");
            }
        }
        else
        {
            Debug.Log("Звуки отключены. Звук не воспроизведён.");
        }
    }

    /// <summary>
    /// Переключает состояние звуковых эффектов.
    /// </summary>
    /// <param name="isOn">True - включить звуки, False - отключить.</param>
    public void ToggleSound(bool isOn)
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.ToggleSound(isOn);
            sfxSource.mute = !isOn;

            Debug.Log($"Звуковые эффекты {(isOn ? "включены" : "отключены")}.");
        }
        else
        {
            Debug.LogError("DataManager не найден. Убедитесь, что DataManager присутствует в сцене.");
        }
    }

    /// <summary>
    /// Переключает состояние музыки главного меню.
    /// </summary>
    /// <param name="isOn">True - включить музыку, False - отключить.</param>
    public void ToggleMainMenuMusic(bool isOn)
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.ToggleMainMenuMusic(isOn);
            Debug.Log($"ToggleMainMenuMusic called with isOn = {isOn}");

            if (isOn)
            {
                if (mainMenuMusicClip != null && !mainMenuMusicSource.isPlaying)
                {
                    mainMenuMusicSource.Play();
                    Debug.Log("Музыка главного меню включена.");
                }
                else
                {
                    Debug.Log("MainMenuMusicSource уже играет или клип не назначен.");
                }
            }
            else
            {
                Debug.Log("Отключение музыки главного меню...");
                if (mainMenuMusicSource.isPlaying)
                {
                    mainMenuMusicSource.Stop();
                    Debug.Log("Музыка главного меню выключена.");
                }
                else
                {
                    Debug.Log("MainMenuMusicSource уже остановлена.");
                }
            }
        }
        else
        {
            Debug.LogError("DataManager не найден. Убедитесь, что DataManager присутствует в сцене.");
        }
    }

    /// <summary>
    /// Переключает состояние музыки игровой сцены.
    /// </summary>
    /// <param name="isOn">True - включить музыку, False - отключить.</param>
    public void ToggleGameSceneMusic(bool isOn)
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.ToggleGameSceneMusic(isOn);
            Debug.Log($"ToggleGameSceneMusic called with isOn = {isOn}");

            if (isOn)
            {
                if (gameSceneMusicClip != null && !gameSceneMusicSource.isPlaying)
                {
                    gameSceneMusicSource.Play();
                    Debug.Log("Музыка игровой сцены включена.");
                }
                else
                {
                    Debug.Log("GameSceneMusicSource уже играет или клип не назначен.");
                }
            }
            else
            {
                Debug.Log("Отключение музыки игровой сцены...");
                if (gameSceneMusicSource.isPlaying)
                {
                    gameSceneMusicSource.Stop();
                    Debug.Log("Музыка игровой сцены выключена.");
                }
                else
                {
                    Debug.Log("GameSceneMusicSource уже остановлена.");
                }
            }
        }
        else
        {
            Debug.LogError("DataManager не найден. Убедитесь, что DataManager присутствует в сцене.");
        }
    }

    /// <summary>
    /// Устанавливает фоновую музыку главного меню.
    /// </summary>
    /// <param name="newClip">Новый аудиоклип для музыки главного меню.</param>
    public void SetMainMenuMusic(AudioClip newClip)
    {
        if (mainMenuMusicSource.clip == newClip)
        {
            // Если текущий клип уже установлен, ничего не делаем
            return;
        }

        mainMenuMusicSource.Stop();
        mainMenuMusicSource.clip = newClip;
        if (DataManager.Instance != null && DataManager.Instance.IsMainMenuMusicOn && newClip != null)
        {
            mainMenuMusicSource.Play();
        }

        Debug.Log("Музыка главного меню изменена.");
    }

    /// <summary>
    /// Устанавливает фоновую музыку игровой сцены.
    /// </summary>
    /// <param name="newClip">Новый аудиоклип для музыки игровой сцены.</param>
    public void SetGameSceneMusic(AudioClip newClip)
    {
        if (gameSceneMusicSource.clip == newClip)
        {
            // Если текущий клип уже установлен, ничего не делаем
            return;
        }

        gameSceneMusicSource.Stop();
        gameSceneMusicSource.clip = newClip;
        if (DataManager.Instance != null && DataManager.Instance.IsGameSceneMusicOn && newClip != null)
        {
            gameSceneMusicSource.Play();
        }

        Debug.Log("Музыка игровой сцены изменена.");
    }
}
