using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenemanagerMenuForSelect : MonoBehaviour
{
    public void PlayActGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);   
    }
}
