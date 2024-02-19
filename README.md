# 프로젝트에 대해
<table style="border-collapse: collapse; width: 0%; height: 107px;" border="1"><tbody><tr style="height: 20px;"><td style="width: 21.8604%; height: 10px; text-align: center;"><b>프로젝트 명</b></td><td style="width: 78.1396%; height: 10px; text-align: center;">Knock</td></tr><tr style="height: 20px;"><td style="width: 21.8604%; height: 20px; text-align: center;"><b>수행기간</b></td><td style="width: 78.1396%; height: 20px; text-align: center;">2020. 05. 18. ~ 06. 19. (기획 1주, 개발 4주)</td></tr><tr style="height: 20px;"><td style="width: 21.8604%; height: 20px; text-align: center;"><b>프로젝트 목표 / 소개</b></td><td style="width: 78.1396%; height: 20px; text-align: center;">스테이지 형식의 간단한 방탈출 게임입니다.<br>괴물이나 귀신에 의한 시각적인 공포가 아닌 현실적인 사운드와<br>무지에서 오는 상상력을 이용해 플레이어가 서스펜스를 느낄 수 있도록 설계했습니다.<br>장르는 공포 방탈출 게임이며 플랫폼은 PC입니다.</td></tr><tr style="height: 20px;"><td style="width: 21.8604%; height: 20px; text-align: center;"><b>개발환경</b><br><b>(사용 도구/ 언어)</b></td><td style="width: 78.1396%; height: 20px; text-align: center;">C#<br>유니티3D 엔진</td></tr><tr style="height: 20px;"><td style="width: 21.8604%; height: 10px; text-align: center;"><b>담당 역할</b></td><td style="width: 78.1396%; height: 10px; text-align: center;">기획과&nbsp;프로그래밍을&nbsp;홀로&nbsp;했으며&nbsp;3D모델&nbsp;및&nbsp;이미지는&nbsp;무료&nbsp;에셋을&nbsp;이용했습니다.<br>기획으로는 스토리 구축, 트리거 순서, 모델 선정, UI 위치 설정, 아이템 정보를 기입했습니다.<br>프로그래밍으로는 전체적인 게임의 알고리즘을 설계하였고,<br>대표적으로 ScriptableObject를 활용해 아이템을 생성, 도감 및 인벤토리를 구현했습니다.</td></tr><tr style="height: 27px;"><td style="width: 21.8604%; text-align: center; height: 27px;"><b>플레이 영상</b></td><td style="width: 78.1396%; text-align: center; height: 27px;"><a href="https://youtu.be/2Yhp5GcEhXM" target="_blank" rel="noopener">youtu.be/2Yhp5GcEhXM</a></td></tr></tbody></table>

# 프로젝트 리뷰
![01](https://user-images.githubusercontent.com/63341938/100213535-14ef8f80-2f52-11eb-8c4a-2d3e1db3a7c5.PNG)  
___<게임 시작시 첫 화면>___

 타이틀 로고는 유니티 내부에서 지원하는 텍스트 애니메이션을 이용해서 흰색에서 붉은색, 붉은색에서 투명으로 계속해서 전환되게끔 만들었습니다. START 버튼을 클릭하면 로딩 후 게임이 시작되며 EXIT 버튼을 클릭하면 게임이 종료됩니다.

![01_1](https://user-images.githubusercontent.com/63341938/100217273-9ea15c00-2f56-11eb-8b31-b18693b2d718.PNG)
___<옵션 창 화면>___

 OPTION 버튼을 클릭하면 OPTION UI가 활성화됩니다. SetResolution을 이용한 해상도 설정과 AudioMixer를 이용한 배경음과 효과음을 조절할 수 있는 볼륨 슬라이더를 구현했습니다. 사운드의 경우 아래에 있는 SoundManager 스크립트를 만들어 **싱글턴 패턴**으로 어디서든 불러올 수 있도록 처리했습니다.
 
```cs
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    #region singleton
    void Awake() 
    {
        if (instance == null)   
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion singleton    

    // 배경음 / 효과음 구분하기
    public AudioMixerGroup bgmMixer;
    public AudioMixerGroup effectMixer;

    public AudioSource[] audioSourceEffect;         
    public AudioSource audioSourceBgm;              

    public Sound[] effectSounds; 
    public Sound[] bgmSound;                        

    // 실행되고있는 사운드 이름을 가진 배열들
    public string[] playSoundName;                  

    void Start()
    {
        // 배경음 오디오 소스에 배경음 채널을 걸어줌.
        audioSourceBgm.outputAudioMixerGroup = bgmMixer;
        // 효과음 오디오 소스에 효과음 채널을 걸어줌
        for (int i = 0; i < audioSourceEffect.Length; i++)
            audioSourceEffect[i].outputAudioMixerGroup = effectMixer;
        // 사운드 이름 배열을 효과음 배열의 길이와 같이 맞춰줌
        playSoundName = new string[audioSourceEffect.Length];   
    }

    // 효과음 실행 함수
    // 하나의 효과음이 반복해서 재생되지 않도록 세팅
    public void PlaySoundEffect(string _name)            
   {
        for (int i = 0; i < effectSounds.Length; i++)            
        {
            if(_name == effectSounds[i].name)                    
           {
                for (int j = 0; j < audioSourceEffect.Length; j++) 
                {
                    if (!audioSourceEffect[j].isPlaying)     
                   {
                        playSoundName[i] = effectSounds[i].name;
                        audioSourceEffect[j].clip = effectSounds[i].clip;
                       audioSourceEffect[j].Play();
                        return;
                    }
                }
                return;
            }
        }
        return;
    }

    // 배경음 실행 함수
    public void PlaySoundBGM(string _name)
    {
        for (int i = 0; i < bgmSound.Length; i++)
        {
            if(_name == bgmSound[i].name)
            {
                if(!audioSourceBgm.isPlaying)
                {
                    audioSourceBgm.clip = bgmSound[i].clip;
                    audioSourceBgm.loop = true;
                    audioSourceBgm.Play();
                    return;
                }
            }
        }
    }

    // 배경음 종료
    public void StopBGM(string _name)
    {
        if(audioSourceBgm.isPlaying)
        {
            if(_name == audioSourceBgm.clip.name)
            {
                audioSourceBgm.Stop();
            }
        }
    }

    // 실행중인 모든 사운드 종료
    public void StopAllSE()    
    {
        for (int i = 0; i < audioSourceEffect.Length; i++)  
        {
            audioSourceEffect[i].Stop();                    
        }
        audioSourceBgm.Stop();
    }

    // 하나의 효과음만 종료
    public void StopSE(string _name)                        
    {
        for (int i = 0; i < audioSourceEffect.Length; i++) 
        {
            if(playSoundName[i] == _name)                  
            {
                audioSourceEffect[i].Stop(); 
                return;
            }
        }
        // 재생중인 사운드가 없으면 출력
        Debug.Log("재생 중인" + _name + " 사운드가 없습니다");
    }
}
```

![02](https://user-images.githubusercontent.com/63341938/100217461-d1e3eb00-2f56-11eb-8415-269cbf6caa80.PNG)  
___<로딩창 화면>___  

 START 버튼을 누르면 로딩 화면을 통해 게임 씬으로 이동하게 됩니다. 아래 Tip같은 경우는 미리 여러 개를 만들어 놓고 플레이 시마다 랜덤으로 출력하도록 설정했습니다. 하단에 위치한 붉은색 게이지 바는 아래 Script를 이용해 구현했습니다. 로딩 시간을 어느 정도 체감할 수 있도록 시간을 임의로 늘려 구현했습니다.

```cs
public class Loading : MonoBehaviour
{
    // 게이지를 시각화할 이미지
    [SerializeField]
    private Image progressIcon = null;

    void Start()
    {
        progressIcon.fillAmount = 0f;
        StartCoroutine(LoadAsyncScene());
    }

    // 코루틴을 이용하여 비동기 로딩 처리
    IEnumerator LoadAsyncScene()
    {
        // 모든 렌더링 끝난 뒤 시작하도록 설정.
        yield return new WaitForEndOfFrame();

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync("Present");
        asyncScene.allowSceneActivation = false;

        float timeC = 0;

        while (!asyncScene.isDone)
        {
            yield return new WaitForSeconds(0.08f);
            timeC += Time.deltaTime;

            if (asyncScene.progress >= 0.9f)
            {
                progressIcon.fillAmount = Mathf.Lerp(progressIcon.fillAmount, 1, timeC);

                if (progressIcon.fillAmount == 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                }
            }
            else
            {
                progressIcon.fillAmount = Mathf.Lerp(progressIcon.fillAmount, asyncScene.progress, timeC);

                if (progressIcon.fillAmount >= asyncScene.progress)
                {
                    timeC = 0f;
                }
            }
        }
    }
}
```

![02_1](https://user-images.githubusercontent.com/63341938/100217510-e2946100-2f56-11eb-8d6a-a27c36c31495.PNG)  
___<도감 시스템, 아이템 획득 이전>___ 

 게임을 만들면서 생각했습니다. 인벤토리 없이 그냥 소비 아이템을 사용할 수 있는 퀵슬롯과 아이템을 획득하면 그 아이템에 대한 정보를 얻을 수 있는 도감만 있으면 될 것 같다고 판단하여 두 가지 모두 구현했습니다. 아이템 같은 경우에는 ScriptableObject를 이용했습니다. 아이템을 얻을 때마다 도감에 해당 아이템이 채워지게 되며 도감에 입력된 아이템을 클릭할 경우 해당 아이템에 대한 정보를 출력하도록 처리했습니다.

![02_2](https://user-images.githubusercontent.com/63341938/100217565-f3dd6d80-2f56-11eb-804e-2d50d7146de2.PNG)  
___<아이템 발견 시>___  

 아이템을 발견했습니다. 화면 정중앙에 위치해있던 조준점을 오브젝트에 갖다대면 흰색에서 붉은색으로 색이 변경됩니다. 획득 할 수 있는 아이템 및 움직일 수 있는 물체라는 표시로 사용했습니다. 그리고 조준점 하단에는 아이템의 이름과 획득 단축키를 설명하는 툴팁을 표현했습니다. 또한 오브젝트 조준은 RayCast로 구현했으며 각 오브젝트 마다 다른 이름을 가지고 있으며 단축키를 눌렀을 시 서로 다른 행동을 취합니다. 단서같은 아이템의 경우 획득 시 단서를 확인할 수 있습니다.

![02_3](https://user-images.githubusercontent.com/63341938/100217607-0192f300-2f57-11eb-86c4-386ab6932aef.PNG)  
___<아이템 획득 후>___

 앞서 말씀드린 것과 같이 아이템 획득 시 해당 아이템이 도감에 추가되었습니다. 도감의 종류가 세 가지였기 때문에 상속을 이용해서 구현했습니다. 클래스 추상화라는 개념도 해당 스크립트를 설계하면서 더욱 잘 알게 되었습니다.
 
```cs
public abstract class CollectionSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // 도감에 보관될 아이템 정보들
    public Item item_Collection;
    public Image item_Collection_Image;
    public bool isAcquired = false;

    // protected 접근제한자로 상속받은 자식 클래스만 사용할 수 있도록 구현.
    protected SlotToolTip theSlotToolTip;
    protected Collection_Information c_Info;

    protected void Start()
    {
        theSlotToolTip = FindObjectOfType<SlotToolTip>();
        c_Info = FindObjectOfType<Collection_Information>();
    }

    // 추상 메서드를 생성해서 자식 클래스에서 입력하도록 구현.
    public abstract void SetColor(float _rgb);

    // 아이템을 획득하게 되면 어디로 저장할 것인지, 어떤 변수를 건드려 바꿀 것인지 체크.
    public abstract void AddCollection(Item _item);

    // 도감 아이템 클릭시 실행되는 인터페이스
    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 왼쪽 클릭으로 UI를 클릭했을 때 실행
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 아이템 정보창 호출.
            c_Info.c_InformationBox.SetActive(true);

            // 도감에 보이는 아이템이 실루엣이 아닌 컬러일 때에만
            if (item_Collection_Image.color == new Color(1f, 1f, 1f))
                c_Info.Edit_Information(item_Collection);
            else
                // 실루엣일 경우에는 디폴트값을 출력해줍니다.
                c_Info.Default_Information();
        }
    }

    // 도감 아이템에 마우스를 갖다댔을 때 실행되는 인터페이스
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템을 획득했다면
        if (isAcquired)
            ShowToolTip(item_Collection, transform.position, true);
        else
            // 미획득 상태라면 디폴트 값을 넘겨줍니다.
            ShowToolTipDefault(transform.position);
    }

    // 도감 아이템에서 마우스를 뗐을 때 실행되는 인터페이스
    public void OnPointerExit(PointerEventData eventData) => HideToolTip();

    protected void ShowToolTip(Item _item, Vector3 _pos, bool isCollection) => theSlotToolTip.ShowToolTip(_item, _pos, isCollection);

    protected void HideToolTip() => theSlotToolTip.HideToolTip();

    protected void ShowToolTipDefault(Vector3 _pos) => theSlotToolTip.ShowToolTipDefault(_pos);
}
```

# 끝

여기까지 이 게임에서 중점적으로 사용된 기능들의 코드를 리뷰해보았습니다.  
플레이 영상은 글 상단 표 링크에 달아놨습니다. 
저장소를 다운로드하셔서 에디터에서 확인 가능하고 플레이를 원하신다면 Release 탭에서 다운로드하여 즐겨주시기 바랍니다.
감사합니다.
