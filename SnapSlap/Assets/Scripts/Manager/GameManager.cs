using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class GameManager : Singleton<GameManager> {
    [Separator("Player hands")]

    [SerializeField, Tooltip("The hand for player 1"), MustBeAssigned]
    private PlayerHand player1Hand;

    [SerializeField, Tooltip("The hand for player 2"), MustBeAssigned]
    private PlayerHand player2Hand;

    [Separator("Scene Objects")]

    [SerializeField, Tooltip("The camera that can be shaked"), MustBeAssigned]
    private ObjectShaker shakableCamera;

    public bool EitherPlayerIsPunching() {
        return player1Hand.IsPunching || player2Hand.IsPunching;
    }

    public void ShakeCamera() {
        shakableCamera.TriggerShake();
    }
}
