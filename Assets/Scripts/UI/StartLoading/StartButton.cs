using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public static StartButton instance;

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

    private SaveAndLoad theSaveAndLoad;

    public GameObject loadBG = null;
    [SerializeField] GameObject optional = null;
    [SerializeField] GameObject holdPanel = null;

    private void Start()
    {
        theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
    }

    public void OnStart()
    {
        StartCoroutine("StartLoad");
        GameManager.instance.CloseUI();
        SoundManager.instance.PlaySoundEffect("Button_Click");
        SoundManager.instance.StopBGM("Rain_On_Window");
    }
    
    IEnumerator StartLoad()
    {
        Instantiate(holdPanel);
        GetComponent<Canvas>().enabled = false;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Loading");
    }

    public void OnLoad()
    {
        theSaveAndLoad.LoadData();
        SoundManager.instance.PlaySoundEffect("Button_Click");
    }

    public IEnumerator LoadCoroutine()
    {
        loadBG.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync("Present");
        
        while(!operation.isDone)
        {
            yield return null;
        }
        
        GetComponent<Canvas>().enabled = false;
    }

    public void OnOption()
    {
        optional.SetActive(true);
        SoundManager.instance.PlaySoundEffect("Button_Click");
    }

    public void OffOption()
    {
        optional.SetActive(false);
        SoundManager.instance.PlaySoundEffect("Pick_Up_Item");
        if (GameMenu.isOptional)
        {
            GameMenu.isOptional = false;
            GameObject.Find("UI_Present").GetComponent<Canvas>().enabled = true;
            gameObject.GetComponent<Canvas>().enabled = false;
        }
    }

    public void OnExit()
    {
        SoundManager.instance.PlaySoundEffect("Button_Click");
        Application.Quit();
        Debug.Log("Game Over");
    }
}
