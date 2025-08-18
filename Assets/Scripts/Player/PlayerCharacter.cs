using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Components
    public PlayerController PlayerController { get; private set; }
    public PlayerCondition PlayerCondition { get; private set; }

    private PlayerController playerController;
    private PlayerCondition playerCondition;

    [Header("Sound")]
    public AudioClip JumpSound;
    public AudioClip HitSound;
    public AudioClip DeathSound;

    private void Awake()
    {
        GameManager.Player.PlayerCharacter = this;

        playerController = GetComponent<PlayerController>();
        PlayerController = playerController;

        playerCondition = GetComponent<PlayerCondition>();
        PlayerCondition = playerCondition;
    }
}
