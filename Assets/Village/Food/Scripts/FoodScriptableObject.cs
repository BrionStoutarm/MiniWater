using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodType", menuName = "ScriptableObjects/FoodType")]
public class FoodScriptableObject : ScriptableObject
{
    public string foodName;
    public string foodDescription;

    public List<FoodScriptableObject>  ingredients;
    public List<UtensilScriptableObject> requiredUtensils;

    public Transform prefab;

    public bool isCooked;
    public bool isEdible;
    public float timeToCook;
}
