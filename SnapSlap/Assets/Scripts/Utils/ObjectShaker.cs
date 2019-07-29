using UnityEngine;

using MyBox;

public class ObjectShaker : MonoBehaviour {
    [SerializeField, Tooltip("How long will this object shake for"), PositiveValueOnly]
    private float shakeDuration;

    [SerializeField, Tooltip("How far this object will shake away from its original position"), PositiveValueOnly]
    private float shakeMagnitude = 0.7f;

    [SerializeField, Tooltip("How quickly the shaking effect will disappear"), PositiveValueOnly]
    private float dampingSpeed = 1.0f;

    private Vector3 initialPosition;

    private float currentShakeTimer;

    public bool IsShaking { get; private set; }

    private void Awake() {
        initialPosition = transform.position;
        IsShaking = false;
    }

    private void Update() {
        if (IsShaking) {
            if (currentShakeTimer > 0) {
                UpdateShaking(Time.deltaTime);
            } else {
                transform.position = initialPosition;
                IsShaking = false;
            }
        }
    }

    private void UpdateShaking(float deltaTime) {
        transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

        currentShakeTimer -= deltaTime * dampingSpeed;
    }

    public void InterruptShaking() {
        currentShakeTimer = 0f;
    }

    public void TriggerShake() {
        if (IsShaking) { return; }

        IsShaking = true;
        currentShakeTimer = shakeDuration;
        initialPosition = transform.position;
    }
}
