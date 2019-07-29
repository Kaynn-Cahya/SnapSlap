using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class GameManager : Singleton<GameManager> {

    [SerializeField, Tooltip("The hand for player 1"), MustBeAssigned]
    private PlayerHand player1Hand;

    [SerializeField, Tooltip("The hand for player 2"), MustBeAssigned]
    private PlayerHand player2Hand;

    public bool EitherPlayerIsPunching() {
        return player1Hand.IsPunching || player2Hand.IsPunching;
    }
}
