using TMPro;
using UnityEngine;

public class ShopGameSlot : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Cost;

    private ShopItemScritpableObject ShopItem;

    public void Init(ShopItemScritpableObject InitItem)
    {
        Name.text = InitItem.Name;
        Level.text = (GameManager.ValueHolder.UpgradesLevels[ShopItemScritpableObject.RuntimeLookup[InitItem.GUIDContainer.Guid]] + 1).ToString() + " Óð. ";
        Cost.text = InitItem.CalculateCost().ToString();
        ShopItem = InitItem;
    }


    public void Refresh()
    {

        Level.text = (GameManager.ValueHolder.UpgradesLevels[ShopItemScritpableObject.RuntimeLookup[ShopItem.GUIDContainer.Guid]] + 1).ToString() + " Óð. ";
        Cost.text = ShopItem.CalculateCost().ToString();
    }


    public void BuyUpgrade()
    {
        var cost = ShopItem.CalculateCost();

        if (cost > GameManager.ValueHolder.Points)
        {
            Debug.Log("Error, nety deneg");
            return;
        }

        if( GameManager.ValueHolder.UpgradesLevels[ShopItemScritpableObject.RuntimeLookup[ShopItem.GUIDContainer.Guid]] > ShopItem.MaxLvl)
        {
            Debug.Log("Error, max upg");
            return;
        }



        Debug.Log("Sold");
        GameManager.ValueHolder.Points -= cost;
        GameManager.ValueHolder.UpgradesLevels[ShopItemScritpableObject.RuntimeLookup[ShopItem.GUIDContainer.Guid]] += 1;
        foreach (var ValuesPair in ShopItem.UpgradeEnumsVal)
        {
            GameManager.ValueHolder.EnumsValuesDictionary[ValuesPair.Enum] += ValuesPair.val;
        }
        GameManager.SaveData();
        Refresh();

    }
}
