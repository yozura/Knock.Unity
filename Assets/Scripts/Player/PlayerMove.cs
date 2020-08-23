using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed = 0.0f;                                // 걷는 속도
    [SerializeField]
    private float runSpeed = 0.0f;                                 // 뛰는 속도
    private float applySpeed;                               // 걷거나 뛰는 속도를 받아 보급

    // 앉기 조정 변수
    [SerializeField]
    private float crouchSpeed = 0.0f;                              // 앉는 속도

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY = 0.0f;                               // 현재 앉은 좌표
    private float originPosY;                               // 서있던 원래 좌표
    private float applyCrouchPosY;                          // 상태에 따라 보급되는 좌표

    // 카메라 관련
    [SerializeField]
    private float lookSensivity = 0.0f;                            // 카메라 민감도
    [SerializeField]
    private float cam_rotation_limit = 0.0f;                       // 카메라 위 아래 회전 제한
    private float current_camRotation;                      // 현재 카메라 위치

    // 컴포넌트 호출
    [SerializeField]
    private Camera myCamera = null;
    private Rigidbody myRigid;
    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;
    private Inventory theInven;
    
    [SerializeField]
    private Light theSL = null;

    // 플레이어 상태 변수
    private bool isWalk = false;                            // 걷고있는지
    private bool isCrouch = false;                          // 앉아있는지
    private bool isLight = false;                           // 불이 켜져있는지
    public bool isRun = false;                             // 뛰고있는지

    // 움직임 체크 변수
    private Vector3 lastPos;                                // 전 프레임의 위치값

    void Start()
    {
        theCrosshair = FindObjectOfType<Crosshair>();
        theGunController = FindObjectOfType<GunController>();
        theStatusController = FindObjectOfType<StatusController>();
        theInven = FindObjectOfType<Inventory>();
        myRigid = GetComponent<Rigidbody>();

        // 변수 초기화
        applySpeed = walkSpeed;
        originPosY = myCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        if(GameManager.canPlayerMove && !GameManager.isPause)
        {
            TryRun();
            TryCrouch();
            CameraRotationY();
            PlayerRotation();
        }
        if(theInven.c_ETCSlots[1].isAcquired)
                LightTurnOnOff();
    }

    void FixedUpdate()
    {
        if (GameManager.canPlayerMove && !GameManager.isPause)
        {
            Move();
            MoveCheck();
        }
    }
    private void LightTurnOnOff()
    {
        if (!isLight)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isLight = true;
                theSL.intensity = 1.5f;
                SoundManager.instance.PlaySoundEffect("FlashLight_OnOff");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isLight = false;
                theSL.intensity = 0.0f;
                SoundManager.instance.PlaySoundEffect("FlashLight_OnOff");
            }
        }
    }

    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))       // Lctrl을 누르면 앉음
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;                            // 앉은 상태 변경 true 면 false, false면 true

        if (isCrouch)                        // 앉은 상태 활성화
        {
            applySpeed = crouchSpeed;       // 플레이어 속도를 앉았을 때 움직이는 속도 변환
            applyCrouchPosY = crouchPosY;   // 빈 변수에 앉았을때 좌표 대입
        }
        else                                // 앉은 상태 비활성화
        {
            applySpeed = walkSpeed;         // 원래 속도로 변환
            applyCrouchPosY = originPosY;   // 원래 좌표로 변환
        }

        StartCoroutine(CrouchCoroutine());  // 코루틴을 이용하여 부드럽게 앉기
    }

    IEnumerator CrouchCoroutine()
    {
        float _positionY = myCamera.transform.localPosition.y; // 카메라의 y 좌표
        int count = 0;

        while(_positionY != applyCrouchPosY)        // 카메라의 y좌표와 플레이어와 다를 때
        {
            count++;        // 시간 변수 증가
            _positionY = Mathf.Lerp(_positionY, applyCrouchPosY, 0.3f); // 선형 보간을 이용해 카메라의 y좌표에서 플레이어가 앉은 좌표까지 0.3f의 속도로 부드럽게 변환
            myCamera.transform.localPosition = new Vector3(0f, _positionY, 0f);
            if (count > 15)  // 해당 while 반복문이 15번 반복되었을 때
                break;       // 종료
            yield return null;  // 다음 update 문이 실행될때까지 기다린다음 실행 재개 이후에 LateUpdate가 실행
            // update -> coroutine null -> lateupdate 순
        }
        myCamera.transform.localPosition = new Vector3(0f, applyCrouchPosY, 0f);    // 마지막으로 Lerp 함수는 1에 가까워질뿐 1이 되지는 않기 때문에 보간하고자하는 위치를 고대로 확정시킨다.
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 moveX = moveDirX * transform.right;
        Vector3 moveZ = moveDirZ * transform.forward;
        Vector3 velocity = (moveX + moveZ).normalized * applySpeed;

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)     // 1프레임 전 포지션과 현재 포지션을 비교하여 0.01f 보다 값이 클 경우 걷기
            {
                isWalk = true;  // 걷는중
            }
            else
            {
                isWalk = false; // 안움직임
            }
            lastPos = transform.position;   // 현재 포지션을 라스트 포지션에 대입
        }
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSp() > 0)
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSp() <= 0)
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();
        theStatusController.DecreaseStamina(10);

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;  
    }

    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    private void CameraRotationY()
    {
        float camRotationX = Input.GetAxisRaw("Mouse Y");
        float cameraVelocity = camRotationX * lookSensivity;
        current_camRotation -= cameraVelocity;
        current_camRotation = Mathf.Clamp(current_camRotation, -cam_rotation_limit, cam_rotation_limit);

        myCamera.transform.localEulerAngles = new Vector3(current_camRotation, 0f, 0f);
    }

    private void PlayerRotation()
    {
        float rotPlayer = Input.GetAxisRaw("Mouse X");
        Vector3 playerRotationY = new Vector3(0f, rotPlayer, 0f) * lookSensivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(playerRotationY));
    }
}
