using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectArea : MonoBehaviour
{
    public Text objectNameText;
    public Text objectDescriptionText;
    public Slider objectProgressSlider;

    public void Awake() {
        enabled = false;
    }
}
