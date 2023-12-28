using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeValueHolder : ISerializationCallbackReceiver
{

    [NonSerialized] public int Points;
    [NonSerialized] public Dictionary<ShopItemScritpableObject, int> UpgradesLevels = new Dictionary<ShopItemScritpableObject, int>();

    [NonSerialized] public Dictionary<UpgradeEnums, int> EnumsValuesDictionary = new Dictionary<UpgradeEnums, int>();


    [SerializeField] private int _points;
    [SerializeField] private List<GUIDContainer> _upgradesLevels = new List<GUIDContainer>();
    [SerializeField] private List<int> _upgradeCount = new List<int>();

    public void OnBeforeSerialize()
    {
        _upgradesLevels.Clear();
        _upgradeCount.Clear();
        _points = Points;
        foreach (KeyValuePair<ShopItemScritpableObject, int> kvp in UpgradesLevels)
        {
            _upgradesLevels.Add(kvp.Key.GUIDContainer);
            _upgradeCount.Add(kvp.Value);
        }

        
    }


    public void AddEnums()
    {
        Debug.Log("AddingEnumsToValUpg");
        foreach (var Upg in (UpgradeEnums[])Enum.GetValues(typeof(UpgradeEnums)))
        {
            EnumsValuesDictionary.Add(Upg, 0);
        }
    }



    public void OnAfterDeserialize()
    {
        AddEnums();



        for (int i = 0; i < _upgradeCount.Count; i++)
        {
            UpgradesLevels[(ShopItemScritpableObject.RuntimeLookup[_upgradesLevels[i].Guid])] =
                _upgradeCount[i];
            
        }

        Points = _points;

        UpdateEnums();
    }

    private void UpdateEnums()
    {
        foreach (var kvp in UpgradesLevels)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                foreach (var EnumPair in kvp.Key.UpgradeEnumsVal)
                {
                    EnumsValuesDictionary[EnumPair.Enum] += EnumPair.val;
                }
            }
        }

        //foreach (var ValuesPair in ShopItemScritpableObject.RuntimeLookup[_upgradesLevels[i].Guid].UpgradeEnumsVal)
        //{
        //    EnumsValuesDictionary[ValuesPair.Enum] += ValuesPair.val;
        //}
    }


}
