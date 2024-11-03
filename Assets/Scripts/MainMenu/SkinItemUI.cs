using UnityEngine;
using UnityEngine.UI;

public class SkinItemUI : MonoBehaviour
{
    [Header("UI Components")]
    public Button skinButton;
    public Image skinImage; // UI Image to display the skin

    private SkinDataSO skinData;
    private bool isPurchased;
    private bool isSelected;

    private void Start()
    {
        if (skinButton == null)
        {
            Debug.LogError("SkinButton is not assigned in the inspector.");
            return;
        }

        if (skinImage == null)
        {
            Debug.LogError("SkinImage is not assigned in the inspector.");
            return;
        }

        // Add listener to the button
        skinButton.onClick.AddListener(OnSkinButtonClicked);

        // Subscribe to skin selection changes
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnSkinSelected += UpdateSkinSelection;
        }
    }

    private void OnDestroy()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnSkinSelected -= UpdateSkinSelection;
        }
    }

    /// <summary>
    /// Sets up the skin item UI with the provided skin data.
    /// </summary>
    /// <param name="data">The SkinDataSO object containing skin information.</param>
    public void Setup(SkinDataSO data)
    {
        skinData = data;
        skinImage.sprite = skinData.SkinSprite;

        // Determine if the skin is purchased
        isPurchased = DataManager.Instance.PurchasedSkins.Contains(skinData.SkinID);

        // Determine if the skin is selected
        isSelected = DataManager.Instance.SelectedSkin == skinData.SkinID;

        UpdateSkinUI();
    }

    /// <summary>
    /// Handles the skin button click event.
    /// </summary>
    private void OnSkinButtonClicked()
    {
        if (isPurchased)
        {
            // If the skin is already purchased, select it
            DataManager.Instance.SelectSkin(skinData.SkinID);
            Debug.Log("Skin selected: " + skinData.SkinID);
        }
        else
        {
            // Attempt to purchase the skin
            if (DataManager.Instance.SpendCoins(skinData.Price))
            {
                DataManager.Instance.PurchaseSkin(skinData.SkinID);
                isPurchased = true;
                Debug.Log("Skin purchased: " + skinData.SkinID);
                
                // Play purchase sound
                AudioManager.Instance?.PlaySound(SoundType.Purchase);

                // Update UI
                UpdateSkinUI();
            }
            else
            {
                // Show notification about insufficient coins
                Debug.Log("Not enough coins to purchase skin: " + skinData.SkinID);
                NotificationManager.Instance?.ShowNotification("Not enough coins to purchase.");
            }
        }

        // Optionally, update all skin UI elements to reflect the new selection
        // This is handled via the OnSkinSelected event
    }

    /// <summary>
    /// Updates the UI elements based on the skin's purchase and selection status.
    /// </summary>
    private void UpdateSkinUI()
    {
        if (isSelected)
        {
            // Apply the selected sprite
            skinImage.sprite = skinData.SelectedSprite;
        }
        else if (isPurchased)
        {
            // Apply the purchased sprite
            skinImage.sprite = skinData.PurchasedSprite;
        }
        else
        {
            // Apply the default sprite
            skinImage.sprite = skinData.SkinSprite;
        }
    }

    /// <summary>
    /// Updates the selection state of the skin item.
    /// </summary>
    /// <param name="selectedSkinId">The ID of the selected skin.</param>
    private void UpdateSkinSelection(string selectedSkinId)
    {
        bool wasSelected = isSelected;
        isSelected = skinData.SkinID == selectedSkinId;

        if (wasSelected != isSelected)
        {
            UpdateSkinUI();
        }
    }
}
