using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrashBall : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;
    public Vector3 targetPosition;

    bool targetReached = false;


    void Start()
    {
        
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance > 1 && !targetReached)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
        else
            targetReached = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit the Player");
            Destroy(gameObject);
        }
    }
}