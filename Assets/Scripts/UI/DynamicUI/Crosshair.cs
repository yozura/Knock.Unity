using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator = null;

    // 크로스헤어 비활성화를 위한 부모 객체
    public GameObject go_CrosshairHUD = null;

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
}
