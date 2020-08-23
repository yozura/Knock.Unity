using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private GameObject collection = null;
    [SerializeField] private GameObject potionItemCollection = null;
    [SerializeField] private GameObject noteCollection = null;
    [SerializeField] private GameObject etcCollection = null;
    [SerializeField] private GameObject info_collection = null;

    private SlotToolTip tooltip;

    private void Start() => tooltip = FindObjectOfType<SlotToolTip>();

    void Update()
    {
        if (GameManager.canPlayerMove && !GameManager.isPause)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OpenCollectionWindow();
                GameManager.instance.OpenUI();
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                CloseCollectionWindow();
                GameManager.instance.CloseUI();
                tooltip.HideToolTip();
            }
        }
    }

    private void OpenCollectionWindow() => collection.SetActive(true);

    private void CloseCollectionWindow()
    {
        collection.SetActive(false);
        info_collection.SetActive(false);
        potionItemCollection.SetActive(false);
        etcCollection.SetActive(false);
        noteCollection.SetActive(false);
    }

    // 아이템 수집창
    public void GoPotionItem()
    {
        potionItemCollection.SetActive(true);
        noteCollection.SetActive(false);
        etcCollection.SetActive(false);
        info_collection.SetActive(false);
    }
    
    // 노트 수집창
    public void GoNoteCollection()
    {
        noteCollection.SetActive(true);
        potionItemCollection.SetActive(false);
        etcCollection.SetActive(false);
        info_collection.SetActive(false);
    }

    // 그 외 수집창
    public void GoETCCollection()
    {
        etcCollection.SetActive(true);
        noteCollection.SetActive(false);
        potionItemCollection.SetActive(false);
        info_collection.SetActive(false);
    }
}
