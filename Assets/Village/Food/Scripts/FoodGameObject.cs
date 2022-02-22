using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGameObject : MonoBehaviour
{
    FoodScriptableObject foodType;



    public static FoodGameObject CreateFoodObject(Vector3 worldPosition, FoodScriptableObject foodType) {
        Transform placedObjectTransform = Instantiate(foodType.prefab, worldPosition, Quaternion.identity);

        FoodGameObject placedObject = placedObjectTransform.GetComponent<FoodGameObject>();

        placedObject.foodType = foodType;

        return placedObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
