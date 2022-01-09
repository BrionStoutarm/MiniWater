using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectArea : MonoBehaviour {
    public Text selectedObjectText;
    public LayoutGroup layoutGroup;
    private void Awake() {
    }

    public void UpdateUI(PlacedObject placedObject) {

        selectedObjectText.text = placedObject.GetObjectName();

    }

    public void Clear() {
        ResetText();
    }

    private void ResetText() {
        selectedObjectText.text = "";
    }
}
