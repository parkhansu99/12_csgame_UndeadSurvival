using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩을 보관할 변수
    public GameObject[] prefabs;
    // 풀 담당을 하는 리스트들
    List<GameObject>[] pools;
    private void Awake() 
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length ; index++){
            pools[index] = new List<GameObject>();
        } // for (시작문 ; 조건문 ; 증감문)
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // 선택한 풀의 놀고 있는(비활성화된) 게임오브젝트 접근
        foreach (GameObject item in pools[index]){
            if (!item.activeSelf) { // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true); // 필드에 나가도록
                break;
            }
        }
        
        if (select == null) { // 못 찾으면 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform); // Instantiate는 원본 오브젝트를 복제하여 장면에 생성하는 함수 (게임 오브젝트, transform)
            pools[index].Add(select); // 복제된 원본 오브젝트가 저장된 select를 pools에 추가하기
        }
        return select;
    }
}
