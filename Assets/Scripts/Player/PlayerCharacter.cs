using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Components
    public PlayerController PlayerController { get; private set; }
    public PlayerCondition PlayerCondition { get; private set; }
    public PlayerCustomizingHandler CustomizingHandler { get; private set; }

    private PlayerController playerController;
    private PlayerCondition playerCondition;
    private PlayerCustomizingHandler customizingHandler;

    [Header("Sound")]
    public AudioClip JumpSound;
    public AudioClip HitSound;
    public AudioClip DeathSound;

    // 현재 착용 악세사리
    public AccessorySO CurrentAccessory { get; private set; }

    private void Awake()
    {
        GameManager.Player.PlayerCharacter = this;

        playerController = GetComponent<PlayerController>();
        PlayerController = playerController;

        playerCondition = GetComponent<PlayerCondition>();
        PlayerCondition = playerCondition;

        customizingHandler = GetComponent<PlayerCustomizingHandler>();
        CustomizingHandler = customizingHandler;
    }

    private void Start()
    {
        Init();

        // 테스트용: 특정 ID 악세사리 착용
        //GameManager.Player.ChangeAndSaveEquippedAccessory(1);
    }

    public void Init()
    {
        int savedID = GameManager.Player.SavedEquippedAccessoryID;

        AccessorySO accessoryToEquip = GameManager.Player.GetAccessoryByID(savedID);

        if (accessoryToEquip != null)
        {
            UpdateAccessory(accessoryToEquip);
        }
        else
        {
            Debug.LogWarning($"ID({savedID})에 해당하는 악세사리를 찾을 수 없어 착용하지 않습니다.");
        }

    }

    public void UpdateAccessory(AccessorySO accessory)
    {
        // accessory가 null이면 장착 해제
        if (accessory == null)
        {
            // 현재 착용 중인 악세사리가 있었다면 해제
            if (CurrentAccessory != null)
            {
                CustomizingHandler.UnequipAccessory(CurrentAccessory.Type);
                CurrentAccessory = null;
                Debug.Log("악세사리를 해제했습니다.");
            }

            return;
        }

        CurrentAccessory = accessory;
        CustomizingHandler.EquipAccessory(CurrentAccessory);

        Debug.Log($"{gameObject.name} 캐릭터가 ID:{accessory.ItemID} / 이름:{accessory.name} 을(를) 착용했습니다.");
    }
}
