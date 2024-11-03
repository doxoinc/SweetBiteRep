using UnityEngine;

public enum SkinType
{
    Snake,
    Plate
}

[CreateAssetMenu(fileName = "NewSkinData", menuName = "Shop/Skin Data")]
public class SkinDataSO : ScriptableObject
{
    public string SkinID;
    public string SkinName;
    public int Price;
    public SkinType Type;
    public Sprite SkinSprite;       // Default sprite
    public Sprite PurchasedSprite;  // Sprite when purchased
    public Sprite SelectedSprite;   // Sprite when selected
}
