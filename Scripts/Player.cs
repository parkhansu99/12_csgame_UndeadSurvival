using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // InputSystem을 사용하기

public class Player : MonoBehaviour
{
    public Vector2 inputVec; // 방향(키에 따른 위치)를 입력받을 변수 선언 (public으로 하면 GUI로 확인 가능)
    public float speed; // 속도를 편하게 관리하도록 변수 추가
    public Scanner scanner;
    Rigidbody2D rigid; // rigidbody를 저장할 변수를 선언
    SpriteRenderer spriter; // sprite라는 변수를 선언 (방향 전환)    
    Animator anim;


    private void Awake() { // 시작할 때 단 한번만 실행되는 생명주기 Awake
        rigid = GetComponent<Rigidbody2D>(); // (오브젝트[캐릭터, 적, 무기 등]에 적용된) Rigidbody2D 컴포넌트(동작을 관장하는 매크로)를 가져오는 함수
        spriter = GetComponent<SpriteRenderer>(); // 컴포넌트 SpriteRenderer를 가져오는 함수
        anim = GetComponent<Animator>(); // 컴포넌트 Animator를 가져오는 함수
        scanner = GetComponent<Scanner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    void FixedUpdate() { // 물리 연산 프레임마다 호출되는 생명주기 FixedUpdate
        // 1. 힘을 준다
        // rigid.AddForce(inputVec);

        // 2. 속도 제어
        // rigid.velocity = inputVec;

        // 3. 위치 이동 (우리가 사용할 것)
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // 어떤 상황에도 똑같은 속력(normalized는 피타고라스(대각선 이동) 정리), fixedDeltaTime(FixedUpdate라서 fixed가 붙음)
        // 단, speed 값을 Unity에서 GUI로 넣어야 움직일 수 있음
        rigid.MovePosition(rigid.position + nextVec); // 현재 위치와 방향(둘다 좌표라서 더할 수 있음)을 고려하여 위치 이동하겠단 것
    }

    // Update is called once per frame
    // void Update()
    // {
    //     inputVec.x = Input.GetAxis("Horizontal"); // 방향키를 누른 X축의 위치로 이동
    //     inputVec.y = Input.GetAxis("Vertical"); // 방향키를 누른 Y축의 위치로 이동
    //     // GetAxis는 부드러운 움직임 (GetAxisRaw는 딱딱한 움직임)

    // }

    // Update를 주석 처리 : OnMove를 사용할 것이기 때문

    void OnMove(InputValue value) 
    {
        // Debug.Log("OnMove 호출됨: " + value.Get<Vector2>()); // 호출 여부 확인해보기
        inputVec = value.Get<Vector2>();
    }
    private void LateUpdate() 
    {   
        anim.SetFloat("Speed", inputVec.magnitude); // Speed에 inputVec의 순수한 (벡터)값만 넣기
        if (inputVec.x != 0) { // 0이 아니면 = 방향이 전환되는 상황이면 (방향 변수에 값이 있는 상태)
            spriter.flipX = inputVec.x < 0; // if문을 써도 되지만 간단하게 하기 위해 true일 때의 조건만 적어준 것
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // 'Enemy' 태그를 가진 오브젝트와 충돌했을 때
        {
            GameManager.instance.health -= 10; // 체력을 감소시킴
            Debug.Log("Player가 적과 충돌함! 체력 감소됨: " + GameManager.instance.health);
            
            if (GameManager.instance.health <= 0) 
            {
                Debug.Log("Player가 사망했습니다.");
                gameObject.SetActive(false);
                // 사망 로직 추가 (예: 게임 종료, 리스폰, 애니메이션 등)
            }
        }
    }

}
