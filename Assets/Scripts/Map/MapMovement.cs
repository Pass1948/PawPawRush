using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public float movementSpeed = 10f; // 게임 속도

    private void Awake()
    {
        //GameManager.Map.MapMovement = this; // MapMovement를 MapManager에 등록
    }

    public void Move()// 오브젝트 이동
    {
        transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
    }
}