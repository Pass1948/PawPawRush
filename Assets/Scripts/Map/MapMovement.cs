using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public float movementSpeed = 10f; //  속도
    public Transform obstacleParent;

    public void MoveMap()
    {
        if (obstacleParent != null)
        {
            foreach (Transform obstacle in obstacleParent)
            {
                obstacle.Translate(Vector3.back * (movementSpeed * Time.deltaTime));
            }
        }
    }
}