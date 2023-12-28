using TMPro;
using UnityEngine;

public class ShopGameSlot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _level;
    [SerializeField]
    private TextMeshProUGUI _cost;

    private ShopItemScritpableObject ShopItem;
    private int calculatedCost;

    public TextMeshProUGUI Name { get => _name;}
    public TextMeshProUGUI Level { get => _level; }
    public TextMeshProUGUI Cost { get => _cost; }

    public void Init(ShopItemScritpableObject InitItem)
    {
        Name.text = InitItem.Name;
        Level.text = (GameManager.ValueHolder.UpgradesLevels[ShopItemScritpableObject.RuntimeLookup[InitItem.GUIDContainer.Guid]] +1).ToString() + " Óð. ";
        Cost.text = InitItem.CalculateCost().ToString();
        ShopItem = InitItem;
        calculatedCost = ShopItem.CalculateCost();
    }


    public void Refresh()
    {

        Level.text = (GameManager.ValueHolder.UpgradesLevels[ShopItemScritpableObject.RuntimeLookup[ShopItem.GUIDContainer.Guid]] + 1).ToString() + " Óð. ";
        calculatedCost = ShopItem.CalculateCost();
        Cost.text = calculatedCost.ToString();
    }


    public void BuyUpgrade()
    {
       

        if (calculatedCost > GameManager.ValueHolder.Points)
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
        GameManager.ValueHolder.Points -= calculatedCost;
        GameManager.ValueHolder.UpgradesLevels[ShopItemScritpableObject.RuntimeLookup[ShopItem.GUIDContainer.Guid]] += 1;
        //foreach (var ValuesPair in ShopItem.UpgradeEnumsVal)
        //{
        //    GameManager.ValueHolder.EnumsValuesDictionary[ValuesPair.Enum] += ValuesPair.val;
        //}
        GameManager.SaveData();
        Refresh();
        ShopManager.RefreshPointsText();

    }
}
