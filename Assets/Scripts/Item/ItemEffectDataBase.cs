using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;       // 아이템의 이름
    public string part;           // 효과받을 대상
    public int num;               // 효능
}

public class ItemEffectDataBase : MonoBehaviour
{
    private const string SP = "SP";
    
    // 필요한 컴포넌트
    [SerializeField] private ItemEffect[] itemEffects = null;
    [SerializeField] private GameObject p_SP_POTION = null;
    private Inventory theInvens;
    private StatusController thePlayerStatus;
    public GameObject player;

    private void Awake()
       => DontDestroyOnLoad(gameObject);

    private void Update()
       => QuickSlots();

    public void UseItem(Item _item)
    {
        theInvens = FindObjectOfType<Inventory>();
        thePlayerStatus = FindObjectOfType<StatusController>();
        if (_item.itemType == Item.ItemType.Potion)
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if(itemEffects[i].itemName == _item.itemName)
                {
                    for(int j = 0; j < itemEffects[i].part.Length; j++)
                    {
                        switch (itemEffects[i].part)
                        {
                            case SP:
                                thePlayerStatus.IncreaseSP(itemEffects[i].num);
                                GameObject sp_clone = Instantiate(
                                                        p_SP_POTION,
                                                        new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z),
                                                        Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;
                                sp_clone.transform.parent = player.transform;
                                PlayerScript.instance.SoundPrint(theInvens.c_itemSlots[0].item_Collection.itemRealName + " 을(를) 마시는 소리");
                                SoundManager.instance.PlaySoundEffect("Potion_Drink");
                                Destroy(sp_clone, 1f);
                                break;
                            default:
                                Debug.Log("선택이 잘못되었습니다");
                                break;
                        }
                    }
                }
            }
            return;
        }
    }

    public void QuickSlots()
    {
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            theInvens = FindObjectOfType<Inventory>();
            for (int i = 0; i < theInvens.slots.Length; i++)
            {
                if (theInvens.slots[0].item != null)
                {
                    UseItem(theInvens.slots[0].item);
                    theInvens.slots[0].SetSlotColor(-1);
                    return;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            theInvens = FindObjectOfType<Inventory>();
            for (int i = 0; i < theInvens.slots.Length; i++)
            {
                if (theInvens.slots[1].item != null)
                {
                    UseItem(theInvens.slots[1].item);
                    theInvens.slots[1].SetSlotColor(-1);
                    return;
                }
            }
        }
    }
}
