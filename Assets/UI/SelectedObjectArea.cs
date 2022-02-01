using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectArea : MonoBehaviour
{
    public Text objectNameText;
    public Text objectDescriptionText;
    public Slider objectProgressSlider;

    private Vector3 origScale;

    public void Awake() {
        origScale = transform.localScale;
    }

    public void SetVisible(bool visible) {
        if (visible) {
            transform.localScale = origScale;
        }
        else {
            transform.localScale = Vector3.zero;
        }
    }
}
