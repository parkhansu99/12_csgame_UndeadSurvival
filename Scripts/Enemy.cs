using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    bool isLive = true; // 기본값은 살아있어야 하기 때문
    Animator anim;
    Rigidbody2D rigid;
    
    SpriteRenderer spriter;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() 
    {
        if (!isLive)
            return; // 죽었다면 아래를 실행하지 말고 나가라
        
        // 방향은 위치 차이의 정규화(normalized)
        Vector2 dirVec = target.position - rigid.position; // 위치 차이 = 타겟(Player)위치 - 나(Enemy)의 위치
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // 위치 차이의 정규화 * 스피드 * 프레임에 따라 달라지지 않도록 하는 것
        rigid.MovePosition(rigid.position + nextVec); // 현재 위치에서 다음 위치로 간다는 것
        rigid.velocity = Vector2.zero; // 물리 속도(부딪히더라도)가 이동에 영향을 주지 않도록 속도 제거하기
        // 속도를 0보다 크게 지정해야 적이 움직임
    }
    void LateUpdate() 
    {
        if (!isLive)
            return; // 죽었다면 아래를 실행하지 말고 나가라
        spriter.flipX = target.position.x < rigid.position.x; // 타겟(Player)위치 < 나(Enemy)의위치이면 FlipX를 하라는 것
    }

    void OnEnable() // Enemy를 Prefab으로 만드는 과정에서 target을 Player 오브젝트로 설정해둔 것이 유실되는 문제 해결 : GameManager에서 선언된 변수를 자동으로 가져오는 것
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true; // 스폰되면 살아 있어야 하기 때문
        health = maxHealth; // 스폰되면 최대 체력으로 부활하기 때문
    } 

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Bullet") || !isLive) // 무기와 충돌한 경우가 아니거나 살아있지 않은 경우
            return; // 나가기
        health -= collision.GetComponent<Bullet>().damage; // health에서 damage의 값을 빼는 것
        if (health > 0) { // 살았다면
            anim.SetTrigger("Hit");
        } else {  // 죽었다면
            isLive = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            // Dead(); // Animation에서 GUI로 추가할 것 : 애초에 없어도 작동은 함
        }
    }

    void Dead() 
    {
        gameObject.SetActive(false); // Dead가 호출되면 현재 Cs(Enemy)를 비활성화(없애기)
    }
}
