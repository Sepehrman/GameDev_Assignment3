using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    // Start is called before the first frame update
    
    public BallController playerBall;
    Vector3 startPosition;
    public Canvas endgameUI;
    public TMP_Text winnerText;
    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (playerBall.p1Score >= 10 || playerBall.p2Score >= 10) {
            EndGame();
        } else {
            if (endgameUI != null) {
                endgameUI.enabled = false;
            }
        }
    }

    void StartGame() {
        startPosition = playerBall.transform.position;
        Time.timeScale = 1;
    }


    void EndGame() {
        winnerText.text = playerBall.p1Score >= 10 ? "Player 1 is the Winner!" : "Player 2 is the Winner!";
        if (endgameUI != null) {
            endgameUI.enabled = true;
            Time.timeScale = 0;
        }
    }

}