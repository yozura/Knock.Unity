using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDataBase
{
    public Item items;   
    public Vector3 item_Pos;
}
public class ItemVectorInfo : MonoBehaviour
{
    [SerializeField] private ItemDataBase[] itemDB;
}
