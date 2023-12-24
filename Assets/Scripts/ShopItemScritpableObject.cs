using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CarRage/Upgrade")]
public class ShopItemScritpableObject : ScriptableObject, ISerializationCallbackReceiver
{

    public int InitialCost = 50;
    public float CostMult = 1.5f;
    public int MaxLvl = 50;
    public string Name ;
    public static Dictionary<Guid, ShopItemScritpableObject> RuntimeLookup = new Dictionary<Guid, ShopItemScritpableObject>();

    public List<UpgradeEnumValue> UpgradeEnumsVal = new List<UpgradeEnumValue>();
    [HideInInspector] public GUIDContainer GUIDContainer;

    public int CalculateCost()
    {
        var cost = (int)(InitialCost + CostMult * GameManager.ValueHolder.UpgradesLevels[this]);
       // Debug.Log(cost);
        return cost;
    }


    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        Debug.Log("DESERIALIZE " + GUIDContainer.Guid);
        RuntimeLookup[GUIDContainer.Guid] = this;
    }
}


[Serializable]
public struct GUIDContainer : ISerializationCallbackReceiver
{
    private Guid _guid;
    [SerializeField] private string _serializedGuid;



    public GUIDContainer(Guid guid)
    {
        _guid = guid;
        _serializedGuid = _guid.ToString();
    }

    public System.Guid Guid
    {
        get { return _guid; }
    }

    public void OnBeforeSerialize()
    {
        if (Guid != System.Guid.Empty)
        {
            _serializedGuid = Guid.ToString();
        }
    }

    public void OnAfterDeserialize()
    {
        //Debug.Log("I am dping spm");
        if (_serializedGuid != null && !string.IsNullOrEmpty(_serializedGuid))
        {
            _guid = new System.Guid(_serializedGuid);
            return;
        }
        _guid = Guid.NewGuid();

    }
}
