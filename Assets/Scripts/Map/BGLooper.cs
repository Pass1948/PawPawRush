using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 이전에 사용되던 BGLooper와 달리, 이 스크립트는 발판이나 장애물을 직접 삭제하지 않습니다.
        // 배경 오브젝트가 트리거에 닿으면, 위치만 다시 앞으로 옮겨주는 역할만 합니다.
        // 이 로직은 배경 오브젝트의 종류에 따라 다르게 구현될 수 있습니다.
        // 따라서 기존에 제시된 코드를 그대로 유지하되, 역할이 다르다는 점만 인지하시면 됩니다.
    }
}
