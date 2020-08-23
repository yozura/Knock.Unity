using System.Collections;
using UnityEngine;

public class KnifeController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.CompareTag("NPC"))
                    hitInfo.transform.GetComponent<Zombie>().Damaged(currentCloseWeapon.damage, transform.position);
                Debug.Log(hitInfo.transform.name);
                isSwing = false;
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
