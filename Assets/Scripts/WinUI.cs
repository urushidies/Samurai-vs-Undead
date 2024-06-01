using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public WaveSpawner WVESPWNR;
    public GameObject winUI;
    private int enemyCount;

    private void Start()
    {
        // Get the initial count of enemies in the scene
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Hide the win UI element initially
        winUI.SetActive(false);
    }

    void Update()
    {
       if(WVESPWNR.isSpawning == false && WVESPWNR.AreAllEnemiesDead())
       {
            ShowWinUI();
       }
       if(Input.anyKeyDown)
       {
        RestartScene();
       }
    }

    private void ShowWinUI()
    {
        // Show the win UI element
        winUI.SetActive(true);
    }
    public void RestartScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
