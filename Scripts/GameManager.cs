using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public float gameTime;
    public float maxgameTime = 2 * 10f;
    [Header("# Player Info")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = {6,5,10,100,150,210,280,360,450,600};

    [Header("# Game Object")]
    public PoolManager pool; // Prefabs를 가져오는 것
    public Player player; // player 변수를 선언함 : Player.cs에 'public class Player : MonoBehaviour'를 선언하도록 고쳐야 함

    void Awake() 
    {
        instance = this;
    }

    void Start() 
    {
        health = maxHealth; // 시작할 때 현재 체력과 최대 체력이 같도록 동기화
    }

    void Update()
    {
        gameTime += Time.deltaTime; // 시간이 흐르는 것을 기록(저장)

        if (gameTime > maxgameTime) { // 최대 시간보다 현재 시간이 더 크면
            gameTime = maxgameTime; // 현재 시간을 최대 시간으로 고정하기
        }
    }

    public void GetExp()
    {
        exp++;
        if (exp == nextExp[level]){
            level++;
            exp = 0;
        }
    }

}