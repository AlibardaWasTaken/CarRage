using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance = null;
    [SerializeField]
    private List<ShopItemScritpableObject> _Items = new List<ShopItemScritpableObject>();


    [SerializeField]
    private GameObject _UpgradePrefab;

    [SerializeField]
    private TextMeshProUGUI _PointsText;


    public GameObject contentObject;

    public List<ShopItemScritpableObject> Items { get => _Items; set => _Items = value; }


    public GameObject UpgradePrefab { get => _UpgradePrefab; }
    public TextMeshProUGUI PointsText { get => _PointsText; }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
            
        else
        {
            Destroy(gameObject);

        }

    }

    public void Start()
    {
        CreateUpgradesPrefabs();
        RefreshPointsText();
    }

    public static void RefreshPointsText()
    {
        instance._PointsText.text = GameManager.ValueHolder.Points.ToString();
    }

    private void CreateUpgradesPrefabs()
    {
        foreach (var itemScritpableObject in Items)
        {
           var obj = Instantiate(UpgradePrefab, contentObject.transform);
            obj.GetComponent<ShopGameSlot>().Init(itemScritpableObject);
        }
    }





}
