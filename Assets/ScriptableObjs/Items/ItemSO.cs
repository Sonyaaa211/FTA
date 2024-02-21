using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ItemInfo")]
public class ItemSO : ScriptableObject
{
    public Image itemImage;
    public string itemName;

    public string itemDescription;

    public int itemPrice;
    public Curency curency = Curency.Coin;

    public GameObject itemPrefabs;
    public bool ownsItem = false;
}
