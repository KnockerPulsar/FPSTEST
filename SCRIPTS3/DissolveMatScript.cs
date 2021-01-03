using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Eventing.Reader;

public enum dissolveDirection { topToBot, botToTop, leftToRight, rightToLeft };

public class DissolveMatScript : MonoBehaviour
{

    public dissolveDirection dir = (int)dissolveDirection.topToBot;
    public float fadeValue = 0;
    public float fadeSpeed = 1;
    [SerializeField] public float maxFade = 10;
    [SerializeField] public float minFade = 0;
    public Color fadeColor = Color.white;
    [ColorUsage(true, true)]
    public Color emissiveColor = Color.green * 2f;
    public float edgeThickness = -0.02f;
    public bool multiMaterials = false;
    public int rendererMatIndex = 0;
    public Vector2Int rendererMatRange = new Vector2Int(0, 1);

    public bool dissolveOutTest = false;
    public bool dissolveInTest = false;

    Renderer rend;
    List<Material> mat = new List<Material>();
    float checkFadeValue = 0;
    Color checkFadeColor = Color.white;
    Color checkEmssiveColor = Color.green * 2f;
    float checkEdgeThickness = -0.02f;
    int checkDir = 0;
    public bool dissolving = false;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (multiMaterials)
            mat = rend.materials.ToList();
        else
            mat.Add(rend.materials[0]);

        foreach (Material material in mat)
        {
            material.SetFloat("DissolveFade", fadeValue);
            material.SetColor("DissolveFadeColor", fadeColor);
            material.SetColor("EmissiveColor", emissiveColor);
            material.SetFloat("DissolveEdgeThickness", edgeThickness);
            material.SetFloat("DissolveDirection", (int)dir);
        }
    }

    private void Update()
    {

        if (checkFadeValue != fadeValue)
        {
            checkFadeValue = fadeValue;
            foreach (Material material in mat)
            {
                material.SetFloat("DissolveFade", fadeValue);
            }
        }
        if (checkFadeColor != fadeColor)
        {
            checkFadeColor = fadeColor;
            foreach (Material material in mat)
            {
                material.SetColor("DissolveFadeColor", fadeColor);
            }
        }
        if (checkEdgeThickness != edgeThickness)
        {
            checkEdgeThickness = edgeThickness;
            foreach (Material material in mat)
            {
                material.SetFloat("DissolveEdgeThickness", edgeThickness);
            }
        }
        if (checkDir != (int)dir)
        {
            checkDir = (int)dir;
            foreach (Material material in mat)
            {
                material.SetFloat("DissolveDirection", (int)dir);
            }
        }
        if (checkEmssiveColor != emissiveColor)
        {
            checkEmssiveColor = emissiveColor;
            foreach (Material material in mat)
            {
                material.SetColor("EmissiveColor", emissiveColor);
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
        fadeValue = minFade;
        while (fadeValue <= maxFade)
        {
            foreach (Material material in mat)
            {
                fadeValue += fadeSpeed * Time.deltaTime;
                material.SetFloat("DissolveFade", fadeValue);
            }

            yield return null;
        }
        dissolving = false;
    }
    IEnumerator dissolveIn()
    {
        dissolving = true;
        fadeValue = maxFade;
        while (fadeValue >= minFade)
        {
            foreach (Material material in mat)
            {
                fadeValue -= fadeSpeed * Time.deltaTime;
                material.SetFloat("DissolveFade", fadeValue);
            }

            yield return null;
        }
        dissolving = false;
    }

    public void StartDissolvingOut() => StartCoroutine(DissolveOut());
    public void StartDissolvingIn() => StartCoroutine(dissolveIn());
}
