using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedProjectileScript : MonoBehaviour
{
    public float ForceMultiplier = 10000f;
    Rigidbody RB;

    // Start is called before the first frame update
    void Awake()
    {
        RB = GetComponent<Rigidbody>();
        RB.AddForce(gameObject.transform.up * ForceMultiplier);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            print("Projectile hit player");
            Invoke(nameof(DelayedDestroy), 0.1f);
            //Some player damage code
        }
        else if (other.CompareTag("Projectile"))
        {
            print("Hit another projectile");
        }
        else
        {
            print("Destroying projectile");
            Destroy(gameObject);
        }
    }

    void DelayedDestroy()
    {
        Destroy(gameObject);
    }
}
