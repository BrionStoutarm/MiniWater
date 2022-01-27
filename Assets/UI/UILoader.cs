using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour {
    private static UILoader s_instance;
    public static UILoader Instance {
        get => s_instance;
        set {
            if (value != null && s_instance == null) {
                s_instance = value;
            }
        }
    }

    private void Awake() {
        s_instance = this;
    }

    // Update is called once per frame
    public bool isUiLoaded = false;
    void Update() {
        //can load different menus or whatnot here

        //basic UI
        if (Input.GetKeyDown(KeyCode.F1)) {
            if (SceneManager.GetSceneByName("UI").isLoaded == false) {
                SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
                isUiLoaded = true;
            }
            else {
                SceneManager.UnloadSceneAsync("UI");
                isUiLoaded = false;
            }
        }
    }
}
