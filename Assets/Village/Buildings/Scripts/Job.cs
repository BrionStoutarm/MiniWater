using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job : MonoBehaviour
{
    public string name;
    public string description;
    private BuildingPlacedObject building;

    public void SetJobBuilding(BuildingPlacedObject building) { this.building = building; }

    public abstract void DoJob();
}
