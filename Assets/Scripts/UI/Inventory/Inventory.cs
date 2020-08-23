using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 인벤토리 활성화했는지
    public static bool inventoryActivated = false;

    // 필요한 오브젝트
    [SerializeField] private GameObject go_slotsParent = null;
    [SerializeField] private GameObject go_CollectionItemParent = null;
    [SerializeField] private GameObject go_CollectionEtcParent = null;
    [SerializeField] private GameObject go_CollectionNoteParent = null;

    // 필요한 컴포넌트
    private Collection_Information c_Info = null;
    private FurnitureAction furAct;

    // 슬롯들
    public Slot[] slots = null;
    public CollectionItem[] c_itemSlots = null;
    public CollectionETC[] c_ETCSlots = null;
    public CollectionNote[] c_NoteSlots = null;
    
    // 세이브 목록
    public Slot[] GetSlots() { return slots; }                      // 슬롯들을 세이브데이타에 가져갈 수 있도록 get 함수를 제작
    public CollectionItem[] GetItemSlots() { return c_itemSlots; }  
    public CollectionETC[] GetEtcSlots() { return c_ETCSlots; }
    public CollectionNote[] GetNoteSlots() { return c_NoteSlots; }

    [SerializeField] private Item[] items = null;   // 아이템들을 비교할 수 있도록 아이템 클래스를 가져오기
    
    // 인벤토리 슬롯 불러오기
    public void LoadToInven(int _arrayNum, string _itemName, int _itemCount)    // 인벤토리에 아이템을 Load하기
    {
        for (int i = 0; i < items.Length; i++)   
        {
            if (items[i].itemName == _itemName)                 // 존재하는 아이템과 저장한 아이템의 이름이 같으면
                slots[_arrayNum].AddItem(items[i], _itemCount); // 아이템 로드해서 인벤토리에 넣기
        }
    }

    // 도감 Item 계열 불러오기
    public void LoadToCollectionItem(int _arrayNum, string _itemName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
            {
                c_itemSlots[_arrayNum].AddCollection(items[i]);
                Debug.Log(c_itemSlots[_arrayNum].item_Collection);
            }
        }
    }
    
    // 도감 ETC 계열 불러오기
    public void LoadToCollectionETC(int _arrayNum, string _itemName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
            {
                c_ETCSlots[_arrayNum].AddCollection(items[i]);
                Debug.Log(c_ETCSlots[_arrayNum].item_Collection);
            }
        }
    } 

    // 도감 Note 계열 불러오기
    public void LoadToCollectionNote(int _arrayNum, string _itemName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
            {
                c_NoteSlots[_arrayNum].AddCollection(items[i]);
                Debug.Log(c_NoteSlots[_arrayNum].item_Collection);
            }
        }
    }

    void Start()
    {
        c_Info = FindObjectOfType<Collection_Information>();
        furAct = FindObjectOfType<FurnitureAction>();
        slots = go_slotsParent.GetComponentsInChildren<Slot>();
        c_itemSlots = go_CollectionItemParent.GetComponentsInChildren<CollectionItem>();
        c_ETCSlots = go_CollectionEtcParent.GetComponentsInChildren<CollectionETC>();
        c_NoteSlots = go_CollectionNoteParent.GetComponentsInChildren<CollectionNote>();
    }

    public void AcquireItem(Item _item, int _count = 1) // 아이템 습득 시 발생하는 함수 
    {
        if (Item.ItemType.Potion == _item.itemType)     // 습득한 아이템이 포션일 경우에 실행
        {
            for(int i = 0; i< slots.Length; i++)      // 인벤토리 창 전부를 뒤져서
            {
                if (slots[i].item != null)            // 아이템이 하나 이상 있다면
                {
                    if (slots[i].item.itemName == _item.itemName)   // 내가 주운 아이템이 그 아이템과 똑같은 아이템이면 
                    {
                        if (slots[i].itemCount >= 99)
                        {
                            PlayerScript.instance.NPCPrint("더 가질 수 없습니다.");
                            return;
                        }
                        
                        slots[i].SetSlotColor(_count);              // 그 아이템의 갯수를 추가시켜준다.
                        PlayerScript.instance.DesPrint(slots[i].item.itemRealName + " (를)을 얻었다");
                        SoundManager.instance.PlaySoundEffect("Pick_Up_Item");
                        return;
                    }
                }
                else                                  // 동일한 아이템이 없다면
                {
                    slots[i].AddItem(_item);          // 아이템을 슬롯에 추가시켜준다.
                    PlayerScript.instance.DesPrint(slots[i].item.itemRealName + " (를)을 얻었다");
                    SoundManager.instance.PlaySoundEffect("Pick_Up_Item");
                    break;
                }
            }

            for (int i = 0; i < c_itemSlots.Length; i++)
            {
                if (c_itemSlots[i].item_Collection != null)
                {
                    if (c_itemSlots[i].item_Collection.itemName == _item.itemName)
                    {
                        if (!c_itemSlots[i].isAcquired)
                        {
                            c_itemSlots[i].AddCollection(_item);
                            SoundManager.instance.PlaySoundEffect("Pick_Up_Item");
                            return;
                        }
                    }
                }
            }
            return;
        }
        
        if(Item.ItemType.ETC == _item.itemType)
        {
            for (int i = 0; i < c_ETCSlots.Length; i++)
            {
                if(c_ETCSlots[i].item_Collection != null)
                {
                    if (c_ETCSlots[i].item_Collection.itemName == _item.itemName)
                    {
                        if (!c_ETCSlots[i].isAcquired)
                        {
                            c_ETCSlots[i].AddCollection(_item);
                            PlayerScript.instance.DesPrint(c_ETCSlots[i].item_Collection.itemRealName + " (를)을 얻었다");
                            SoundManager.instance.PlaySoundEffect("Pick_Up_Item");
                            return;
                        }
                    }
                }
            }
        }      
        
        if(Item.ItemType.Note == _item.itemType)
        {
            for (int i = 0; i < c_NoteSlots.Length; i++)
            {
                if(c_NoteSlots[i].item_Collection != null)
                {
                    if (c_NoteSlots[i].item_Collection.itemName == _item.itemName)
                    {
                        if (!c_NoteSlots[i].isAcquired)
                        {
                            c_NoteSlots[i].AddCollection(_item); 
                            PlayerScript.instance.DesPrint(c_NoteSlots[i].item_Collection.itemRealName + " (를)을 얻었다");
                            SoundManager.instance.PlaySoundEffect("Pick_Up_Item");
                            c_Info.c_InformationBox.SetActive(true);
                            c_Info.c_ItemTitle.text = c_NoteSlots[i].item_Collection.itemRealName;
                            c_Info.c_ItemDes.text = c_NoteSlots[i].item_Collection.itemRealDesc;
                            c_Info.c_ItemImage.sprite = c_NoteSlots[i].item_Collection_Image.sprite;
                            GameManager.instance.OpenUI();
                            SoundManager.instance.PlaySoundEffect("Page_Turn");
                            if (c_NoteSlots[i].item_Collection.itemName == "NOTE_01")
                            {
                                furAct.StartCoroutine(furAct.fl_Coroutine);
                                PlayerScript.instance.NPCPrint("전등이 깜빡거린다.");
                            }
                            if (c_NoteSlots[i].item_Collection.itemName == "Book_04")
                            {
                                SoundManager.instance.PlaySoundEffect("Open_Door");
                                furAct.toilet_Door00.GetComponent<OpenManager>().isLocked = false;
                                furAct.toilet_Door00.GetComponent<Animator>().SetBool("IsOpen", true);
                                furAct.key_end.SetActive(true);
                                furAct.book05.SetActive(true);
                                ObjectManager.instance.KineToDyna(furAct.book05);
                                SoundManager.instance.PlaySoundEffect("ArtFrame_Drop");
                            }
                            return;
                        }
                    }
                }
            }
        }
    }
}
