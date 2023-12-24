using UnityEngine;

public static class SaveInitiator
{

    private static readonly string SaveKey = "Save";





    public static void SaveValues(UpgradeValueHolder HolderToSave)
    {
       var ConvertedHolder = JsonUtility.ToJson(HolderToSave,true);
        Debug.Log("Save - " + ConvertedHolder);
        PlayerPrefs.SetString(SaveKey, ConvertedHolder);
    }


    public static UpgradeValueHolder GetValues()
    {
        var LoadedJsonHolder = PlayerPrefs.GetString(SaveKey);
        Debug.Log("get - " + LoadedJsonHolder);
        var Holder = JsonUtility.FromJson<UpgradeValueHolder>(LoadedJsonHolder);
        return Holder;
    }

}
