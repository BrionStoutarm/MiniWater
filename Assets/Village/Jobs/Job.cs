using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job : MonoBehaviour
{
    public string jobName;
    public string description;
    public float progressRate;
    public float energyCost;
    public int jobProgressAmount;

    protected BuildingPlacedObject building;
    protected Villager workingVillager;
    protected int jobProgress = 0;

    public void SetJobBuilding(BuildingPlacedObject building) { this.building = building; }
    public void SetWorkingVillager(Villager villager) { this.workingVillager = villager; }
    // called every "jobProgressRate" seconds
    public abstract void DoJob();

    public int GetJobAmount() { return jobProgress; }
}
