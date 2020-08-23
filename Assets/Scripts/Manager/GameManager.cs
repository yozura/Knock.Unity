using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Singleton
    void Awake()
    {
       // instance가 생성되지 않았을 때, 예상치 못한 버그를 방지하기 위해 조건문 추가
       if (instance == null)  
       {
           // 클래스 instance화
           instance = this;
           // 이 스크립트가 붙어있는 GameObject를 씬이 넘어가거나 오류로 지워지지 않게끔 설정
           DontDestroyOnLoad(gameObject);  
       }
       else
           Destroy(gameObject);
    }
    #endregion Singleton

    public static bool canPlayerMove = true;        // 플레이어의 움직임 제어
    public static bool isPause = false;             // 게임 일시정지 유무
    public static bool isPanic = false;             // 패닉 상태 여부

    void Start()
    {
        SoundManager.instance.PlaySoundBGM("Rain_On_Window");
    }

    private void Update()
    {
        if (canPlayerMove)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
                OpenUI();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
                CloseUI();
        }
    }

    public void OpenUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canPlayerMove = false;
    }

    public void CloseUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canPlayerMove = true;
    }
}
