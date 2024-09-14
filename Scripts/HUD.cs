using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Text를 입력하는 순간 자동 추가

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health };
    public InfoType type;
    Text myText;
    Slider mySlider;
    void Awake() 
    {
        if (type == InfoType.Level || type == InfoType.Kill || type == InfoType.Time) 
        {
            myText = GetComponent<Text>(); // 텍스트에만 해당되는 경우
        } 
        else if (type == InfoType.Exp || type == InfoType.Health) 
        {
            mySlider = GetComponent<Slider>(); // 슬라이더에만 해당되는 경우
        }
    }


    void LateUpdate() 
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level); // 레벨 텍스트
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill); // 킬 텍스트
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxgameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60); // 60을 나눈 몫만 구하기 (버림)
                int sec = Mathf.FloorToInt(remainTime % 60); // 60을 나눈 나머지만 구하기 (버림)
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec); // D2는 자리수를 지정하는 것
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                Debug.Log($"Current Health: {curHealth}, Max Health: {maxHealth}"); // 디버깅
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
