using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunController))]
public class WeaponManager : MonoBehaviour
{
    // 공유 자원, 클래스 변수 = 정적 변수 = static
    // 무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;

    // 현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // 무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime = 0;
    // 무기 교체 딜레이가 끝난 시점
    [SerializeField]
    private float changeWeaponEndDelayTime = 0;

    // 무기 종류들 전부 관리
    [SerializeField]
    private Gun[] snipers = null;
    [SerializeField]
    private CloseWeapon[] knives = null;
    [SerializeField]
    private CloseWeapon[] axes = null;

    // 필요한 컴포넌트    
    [SerializeField]
    private GunController theSniperController = null;
    [SerializeField]
    private KnifeController theKnifeController = null;
    [SerializeField]
    private AxeController theAxeController = null;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 딕셔너리 컨테이너 사용
    private Dictionary<string, Gun> snipersDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> knivesDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axesDictionary = new Dictionary<string, CloseWeapon>();

    // 현재 무기의 타입
    [SerializeField]
    public string currentWeaponType = null;

    public bool isnt;

    void Start()
    {
        for (int i = 0; i < snipers.Length; i++)
        {
            snipersDictionary.Add(snipers[i].gunName, snipers[i]);
        }
        for (int i = 0; i < knives.Length; i++)
        {
            knivesDictionary.Add(knives[i].closeWeaponName, knives[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axesDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isChangeWeapon == false && GameManager.canPlayerMove)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeaponType != "KNIFE" && isnt)
            { 
                StartCoroutine(ChangeWeaponCoroutine("KNIFE", "Knife")); // 무기 교체 실행 (Knife)
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeaponType != "SNIPER" && isnt)
            {
                StartCoroutine(ChangeWeaponCoroutine("SNIPER", "Sniper"));  // 무기 교체 실행 (SNIPER)
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && currentWeaponType != "AXE" && isnt)
            {
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));  // 무기 교체 실행 (Axe)
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        //currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);
        currentWeaponType = _type;

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch(currentWeaponType)
        {
            case "KNIFE":
                KnifeController.isActivate = false;
                break;
            case "SNIPER":
                theSniperController.CancelFineSight();
                theSniperController.CancelReload();
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if (_type == "SNIPER")
        {
            theSniperController.GunChange(snipersDictionary[_name]);
            theSniperController.enabled = true;
            theAxeController.enabled = false;
            theKnifeController.enabled = false;
        }
        else if (_type == "KNIFE")
        {
            theKnifeController.CloseWeaponChange(knivesDictionary[_name]);
            theKnifeController.enabled = true;
            theAxeController.enabled = false;
            theSniperController.enabled = false;
        }
        else if (_type == "AXE")
        {
            theAxeController.CloseWeaponChange(axesDictionary[_name]);
            theAxeController.enabled = true;
            theKnifeController.enabled = false;
            theSniperController.enabled = false;
        }
    }
}
    