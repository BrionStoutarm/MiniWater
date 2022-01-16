using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacedObject : MonoBehaviour
{
    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlaceableScriptableObject.Dir dir, PlaceableScriptableObject placeableType, int gridDensity) {
        Transform placedObjectTransform = Instantiate(placeableType.prefab, worldPosition, Quaternion.Euler(0, placeableType.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();

        placedObject.placeableType = placeableType;
        placedObject.origin = origin;
        placedObject.dir = dir;
        placedObject.originalScale = placeableType.prefab.localScale;
        placedObject.gridDensity = gridDensity;

        return placedObject;
    }

    protected PlaceableScriptableObject placeableType;
    protected Vector2Int origin;
    protected PlaceableScriptableObject.Dir dir;
    protected Vector3 originalScale;
    protected int gridDensity; //grid sizes are gonna be 8x10, 16x20, etc.  cellScale will just be 1, 2, etc to correspond with grid scale

    public List<Vector2Int> GetGridPositionList() {
        return placeableType.GetGridPositionList(origin, dir, gridDensity);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void SetVisible(bool isVisible) {
        if(isVisible) {
            gameObject.transform.localScale = originalScale;
        }
        else {
            gameObject.transform.localScale = Vector3.zero;
        }
    }

    public string GetObjectName() {
        return placeableType.nameString;
    }

    public PlaceableScriptableObject GetPlaceableType() {
        return placeableType;
    }
}
