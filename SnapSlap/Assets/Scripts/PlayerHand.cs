using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MyBox;

public class PlayerHand : MonoBehaviour {

    [Separator("Debug")]
    [SerializeField, Tooltip("The button to press to activate punch"), SearchableEnum]
    private KeyCode activatePunchKeycode;

    [SerializeField, Tooltip("The button used to activate punch"), MustBeAssigned]
    private Button activatePuncButton;

    [Separator("Player Identifier")]

    [SerializeField, Tooltip("True if this is the hand on the left (Player 2)")]
    private bool handOnLeft;

    [Separator("Punch properties")]
    [SerializeField, Tooltip("Hand drawback speed"), PositiveValueOnly]
    private float handDrawbackSpeed;

    [SerializeField, Tooltip("Hand drawback distance before punching"), PositiveValueOnly]
    private float handDrawbackDistance;

    [SerializeField, Tooltip("The speed of punching"), PositiveValueOnly]
    private float punchingSpeed;

    /// <summary>
    /// True if the player is currently punching
    /// </summary>
    public bool IsPunching { get; private set; }

    /// <summary>
    /// The position which the hand started on.
    /// </summary>
    private Vector3 startPosition;

    private Coroutine punchingCoroutine;

    /// <summary>
    /// True if this hand came into contact with the other player's hand recently.
    /// </summary>
    private bool contactedOtherHandRecently;

    private void Awake() {
        IsPunching = false;
        startPosition = transform.position;
    }

    private void Update() {
        if (Input.GetKeyDown(activatePunchKeycode)) {
            if (!activatePuncButton.interactable) { return; }
            TriggerPunch();
        }
    }

    private void OnCollisionEnter(Collision collision) {

        if (TryGetPlayerHandFromCollision(out PlayerHand otherPlayerHand)) {
            if (IsPunching) {
                contactedOtherHandRecently = true;
                GameManager.Instance.ShakeCamera();
                AudioManager.Instance.PlaySFXByAudioType(AudioType.Impact);

                GameManager.Instance.TriggerPlayerGotPunched(handOnLeft);
            } else {
                StartCoroutine(RecoilCoroutine());
            }
        }

        #region Local_Function

        bool TryGetPlayerHandFromCollision(out PlayerHand result) {
            result = collision.gameObject.GetComponent<PlayerHand>();

            return result != null;
        }

        #endregion
    }

    private IEnumerator RecoilCoroutine() {
        float currentRecoilDistance = 0f;

        while (InHandRecoilAnimation()) {
            RecoilHandOnCurrentFrame(Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        Vector3 start = transform.position;
        float progress = 0f;
        while (progress < 1f) {

            LerpHandBackToStartPositionByProgress();

            progress += Time.deltaTime * 20;
            yield return new WaitForEndOfFrame();
        }

        transform.position = startPosition;

        #region Local_Function

        void LerpHandBackToStartPositionByProgress() {
            var temp = Vector3.Lerp(start, startPosition, progress);
            transform.position = temp;
        }

        bool InHandRecoilAnimation() {
            return currentRecoilDistance < handDrawbackDistance;
        }

        void RecoilHandOnCurrentFrame(float deltaTime) {
            float currentFrameDistanceCovered = deltaTime * handDrawbackSpeed;

            currentRecoilDistance += currentFrameDistanceCovered;

            float xValue = DetermineHandDrawbackDirection();
            transform.position += new Vector3(currentFrameDistanceCovered * xValue, 0f, 0f);
        }

        #endregion
    }

    public void TriggerPunch() {
        if (NotAllowedToPunch()) {
            return;
        }

        AudioManager.Instance.PlaySFXByAudioType(AudioType.ButtonPress);

        IsPunching = true;
        punchingCoroutine = StartCoroutine(PunchCoroutine());

        #region Local_Function
        bool NotAllowedToPunch() {
            return GameManager.Instance.EitherPlayerIsPunching() || GameManager.Instance.RoundOver || GameManager.Instance.GameOver;
        }

        #endregion
    }


    private IEnumerator PunchCoroutine() {

        float currentDrawbackDistance = 0f;

        while (InHandDrawbackAnimation()) {
            MovePunchDrawbackOnCurrentFrame(Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        while (!contactedOtherHandRecently) {
            MoveHandFowardOnCurrentFrame(Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        Vector3 start = transform.position;
        float progress = 0f;
        while (progress < 1f) {

            LerpHandBackToStartPositionByProgress();

            progress += Time.deltaTime * 20;
            yield return new WaitForEndOfFrame();
        }

        transform.position = startPosition;

        EndPunching();

        yield return null;

        #region Local_Function

        void EndPunching() {
            IsPunching = false;
            contactedOtherHandRecently = false;
        }

        void LerpHandBackToStartPositionByProgress() {
            var temp = Vector3.Lerp(start, startPosition, progress);
            transform.position = temp;
        }

        bool InHandDrawbackAnimation() {
            return currentDrawbackDistance < handDrawbackDistance;
        }

        void MovePunchDrawbackOnCurrentFrame(float deltaTime) {
            float currentFrameDistanceCovered = deltaTime * handDrawbackSpeed;

            currentDrawbackDistance += currentFrameDistanceCovered;

            float xValue = DetermineHandDrawbackDirection();
            transform.position += new Vector3(currentFrameDistanceCovered * xValue, 0f, 0f);
        }

        void MoveHandFowardOnCurrentFrame(float deltaTime) {
            float currentFrameDistanceCovered = deltaTime * punchingSpeed;

            float xValue = DetermineHandFowardDirection();
            transform.position += new Vector3(currentFrameDistanceCovered * xValue, 0f, 0f);
        }

        #endregion
    }

    #region Util

    /// <summary>
    /// The direction, which determines where the hand in the X-axis should move if it wants to drawback.
    /// </summary>
    /// <returns>-1f or 1f</returns>
    private float DetermineHandDrawbackDirection() {
        return handOnLeft ? -1f : 1f;
    }

    /// <summary>
    /// he direction, which determines where the hand in the X-axis should move if it wants to move foward.
    /// </summary>
    /// <returns>-1f or 1f</returns>
    private float DetermineHandFowardDirection() {
        return handOnLeft ? 1f : -1f;
    }

    #endregion
}
