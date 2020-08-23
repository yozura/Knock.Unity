using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator = null;

    // 크로스헤어의 상태에 따른 총의 정확도
    private float gunAccuracy;

    // 크로스헤어 비활성화를 위한 부모 객체
    public GameObject go_CrosshairHUD = null;
    [SerializeField]
    private GunController theGunController = null;

    public void AcquireAnimation(bool _flag)
    {
        animator.SetBool("Acquire", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }

    public void FineSightAnimation(bool _flag)
    {
        animator.SetBool("FineSight", _flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("Walking"))
            animator.SetTrigger("WalkFire");
        else if (animator.GetBool("Crouch"))
            animator.SetTrigger("CrouchFire");
        else
            animator.SetTrigger("IdleFire");
    }
    
    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
            gunAccuracy = 0.003f;
        else if (animator.GetBool("Crouch"))
            gunAccuracy = 0.0003f;
        else if (theGunController.GetFineSightMode())
            gunAccuracy = 0.0001f;
        else
            gunAccuracy = 0.001f;

        return gunAccuracy;
    }
}
