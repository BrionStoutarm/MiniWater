using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerJob : JobGameObject
{
    //will extract this later to be villager dependent
    public int carryingCapacity = 5;
    public List<FoodGameObject> carriedObjects;

    //will need to have a priority queue or stack for when there are many things with different priority to be picked up
    private BuildingPlacedObject currentTarget;
    private Queue<BuildingPlacedObject> pickupBuildings;

    private bool isMoving = false;
    private bool isCarrying = false;
    private bool isLooking = false;
    // Start is called before the first frame update
    void Start()
    {
        pickupBuildings = new Queue<BuildingPlacedObject>();
        carriedObjects = new List<FoodGameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isLooking) {
            Debug.Log("Looking for buildings");
            FindTargets();
            if(pickupBuildings.Count > 0) {
                Debug.Log("Found pickup building(s)");
                SetPickupDestination();
            }
        }
        else if(isMoving && !isCarrying) {
            if (workingVillager.HasArrived()) {
                Debug.Log("PickingUpFood");
                PickupFoods();
                SetDropoffDestination();
            }
            //if not full look for more..later
        }
        else if(isMoving && isCarrying) {
            if(workingVillager.HasArrived()) {
                Debug.Log("Dropping off food");
                DropoffFoods();
            }
        }
    }

    public override void DoJob() {
        StartJob();
    }
    
    private void PickupFoods() {
        for(int i = 0; i < carryingCapacity; i++) {
            if (currentTarget.HasFoodWaiting())
                carriedObjects.Add(currentTarget.GetNextFoodObject());
            if (!currentTarget.HasFoodWaiting())
                break;
        }
    }

    private void DropoffFoods() {
        foreach(FoodGameObject food in carriedObjects) {
            currentTarget.AddFoodObject(food);
        }
        carriedObjects.Clear();
        isMoving = false;
        isCarrying = false;
        isLooking = true;
    }

    private void SetPickupDestination() {
        currentTarget = pickupBuildings.Dequeue();
        workingVillager.SetDestination(currentTarget.pickupLocation.position);
        isLooking = false;
        isMoving = true;
    }

    private void SetDropoffDestination() {
        currentTarget = workingVillager.CurrentJobBuilding();
        workingVillager.SetDestination(workingVillager.CurrentJobBuilding().pickupLocation.position);
    }

    private void StartJob() {
        if (!isMoving && !isCarrying && !isLooking)
            isLooking = true;
    }

    private void FindTargets() {
        foreach (BuildingPlacedObject building in VillageManager.Instance.BuildingsWithFoodToBeStored()) {
            pickupBuildings.Enqueue(building);
        }
        if (pickupBuildings.Count > 0) {
            isLooking = false;
        }
    }
}
