using UnityEngine;

public class CollectionETC : CollectionSystem
{
    public override void SetColor(float _rgb)
    {
        Color color = item_Collection_Image.color;
        color.r = _rgb; color.g = _rgb; color.b = _rgb;
        item_Collection_Image.color = color;
    }

    public override void AddCollection(Item _item)
    {
        item_Collection = _item;
        isAcquired = true;
        SetColor(1f);
    }
}


