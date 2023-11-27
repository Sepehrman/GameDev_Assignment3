using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public GameObject endGameUI; // Reference to the end game UI GameObject

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            // Check if the colliding object has the "Player" tag
            endGameUI.SetActive(true); // Activate the end game UI
            GameManager.Instance.UnlockCursor();
        }
    }
}
