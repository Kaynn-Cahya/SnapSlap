using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MyBox;

public class GameManager : Singleton<GameManager> {
    [Separator("Player hands")]
    [SerializeField, Tooltip("The hand for player 1"), MustBeAssigned]
    private PlayerHand player1Hand;

    [SerializeField, Tooltip("The hand for player 2"), MustBeAssigned]
    private PlayerHand player2Hand;

    [Separator("Shakable Camera")]
    [SerializeField, Tooltip("The camera that can be shaked"), MustBeAssigned]
    private ObjectShaker shakableCamera;

    [Separator("Player Displays")]
    [SerializeField, Tooltip("The gameobject to show that player 1 gets a free punch"), MustBeAssigned]
    private GameObject player1FreePunchDisplay;

    [SerializeField, Tooltip("The gameobject to show that player 2 gets a free punch"), MustBeAssigned]
    private GameObject player2FreePunchDisplay;

    [SerializeField, Tooltip("The gameobject to show that player 1 won"), MustBeAssigned]
    private GameObject player1WinsDisplay;

    [SerializeField, Tooltip("The gameobject to show that player 2 won"), MustBeAssigned]
    private GameObject player2WinsDisplay;

    [Separator("Fade panel")]
    [SerializeField, Tooltip("The panel used to fade in/out whenever the round ends"), MustBeAssigned]
    private GraphicFader fadePanel;

    [SerializeField, Tooltip("How fast the fade panel fades"), PositiveValueOnly]
    private float fadePanelFadingSpeed;

    [Separator("Score Text")]
    [SerializeField, Tooltip("The respective score text for the players"), MustBeAssigned]
    private Text player1ScoreText, player2ScoreText;

    [Separator("Game Input")]
    [SerializeField, Tooltip("The hidden button for player 1 to punch"), MustBeAssigned]
    private Button player1PunchButton;

    [SerializeField, Tooltip("The hidden button for player 2 to punch"), MustBeAssigned]
    private Button player2PunchButton;

    /// <summary>
    /// True if the punch is now valid.
    /// </summary>
    public bool PunchIsValid { get; set; }

    public int Player1Score { get; private set; }
    public int Player2Score { get; private set; }

    public bool RoundOver { get; private set; }


    private void Awake() {
        PunchIsValid = false;
        RoundOver = false;
        Player1Score = 0;
        Player2Score = 0;
    }

    public bool EitherPlayerIsPunching() {
        return player1Hand.IsPunching || player2Hand.IsPunching;
    }

    public void ShakeCamera() {
        shakableCamera.TriggerShake();
    }

    public void TriggerPlayerGotPunched(bool player1GotPunched) {
        SnapManager.Instance.DisableReadyGoImage();
        SnapManager.Instance.StopPunchCountdown();
        DisableAllDisplays();

        if (PunchIsValid) {
            HandleValidPunchThrown();
        } else {
            HandleInvalidPunchThrown();
        }

        #region Local_Function

        void HandleValidPunchThrown() {
            ShowWinDisplayByPlayerPunched();

            DisableAllButtons();
            fadePanel.FadeGraphicToColor(Color.black, fadePanelFadingSpeed, FadePanelToTransparentBeforeStartingNextRound);

            AddScoreToRespectivePlayer();

            RoundOver = true;
        }

        void AddScoreToRespectivePlayer() {
            if (player1GotPunched) {
                AddScoreToPlayer2();
            } else {
                AddScoreToPlayer1();
            }
        }

        void ShowWinDisplayByPlayerPunched() {
            GameObject displayToShow = player1GotPunched ? player2WinsDisplay : player1WinsDisplay;
            displayToShow.SetActive(true);
        }

        void HandleInvalidPunchThrown() {
            SnapManager.Instance.DisplayValidPunchColor();
            Button buttonToDisable = player1GotPunched ? player2PunchButton : player1PunchButton;
            buttonToDisable.interactable = false;

            GameObject displayToShow = player1GotPunched ? player1FreePunchDisplay : player2FreePunchDisplay;
            displayToShow.SetActive(true);

            PunchIsValid = true;
        }

        #endregion
    }

    #region Util

    private void AddScoreToPlayer1() {
        ++Player1Score;

        player1ScoreText.text = Player1Score.ToString();
    }

    private void AddScoreToPlayer2() {
        ++Player2Score;

        player2ScoreText.text = Player2Score.ToString();
    }

    private void FadePanelToTransparentBeforeStartingNextRound() {
        DisableAllDisplays();
        SnapManager.Instance.DisplayInvalidPunchColor();
        fadePanel.FadeGraphicToColor(new Color(0f, 0f, 0f, 0f), fadePanelFadingSpeed, StartNextRound);
    }

    private void StartNextRound() {
        EnableAllButtons();
        SnapManager.Instance.RestartPunchCountdown();

        RoundOver = false;
    }

    private void DisableAllButtons() {
        player2PunchButton.interactable = false;
        player1PunchButton.interactable = false;
    }

    private void EnableAllButtons() {
        player2PunchButton.interactable = true;
        player1PunchButton.interactable = true;
    }

    private void DisableAllDisplays() {
        player2WinsDisplay.SetActive(false);
        player1WinsDisplay.SetActive(false);
        player2FreePunchDisplay.SetActive(false);
        player1FreePunchDisplay.SetActive(false);
    }

    #endregion
}
