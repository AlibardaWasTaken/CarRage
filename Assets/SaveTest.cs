using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveTest : MonoBehaviour
{
    
    public UpgradeValueHolder holder = new UpgradeValueHolder();

    public TextMeshProUGUI TextObj;
    public void Start()
    {
        //holder = SaveInitiator.GetValues();
        //var values = SaveInitiator.GetValues();
        ////if (values != null)
        ////{
        ////    Load();
        ////}
        ////else
        ////{
        ////    holder = new UpgradeValueHolder();
        ////}
    }
    public void PlusOneStat ()
    {
        holder.Points ++;
        TextObj.text = holder.Points.ToString();
    }


    public void MinusOneStat()
    {
        holder.Points--;
        TextObj.text = holder.Points.ToString();
    }

    public void Save()
    {
        SaveInitiator.SaveValues(holder);
    }


    public void Load()
    {
       holder = SaveInitiator.GetValues();
        TextObj.text = holder.Points.ToString();
    }

}
