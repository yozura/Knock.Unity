using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // 스태미나
    [SerializeField]
    private int sp = 0;
    private int currentSp = 0;

    // 스태미나 리젠
    [SerializeField]
    private int spIncreaseSpeed = 0;

    // 스태미나 회복 시간
    [SerializeField]
    private int spRechargeTime = 0;
    private int currentSpRechargeTime = 0;

    // 스태미나 감소 여부
    private bool spUsed = false;

    // 필요한 이미지
    [SerializeField]
    private Image gaugeBar = null;

    public GameObject go_spBox;

    void Start()
    {
        currentSp = sp;
    }

    void Update()
    {
        SPRechargeTime();
        SPRecovery();
        GaugeUpdate();
    }

    // 게이지를 업데이트 해줌
    void GaugeUpdate()
    {   
        gaugeBar.fillAmount = (float)currentSp / sp;
    }

    // 달리기로 인해 스태미나가 깎인 경우
    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp - _count > 0)
        {
            go_spBox.SetActive(true);
            gaugeBar.gameObject.SetActive(true);
            currentSp -= _count;
        }
        else
            currentSp = 0;
    }

    // 스태미나가 반절 채워진다
    public void IncreaseSP(int _count)
    {
        currentSp += _count;
        if (currentSp >= sp)
            currentSp = sp;
    }
    
    // SP가 회복되기까지의 딜레이
    void SPRechargeTime()
    {
        if(spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
            {
                currentSpRechargeTime++;

            }
            else
                spUsed = false;
        }
    }

    // SP가 자연 회복되는 경우
    void SPRecovery()
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;

            if(currentSp == sp)
            {
                go_spBox.SetActive(false);
                gaugeBar.gameObject.SetActive(false);
            }
        }
    }    
    
    // 외부에서 Sp를 사용할 때 사용
    public int GetCurrentSp()
    {
        return currentSp;
    }

}

