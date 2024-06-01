using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    public GameObject deathUI;

    private void Start()
    {
        // Hide the death UI element initially
        deathUI.SetActive(false);
    }

    private void Update()
    {
        // Check for any key press to restart the scene
        if (Input.anyKeyDown)
        {
            RestartScene();
        }
    }

    public void ShowDeathUI()
    {
        // Show the death UI element
        deathUI.SetActive(true);
    }

    public void RestartScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
