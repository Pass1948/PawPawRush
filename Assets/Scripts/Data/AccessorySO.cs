using UnityEngine;

public enum AccessoryType
{
    None,
    Hat, // 우선 모자만 사용
    Backpack
}

[CreateAssetMenu(fileName = "New Accessory", menuName = "Customizing/Accessory Item")]
public class AccessorySO : ScriptableObject
{
    public string itemName;
    public AccessoryType type;
    public GameObject prefab; // 액세서리 프리팹
}
