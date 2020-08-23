using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] protected string zombie_name;    // 좀비 이름
    [SerializeField] protected int hp;                // 좀비 체력
    [SerializeField] protected float walkSpeed;       // 걷는 속도
    [SerializeField] protected float runSpeed;        // 뛰는 속도
    [SerializeField] protected float turningSpeed;    // 회전 속도
    protected float applySpeed;                       // 보급 속도

    [SerializeField]
    protected Vector3 direction;                    // 방향

    // 상태 변수
    protected bool isAction;                          // 행동 중인지 체크
    protected bool isWalking;                         // 걷는 중인지 체크
    protected bool isRunning;                         // 뛰는 중인지 체크
    protected bool isDead;                            // 죽었는지 체크

    [SerializeField] protected float walkTime;        // 걷는 시간
    [SerializeField] protected float runTime;         // 뛰는 시간
    [SerializeField] protected float waitTime;        // 대기 시간
    protected float currentTime;

    // 필요한 컴포넌트
    [SerializeField] protected Animator zombieAni;
    [SerializeField] protected BoxCollider boxCol;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected AudioClip[] zombie_normal_sounds;
    [SerializeField] protected AudioClip[] zombie_hurt_sound;
    [SerializeField] protected AudioClip zombie_dead_sound;
    protected AudioSource theAudio;

    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        if (!isDead)
            ElapseTime();
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            Rotation();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
    }

    protected void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                // 다음 랜덤 행동 개시
                ReSet();
            }
        }
    }

    protected virtual void ReSet()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        applySpeed = walkSpeed;
        zombieAni.SetBool("Walk", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
    }

    public virtual void Damaged(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            int _random = Random.Range(0, 3);
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(zombie_hurt_sound[_random]);
            zombieAni.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        Debug.Log("죽음");
        PlaySE(zombie_dead_sound);
        isWalking = false;
        isRunning = false;
        isDead = true;
        zombieAni.SetTrigger("Dead");
        Destroy(this.gameObject, 5f);
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3);   // 일상 사운드 3개
        PlaySE(zombie_normal_sounds[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
