using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utensil : MonoBehaviour
{
    public UtensilScriptableObject utensilType;
    [SerializeField]
    bool needsCleaning = false;
    [SerializeField]
    float dirtyAmount = 0.0f;

    public static Utensil Create(Transform parent, UtensilScriptableObject utensilType) {
        Transform placedObjectTransform = Instantiate(utensilType.prefab, parent);

        Utensil spawnedUtensil = placedObjectTransform.GetComponent<Utensil>();
        spawnedUtensil.utensilType = utensilType;
        

        return spawnedUtensil;
    }
    public void IncreaseDirtAmount(float amt) {
        dirtyAmount += amt;
        if (dirtyAmount >= 100f) {
            dirtyAmount = 100f;
            needsCleaning = true;
            //other notifications
            Debug.Log("Dish: " + name + " needs to be cleaned");
        }
        Debug.Log(name + " is " + dirtyAmount + " dirty" );
    }
}
