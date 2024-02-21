using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "PlayerInfor")]
public class PlayerInfor : ScriptableObject
{
    public new string playerName;
    public string id;

    public int coin;
    public int diamond;

    public int level = 1;

    public SkinSO[] skinsOwned;
    public ItemSO[] itemsOwned;

    public SkinSO skinUse;
    public ItemSO[] itemsUse;

}
