using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public GameObject mazeGenerator;

    public GameObject canvas;

    public GameObject gameManager;  // Reference to self to destroy if needed

    public static GameManager Instance { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameManager.gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Debug.Log("GameManager Ready");
        DontDestroyOnLoad(this.gameObject);
    }

    public void ResetEnemyPosition()
    {
        enemy.GetComponent<AIAgent>().ResetPosition();
    }

    public void LockCursor()
    {
        player.GetComponent<PlayerController>().LockCursor();
    }

    public void UnlockCursor()
    {
        player.GetComponent<PlayerController>().UnlockCursor();
    }

    public void Restart()
    {
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);

        // Get the current scene's build index
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the current scene by its build index
        SceneManager.LoadScene(currentSceneBuildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


}
