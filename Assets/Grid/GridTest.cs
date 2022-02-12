using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    public int gridWidth, gridHeight;
    public float gridScale;
    public int gridDensity;
    public Collider deckCollider;

    Grid<GridObject> grid;
    // Start is called before the first frame update
    void Start()
    { 
        Debug.Log("Grid size: " + grid.Width() * grid.Height());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Grid size: " + grid.Width() * grid.Height());
    }
}
