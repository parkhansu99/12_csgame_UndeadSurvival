using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponer : MonoBehaviour
{

    public Transform[] spawnPoint;
    public SpawnData[] spawnData; // SpawnData 클래스 가져오기
    int level;
    float timer;

    void Awake() 
    {
        spawnPoint = GetComponentsInChildren<Transform>(); // 스폰 지점으로 자식에 있는 Point의 위치를 지정하기
    }

    // // Start is called before the first frame update
    // void Start()
    // {
    //     GameManager.instance.pool.Get(1); // GameManager에 만들어 놓은 pool 가져오기
    // }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // 시간이 흐르는 것을 기록(저장)
        level = Mathf.FloorToInt(GameManager.instance.gameTime / 10f); // 10초당 1레벨 증가하도록 : GameManager의 gameTime 변수를 10으로 나눈 것을 가져오기 -> 실수를 정수로 바꾸기

        if (timer > spawnData[level].spawnTime) {
            timer = 0; // 타이머를 0으로 되돌리고
            Spawn(); // 스폰(적 생성)하기
        }
    }
    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0); // 적으로 GameManager의 첫번째 pool을 지정
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // spawnPoint라는 리스트의 처음부터 끝까지의 위치를 적이 (다음에) 생성될 위치에 집어넣는 것
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable] // GUI(Inspector)에서 보이고 수정할 수 있도록 하기
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}