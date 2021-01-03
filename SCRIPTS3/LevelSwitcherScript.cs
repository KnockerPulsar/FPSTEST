using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Responsible for switching the levels when the player enters a level portal
public class LevelSwitcherScript : MonoBehaviour
{
    //Checks if the player overlapped the collision and if so, loads the next level.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            NextLevel();
    }

    //Checks if the current level is the final level and if so, loads the main menu.
    //Otherwise, loads the next level.
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
