using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{


    public GameObject drop;

    public float ActivationDelay = 0.5f;

    //Should have a list of enemy GameObjects to check for death.
    public int enemiesInArena = 0;
    //Should have a list of doors to activate and deactivate.
    public GameObject[] doors;
    //A box collider to activate on the player's first entrance.
    public BoxCollider Bounds;

    public AudioClip[] music;
    public AudioSource audioSource;
    public AnimationCurve musicFadeCurve;
    public float musicFadeDuration = 3f;
    public bool activated = false;

    Vector3 triggerSize => new Vector3(Bounds.size.x * transform.localScale.x, Bounds.size.y * transform.localScale.y, Bounds.size.z * transform.localScale.z);

    private void Start()
    {
        if (drop && drop.activeSelf == true)
            drop.SetActive(false);

        Collider[] colliders = Physics.OverlapBox(transform.position, triggerSize, Bounds.transform.rotation);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                enemiesInArena++;
            }
        }
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().Open();
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.isTrigger == false && other.CompareTag("Player"))
    //    {
    //        print("Arena activating");
    //        if (enemiesInArena > 0)
    //        {
    //            StartCoroutine(Activate());
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (pickedUp)
    //        if (other.isTrigger == false && other.CompareTag("Player"))
    //        {
    //            doors[0].GetComponent<Door>().Open();
    //        }
    //}

    public IEnumerator Activate()
    {
        activated = true;
        yield return new WaitForSeconds(ActivationDelay);

        foreach (GameObject door in doors)
        {
            //Get the door script and call the activation function.
            Door doorScript = door.GetComponent<Door>();

            if (doorScript)
                doorScript.Close();
        }

        StartCoroutine(FadeIntoMusic());
        StopCoroutine(FadeOutOfMusic());
    }

    void Deactivate()
    {
        activated = false;
        if (drop)
        {
            drop.SetActive(true);
        }

        foreach (GameObject door in doors)
        {
            //Get the door script and call the deactivation function.
            Door doorScript = door.GetComponent<Door>();
            if (doorScript)
                doorScript.Open();
        }

        StopCoroutine(FadeIntoMusic());
        StartCoroutine(FadeOutOfMusic());
    }

    public void DecrementEnemyCounter()
    {
        enemiesInArena--;
        if (enemiesInArena == 0)
        {
            //print("Deactivating");
            Deactivate();
        }
    }

    public IEnumerator FadeIntoMusic()
    {
        audioSource.clip = music[Random.Range(0, music.Length)];
        audioSource.Play();

        float fade = 0;
        while (fade < musicFadeDuration)
        {
            audioSource.volume = musicFadeCurve.Evaluate(fade / musicFadeDuration);
            fade += Time.deltaTime;
            yield return null;
        }
    }


    public IEnumerator FadeOutOfMusic()
    {
        float fade = musicFadeDuration;
        while (fade > 0)
        {
            audioSource.volume = musicFadeCurve.Evaluate(fade / musicFadeDuration);
            fade -= Time.deltaTime;
            yield return null;
        }
        audioSource.Stop();
    }
}
