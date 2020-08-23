using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public static bool isActivate = false;
    
    // 장착된 총
    [SerializeField]
    private Gun currentGun;

    // 연사 속도
    private float currentFireRate;

    // 총 상태
    private bool isReload = false;
    public bool isfineSightMode = false;
    
    // 본래 포지션 값.
    private Vector3 originPos;

    // 효과음
    private AudioSource audioSource;

    // 충돌 정보 받아옴
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layermask = 0;

    // 필요한 컴포넌트
    private Camera theCam;
    [SerializeField]
    private Scope theScope = null;

    // 피격 이펙트
    [SerializeField]
    private GameObject hit_effect_prefab = null;
    private Crosshair theCrosshair;

    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theCam = GameObject.Find("FPSCamera").GetComponent<Camera>();

        if (currentGun != null)
        {
            WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
            WeaponManager.currentWeaponAnim = currentGun.anim;
        }
    }

    void Update()
    {
        if (GameManager.canPlayerMove)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }

    // 연사 속도 계산
    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    // 발사 시도
    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    private void Fire() // 발사 전 계산
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine("ReloadCoroutine");
            }
        }
    }

    private void Shoot() // 발사 후 계산
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;      // 연사속도 재계산
        PlaySE(currentGun.fire_Sound);
        //currentGun.muzzleFlash.Play();
        Hit();
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
        Debug.Log("발싸");
    }

    // 적 피격
    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward +
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        0), out hitInfo, currentGun.range, layermask))
        {
            if (hitInfo.transform.CompareTag("NPC"))
                hitInfo.transform.GetComponent<Zombie>().Damaged(currentGun.damage, transform.position);

            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    // 반동 코루틴
    private IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilback = new Vector3(originPos.x, originPos.y, currentGun.retroActionForce);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.fineSightOriginPos.x, currentGun.fineSightOriginPos.y, currentGun.retroActionFineSightForce);

        if(!isfineSightMode)
        {
            currentGun.transform.localPosition = originPos;
            
            // 반동 시작
            while(currentGun.transform.localPosition.z <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilback, 0.4f);
                yield return null;
            }

            // 원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작
            while (currentGun.transform.localPosition.z <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }

    }

    // 재장전 시도
    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            PlaySE(currentGun.reload_Sound);
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void CancelReload()
    {
        if (!isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    // 재장전 코루틴
    private IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;
            yield return new WaitForSeconds(currentGun.reloadTime);
            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다");
        }
    }

    // 정조준 시도
    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    // 정조준 취소
    public void CancelFineSight()
    {
        if (isfineSightMode)
            FineSight();
    }

    // 정조준 로직 가동
    private void FineSight()
    {
        isfineSightMode = !isfineSightMode;
        theCrosshair.FineSightAnimation(isfineSightMode);
        currentGun.anim.SetBool("Scope", isfineSightMode);

        if (isfineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
            theScope.StartCoroutine("ScopeActivate");
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeActivateCoroutine());
            theScope.StartCoroutine("ScopeDeActivate");
        }
    }

    // 정조준 활성화
    private IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    // 정조준 비활성화
    private IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    // 사운드 재생
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isfineSightMode;
    }

    public virtual void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentGun = _gun;
        
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
