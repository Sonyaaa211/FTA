using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class UIManager : Singleton<UIManager>, StartGame
{
    [SerializeField] PlayerInfor playerInfor;

    

    [Header("UI window")]
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject intro;
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;

    [Header("Curency")]
    public TMP_Text coinText;
    public TMP_Text diamondText;

    [Header("Shop related")]
    public SkinSO[] skins;
    public ItemSO[] items;
    [SerializeField] private Transform shopSkinTransform;
    [SerializeField] private Transform shopItemTransform;
    [SerializeField] private GameObject itemBox;

    [Header("Inventory related")]
    [SerializeField] private Transform invenSkinTransform;
    [SerializeField] private Transform invenItemTransform;
    [SerializeField] private GameObject invenItemBox;

    public IEnumerator StartGame()
    {
        yield return null;
    }
    public void UpdateCurency()
    {
        coinText.text = playerInfor.coin.ToString();
        diamondText.text = playerInfor.diamond.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateCurency();

        foreach(var skin in skins)
        {
            GameObject tmp = Instantiate(itemBox, shopSkinTransform);
            tmp.GetComponent<SkinBoxCreate>().BoxInit(skin);

            if (skin.ownSkin)
            {
                tmp = Instantiate(invenItemBox, invenSkinTransform);
                tmp.GetComponent <SkinBoxCreate>().BoxInit(skin);
            }
        }
        foreach (var item in items)
        {
            GameObject tmp = Instantiate(itemBox, shopItemTransform);
            tmp.GetComponent<SkinBoxCreate>().BoxInit(item);

            if (item.ownsItem) 
            {
                tmp = Instantiate(invenItemBox, invenItemTransform);
                tmp.GetComponent<SkinBoxCreate>().BoxInit(item);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
