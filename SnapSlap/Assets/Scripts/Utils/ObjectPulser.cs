using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class ObjectPulser : MonoBehaviour {

    [SerializeField, Tooltip("The min vector to scale to"), MustBeAssigned]
    private Vector3 minScale;

    [SerializeField, Tooltip("The max vector to scale to"), MustBeAssigned]
    private Vector3 maxScale;

    private bool goingToMax;

    private float currScaleProgress;

    private void Awake() {
        goingToMax = false;
        transform.localScale = minScale;
        currScaleProgress = 0f;
    }

    private void Update() {

        if (currScaleProgress < 1f) {

            if (goingToMax) {
                transform.localScale = Vector3.Lerp(minScale, maxScale, currScaleProgress);
            } else {
                transform.localScale = Vector3.Lerp(maxScale, minScale, currScaleProgress);
            }

            currScaleProgress += Time.deltaTime;
        } else {
            goingToMax = !goingToMax;
            currScaleProgress = 0f;
        }
    }

}
