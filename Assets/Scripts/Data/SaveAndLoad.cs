using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;               // 플레이어의 저장 포지션
    public Vector3 playerRot;               // 플레이어의 저장 회전각

    public List<int> inventoryArrayNum = new List<int>();       // 인벤토리 아이템의 슬롯 위치를 list로 저장한다.
    public List<string> inventoryItemName = new List<string>(); // 인벤토리 아이템의 이름을 list로 저장한다.
    public List<int> inventoryItemCount = new List<int>();      // 인벤토리 아이템의 개수를 list로 저장한다.

    public List<int> collectionItemArrayNum = new List<int>();
    public List<string> collectionItemName = new List<string>();

    public List<int> collectionEtcArrayNum = new List<int>();
    public List<string> collectionEtcName = new List<string>();

    public List<int> collectionNoteArrayNum = new List<int>();
    public List<string> collectionNoteName = new List<string>();
}

public class SaveAndLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();     // 세이브할 정보들 클래스 변수로 호출

    public string SAVE_DATA_DIRECTORY;             // 세이브 파일을 저장할 폴더 위치를 넣을 변수
    public string SAVE_FILE_NAME = "/SaveFile.txt";// 세이브 파일의 이름

    private PlayerMove thePlayer;                   // 플레이어의 움직임을 가져올 클래스 변수 호출
    private Inventory theInven;
    

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/"; // 세이브 파일을 저장할 폴더의 위치 지정 dataPath는 기본적으로 자기자신의 폴더를 의미합니다.
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))             // 세이브 폴더 위치에 폴더가 없을 경우 실행
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);     // 폴더를 생성해줍니다.
    }

    public void SaveData()
    {
        Debug.Log("저장이 시작되었습니다.");
        thePlayer = FindObjectOfType<PlayerMove>();             // 플레이어의 움직임을 제어하는 클래스를 하이어라키에서 찾음.
        theInven = FindObjectOfType<Inventory>();               // 플레이어의 인벤토리를 제어하는 클래스를 하이어라키에서 찾음.
        
        saveData.playerPos = thePlayer.transform.position;      // 세이브 시점의 플레이어의 포지션값을 저장한다.
        saveData.playerRot = thePlayer.transform.eulerAngles;   // 세이브 시점의 플레이어의 회전각값을 저장한다. 벡터는 eulerAngles를 이용할 것
        
        Slot[] slots = theInven.GetSlots();                     // 인벤토리 슬롯을 비교할 대상을 가져오기
        CollectionItem[] c_ItemSlots = theInven.GetItemSlots();
        CollectionETC[] c_EtcSlots = theInven.GetEtcSlots();
        CollectionNote[] c_NoteSlots = theInven.GetNoteSlots();

        
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)                           // 슬롯에 아이템이 있으면 순차적으로 아이템의 정보를 저장
            {
                saveData.inventoryArrayNum.Add(i);
                saveData.inventoryItemName.Add(slots[i].item.itemName);
                saveData.inventoryItemCount.Add(slots[i].itemCount);
            }
        }
        
        for (int i = 0; i < c_ItemSlots.Length; i++)
        {
            if (c_ItemSlots[i].item_Collection_Image.color == new Color(1f, 1f, 1f))  // 도감에 아이템 이미지의 색상이 검정색이면 저장
            {
                saveData.collectionItemArrayNum.Add(i);
                saveData.collectionItemName.Add(c_ItemSlots[i].item_Collection.itemName);
            }
        }

        for (int i = 0; i < c_EtcSlots.Length; i++)
        {
            if (c_EtcSlots[i].item_Collection_Image.color == new Color(1f, 1f, 1f))  // 도감에 아이템 이미지의 색상이 검정색이면 저장
            {
                saveData.collectionEtcArrayNum.Add(i);
                saveData.collectionEtcName.Add(c_EtcSlots[i].item_Collection.itemName);
            }
        }
        
        for (int i = 0; i < c_NoteSlots.Length; i++)
        {
            if (c_NoteSlots[i].item_Collection_Image.color == new Color(1f, 1f, 1f))  // 도감에 아이템 이미지의 색상이 검정색이면 저장
            {
                saveData.collectionNoteArrayNum.Add(i);
                saveData.collectionNoteName.Add(c_NoteSlots[i].item_Collection.itemName);
            }
        }
        string json = JsonUtility.ToJson(saveData);             // json을 이용해서 세이브데이터를 json화 시킨다.
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILE_NAME, json); // json화 시킨 SaveData를 File로 저장시킨다.
    }

    public void LoadData()              
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILE_NAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILE_NAME);       // 세이브파일을 읽어들여서 loadjson에 저장합니다.
            saveData = JsonUtility.FromJson<SaveData>(loadJson);    // load할 정보를 다시 savedata에 대입합니다.

            StartButton.instance.loadBG.SetActive(true);
            SceneManager.LoadScene("Present");

            StartButton.instance.GetComponent<Canvas>().enabled = false;

            thePlayer = FindObjectOfType<PlayerMove>();             // PlayerMove 클래스를 찾아옵니다
            theInven = FindObjectOfType<Inventory>();               // inventory 클래스를 찾아옵니다.

            thePlayer.transform.position = saveData.playerPos;      // 플레이어의 위치값에 저장했었던 이전 포지션값을 대입합니다.
            thePlayer.transform.eulerAngles = saveData.playerRot;   // 위와 같이 저장했던 회전값을 대입합니다.

            for (int i = 0; i < saveData.inventoryItemName.Count; i++)    // 저장했던 아이템 및 인벤토리를 불러옵니다.
            {
                theInven.LoadToInven(saveData.inventoryArrayNum[i], saveData.inventoryItemName[i], saveData.inventoryItemCount[i]);
            }
            for (int i = 0; i < saveData.collectionItemName.Count; i++)
            {
                theInven.LoadToCollectionItem(saveData.collectionItemArrayNum[i], saveData.collectionItemName[i]);
            }
            for (int i = 0; i < saveData.collectionEtcName.Count; i++)
            {
                theInven.LoadToCollectionETC(saveData.collectionEtcArrayNum[i], saveData.collectionEtcName[i]);
            }
            for (int i = 0; i < saveData.collectionNoteName.Count; i++)
            {
                theInven.LoadToCollectionNote(saveData.collectionNoteArrayNum[i], saveData.collectionNoteName[i]);
            }
            Debug.Log("로드 완료");
        }
        else
        {
            StartButton.instance.StopCoroutine("LoadCoroutine");
        }
    }
}

