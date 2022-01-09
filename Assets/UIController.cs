using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Button toggleBuildButton;
    public Button assignVillagerButton;
    public Text foodSupplyText;
    public Text waterSupplyText;
    public Text woodSupplyText;
    public Text metalSupplyText;

    [SerializeField] private SelectedObjectArea m_selectedObjectArea;

    public class UILoadEventArgs : EventArgs { }
    public static EventHandler<UILoadEventArgs> UILoadEvent;

    private BuildingPlacedObject m_currentSelectedBuilding;
    private void Awake() {
        if(GridBuildingSystem.Instance != null) {
            toggleBuildButton.onClick.AddListener(GridBuildingSystem.Instance.toggleActive);
        }
        if(VillageManager.Instance != null) {
            VillageManager.Instance.OnResourceAmountChange += UpdateResourceUI;
        }


        foodSupplyText.text = "Food Supply: " + VillageManager.Instance.foodSupply.ToString();
        waterSupplyText.text = "Water Supply: " + VillageManager.Instance.waterSupply.ToString();
        woodSupplyText.text = "Wood Supply: " + VillageManager.Instance.woodSupply.ToString();
        metalSupplyText.text = "Metal Supply: " + VillageManager.Instance.metalSupply.ToString();

        ClickSelectController.OnSelectedObjectChanged += UpdateAssignVillagerButton;
    }

    void UpdateAssignVillagerButton(object sender, ClickSelectController.SelectedObjectEventArgs args) {
        if(args.placedObject != null) {
            m_currentSelectedBuilding = args.placedObject as BuildingPlacedObject;
            assignVillagerButton.onClick.AddListener(AssignVillagerButtonOnClick);
        }
    }

    void AssignVillagerButtonOnClick() {
        VillageManager.Instance.AssignVillagers(1, m_currentSelectedBuilding.transform);
    }


    void UpdateResourceUI(object sender, VillageManager.OnResourceAmountChangeArgs args) {
        foodSupplyText.text = "Food Supply: " + args.foodSupply.ToString();
        waterSupplyText.text = "Water Supply: " + args.waterSupply.ToString();
        woodSupplyText.text = "Wood Supply: " + args.woodSupply.ToString();
        metalSupplyText.text = "Metal Supply: " + args.metalSupply.ToString();
    }
}
