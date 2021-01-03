using System.Collections;
using UnityEngine;

public class EndlessPit : MonoBehaviour
{
    public Transform respawnTransfrom;
    public float lerpTime = 2f;
    public float distanceThreshold = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(LerpTransform(other.transform));
    }

    IEnumerator LerpTransform(Transform playerTransform)
    {
        float distance = Vector3.Distance(playerTransform.position, respawnTransfrom.position);
        distance *= distance;
        float lerpIncr = (Time.deltaTime) / (lerpTime);
        float lerpVal = 0;
        while (Vector3.Distance(playerTransform.position, respawnTransfrom.position) > distanceThreshold)
        {
            lerpVal += lerpIncr;
            lerpIncr = (Time.deltaTime) / (lerpTime);
            playerTransform.position = Vector3.Slerp(playerTransform.position, respawnTransfrom.position, lerpVal);
            yield return null;
        }
    }
}
