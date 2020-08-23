using UnityEngine;
using UnityEngine.UI;

public class Collection_Information : MonoBehaviour
{
    public GameObject c_InformationBox;
    public Text c_ItemTitle;
    public Text c_ItemDes;
    public Image c_ItemImage;

    private Inventory invens;

    void Start() => invens = FindObjectOfType<Inventory>();
    
    public void Edit_Information(Item _item)
    {
        if (Item.ItemType.Potion == _item.itemType)
        {
            for (int i = 0; i < invens.c_itemSlots.Length; i++)
            {
                if (invens.c_itemSlots[i].item_Collection.itemName == _item.itemName)
                {
                    if (invens.c_itemSlots[i].isAcquired)
                    {
                        c_ItemTitle.text = invens.c_itemSlots[i].item_Collection.itemRealName;
                        c_ItemDes.text = invens.c_itemSlots[i].item_Collection.itemRealDesc;
                        c_ItemImage.sprite = invens.c_itemSlots[i].item_Collection.itemImage;
                        SetColor(1f);
                        return;
                    }
                    else
                        Default_Information();
                    return;
                }
            }
        }

        if(Item.ItemType.ETC == _item.itemType)
        {
            for (int i = 0; i < invens.c_ETCSlots.Length; i++)
            {
                if (invens.c_ETCSlots[i].item_Collection.itemName == _item.itemName)
                {
                    if (invens.c_ETCSlots[i].isAcquired)
                    {
                        c_ItemTitle.text = invens.c_ETCSlots[i].item_Collection.itemRealName;
                        c_ItemDes.text = invens.c_ETCSlots[i].item_Collection.itemRealDesc;
                        c_ItemImage.sprite = invens.c_ETCSlots[i].item_Collection.itemImage;
                        SetColor(1f);
                        return;
                    }
                    else
                        Default_Information();
                    return;
                }
            }
        }

        if(Item.ItemType.Note == _item.itemType)
        {
            for (int i = 0; i < invens.c_NoteSlots.Length; i++)
            {
                if (invens.c_NoteSlots[i].item_Collection.itemName == _item.itemName)
                {
                    if (invens.c_NoteSlots[i].isAcquired)
                    { 
                        c_ItemTitle.text = invens.c_NoteSlots[i].item_Collection.itemRealName;
                        c_ItemDes.text = invens.c_NoteSlots[i].item_Collection.itemRealDesc;
                        c_ItemImage.sprite = invens.c_NoteSlots[i].item_Collection.itemImage;
                        SetColor(1f);
                        return;
                    }
                    else
                        Default_Information();
                    return;
                }
            }
        }
    }

    public void Default_Information()
    {
        c_ItemTitle.text = "알 수 없음";
        c_ItemDes.text = "알 수 없음";
        c_ItemImage.sprite = null;
        SetColor(0f);
    }

    public void SetColor(float _color)
    {
        Color color = c_ItemImage.color;
        color.r = _color; color.g = _color; color.b = _color; color.a = _color;
        c_ItemImage.color = color;
    }

    public void Exit_Information()
    {
        c_InformationBox.SetActive(false);
        GameManager.instance.CloseUI();
    }
}
