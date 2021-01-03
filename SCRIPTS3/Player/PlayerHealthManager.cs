using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{

    public float currentHealth = 70f;
    public float maxHealth = 100f;
    public Slider HealthSlider;
    public TextMeshProUGUI HealthText;

    // Start is called before the first frame update
    void Start()
    {
        PlayerVariables.pHM = this;

        if (currentHealth == 0)
            currentHealth = maxHealth;
        else
        {
            HealthSlider.value = currentHealth / maxHealth;
            HealthText.SetText(currentHealth.ToString());
        }
}

    public void RecieveDamage(float amount)
    {
        if (amount < currentHealth)
        {
            currentHealth -= amount;
            HealthText.SetText(currentHealth.ToString());
            HealthSlider.value = currentHealth / maxHealth;
        }
        else
            OnDeath();
    }

    public void Heal(float amount)
    {
        if (amount <= maxHealth - currentHealth)
            currentHealth += amount;
        else
            currentHealth = maxHealth;

        if (HealthText)
        {
            HealthText.SetText(currentHealth.ToString());
            HealthSlider.value = currentHealth / maxHealth;

        }

    }

    public void OnDeath()
    {
        currentHealth = 0;
        HealthText.SetText(currentHealth.ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        HealthSlider.enabled = (true);
    }

    private void OnDisable()
    {
        HealthSlider.enabled = (false);
    }
}
