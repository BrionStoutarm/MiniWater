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

    private Villager selectedVillager;
    private BuildingPlacedObject selectedBuilding;


    public int foodConsumptionModifier = 1;
    public int waterConsumptionModifier = 2;

    public event EventHandler<OnBuildingSelectedArgs> OnBuildingSelectedEvent;
    public class OnBuildingSelectedArgs : EventArgs {
        public BuildingPlacedObject selectedBuilding;
    }

    public event EventHandler<OnVillagerSelectedArgs> OnVillagerSelectedEvent;
    public class OnVillagerSelectedArgs : EventArgs {
        public Villager selectedVillager;
    }

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
        s_instance = this;
        foodSupply = startFoodSupply;
        woodSupply = startWoodSupply;
        waterSupply = startWaterSupply;
        metalSupply = startMetalSupply;

        GridBuildingSystem.Instance.OnPlacedBuilding += HandlePlacedBuilding;
        GameManager.Instance.TimeStepEvent += ConsumeVillagerResources;
        PlayerInput.OnRightClickEvent += HandleRightClickEvent;
        ClickSelectController.SelectedVillagerChanged += HandleSelectedVillager;
        ClickSelectController.SelectedBuildingChanged += HandleSelectedBuilding;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void CreateVillager() {
        Villager villager = Instantiate(s_instance.villagerPrefab, new Vector3(0, 14, 0), Quaternion.identity);
        s_instance.villagerList.Add(villager);
        Instance.inactiveVillagers.Enqueue(villager);
    }

    //will be triggered by a button on each building 
    public void AssignVillagers(int numberOfVillagers, Vector3 goal) {
        for(int i = 0; i < numberOfVillagers; i++) {
            if (inactiveVillagers.Count == 0)
                break;
            Villager villager = inactiveVillagers.Dequeue();
            villager.Assign(goal);
        }
    }

    void ConsumeVillagerResources(object sender, GameManager.TimeStepArgs args) {
        foodSupply -= villagerList.Count * foodConsumptionModifier;
        waterSupply -= villagerList.Count * waterConsumptionModifier;
        if(OnResourceAmountChange != null) { OnResourceAmountChange(this, new OnResourceAmountChangeArgs { foodSupply = foodSupply, waterSupply = waterSupply , metalSupply = metalSupply, woodSupply = woodSupply});  }
        //UpdateUI();
    }

    void HandlePlacedBuilding(object sender, GridBuildingSystem.OnPlacedBuildingArgs args) {
        foodSupply -= args.placedObject.cost;
        waterSupply -= args.placedObject.cost;
        woodSupply -= args.placedObject.cost;
        metalSupply -= args.placedObject.cost;

        if (OnResourceAmountChange != null) { OnResourceAmountChange(this, new OnResourceAmountChangeArgs { foodSupply = foodSupply, waterSupply = waterSupply, woodSupply = woodSupply, metalSupply = metalSupply }) ; }
    }

    void HandleSelectedVillager(object sender, ClickSelectController.SelectedVillagerArgs args) {
        selectedVillager = args.selectedVillager;
        if (selectedVillager != null) {
            Debug.Log("Selected Villager: " + selectedVillager.name);
        }
        else {
            Debug.Log("Deselect Villager");
        }
    }

    void HandleSelectedBuilding(object sender, ClickSelectController.SelectedBuildingArgs args) { 
        selectedBuilding = args.selectedBuilding;
        if(selectedBuilding != null) {
            Debug.Log("Selected building: " + selectedBuilding.name);
        }
        else {
            Debug.Log("Deselect building");
        }
    }

    void HandleRightClickEvent(object sender, PlayerInput.OnRightClickArgs args) {
        if(selectedVillager != null) {
            RaycastHit hit = StaticFunctions.GetMouseRaycastHit();
            BuildingPlacedObject building = hit.collider.gameObject.transform.parent.GetComponent<BuildingPlacedObject>();
            if (building != null) {
                building.AssignVillager(selectedVillager);
            }
            selectedVillager.Assign(StaticFunctions.GetMouseRaycastHit().point);
        }
    }

    void HandleSelectedObject(object sender, PlayerInput.OnObjectSelectedArgs args) {
        GameObject obj = args.obj;
        if(obj.transform.parent.GetComponent("Villager") != null) {
            Debug.Log("Selected Villager");
            selectedVillager = obj.transform.parent.GetComponent<Villager>();
            if (OnVillagerSelectedEvent != null) { OnVillagerSelectedEvent(this, new OnVillagerSelectedArgs { selectedVillager = selectedVillager }); }
        }
        if (obj.transform.parent.GetComponent("BuildingPlacedObject") != null) {
            Debug.Log("Selected Building");
        }
    }
}
