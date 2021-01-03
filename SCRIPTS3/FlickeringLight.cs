using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public float minFlickerDelay = 0.2f;
    public float maxFlickerDelay = 1;
    Light light;
    private void Start()
    {
        light = GetComponent<Light>();
        if (light)
            StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            float flickerDuration = Random.Range(minFlickerDelay, maxFlickerDelay);
            light.enabled = !light.enabled;
            yield return new WaitForSeconds(flickerDuration);
        }
    }

}
