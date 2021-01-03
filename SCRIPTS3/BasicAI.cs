using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
     
    public float visionConeAngle = 30f;
    public float range = 50f;
    NavMeshAgent agent;
    public float playerEnemyAngle;
    Vector3 playerEnemyVector;
    Vector3 playerHorizontalPos;
    float distFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        visionConeAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }
    // Update is called once per frame
    void Update()
    {
        //Finding the horizontal vector between the player and the enemy (putting them both in the same XZ plane).
        playerHorizontalPos = PlayerVariables.player.transform.position;
        playerHorizontalPos.y = transform.position.y;

        //Calculating the vector between the player and the enemy then normalizing it.
        playerEnemyVector = playerHorizontalPos - transform.position;
        playerEnemyVector.Normalize();

        //Calculating the angle between the enemy and the player along with the distance between them
        playerEnemyAngle = Vector3.Dot(playerEnemyVector, transform.forward);
        distFromPlayer = Vector3.Distance(playerHorizontalPos, transform.position);

        //If the distance is less than the range and the player is in the enemy's cone of vision, rotates and moves the enemy towards the player.
        if (playerEnemyAngle > visionConeAngle && distFromPlayer < range)
        {
            agent.SetDestination(PlayerVariables.player.transform.position);
            RotateTowardsTarget();
        }
    }

    void RotateTowardsTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerEnemyVector, Vector3.up), Time.deltaTime * 100f);
    }
}
