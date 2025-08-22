using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerCharacter PlayerCharacter { get; set; }

    // 게임에 존재하는 모든 악세사리 리스트
    public Dictionary<int, AccessorySO> AllAccessoriesDict { get; private set; }

    public int SavedEquippedAccessoryID { get; private set; }
    public const int UNEQUIPPED_ID = -1; // 악세사리 미착용 아이디

    private void Awake()
    {
        // 리소스를 미리 불러오는것 보다 필요할때 불러오자
        // key 값이 필요했다면, 프리팹은 빼고 불러오던 하는게 좋음
        var loadedAccessories = GameManager.Resource.LoadAll<AccessorySO>("Data/AccessorySO");

        
        AllAccessoriesDict = new Dictionary<int, AccessorySO>();
        foreach (var accessory in loadedAccessories)
        {
            // 딕셔너리의 특성을 잘 이용
            if (!AllAccessoriesDict.ContainsKey(accessory.ItemID))
            {
                AllAccessoriesDict.Add(accessory.ItemID, accessory);
            }
            else
            {
                // ID는 반드시 고유해야 함
                Debug.LogWarning($"중복된 아이템 ID({accessory.ItemID})가 존재합니다: {accessory.name}");
            }
        }

        // PlayerPrefs에서 악세사리 ID를 불러옴. 없으면 미착용 아이디
        // 프리팹 키값 상수화
        SavedEquippedAccessoryID = PlayerPrefs.GetInt("EquippedAccessoryID", UNEQUIPPED_ID);
    }

    public AccessorySO GetAccessoryByID(int itemID)
    {
        if (AllAccessoriesDict.TryGetValue(itemID, out AccessorySO accessory))
        {
            return accessory;
        }

        return null;
    }

    // UI에서 호출. 플레이어의 선택을 변경/저장
    public void ChangeAndSaveEquippedAccessory(int itemID)
    {
        // 딕셔너리에 해당 ID의 아이템이 있는지 확인
        if (!AllAccessoriesDict.ContainsKey(itemID))
        {
            Debug.LogWarning($"존재하지 않는 악세사리 ID입니다: {itemID}");
            return;
        }

        int idToSave;
        AccessorySO accessoryToUpdate;

        // 요청된 ID가 현재 착용 ID와 같다면 해제, 다르다면 장착
        if (itemID == SavedEquippedAccessoryID)
        {
            idToSave = UNEQUIPPED_ID; // 미착용 상태로 변경
            accessoryToUpdate = null; // 캐릭터에게 null 전달 -> 착용 해제
            Debug.Log($"악세사리 해제: ID {itemID}");
        }
        else
        {
            idToSave = itemID; // 새로운 아이템 ID로 변경
            accessoryToUpdate = AllAccessoriesDict[itemID];
            Debug.Log($"악세사리 변경: ID {itemID}");
        }

        // 선택된 ID 저장
        SavedEquippedAccessoryID = idToSave;
        PlayerPrefs.SetInt("EquippedAccessoryID", SavedEquippedAccessoryID);

        // 현재 씬에 플레이어 캐릭터가 존재하면 즉시 외형 업데이트
        if (PlayerCharacter != null)
        {
            PlayerCharacter.UpdateAccessory(accessoryToUpdate);
        }
    }
}
