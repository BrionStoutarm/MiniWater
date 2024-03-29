﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event EventHandler<OnLeftClickArgs> OnLeftClickEvent;
    public class OnLeftClickArgs : EventArgs { }

    public static event EventHandler<OnRightClickArgs> OnRightClickEvent;
    public class OnRightClickArgs : EventArgs { }

    public static event EventHandler<OnObjectSelectedArgs> OnObjectSelectedEvent;
    public class OnObjectSelectedArgs : EventArgs {
        public GameObject obj;
    }

    public static event EventHandler<OnUpArrowArgs> OnUpArrowEvent;
    public class OnUpArrowArgs : EventArgs { }

    public static event EventHandler<OnDownArrowArgs> OnDownArrowEvent;
    public class OnDownArrowArgs : EventArgs { }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (OnLeftClickEvent != null) OnLeftClickEvent(this, new OnLeftClickArgs { });
        }

        if (Input.GetMouseButtonDown(1)) {
            if (OnRightClickEvent != null) OnRightClickEvent(this, new OnRightClickArgs { });
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (OnUpArrowEvent != null) OnUpArrowEvent(this, new OnUpArrowArgs { });
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (OnDownArrowEvent != null) OnDownArrowEvent(this, new OnDownArrowArgs { });
        }
    }

    //should probably be double click to auto zoom on object
    private void HandleLeftClick(object sender, OnLeftClickArgs args)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawLine(ray.GetPoint(100.0f), Camera.main.transform.position, Color.red, 10.0f);

        RaycastHit hit;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Debug.Log("Missed");
            return;
        }
    }
}
