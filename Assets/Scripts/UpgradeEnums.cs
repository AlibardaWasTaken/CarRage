
public enum UpgradeEnums
{
 Fuel,
 MaxHumans,
 Crusher,
 Armor,
 Points
}

[System.Serializable]
public class UpgradeEnumValue
{
    public UpgradeEnums Enum;
    public int val;
}