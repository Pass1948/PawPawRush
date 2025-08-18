using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public float movementSpeed = 10f; // 게임 속도

    void Update()
    {
        // 매 프레임마다 오브젝트를 뒤로 이동
        transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
    }
}