using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : BasePickup
{
    public float healAmount = 20f;

    public override void InitiatePickup()
    {
        pickedUp = true;
        PlayerHealthManager pHM = player.GetComponent<PlayerHealthManager>();
        if (pHM)
            pHM.Heal(healAmount);

    }
}
