using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); // 원형의 캐스트를 쏴서 감지함
        // 캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어
        nearestTarget = GetNearest(); // 가장 가까운 타겟을 저장함
    }
    
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100; // 아무 값이나 넣기

        foreach (RaycastHit2D target in targets){ // 반복해라
            Vector3 myPos = transform.position; // 나의 위치
            Vector3 targetPos = target.transform.position; // 적의 위치
            float curDiff = Vector3.Distance(myPos, targetPos);
            if (curDiff < diff){ // 나의 위치와 적의 위치 간 차이가 diff보다 작으면
                diff = curDiff; // diff에 나의 위치와 적의 위치 간 차이를 넣는 방식
                // 즉, 이것들을 반복하면 가장 가까운 거리의 적이 타겟팅됨
                result = target.transform; // 그리고 그러한 가장 가까운 적을 결과로
            }
        }
        return result; // 리턴하겠다는 의미
    }
}
