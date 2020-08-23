using UnityEngine;

// ScriptableObject 클래스는 게임 오브젝트에 붙이지 않아도 기능한다.
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemRealName;         // 툴팁상 표기될 아이템의 이름
    public string itemName;             // 스크립트상 구분 될 아이템의 이름
    [TextArea]                          // 한줄에서 개행이 가능해짐
    public string itemDesc;             // 아이템 툴팁 설명
    [TextArea]
    public string itemRealDesc;         // 도감에서 클릭 시 호출되는 아이템 설명
    public Sprite itemImage;            // 아이템의 이미지
    public GameObject itemPrefab;       // 아이템의 프리팹
    public ItemType itemType;           // 아이템의 종류

    // 아이템 분류
    public enum ItemType
    {
        Potion,
        Note,
        ETC,
    };
}
