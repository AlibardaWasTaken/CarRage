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
        _points = Points;
        foreach (KeyValuePair<ShopItemScritpableObject, int> kvp in UpgradesLevels)
        {
            _upgradesLevels.Add(kvp.Key.GUIDContainer);
            _upgradeCount.Add(kvp.Value);
        }

        
    }


    public void OnAfterDeserialize()
    {
       

        for (int i = 0; i < _upgradeCount.Count; i++)
        {
            UpgradesLevels[(ShopItemScritpableObject.RuntimeLookup[_upgradesLevels[i].Guid])] =
                _upgradeCount[i];

            foreach (var ValuesPair in ShopItemScritpableObject.RuntimeLookup[_upgradesLevels[i].Guid].UpgradeEnumsVal)
            {
                EnumsValuesDictionary[ValuesPair.Enum] += ValuesPair.val;
            }
            
        }

        Points = _points;
    }



}
