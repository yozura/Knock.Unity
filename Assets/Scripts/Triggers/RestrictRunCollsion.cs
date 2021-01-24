using System.Collections;
using UnityEngine;

public class RestrictRunCollsion : MonoBehaviour
{
    private PlayerMove p_Move;
    private Inventory inven;

    public GameObject[] doors;
    public GameObject[] draws;

    WaitForSeconds wait;

    private void Start()
    {
        inven = FindObjectOfType<Inventory>();
        p_Move = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        wait = new WaitForSeconds(0.2f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (p_Move.isRun && !GameManager.isPanic && inven.c_NoteSlots[3].isAcquired)
            {
                if(!inven.c_NoteSlots[4].isAcquired)
                {
                    GameManager.isPanic = true;
                    SoundManager.instance.StopBGM("Rain_On_Window");
                    SoundManager.instance.StopBGM("TV_LightStatic");
                    SoundManager.instance.PlaySoundBGM("Restrict_Run");
                    SoundManager.instance.PlaySoundEffect("Panic_Breath");
                    PlayerScript.instance.SoundPrint("거친 숨소리");
                    PlayerScript.instance.DesPrint("사물들이 제멋대로 움직인다.");
                    StartCoroutine("RunDie");
                }
            }
        }
    }

    IEnumerator RunDie()
    {
        float count = 0;
        while(GameManager.isPanic)
        {
            // 끝낼 지점을 만들 카운트 생성
            count++;

            // 1번 돌면 끝내겠단 조건문
            if (count > 1)
            {
                GameManager.isPanic = false;
                SoundManager.instance.StopAllSE();
                StopCoroutine("RunDie");
                yield break;
            }

            // 문이 먼저 열림
            PlayerScript.instance.SoundPrint("문 열리는 소리");
            for (int i = 0; i < doors.Length; i++)
            {
                yield return wait;
                if (i >= doors.Length / 2)
                {
                    yield return wait;
                    SoundManager.instance.PlaySoundEffect("Open_Door");
                    doors[i].GetComponent<Animator>().SetBool("IsOpen", true);
                }
                SoundManager.instance.PlaySoundEffect("Open_Door");
                doors[i].GetComponent<Animator>().SetBool("IsOpen", true);
            }
            yield return wait;
            // 그다음에 서랍 열리고
            PlayerScript.instance.SoundPrint("서랍 열리는 소리");
            SoundManager.instance.PlaySoundEffect("Open_Draw");
            for (int i = 0; i < draws.Length; i++)
            {
                if (i >= draws.Length / 2)
                {
                    yield return wait;
                    draws[i].GetComponent<Animator>().SetBool("IsOpen", true);
                }
                yield return wait;
                draws[i].GetComponent<Animator>().SetBool("IsOpen", true);
            }
            // 문 닫히고
            PlayerScript.instance.SoundPrint("문 닫히는 소리");
            SoundManager.instance.PlaySoundEffect("Close_Door");
            for (int i = 0; i < doors.Length; i++)
            {
                if (i >= doors.Length / 2)
                {
                    yield return wait;
                    doors[i].GetComponent<Animator>().SetBool("IsOpen", false);
                }
                yield return wait;
                doors[i].GetComponent<Animator>().SetBool("IsOpen", false);
            }

            yield return wait;

            // 서랍 닫히고
            SoundManager.instance.PlaySoundEffect("Close_Draw");
            PlayerScript.instance.SoundPrint("서랍 닫히는 소리");

            for (int i = 0; i < draws.Length; i++)
            {
                if (i >= draws.Length / 2)
                {
                    yield return wait;
                    SoundManager.instance.PlaySoundEffect("Close_Draw");
                    draws[i].GetComponent<Animator>().SetBool("IsOpen", false);
                }
                yield return wait;
                draws[i].GetComponent<Animator>().SetBool("IsOpen", false);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
