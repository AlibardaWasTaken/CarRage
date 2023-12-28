using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueTester : MonoBehaviour
{
    private UpgradeValueHolder holder;

    public List<ShopItemScritpableObject> items = new List<ShopItemScritpableObject>();

    private void Start()
    {
        holder = new UpgradeValueHolder();



        //  holder.EnumsValuesDictionary.Add(items[0].UpgradeEnumsVal[0].Enum, items[0].UpgradeEnumsVal[0].val);



        foreach (var item in items)
        {
            holder.UpgradesLevels.Add(item, 0);
            foreach (var en in item.UpgradeEnumsVal)
            {
                holder.EnumsValuesDictionary.Add(en.Enum, en.val);
            }
        }
       
        

    }


    public void Save()
    {
        SaveInitiator.SaveValues(holder);
    }

    public void Load()
    {
        holder = SaveInitiator.GetValues();
    }

}
