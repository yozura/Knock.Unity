using System.Collections;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{
    // 미완성 클래스 == 추상적 클래스

    // 현재 장착된 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격중인지
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;
    
    [SerializeField]
    protected LayerMask layerMask;

    protected void TryAttack()
    {
        if (GameManager.canPlayerMove)
        {
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    StartCoroutine(Attack());
                }
            }
        }
    }

    protected IEnumerator Attack()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Swing");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);

        isSwing = true;
        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);

        isAttack = false;
    }

    // 추상 코루틴
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
            return true;
        else
            return false;
    }

    // 완성 함수이지만 추가편집이 가능한 함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentCloseWeapon = _closeWeapon;

        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
