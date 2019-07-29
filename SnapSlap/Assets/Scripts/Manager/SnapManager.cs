using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField, Tooltip("The image with the text to show 'ready' and 'go'"), MustBeAssigned]
    private Image readyGoImage;

    [SerializeField, Tooltip("The sprite that shows 'ready'"), MustBeAssigned]
    private Sprite readySprite;

    [SerializeField, Tooltip("The sprite that shows 'go'"), MustBeAssigned]
    private Sprite goSprite;

    private float currentWaitingDuration;

    public bool CountdownPaused { get; private set; }

    private void OnValidate() {
        if (minSnapWaitingDuration > maxSnapWaitingDuration) {
            var temp = maxSnapWaitingDuration;
            maxSnapWaitingDuration = minSnapWaitingDuration;
            minSnapWaitingDuration = temp;
            Debug.LogWarning(gameObject.name + " : SnapManager.cs :: minSnapWaitingDuration can not be more than maxSnapWaitingDuration!");
        }
    }

    private void Awake() {
        CountdownPaused = false;
        currentWaitingDuration = 0f;
    }

    private void Start() {
        RestartPunchCountdown();
    }

    private void Update() {
        if (GameManager.Instance.PunchIsValid || CountdownPaused) { return; }

        currentWaitingDuration -= Time.deltaTime;
        if (currentWaitingDuration < 0f) {
            AllowPunching();
        }
    }

    private void AllowPunching() {
        AudioManager.Instance.PlaySFXByAudioType(AudioType.Signal);
        AudioManager.Instance.StopBGMAudioSource();
        ShowGoSprite();
        GameManager.Instance.PunchIsValid = true;
        DisplayValidPunchColor();
    }

    public void RestartPunchCountdown() {
        AudioManager.Instance.PlayBGMAudioSource();
        CountdownPaused = false;
        ShowReadySprite();
        GameManager.Instance.PunchIsValid = false;
        DisplayInvalidPunchColor();
        GenerateCurrentWaitingDuration();
    }

    public void StopPunchCountdown() {
        CountdownPaused = true;
    }

    #region Util

    public void DisableReadyGoImage() {
        readyGoImage.gameObject.SetActive(false);
    }

    private void ShowReadySprite() {
        readyGoImage.gameObject.SetActive(true);
        readyGoImage.sprite = readySprite;
    }

    private void ShowGoSprite() {
        readyGoImage.gameObject.SetActive(true);
        readyGoImage.sprite = goSprite;
    }

    private void GenerateCurrentWaitingDuration() {
        currentWaitingDuration = Random.Range(minSnapWaitingDuration, maxSnapWaitingDuration);
    }

    public void DisplayInvalidPunchColor() {
        gameCamera.backgroundColor = invalidPunchColor;
    }

    public void DisplayValidPunchColor() {
        gameCamera.backgroundColor = validPunchColor;
    }

    #endregion
}
