using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    [Header("UI Elements")]
    public Text notificationText; // Reference to the Text component for notifications

    [Header("Animation Settings")]
    public float bubbleScale = 1.2f; // Scale factor for the bubble effect
    public float bubbleDuration = 0.3f; // Duration for the bubble scaling
    public float fadeDuration = 2f; // Duration for the fade-out effect

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (notificationText == null)
            {
                Debug.LogError("NotificationText is not assigned in the inspector.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Displays a notification with a bubble effect and fades it out.
    /// </summary>
    /// <param name="message">The message to display.</param>
    public void ShowNotification(string message)
    {
        if (notificationText == null)
        {
            Debug.LogError("NotificationText is not assigned.");
            return;
        }

        StopAllCoroutines(); // Stop any existing notifications
        StartCoroutine(DisplayNotification(message));
    }

    private IEnumerator DisplayNotification(string message)
    {
        // Set the message
        notificationText.text = message;
        notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, 1f);
        notificationText.transform.localScale = Vector3.zero;
        notificationText.enabled = true;

        // Bubble scaling up
        float elapsed = 0f;
        while (elapsed < bubbleDuration)
        {
            float scale = Mathf.Lerp(0f, bubbleScale, elapsed / bubbleDuration);
            notificationText.transform.localScale = new Vector3(scale, scale, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        notificationText.transform.localScale = new Vector3(bubbleScale, bubbleScale, 1f);

        // Bubble scaling back to normal
        elapsed = 0f;
        while (elapsed < bubbleDuration)
        {
            float scale = Mathf.Lerp(bubbleScale, 1f, elapsed / bubbleDuration);
            notificationText.transform.localScale = new Vector3(scale, scale, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        notificationText.transform.localScale = Vector3.one;

        // Fade out
        float alpha = 1f;
        elapsed = 0f;
        Color originalColor = notificationText.color;
        while (elapsed < fadeDuration)
        {
            alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            notificationText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        notificationText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        notificationText.enabled = false;
    }
}
