using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject PauseUI;
    public GameObject OptionsMenu;

    bool paused = false;
    float timeScale = 1f;
    Vector3 velBeforePause = Vector3.zero;

    public AudioMixer audioMixer;
    public TextMeshProUGUI audioSliderText;
    public Slider audioSlider;

    public Camera playerWorldCamera;
    public TextMeshProUGUI FOVSliderText;
    public Slider FOVSlider;


    // Update is called once per frame
    void Update()
    {
        CheckPause();
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            paused = !paused;
            PlayerVariables.paused = paused;
            if (paused == true)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                MainUI.SetActive(false);
                PauseUI.SetActive(true);
                velBeforePause = PlayerVariables.playerRB.velocity;
                PlayerVariables.playerRB.velocity = Vector3.zero;

                timeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                MainUI.SetActive(true);

                PauseUI.SetActive(false);
                OptionsMenu.SetActive(false);
                Time.timeScale = timeScale;
                PlayerVariables.playerRB.velocity = velBeforePause;
            }
        }
    }

    public void ShowOptions()
    {
        if (OptionsMenu && !OptionsMenu.activeSelf)
        {
            PauseUI.SetActive(false);
            OptionsMenu.SetActive(true);

            FOVSliderText.text = playerWorldCamera.fieldOfView.ToString();
            FOVSlider.value = playerWorldCamera.fieldOfView;

            float interValue;
            audioMixer.GetFloat("MasterVolume", out interValue);
            audioSlider.value = Mathf.Pow(10, interValue/20f);
            audioSliderText.text = (audioSlider.value * 100f).ToString("00");
        }
    }

    public void UpdateAudio(float sliderValue)
    {
        if (audioMixer)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
            audioSliderText.text = (sliderValue * 100).ToString("00");
        }
    }

    public void UpdateFOV(float sliderValue)
    {
        if (playerWorldCamera)
        {
            playerWorldCamera.fieldOfView = sliderValue;
            FOVSliderText.text = sliderValue.ToString();
        }
    }
}
