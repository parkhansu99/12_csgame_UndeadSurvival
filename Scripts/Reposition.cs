using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{

    Collider2D coll;
    void Awake() 
    {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision) {
        Debug.Log("OnTriggerExit2D called");

        if (!collision.CompareTag("Area")) {
            Debug.Log("Exited collider with tag other than 'Area'");
            return;
        }
        
        Debug.Log("Collided with 'Area' tag");
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Debug.Log($"Player Pos: {playerPos}, My Pos: {myPos}, diffX: {diffX}, diffY: {diffY}");

        Vector3 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        Debug.Log($"Player Direction: {playerDir}, dirX: {dirX}, dirY: {dirY}");

        switch (transform.tag) {
            case "Ground":
                if (diffX > diffY) {
                    Debug.Log("Moving Ground horizontally");
                    transform.Translate(Vector3.right * dirX * 40);
                } else if (diffX < diffY) {
                    Debug.Log("Moving Ground vertically");
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                Debug.Log("Enemy tag detected");
                if (coll.enabled) { // 적이 살아있는 경우에만
                    transform.Translate(playerDir * 20 + new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), 0f)); // 플레이어의 이동 방향에 따라 맞은 편에서 등장하도록 이동
                    // 오류 수정 : UnityEngine.Random을 쓰면 충돌 없이 할 수 있음 (2개 이상 Random을 쓰기 때문에 발생)
                }
                // 적을 위한 코드 추가
                break;
        }
    }
}
