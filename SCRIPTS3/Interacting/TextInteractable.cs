﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInteractable : Interactable
{
    public Canvas canvas;
    public GameObject background;
    public AnimationClip backgroundAnimation;

    public GameObject mainTextObj;
    public GameObject authorObj;
    [TextArea(1, 4)]
    public string text;
    public string authorName;

    public float deactivationTime = 10f;

    public AudioSource audioSource;
    public AudioClip[] typingSounds;


    TextMeshProUGUI textComp;
    Animator backgroundAnimator;
    bool typing = false;

    private void Start()
    {
        textComp = mainTextObj.GetComponent<TextMeshProUGUI>();
    }

    public override void Interact()
    {
        activated = true;
        //pI.interactionPrompt.SetActive(false);

        canvas.gameObject.SetActive(true);
        backgroundAnimator = background.GetComponent<Animator>();
        backgroundAnimator.Play(backgroundAnimation.name + "Fwd");

        textComp.text = "";
        Invoke(nameof(DrawText), backgroundAnimation.length);
        Invoke(nameof(CleanText), deactivationTime + backgroundAnimation.length);
    }

    void DrawText()
    {
        StartCoroutine(DrawingText());
    }

    IEnumerator DrawingText()
    {
        typing = true;
        StartCoroutine(PlayTypingSounds());
        string currentText = "";
        int i = 0;

        while (currentText.Length < text.Length)
        {
            currentText += text[i];
            i++;
            textComp.text = currentText;
            yield return new WaitForSeconds(0.02f);
        }

        typing = false;
        StopCoroutine(PlayTypingSounds());

        yield return new WaitForSeconds(0.5f);

        authorObj.GetComponent<TextMeshProUGUI>().text = "- " + authorName;
        authorObj.SetActive(true);

        StopCoroutine(DrawingText());

    }

    public void CleanText()
    {
        StartCoroutine(CleaningText());
    }

    IEnumerator CleaningText()
    {
        authorObj.SetActive(false);

        string currentText = textComp.text;
        int i = currentText.Length - 1;

        while (currentText.Length > 0)
        {
            currentText = currentText.Remove(i, 1);
            i--;
            textComp.text = currentText;
            yield return new WaitForSeconds(0.02f);
        }

        StartCoroutine(Deactivate());

        StopCoroutine(CleaningText());
    }

    IEnumerator Deactivate()
    {
        backgroundAnimator.Play(backgroundAnimation.name + "Bck");


        yield return new WaitForSeconds(backgroundAnimation.length);

        canvas.gameObject.SetActive(false);
        activated = false;
    }
    IEnumerator PlayTypingSounds()
    {
        while (typing)
        {
            audioSource.clip = typingSounds[Random.Range(0, typingSounds.Length)]; //Plays a random keypress sound
            audioSource.Play();
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
    }
}
