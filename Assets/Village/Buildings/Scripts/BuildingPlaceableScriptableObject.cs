using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingPlaceableScriptableObject", menuName = "ScriptableObjects/BuildingType")]
public class BuildingPlaceableScriptableObject : PlaceableScriptableObject
{
    public int cost;

    public Job buildingJob;
}