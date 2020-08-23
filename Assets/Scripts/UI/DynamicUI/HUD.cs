using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 필요한 컴포넌트
    [SerializeField]
    private GunController gc = null;
    [SerializeField]
    private WeaponManager theWpMgr = null;

    // 필요하면 활성화, 비활성화
    [SerializeField]
    private GameObject go_Bullet = null;
    [SerializeField]
    private GameObject go_Knife = null;
    [SerializeField]
    private GameObject go_Axe = null;

    [SerializeField]
    private Text[] bullet = null;

    private void Update()
    {
        HUDType();
    }

    public void HUDType()
    {
        if (theWpMgr.currentWeaponType == "SNIPER")
        {
            go_Knife.SetActive(false);
            go_Axe.SetActive(false);
            go_Bullet.SetActive(true);
            bullet[0].text = gc.GetGun().currentBulletCount.ToString();
            bullet[1].text = gc.GetGun().carryBulletCount.ToString();
        }
        else if(theWpMgr.currentWeaponType == "KNIFE")
        {
            go_Axe.SetActive(false);
            go_Bullet.SetActive(false);
            go_Knife.SetActive(true);
        }
        else if(theWpMgr.currentWeaponType == "AXE")
        {
            go_Knife.SetActive(false);
            go_Bullet.SetActive(false);
            go_Axe.SetActive(true);
        }
    }

}