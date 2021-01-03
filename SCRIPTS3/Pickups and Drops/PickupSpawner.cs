using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickup;
    public int spawnCount = 10;
    public float spawnDelay = 0.2f;
    public Vector2 randomRange = new Vector2(200, 500f);
    public float upMul = 1f;
    public float forwardMul = 0.5f;

    GameObject spawned;
    Rigidbody spawnedRB;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Spawn));
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if (spawnCount < 1)
            {
                StopCoroutine(nameof(Spawn));
                yield return new WaitForEndOfFrame();
            }

            spawned = Instantiate(pickup, transform.position, Quaternion.identity);
            spawnedRB = spawned.GetComponent<Rigidbody>();
            if (spawnedRB)
            {
                spawnedRB.AddForce((transform.up * upMul + transform.forward * forwardMul) * Random.Range(randomRange.x, randomRange.y));
            }

            transform.Rotate(0, 360 / spawnCount, 0);

            spawnCount--;
            //Debug.Log(spawnCount);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
