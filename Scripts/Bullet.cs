using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage; // 데미지 변수
    public int per; // 관통 변수

    Rigidbody2D rigid;

    void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per > -1) { // 관통이 무한(-1)보다 큰 경우 즉 유한한 관통이면 (원거리 공격에 한정하여) 속도가 적용됨
            rigid.velocity = dir * 15f; // 속력 15f를 곱하여 날아가는 총알 속도 증가
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (!collision.CompareTag("Enemy") || per == -1) // 부딪힌게 적이 아니거나 무한한 관통인 경우
            return; // 나가기(아래 것 실행하지 않기)
        per--; // 관통값이 하나씩 줄어듦
        if (per == -1) { // (관통값이 줄어들다가) -1에 도달하면
            rigid.velocity = Vector2.zero; // (적이 비활성화되기 이전) 물리 속도 초기화하기
            gameObject.SetActive(false); // 총알을 소멸시킴
        }
    }
}
