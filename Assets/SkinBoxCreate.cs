using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkinBoxCreate : MonoBehaviour
{
    public PlayerInfor playerInfor;
    public SkinSO skinInfo;
    public ItemSO itemInfo;
    public TMP_Text name;
    public TMP_Text price;
    public Image icon;
    public Image curencyIcon;

    private void Start()
    {
       
    }
    public void BoxInit(SkinSO info)
    {
        skinInfo = info;
        name.text = skinInfo.skinName;
        price.text = skinInfo.skinPrice.ToString();
        if(skinInfo.skinIcon != null) { icon = skinInfo.skinIcon; }
    }

    public void BoxInit(ItemSO info)
    {
        itemInfo = info;
        name.text = itemInfo.itemName;
        price.text = itemInfo.name.ToString();
        if(itemInfo.itemImage != null) { icon = itemInfo.itemImage; }
    }

    public void Buy()
    {
        if (skinInfo != null)
        {
            if(skinInfo.curency == Curency.Diamond)
            {
                if(playerInfor.diamond > skinInfo.skinPrice)
                {
                    Array.Resize(ref playerInfor.skinsOwned, playerInfor.skinsOwned.Length + 1);
                    playerInfor.skinsOwned[playerInfor.skinsOwned.Length - 1] = skinInfo;
                    playerInfor.diamond -= skinInfo.skinPrice;
                    skinInfo.ownSkin = true;
                    UIManager.Instance.UpdateCurency();
                }
                else
                {

                }
            }
            else
            {
                if (playerInfor.coin > skinInfo.skinPrice)
                {
                    Array.Resize(ref playerInfor.skinsOwned, playerInfor.skinsOwned.Length + 1);
                    playerInfor.skinsOwned[playerInfor.skinsOwned.Length - 1] = skinInfo;
                    playerInfor.coin -= skinInfo.skinPrice;
                    skinInfo.ownSkin = true;
                    UIManager.Instance.UpdateCurency();
                }
                else
                {

                }
            }
        }
        if (itemInfo != null)
        {
            if (itemInfo.curency == Curency.Diamond)
            {
                if (playerInfor.diamond > itemInfo.itemPrice)
                {
                    Array.Resize(ref playerInfor.itemsOwned, playerInfor.itemsOwned.Length + 1);
                    playerInfor.itemsOwned[playerInfor.itemsOwned.Length - 1] = itemInfo;
                    playerInfor.diamond -= itemInfo.itemPrice;
                    itemInfo.ownsItem = true;
                    UIManager.Instance.UpdateCurency();
                }
                else
                {

                }
            }
            else
            {
                if (playerInfor.coin > itemInfo.itemPrice)
                {
                    Array.Resize(ref playerInfor.itemsOwned, playerInfor.itemsOwned.Length + 1);
                    playerInfor.itemsOwned[playerInfor.itemsOwned.Length - 1] = itemInfo;
                    playerInfor.coin -= itemInfo.itemPrice;
                    itemInfo.ownsItem= true;
                    UIManager.Instance.UpdateCurency();
                }
                else
                {

                }
            }
        }

    }
}
