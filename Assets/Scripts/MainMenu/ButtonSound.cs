using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("ButtonSound должен быть прикреплён к объекту с компонентом Button.");
            return;
        }

        // Добавление слушателя нажатия кнопки
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        // Проверка наличия AudioManager и DataManager
        if (AudioManager.Instance != null && DataManager.Instance != null)
        {
            // Проверка, включены ли звуки в настройках
            if (DataManager.Instance.IsSoundOn)
            {
                AudioManager.Instance.PlaySound(SoundType.ButtonClick);
            }
            else
            {
                Debug.Log("Звуки отключены. Звук кнопки не воспроизведён.");
            }
        }
        else
        {
            if (AudioManager.Instance == null)
            {
                Debug.LogError("AudioManager не найден в сцене. Убедитесь, что AudioManager присутствует и инициализирован.");
            }

            if (DataManager.Instance == null)
            {
                Debug.LogError("DataManager не найден в сцене. Убедитесь, что DataManager присутствует и инициализирован.");
            }
        }
    }

    private void OnDestroy()
    {
        // Удаление слушателя при уничтожении объекта
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}
