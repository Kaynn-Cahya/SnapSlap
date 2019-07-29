using UnityEngine;
using UnityEngine.UI;

using MyBox;

public class MaxScoreSlider : MonoBehaviour {

    [SerializeField, Tooltip("The slider when selected the max score"), MustBeAssigned]
    private Slider maxScoreSlider;

    [SerializeField, Tooltip("The text to show the currently selected max score"), MustBeAssigned]
    private Text maxScoreText;

    private void Awake() {
        maxScoreText.text = ((int)maxScoreSlider.value).ToString();

        maxScoreSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float newValue) {
        int newValueInt = (int)newValue;

        maxScoreText.text = newValueInt.ToString();

        GlobalProperties.MaxScore = newValueInt;
    }
}
