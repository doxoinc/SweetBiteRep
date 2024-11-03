using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    // События
    public event Action<int> OnCoinCountChanged;
    public event Action<string> OnSkinSelected;
    public event Action<string> OnDifficultyChanged;

    // Данные игрока
    private int coins;
    public int Coins
    {
        get { return coins; }
        private set
        {
            if (coins != value)
            {
                coins = value;
                OnCoinCountChanged?.Invoke(coins);
                SaveData();
            }
        }
    }

    public HashSet<string> PurchasedSkins { get; private set; }
    public string SelectedSkin { get; private set; }

    // Новое поле для сложности
    private DifficultyLevel selectedDifficulty;
    public DifficultyLevel SelectedDifficulty
    {
        get { return selectedDifficulty; }
        private set
        {
            if (selectedDifficulty != value)
            {
                selectedDifficulty = value;
                OnDifficultyChanged?.Invoke(selectedDifficulty.ToString());
                SaveData();
            }
        }
    }

    // Настройки
    public bool IsSoundOn { get; private set; }
    public bool IsMainMenuMusicOn { get; private set; }
    public bool IsGameSceneMusicOn { get; private set; }

    // Новое свойство для хранения статуса последней игры
    public bool IsLastGameVictory { get; set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (Instance == null)
        {
            GameObject dataManagerObject = new GameObject("DataManager");
            dataManagerObject.AddComponent<DataManager>();
            DontDestroyOnLoad(dataManagerObject);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Методы для управления монетами
    public void AddCoins(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("DataManager: Попытка добавить отрицательное количество монет.");
            return;
        }

        Coins += amount;
        Debug.Log($"Монеты добавлены: {amount}. Текущие монеты: {Coins}");
    }

    public bool SpendCoins(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("DataManager: Попытка потратить отрицательное количество монет.");
            return false;
        }

        if (Coins >= amount)
        {
            Coins -= amount;
            Debug.Log($"Монеты потрачены: {amount}. Остаток монет: {Coins}");
            return true;
        }
        Debug.LogWarning("Недостаточно монет для траты.");
        return false;
    }

    public void ResetCoins()
    {
        Coins = 0;
        Debug.Log("Монеты сброшены до 0.");
    }

    // Методы для управления скинами
    public void PurchaseSkin(string skinId)
    {
        if (!PurchasedSkins.Contains(skinId))
        {
            PurchasedSkins.Add(skinId);
            SavePurchasedSkins();
            Debug.Log($"Скин {skinId} куплен.");
        }
    }

    public void SelectSkin(string skinId)
    {
        if (PurchasedSkins.Contains(skinId))
        {
            SelectedSkin = skinId;
            PlayerPrefs.SetString("SelectedSkin", SelectedSkin);
            Debug.Log($"Скин {skinId} выбран.");
            OnSkinSelected?.Invoke(SelectedSkin);
        }
        else
        {
            Debug.LogWarning($"Скин {skinId} не куплен и не может быть выбран.");
        }
    }

    // Новый метод для установки сложности
    public void SetSelectedDifficulty(DifficultyLevel difficulty)
    {
        SelectedDifficulty = difficulty;
        Debug.Log($"Сложность установлена: {SelectedDifficulty}");
    }

    // Методы для настроек
    public void ToggleSound(bool isOn)
    {
        IsSoundOn = isOn;
        PlayerPrefs.SetInt("IsSoundOn", IsSoundOn ? 1 : 0);
        Debug.Log($"Звук {(IsSoundOn ? "включен" : "выключен")}.");
    }

    public void ToggleMainMenuMusic(bool isOn)
    {
        IsMainMenuMusicOn = isOn;
        PlayerPrefs.SetInt("IsMainMenuMusicOn", IsMainMenuMusicOn ? 1 : 0);
        Debug.Log($"Музыка главного меню {(IsMainMenuMusicOn ? "включена" : "выключена")}.");
    }

    public void ToggleGameSceneMusic(bool isOn)
    {
        IsGameSceneMusicOn = isOn;
        PlayerPrefs.SetInt("IsGameSceneMusicOn", IsGameSceneMusicOn ? 1 : 0);
        Debug.Log($"Музыка игровой сцены {(IsGameSceneMusicOn ? "включена" : "выключена")}.");
    }

    // Загрузка данных
    private void LoadData()
    {
        // Монеты
        Coins = PlayerPrefs.GetInt("Coins", 0);

        // Покупки скинов
        PurchasedSkins = new HashSet<string>();
        string skins = PlayerPrefs.GetString("PurchasedSkins", "");
        if (!string.IsNullOrEmpty(skins))
        {
            string[] skinArray = skins.Split(',');
            foreach (var skin in skinArray)
            {
                PurchasedSkins.Add(skin);
            }
        }

        // Выбранный скин
        SelectedSkin = PlayerPrefs.GetString("SelectedSkin", "snake1"); // Замените "snake1" на ваш дефолтный скин

        // Настройки
        IsSoundOn = PlayerPrefs.GetInt("IsSoundOn", 1) == 1;
        IsMainMenuMusicOn = PlayerPrefs.GetInt("IsMainMenuMusicOn", 1) == 1;
        IsGameSceneMusicOn = PlayerPrefs.GetInt("IsGameSceneMusicOn", 1) == 1;

        // Загрузка выбранной сложности
        string difficultyStr = PlayerPrefs.GetString("SelectedDifficulty", "Normal"); // По умолчанию Normal
        if (Enum.TryParse(difficultyStr, out DifficultyLevel difficulty))
        {
            SelectedDifficulty = difficulty;
        }
        else
        {
            SelectedDifficulty = DifficultyLevel.Normal;
            Debug.LogWarning($"Не удалось распознать сложность '{difficultyStr}'. Установлена сложность по умолчанию: {SelectedDifficulty}");
        }

        // Загрузка статуса последней игры
        IsLastGameVictory = PlayerPrefs.GetInt("IsLastGameVictory", 1) == 1; // По умолчанию победа

        Debug.Log("Данные загружены из PlayerPrefs.");
    }

    // Сохранение покупок скинов
    private void SavePurchasedSkins()
    {
        string skins = string.Join(",", PurchasedSkins);
        PlayerPrefs.SetString("PurchasedSkins", skins);
        Debug.Log("Покупки скинов сохранены.");
    }

    // Сохранение всех данных
    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.SetString("SelectedSkin", SelectedSkin);
        // Покупки скинов сохраняются отдельно в методе SavePurchasedSkins()
        PlayerPrefs.SetInt("IsSoundOn", IsSoundOn ? 1 : 0);
        PlayerPrefs.SetInt("IsMainMenuMusicOn", IsMainMenuMusicOn ? 1 : 0);
        PlayerPrefs.SetInt("IsGameSceneMusicOn", IsGameSceneMusicOn ? 1 : 0);
        PlayerPrefs.SetString("SelectedDifficulty", SelectedDifficulty.ToString());
        PlayerPrefs.SetInt("IsLastGameVictory", IsLastGameVictory ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Все данные сохранены в PlayerPrefs.");
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}

public enum DifficultyLevel
{
    Easy,
    Normal,
    Hard
}