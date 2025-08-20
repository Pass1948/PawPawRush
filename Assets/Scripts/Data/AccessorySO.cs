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
    public int ItemID; // 아이템 ID
    public string ItemName;
    public AccessoryType Type;
    public GameObject Prefab; // 액세서리 프리팹
}
