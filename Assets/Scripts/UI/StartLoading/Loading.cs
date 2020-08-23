using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    // 게이지를 표현할 이미지
    [SerializeField] private Image progressIcon = null;     

    void Start()
    {
        progressIcon.fillAmount = 0f;   // 해당 이미지의 fillamount 값을 0으로 맞춰주고
        StartCoroutine(LoadAsyncScene());
    }

    // 로딩창 안에 내용들
    IEnumerator LoadAsyncScene()
    {
        // 모든 렌더링 끝난 뒤 시작
        yield return new WaitForEndOfFrame();
        // 열어줄 씬을 설정해 비동기적 로드한다.
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync("Present");
        // 다음 화면으로 넘어가기위해 true로 바꿔줄 것이다. false는 아직 안넘어갔다는 뜻
        asyncScene.allowSceneActivation = false;
        float timeC = 0;
        // 비동기적 로드가 완료 될 때까지 실행한다
        while(!asyncScene.isDone)
        {
            yield return new WaitForSeconds(0.08f);
            // 타임 float값에 프레임값을 넣어주고
            timeC += Time.deltaTime;
            // 비동기적 로드가 0.9보다 작을 때
            if(asyncScene.progress >= 0.9f)
            {
                // 타임 float값 만큼 이미지의 fillAmount 값을 선형보간해준다.
                progressIcon.fillAmount = Mathf.Lerp(progressIcon.fillAmount, 1, timeC);
                if(progressIcon.fillAmount == 1.0f)
                    asyncScene.allowSceneActivation = true;
            }
            // fillAmount가 90%이상 되었을 때 실행
            else
            {
                // 이미지의 fillAmount값을 비동기적로드 progress값까지 선형보간해주고
                progressIcon.fillAmount = Mathf.Lerp(progressIcon.fillAmount, asyncScene.progress, timeC);
                // 비동기적로드의 progress는 allowSceneActivation이 true가 되었을 때 1이 되므로 fillAmount가 progress보다 커졌을 때
                if(progressIcon.fillAmount >= asyncScene.progress)
                {   
                    // time을 0으로 만들어준다.
                    timeC = 0f;
                }
            }
        }
    }
}
