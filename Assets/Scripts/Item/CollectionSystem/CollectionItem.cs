using UnityEngine;

// 도감 시스템을 상속시킴
public class CollectionItem : CollectionSystem
{
    // 버츄얼 or 추상으로 만들어진 메서드는 자식 클래스에서 override 시켜서 재구현.
    public override void SetColor(float _rgb)
    {
        // 도감 아이템의 이미지를 가져와 컬러값을 변경해주는 작업.
        Color color = item_Collection_Image.color;
        color.r = _rgb; color.g = _rgb; color.b = _rgb;
        item_Collection_Image.color = color;
    }

    // 종류별로 도감 시스템에 보관하도록 하기 위해 클래스를 나눔.
    // 소모품 획득 시 소모품 클래스로, 노트 획득 시 노트 클래스로
    public override void AddCollection(Item _item)
    {
        item_Collection = _item;
        isAcquired = true;
        SetColor(1f);
    }
}
