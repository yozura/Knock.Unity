using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    // 인벤토리 툴팁
    [SerializeField]
    private GameObject go_Base_Inven = null;
    [SerializeField]
    private Text txt_Name_inven = null;
    [SerializeField]
    private Text txt_Desc_inven = null;
    [SerializeField]
    private Text txt_How_inven = null;

    // 컬렉션 툴팁
    [SerializeField]
    private GameObject go_Base_Collection = null;
    [SerializeField]
    private Text txt_RealName_Collection = null;
    

    public void ShowToolTipDefault(Vector3 _pos)
    {
        go_Base_Collection.SetActive(true);
        _pos += new Vector3(go_Base_Collection.GetComponent<RectTransform>().rect.width * 0.5f, go_Base_Collection.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        go_Base_Collection.transform.position = _pos;
        txt_RealName_Collection.text = "????";
    }

    public void ShowToolTip(Item _item, Vector3 _position)
    {
        go_Base_Inven.SetActive(true);
        //툴팁 위치 변경
        _position += new Vector3(go_Base_Inven.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base_Inven.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        go_Base_Inven.transform.position = _position;
        txt_Name_inven.text = _item.itemRealName;
        txt_Desc_inven.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Potion)
        {
            txt_How_inven.text = "우클릭 - 사용";
        }
    }

    public void ShowToolTip(Item _item, Vector3 _pos, bool isCollection)
    {
        if(isCollection)
        {
            go_Base_Collection.SetActive(true);
            _pos += new Vector3(go_Base_Collection.GetComponent<RectTransform>().rect.width * 0.5f, go_Base_Collection.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
            go_Base_Collection.transform.position = _pos;
            txt_RealName_Collection.text = _item.itemRealName;
        }
    }

    public void HideToolTip()
    {
        go_Base_Inven.SetActive(false);
        go_Base_Collection.SetActive(false);
    }   
}
