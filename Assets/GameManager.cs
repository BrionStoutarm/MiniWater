using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] bool m_debugMode;

    public bool m_showWind = true;

    private static GameManager s_instance;

    public static GameManager Instance {
        get => s_instance;
        set {
            if (value != null)
                s_instance = value; 
        }
    }

    public event EventHandler<TimeStepArgs> TimeStepEvent;
    public class TimeStepArgs : EventArgs { }

    void Awake()
    {
        if (s_instance != null)
        {
            Debug.LogError("Multiple Game Managers");
            return;
        }

        s_instance = this;
    }

    void Start()
    {
        //InvokeRepeating("TriggerTimeStep", 0.0f, 10f);

        //m_mainCamera = GameObject.FindObjectOfType<FollowCamera>();
        //if (m_playerBoat)
        //{
        //    m_mainCamera.Follow(m_playerBoat);
        //    ConstructSailIndicator(m_playerBoat.GetComponent<BoatMovement>());
        //}
        //if (m_showWind)
        //{
        //    ConstructWindGauge();
        //}
    }

    //Triggers the timestep event that other objects will listen to
    private void TriggerTimeStep() {
        if(TimeStepEvent != null) {
            TimeStepEvent(this, new TimeStepArgs { });
        }
    }

    public bool OnDebug() {
        return m_debugMode;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleBuildSystem() {
        GridBuildingSystem.Instance.toggleActive();
    }

}
