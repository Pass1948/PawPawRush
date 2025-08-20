using UnityEngine;
using UnityEngine.UI;

public class HeartIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite fullSprite;
    [SerializeField] Sprite emptySprite;

    public void Set(bool filled)
    {
        image.sprite = filled ? fullSprite : emptySprite;
    }
}