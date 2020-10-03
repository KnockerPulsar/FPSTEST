using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedProjectileScript : MonoBehaviour
{
    public float ForceMultiplier = 10000f;
    public float damage = 10f;
    Rigidbody RB;

    // Start is called before the first frame update
    void Awake()
    {
        RB = GetComponent<Rigidbody>();
        RB.AddForce(gameObject.transform.up * ForceMultiplier);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Projectile hit player");
            Invoke(nameof(DelayedDestroy), 0.1f);
            PlayerHealthManager playerHM = other.gameObject.GetComponent<PlayerHealthManager>();
            if (playerHM)
                playerHM.RecieveDamage(damage);
        }
        else if (other.CompareTag("Projectile") || other.CompareTag("Enemy"))
        {
            print("Hit another projectile");
        }
        else
        {
            print("Destroying projectile, hit" + other.name);
            Destroy(gameObject);
        }
    }

    void DelayedDestroy()
    {
        Destroy(gameObject);
    }
}
