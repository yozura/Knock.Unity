using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;

    public Slot dragSlot;
    
    // 아이템 이미지
    [SerializeField]
    private Image item_Image = null;

    void Start()
    {
        instance = this;    
    }

    public void SetImage(Image _itemImage)
    {
        item_Image.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = item_Image.color;
        color.a = _alpha;
        item_Image.color = color;
    }

}
