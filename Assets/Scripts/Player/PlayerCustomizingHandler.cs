using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomizingHandler : MonoBehaviour
{
    [Header("Sockets")]
    [SerializeField] private Transform hatSocket;

    // 현재 착용 중인 아이템을 관리하는 딕셔너리
    private Dictionary<AccessoryType, GameObject> equippedAccessories = new Dictionary<AccessoryType, GameObject>();

    // 새로운 액세서리 장착
    public void EquipAccessory(AccessorySO item)
    {
        // 이미 해당 타입의 액세서리를 착용 중이라면 먼저 해제
        if (equippedAccessories.ContainsKey(item.Type))
        {
            UnequipAccessory(item.Type);
        }

        // 아이템 타입에 맞는 소켓을 찾음
        Transform targetSocket = GetSocket(item.Type);
        if (targetSocket == null)
        {
            Debug.LogWarning($"Socket for {item.Type} not found!");
            return;
        }

        // TODO: 추후에 리소스 매니저로 인스턴스화
        // 프리팹을 소켓의 자식으로 인스턴스화
        GameObject accessoryInstance = Instantiate(item.Prefab, targetSocket);

        // 착용 중인 아이템 목록에 추가
        equippedAccessories[item.Type] = accessoryInstance;
    }

    // 액세서리 해제
    public void UnequipAccessory(AccessoryType type)
    {
        if (equippedAccessories.TryGetValue(type, out GameObject accessoryInstance))
        {
            Destroy(accessoryInstance);
            equippedAccessories.Remove(type);
        }
    }

    private Transform GetSocket(AccessoryType type)
    {
        switch (type)
        {
            case AccessoryType.Hat:
                return hatSocket;
            default:
                return null;
        }
    }
}
