using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public static bool isOptional;

    [SerializeField] private GameObject keySet = null;
    [SerializeField] private GameObject gameMenu = null;
    [HideInInspector] private GameObject titleMenu;

    void Update()
    {
        if (GameManager.canPlayerMove && !GameManager.isPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameMenu.SetActive(true);
                Time.timeScale = 0f;
                GameManager.isPause = true;
                GameManager.instance.OpenUI();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameMenu.SetActive(false);
                Time.timeScale = 1f;
                GameManager.isPause = false;
                GameManager.instance.CloseUI();
            }
        }
    }

    public void OptionWindow()
    {
        SoundManager.instance.PlaySoundEffect("Button_Click");
        titleMenu = GameObject.Find("TitleMenu");
        isOptional = true;
        GameObject.Find("UI_Present").GetComponent<Canvas>().enabled = false;
        GameObject.Find("TitleMenu").transform.Find("OptionPanel").gameObject.SetActive(true);
        titleMenu.GetComponent<Canvas>().enabled = true;
    }

    public void GoMainMenu()
    {
        SoundManager.instance.PlaySoundEffect("Button_Click");
        Time.timeScale = 1f;
        PlayerScript.instance.StopAllCoroutines();
        SoundManager.instance.StopAllSE();
        PlayerScript.instance.ScriptOnOff(false);
        titleMenu = GameObject.Find("TitleMenu");
        GameManager.isPause = false;
        titleMenu.GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene("GameStart");
    }

    public void GoKeySet()
    {
        SoundManager.instance.PlaySoundEffect("Button_Click");
        gameMenu.SetActive(false);
        keySet.SetActive(true);
    }

    public void BackKeySet()
    {
        SoundManager.instance.PlaySoundEffect("Pick_Up_Item");
        keySet.SetActive(false);
        gameMenu.SetActive(true);
    }
}
