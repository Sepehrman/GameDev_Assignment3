using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    // Start is called before the first frame update
    
    public BallController playerBall;
    Vector3 startPosition;
    public Canvas endgameUI;
    public TMP_Text winnerText;
    public TMP_Text countdownText;
    private int countdownStart = 3;
    private int maxPlayerScore = 5; // First to x wins
    private bool isRunning = true;

    private string sceneToTransitionTo = "MazeSceneNathan"; // Change this after

    void Start()
    {
        //ChangeScene("MazeScene");

        StartGame();
    }

    void Update()
    {
        if (!isRunning) return;

        if (playerBall.p1Score >= maxPlayerScore || playerBall.p2Score >= maxPlayerScore) {
            isRunning = false;
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
            StartCoroutine(Countdown(countdownStart));
        }
    }

    IEnumerator Countdown(int counter)
    {
        countdownText.text = "Returning to Maze Game in " + counter + "...";
        if (counter > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            yield return Countdown(counter - 1);
        } else
        {
            Time.timeScale = 1;
            ChangeScene("MazeScene");
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneToTransitionTo);
    }

}