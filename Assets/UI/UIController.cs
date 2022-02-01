using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Button toggleBuildButton;
    public Text foodSupplyText;
    public Text waterSupplyText;
    public Canvas uiCanvas;
    public SelectedObjectArea selectedObjectUI;
    private ObjectUI selectedObject;

    //[SerializeField] private SelectedObjectArea m_selectedObjectArea;

    public class UILoadEventArgs : EventArgs { }
    public static EventHandler<UILoadEventArgs> UILoadEvent;

    private BuildingPlacedObject m_currentSelectedBuilding;
    private void Awake() {
        if (GridBuildingSystem.Instance != null) {
            toggleBuildButton.onClick.AddListener(GridBuildingSystem.Instance.toggleActive);
        }
        if (VillageManager.Instance != null) {
            VillageManager.Instance.OnResourceAmountChange += UpdateResourceUI;
        }
        if(ClickSelectController.Instance != null) {
            ClickSelectController.Instance.SelectedBuildingChanged += HandleSelectedBuilding;
            ClickSelectController.Instance.SelectedVillagerChanged += HandleSelectedVillager;
        }
        foodSupplyText.text = "Food Supply: " + VillageManager.Instance.foodSupply.ToString();
        waterSupplyText.text = "Water Supply: " + VillageManager.Instance.waterSupply.ToString();
    }

    private void Update() {
        if(selectedObject != null) {
            selectedObject.ShowUI(selectedObjectUI);
        }
    }

    void UpdateResourceUI(object sender, VillageManager.OnResourceAmountChangeArgs args) {
        foodSupplyText.text = "Food Supply: " + args.foodSupply.ToString();
        waterSupplyText.text = "Water Supply: " + args.waterSupply.ToString();
    }

    void HandleSelectedBuilding(object sender, ClickSelectController.SelectedBuildingArgs args) {
        Debug.Log("UIController building");
        selectedObject = args.selectedBuilding;
    }
    void HandleSelectedVillager(object sender, ClickSelectController.SelectedVillagerArgs args) {
        Debug.Log("UIController villager");
        selectedObject = args.selectedVillager;
    }
}
