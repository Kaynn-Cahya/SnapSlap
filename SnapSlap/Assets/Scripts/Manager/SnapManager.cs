using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class SnapManager : Singleton<SnapManager> {

    [SerializeField, Tooltip("The min waiting duration before the timer snaps"), PositiveValueOnly]
    private float minSnapWaitingDuration;

    [SerializeField, Tooltip("The max waiting duration before the timer snaps"), PositiveValueOnly]
    private float maxSnapWaitingDuration;

    [Separator("Visual Properties")]
    [SerializeField, Tooltip("The camera for the game"), MustBeAssigned]
    private Camera gameCamera;

    [SerializeField, Tooltip("The color when the player can not punch yet")]
    private Color invalidPunchColor;

    [SerializeField, Tooltip("The color to show when the player can punch")]
    private Color validPunchColor;

    private float currentWaitingDuration;

    private void OnValidate() {
        if (minSnapWaitingDuration > maxSnapWaitingDuration) {
            var temp = maxSnapWaitingDuration;
            maxSnapWaitingDuration = minSnapWaitingDuration;
            minSnapWaitingDuration = temp;
            Debug.LogWarning(gameObject.name + " : SnapManager.cs :: minSnapWaitingDuration can not be more than maxSnapWaitingDuration!");
        }
    }

    private void Awake() {
        currentWaitingDuration = 0f;
    }

    private void Start() {
        RestartPunchCountdown();
    }

    private void Update() {
        if (GameManager.Instance.PunchIsValid) { return; }

        currentWaitingDuration -= Time.deltaTime;
        if (currentWaitingDuration < 0f) {
            GameManager.Instance.PunchIsValid = true;
            DisplayValidPunchColor();
        }
    }

    public void RestartPunchCountdown() {
        GameManager.Instance.PunchIsValid = false;
        DisplayInvalidPunchColor();
        GenerateCurrentWaitingDuration();
    }


    #region Util

    private void GenerateCurrentWaitingDuration() {
        currentWaitingDuration = Random.Range(minSnapWaitingDuration, maxSnapWaitingDuration);
    }

    private void DisplayInvalidPunchColor() {
        gameCamera.backgroundColor = invalidPunchColor;
    }

    private void DisplayValidPunchColor() {
        gameCamera.backgroundColor = validPunchColor;
    }

    #endregion
}
