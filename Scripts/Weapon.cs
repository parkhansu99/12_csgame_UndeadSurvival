using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    float timer;
    Player player;

    void Awake() 
    {
        player = GetComponentInParent<Player>(); // ☆ GetComponentInParent
    }

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
         switch(id) {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
            break;
         }

         if (Input.GetButtonDown("Jump")){
            LevelUp(20, 5);
         }
    }

    public void LevelUp(float damage, int per)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }

    public void Init() 
    {
         switch(id) {
            case 0:
                speed = -150; // 마이너스해야 시계방향으로 회전 (위의 forward를 back으로 바꾸면 시계방향을 플러스로 바꿀 수 있음)
                Batch(); // 생성된 무기를 배치하는 함수
                break;
            default:
                speed = 0.3f; // 0.3초마다 무기를 계속 생성하기
                break;
         }
    }
    
    void Batch()
    {
        for (int index = 0 ; index < count ; index++){
            Transform bullet;

            if (index < transform.childCount) {
                bullet = transform.GetChild(index);
            } else {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform; // 부모를 변경
            }

            bullet.localPosition = Vector3.zero; // 무기의 위치를 초기화
            bullet.localRotation = Quaternion.identity; // 무기의 회전을 초기화

            Vector3 rotVec = Vector3.forward * 360 * index / count; // count 변수의 수만큼 개수가 늘어남
            bullet.Rotate(rotVec); // 회전 적용하기
            bullet.Translate(bullet.up * 1.5f, Space.World); // 위쪽으로 1.5만큼 이동 (간격 벌리기)
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1은 무한 관통을 의미함 (근접 무기는 계속 관통해야 하기 때문)

        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget) // 가장 가까운 타겟(적)이 없다면
            return; // 나가기
        
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // Batch의 Init 복사해서 count, dir만 수정해주기
    }
}
