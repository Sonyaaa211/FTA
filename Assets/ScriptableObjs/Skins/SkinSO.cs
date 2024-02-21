using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "SkinSO", menuName = "SkinInfo")]
public class SkinSO : ScriptableObject
{
    public Image skinIcon;
    public string skinName;
    public Curency curency = Curency.Diamond;
    public int skinPrice;
    public GameObject skinPrefab;
    public bool ownSkin = false;
}
