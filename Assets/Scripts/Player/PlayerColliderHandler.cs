using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderHandler : MonoBehaviour
{
    private BoxCollider boxCollider;

    private readonly Vector3 slidingColliderScale = new Vector3(1.0f, 0.5f, 1.0f);
    private readonly Vector3 notSlidingColliderScale = new Vector3(1.0f, 2.0f, 1.0f);

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Slide(bool isSliding)
    {
        if (isSliding)
        {
            boxCollider.size = Vector3.Scale(boxCollider.size, slidingColliderScale);
            boxCollider.center = boxCollider.center - new Vector3(0.0f, boxCollider.size.y * 0.5f, 0.0f);
        }
        else
        {
            boxCollider.center = boxCollider.center + new Vector3(0.0f, boxCollider.size.y * 0.5f, 0.0f);
            boxCollider.size = Vector3.Scale(boxCollider.size, notSlidingColliderScale);
        }
    }
}
