using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;

    // 싱글턴 패턴 사용
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion Singleton

    // 스크립트 창의 타입
    public enum ScriptType
    {
        Sound = 0,
        Des,
        NPC
    };
    
    // 스크립트 생성 후 사라지는 시간
    public const float EXIST_TIME = 5.0f;
    // 스크립트 박스가 켜져있는지 확인하는 진릿값
    public bool isActivated = false;

    // 코루틴 반복시 GC를 최소화 하기 위해 미리 선언
    private WaitForSeconds wait;
    // UI Text를 가지고 있는 부모 객체 불러옴
    [SerializeField] private GameObject script_parent = null;
    // UI Text를 배열로 선언
    [SerializeField] private Text[] playerScript = null;

    private void Start()
    {
        // Text를 변경하기 위해 부모 객체 안에 있는 UI Text를 캐싱
        playerScript = script_parent.transform.GetComponentsInChildren<Text>();
        // 최적화를 위해 미리 캐싱
        wait = new WaitForSeconds(EXIST_TIME);
        ScriptOnOff(true);
    }

    // 소리 관련 출력 메서드
    public void SoundPrint(string _msg)
    { 
        // 박스가 켜져있으면 실행
        if (isActivated)
        {
            //Enum값을 Text 배열에 캐스팅해서 구분하기 용이하기 함.
            playerScript[(int)ScriptType.Sound].text = "[" + _msg + "]";
            // 코루틴이 켜진 상태라면 끈 뒤에 다시 코루틴 가동
            StopCoroutine("LimitTime");
            StartCoroutine("LimitTime", (int)ScriptType.Sound);
        }
        else
            ScriptOnOff(true);
    }

    // 행동 관련 출력 메서드
    public void DesPrint(string _msg)
    {
        if (isActivated)
        {
            playerScript[(int)ScriptType.Des].text = _msg;
            StopCoroutine("LimitTime2");
            StartCoroutine("LimitTime2", (int)ScriptType.Des);
        }
        else
            ScriptOnOff(true);
    }

    // 기타 출력 메서드
    public void NPCPrint(string _msg)
    {
        if (isActivated)
        {
            playerScript[(int)ScriptType.NPC].text = _msg;
            StopCoroutine("LimitTime3");
            StartCoroutine("LimitTime3", (int)ScriptType.NPC);
        }
        else
            ScriptOnOff(true);
    }

    // 스크립트 박스를 켜고 끌 때 사용하는 메서드
    public void ScriptOnOff(bool _flag)
    {
        isActivated = _flag;
        for (int i = 0; i < playerScript.Length; i++)
        {
            playerScript[i].gameObject.SetActive(_flag);
        }
        gameObject.SetActive(_flag);
    } 

    // 제한 시간이 지나면 스크립트 초기화
    IEnumerator LimitTime(int _count)
    {
        yield return wait;
        playerScript[_count].text = "";
    }
    
    IEnumerator LimitTime2(int _count)
    {
        yield return wait;
        playerScript[_count].text = "";
    }
    
    IEnumerator LimitTime3(int _count)
    {
        yield return wait;
        playerScript[_count].text = "";
    }
}
    