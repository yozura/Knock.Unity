using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTrigger : MonoBehaviour
{
    public GameObject end_panel;
    public GameObject end_Text;

    private GameObject title;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            StartCoroutine(PresentEnd());
        }
    }

    IEnumerator PresentEnd()
    {
        title = GameObject.Find("TitleMenu");
        SoundManager.instance.StopAllSE();
        end_panel.SetActive(true);
        end_Text.SetActive(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(5f);
        title.GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene("GameStart");
        
    }
}
