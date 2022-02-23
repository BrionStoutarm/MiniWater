using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageManager : MonoBehaviour {
    public int startFoodSupply, startWaterSupply, startWoodSupply, startMetalSupply;

    public int foodSupply { get; private set; }
    public int waterSupply { get; private set; }
    public int woodSupply { get; private set; } 
    public int metalSupply { get; private set; }

    public Villager villagerPrefab;
    public List<Villager> villagerList;
    private Queue<Villager> inactiveVillagers;

    private List<BuildingPlacedObject> buildingList;

    private Villager selectedVillager;
    private BuildingPlacedObject selectedBuilding;


    public int foodConsumptionModifier = 1;
    public int waterConsumptionModifier = 2;

    public event EventHandler<OnResourceAmountChangeArgs> OnResourceAmountChange;
    public class OnResourceAmountChangeArgs : EventArgs {
        public int foodSupply;
        public int waterSupply;
        public int woodSupply;
        public int metalSupply;
        //more to come
    }

    private static VillageManager s_instance;
    public static VillageManager Instance {
        get => s_instance;
        set {
            if(value != null && s_instance == null) {
                s_instance = value;
            }
        }
    }

    private void Awake() {
        villagerList = new List<Villager>();
        inactiveVillagers = new Queue<Villager>();
        buildingList = new List<BuildingPlacedObject>();
        s_instance = this;
        foodSupply = startFoodSupply;
        woodSupply = startWoodSupply;
        waterSupply = startWaterSupply;
        metalSupply = startMetalSupply;

        GridBuildingSystem.Instance.OnPlacedBuilding += HandlePlacedBuilding;
        GameManager.Instance.TimeStepEvent += ConsumeVillagerResources;
        ClickSelectController.Instance.SelectedVillagerChanged += HandleSelectedVillager;
        ClickSelectController.Instance.SelectedBuildingChanged += HandleSelectedBuilding;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<BuildingPlacedObject> BuildingsWithFoodToBeStored() {
        List<BuildingPlacedObject> retList = new List<BuildingPlacedObject>();
        foreach(BuildingPlacedObject building in buildingList) {
            if (building.HasFoodWaiting()) {
                retList.Add(building);
            }
        }
        return retList;
    }

    public static void CreateVillager() {
        Villager villager = Instantiate(s_instance.villagerPrefab, new Vector3(0, 14, 0), Quaternion.identity);
        s_instance.villagerList.Add(villager);
        Instance.inactiveVillagers.Enqueue(villager);
    }

    public bool HasSelectedVillager() { return selectedVillager != null; }

    public void AssignSelectedVillager(BuildingPlacedObject building) {
        selectedVillager.Assign(building);
        building.AssignVillager(selectedVillager);
    }
    public void AssignSelectedVillager(Vector3 destination) {
        selectedVillager.Assign(destination);
    }

    public void UnassignVillager() {
        selectedVillager.Unassign();
    }

    void ConsumeVillagerResources(object sender, GameManager.TimeStepArgs args) {
        foodSupply -= villagerList.Count * foodConsumptionModifier;
        waterSupply -= villagerList.Count * waterConsumptionModifier;
        if(OnResourceAmountChange != null) { OnResourceAmountChange(this, new OnResourceAmountChangeArgs { foodSupply = foodSupply, waterSupply = waterSupply , metalSupply = metalSupply, woodSupply = woodSupply});  }
        //UpdateUI();
    }

    void HandlePlacedBuilding(object sender, GridBuildingSystem.OnPlacedBuildingArgs args) {

        if (OnResourceAmountChange != null) { OnResourceAmountChange(this, new OnResourceAmountChangeArgs { foodSupply = foodSupply, waterSupply = waterSupply, woodSupply = woodSupply, metalSupply = metalSupply }) ; }
        buildingList.Add(args.placedObject);
    }

    void HandleSelectedVillager(object sender, ClickSelectController.SelectedVillagerArgs args) {
        selectedVillager = args.selectedVillager;
        if (selectedVillager != null) {
            //Debug.Log("Selected Villager: " + selectedVillager.name);
        }
        else {
            //Debug.Log("Deselect Villager");
        }
    }

    void HandleSelectedBuilding(object sender, ClickSelectController.SelectedBuildingArgs args) { 
        selectedBuilding = args.selectedBuilding;
        if(selectedBuilding != null) {
            //Debug.Log("Selected building: " + selectedBuilding.name);
        }
        else {
            //Debug.Log("Deselect building");
        }
    }
}
