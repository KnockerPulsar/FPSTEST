using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{

    public float currentHealth = 70f;
    public float maxHealth = 100f;
    public GameObject HealthObj;
    public TextMeshProUGUI HealthText;

    // Start is called before the first frame update
    void Start()
    {
        if (currentHealth == 0)
            currentHealth = maxHealth;
        else
            HealthText.SetText(currentHealth.ToString());
    }


    public void RecieveDamage(float amount)
    {
        if (amount < currentHealth)
        {
            currentHealth -= amount;
            HealthText.SetText(currentHealth.ToString());
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
            HealthText.SetText(currentHealth.ToString());

    }

    public void OnDeath()
    {
        currentHealth = 0;
        HealthText.SetText(currentHealth.ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        HealthObj.SetActive(true);
    }

    private void OnDisable()
    {
        HealthObj.SetActive(false);
    }
}
