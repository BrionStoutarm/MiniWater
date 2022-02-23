using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingPlaceableScriptableObject", menuName = "ScriptableObjects/BuildingType")]
public class BuildingPlaceableScriptableObject : PlaceableScriptableObject
{
    public int cost;

    public JobScriptableObject buildingJob;
    public List<UtensilScriptableObject> compatibleUtensils;
    public int numUtensilSlots;

    public bool canPreventSpoiling = false;
    public bool hasCapacity;
    public int maxCapacity=0;
}