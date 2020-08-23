using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    // 근접 무기 유형
    public bool isKnife;                // 나이프인지
    public bool isAxe;                  // 도끼인지

    // 근접 무기 상태
    public string closeWeaponName;      // 근접무기 이름
    public float range;                 // 공격 범위
    public int damage;                  // 공격력
    public float workSpeed;             // 작업 속도
    public float attackDelay;           // 공격 딜레이
    public float attackDelayA;          // 공격 활성화 시점
    public float attackDelayB;          // 공격 비활성화 시점

    public Animator anim;               // 근접 무기 애니메이션
}
