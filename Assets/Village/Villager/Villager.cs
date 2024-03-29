using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour, ObjectUI {
    public string villagerName, description;
    private NavMeshAgent navMeshAgent;
    public Vector3 goal;
    public Transform homeLocation;
    public float energyLevel = 100;

    private BuildingPlacedObject currentJobBuilding;
    private JobGameObject currentJob;

    public class UnassignVillagerEventArgs : EventArgs {
        public BuildingPlacedObject workedBuilding;
    }
    public event EventHandler<UnassignVillagerEventArgs> UnassignVillagerEvent;

    // Start is called before the first frame update
    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //uiInstance = Instantiate(villagerUIPrefab);
        //uiInstance.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        if (energyLevel == 0) {
            //Debug.Log(gameObject.name + " is out of energy!");
        }
    }

    public void DecreaseEnergy(float amount) {
        if (energyLevel - amount <= 0)
            energyLevel = 0;
        else
            energyLevel -= amount;
    }

    public BuildingPlacedObject CurrentJobBuilding() { return currentJobBuilding; }

    // Assign will have a task as well
    public void Assign(BuildingPlacedObject building) {
        Debug.Log("Assigning " + name + " to " + building.name);
        currentJobBuilding = building;
        currentJob = building.GetJob();
        SetDestination(building.pickupLocation.position);
    }

    public void Assign(Vector3 destination) {
        Unassign();
        SetDestination(destination);
    }

    public void Unassign() {
        if (UnassignVillagerEvent != null) { UnassignVillagerEvent(this, new UnassignVillagerEventArgs { workedBuilding = currentJobBuilding }); }
        currentJobBuilding = null;
        currentJob = null;
        Debug.Log(gameObject.name + " unassigned");
    }

    public bool HasArrived() {
        return Mathf.Abs(Vector3.Distance(transform.position, navMeshAgent.destination)) < 10f;
    }

    public void SetDestination(Vector3 destination) {
        navMeshAgent.destination = destination;
    }

    //UI
    public ObjectUIData GetUIData() {
        return new ObjectUIData(villagerName, description, true, energyLevel);
    }
}
