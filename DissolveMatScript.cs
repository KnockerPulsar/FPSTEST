using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public enum dissolveDirection { topToBot, botToTop, leftToRight, rightToLeft };

public class DissolveMatScript : MonoBehaviour
{

    public dissolveDirection dir = (int)dissolveDirection.topToBot;
    public float fadeValue = 0;
    public float fadeSpeed = 1;
    public float maxFade = 4;
    public Color fadeColor = Color.white;
    public float edgeThickness = -0.02f;
    public bool multiMaterials = false;
    public int rendererMatIndex = 0;
    public Vector2Int rendererMatRange = new Vector2Int(0, 1);

    public bool dissolveOutTest = false;
    public bool dissolveInTest = false;

    Renderer renderer;
    List<Material> mat = new List<Material>();
    float checkFadeValue = 0;
    Color checkFadeColor = Color.white;
    float checkEdgeThickness = -0.02f;
    int checkDir = 0;
    public bool dissolving = false;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        if (multiMaterials)
            mat = renderer.materials.ToList();
        else
            mat.Add(renderer.sharedMaterial);

        foreach (Material material in mat)
        {
            material.SetFloat("DissolveFade", fadeValue);
            material.SetColor("DissolveFadeColor", fadeColor);
            material.SetFloat("DissolveEdgeThickness", edgeThickness);
            material.SetFloat("DissolveDirection", (int)dir);
        }
    }

    private void Update()
    {

        if (checkFadeValue != fadeValue)
        {
            foreach (Material material in mat)
            {
                checkFadeValue = fadeValue;
                material.SetFloat("DissolveFade", fadeValue);
            }
        }
        if (checkFadeColor != fadeColor)
        {
            foreach (Material material in mat)
            {
                checkFadeColor = fadeColor;
                material.SetColor("DissolveFadeColor", fadeColor);
            }
        }
        if (checkEdgeThickness != edgeThickness)
        {
            foreach (Material material in mat)
            {
                checkEdgeThickness = edgeThickness;
                material.SetFloat("DissolveEdgeThickness", edgeThickness);
            }
        }
        if (checkDir != (int)dir)
        {
            foreach (Material material in mat)
            {
                checkDir = (int)dir;
                material.SetFloat("DissolveDirection", (int)dir);
            }
        }

        if (dissolveOutTest && !dissolveInTest && !dissolving)
            StartCoroutine(DissolveOut());
        if (dissolveInTest && !dissolveOutTest && !dissolving)
            StartCoroutine(dissolveIn());



    }

    IEnumerator DissolveOut()
    {
        dissolving = true;
        while (fadeValue <= maxFade)
        {
            foreach (Material material in mat)
            {
                fadeValue += fadeSpeed * Time.deltaTime;
                material.SetFloat("DissolveFade", fadeValue);
            }

            yield return null;
        }
        fadeValue = 0;
        dissolving = false;
    }
    IEnumerator dissolveIn()
    {
        dissolving = true;
        fadeValue = maxFade;
        while (fadeValue >= 0)
        {
            foreach (Material material in mat)
            {
                fadeValue -= fadeSpeed * Time.deltaTime;
                material.SetFloat("DissolveFade", fadeValue);
            }

            yield return null;
        }
        fadeValue = 0;
        dissolving = false;
    }

    public void StartDissolvingOut() => StartCoroutine(DissolveOut());
    public void StartDissolvingIn() => StartCoroutine(dissolveIn());


}
