using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

//Responsible for maintaining the main menu and the options menu along with their interactions.
public class MainMenu : MonoBehaviour
{
    GameObject Main;                        //The main menu.
    GameObject Options;                     //The options menu.
    public TMP_Dropdown resDropDown;        //The resolution dropdown in the options menu.
    Resolution[] resolutions;               //An array to store the device's resolutions.

    //Checks if the player's mouse is captured (when loading the main menu from the last level) and uncaptures it.
    //Checks the device's resolutions and refresh rates and adds them to the dropdown menu.
    //Also finds out the native resolution and sets that as the currently selected resolution.
    //Hides the options menu and shows the main menu.
    private void Start()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;

        resolutions = Screen.resolutions;
        resDropDown.ClearOptions();

        List<String> resOptions = new List<string>();

        int currentResIndex = 0;

        for (int i = 0 ; i < resolutions.Length ; i++)
        {
            Resolution res = resolutions[i];

            string option = res.width + " x " + res.height + "@" + res.refreshRate + "Hz";
            resOptions.Add(option);
            if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
                currentResIndex = i;
        }

        resDropDown.AddOptions(resOptions);
        resDropDown.value = currentResIndex;
        resDropDown.RefreshShownValue();
        resDropDown.RefreshShownValue();

        Main = GameObject.Find("MainMenu");
        Options = GameObject.Find("OptionsMenu");

        Main.SetActive(true);
        Options.SetActive(false);
    }

    //On click of the play button, loads the first level (the tutorial for now)
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    //On click of the options button, hides the main menu and shows the options menu
    public void ShowOptionsMenu()
    {
        Main.SetActive(false);
        Options.SetActive(true);
    }

    //On click of the back button in the options menu, shows the main menu and hides the options menu.
    public void BackFromOptionsMenu()
    {
        Main.SetActive(true);
        Options.SetActive(false);
    }
    
    //Changes the graphics level whenever a dropdown option is selected.
    public void GFX(int val)
    {
        QualitySettings.SetQualityLevel(val);
        print(QualitySettings.GetQualityLevel());
    }

    //Toggles fullscreen on and off.
    public void FullscreenMode(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    //Changes the resolution whenever a dropdown option is selected.
    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

    }

    //Quits the game when the play presses the quit button.
    public void Quit()
    {
        Application.Quit();
    }


}
