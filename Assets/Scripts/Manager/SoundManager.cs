using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    // 오디오 클립의 이름
    public string name; 
    // 오디오 클립의 파일
    public AudioClip clip;  
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
  
    #region singleton
    // 객체 생성시 최초 실행
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
            // 이미 인스턴스가 있다면 해당 GameObejct를 삭제, Awake 문은 스크립트가 GameObject에서 실행할 때 최초 한 번만 실행되기 때문에
            // 싱글톤화 시킨 SoundManager 스크립트를 다시 생성할 필요가 없기 때문에 Awake 실행 이후 소환되는 사운드 매니저의 게임 오브젝트를 삭제해줌.
            Destroy(gameObject);       
    }
    #endregion singleton    

    // 오디오 믹서 채널 배경음 / 효과음 구분하기
    public AudioMixerGroup bgmMixer;
    public AudioMixerGroup effectMixer;

    // 효과음 실행할 오디오소스들
    public AudioSource[] audioSourceEffect;         
    // 배경음
    public AudioSource audioSourceBgm;              

    // 효과음
    public Sound[] effectSounds;                    
    // 배경음
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
    public void PlaySoundEffect(string _name)                   
    {
        // 효과음 배열의 길이만큼 반복
        for (int i = 0; i < effectSounds.Length; i++)            
        {
            // 인자와 같은 이름의 효과음 클립이 있다면 실행
            if(_name == effectSounds[i].name)                    
            {
                // 오디오소스의 배열의 길이 만큼 반복
                for (int j = 0; j < audioSourceEffect.Length; j++) 
                {
                    // 오디오소스가 실행중이지 않을 때 실행
                    if (!audioSourceEffect[j].isPlaying)     
                    {
                        // 사운드 이름을 효과음 이름과 같게함.
                        playSoundName[i] = effectSounds[i].name;
                        // 오디오소스 클립에 효과음 클립을 넣음.
                        audioSourceEffect[j].clip = effectSounds[i].clip;    
                        // 오디오소스를 이용해 효과음 출력
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
        // 배경음이 실행 중일 때
        if(audioSourceBgm.isPlaying)
        {
            // 인자와 실행중인 효과음이 같을 때 종료
            if(_name == audioSourceBgm.clip.name)
            {
                audioSourceBgm.Stop();
            }
        }
    }

    // 실행중인 모든 사운드 종료
    public void StopAllSE()    
    {
        // 효과음 종료
        for (int i = 0; i < audioSourceEffect.Length; i++)  
        {
            audioSourceEffect[i].Stop();                    
        }
        // 배경음 종료
        audioSourceBgm.Stop();
    }

    // 하나의 효과음만 종료
    public void StopSE(string _name)                        
    {
        for (int i = 0; i < audioSourceEffect.Length; i++) 
        {
            // 중지시킬 효과음의 이름이 배열안에 들어있으면 실행
            if(playSoundName[i] == _name)                  
            {
                // 해당 효과음 재생종료
                audioSourceEffect[i].Stop(); 
                return;
            }
        }
        // 재생중인 사운드가 없으면 출력
        Debug.Log("재생 중인" + _name + " 사운드가 없습니다");

    }
}

