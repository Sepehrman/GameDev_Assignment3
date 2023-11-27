using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using System;

public class CommandLine : MonoBehaviour
{
    public TMP_InputField inputField; // Assign the Input Field in the Inspector
    public TMP_Text textPrefab;
    public Plane plane;
    public VerticalLayoutGroup canvas;
    public Material bgMaterial; // Assign your background material in the Inspector
    public BallController playerBall;
    public ComputerController aiPlayer;
    public Canvas console;
    


    public void Start() {
        if (console != null) {
            console.enabled = false;
            Time.timeScale = 1;

        }
    }
    public void AddInputToConsole() {
        TMP_Text newText = Instantiate(textPrefab);
        newText.text = "> " + inputField.text;
        newText.transform.SetParent(canvas.transform, false);
        inputField.text = "";


        if (newText.text.Contains("bg_color=")) {
            try {
                bgMaterial.color = ExtractColorFromCommand(newText.text.Split("=")[1].ToLower());
            } catch (Exception e) {
                Debug.LogError($"Issue extracting Color : {e.Message}");
            }
        } else if (newText.text.Contains("ball_speed=")) {
            try {
                playerBall.ballSpeed = float.Parse(newText.text.Split("=")[1]);
            } catch (Exception e) {
                Debug.Log($"Issue Parsing Number : {e.Message}");
            }
        } else if (newText.text.Contains("enemy_speed=")) {
            try {
                aiPlayer.moveSpeed = float.Parse(newText.text.Split("=")[1]);
            } catch (Exception e) {
                Debug.Log($"Issue Parsing Number : {e.Message}");
            }
        } else {
            Debug.LogError("Command Not Recognized!");
        }
    }


    public void Update() {

        if (Input.GetKey(KeyCode.C)) {
            Time.timeScale = 0;
            console.enabled = true;
        }

        if (Input.GetKey(KeyCode.Escape)) {
            Time.timeScale = 1;
            console.enabled = false;
        }

    }

    Color ExtractColorFromCommand(string colorName) {
        Debug.Log($"Color is {colorName}");

        switch (colorName) {
            case "red":
                return Color.red;
            case "black":
                return Color.black;
            case "blue":
                return Color.blue;
            case "green":
                return Color.green;
            case "yellow":
                return Color.yellow;
            case "white":
                return Color.white;
            case "grey":
                return Color.grey;
            default:
                return bgMaterial.color; // Default to white if the color name is not recognized.
        }
    }
}
