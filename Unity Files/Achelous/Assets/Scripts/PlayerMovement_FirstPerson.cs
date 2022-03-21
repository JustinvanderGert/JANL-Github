using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_FirstPerson : MonoBehaviour
{
    CharacterController characterController;

    public float baseSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;

    float speed = 12f;



    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * -x + transform.right * z;

        characterController.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    public void SpeedBoost(bool crit = false)
    {
        if(crit)
        {
            if (speed == baseSpeed)
            {
                speed *= 2;
            }
        }
        else
        {
            if (speed == baseSpeed)
            {
                speed *= 1.5f;
            }
        }

        StartCoroutine(BoostTimer(10));
    }

    IEnumerator BoostTimer(float time)
    {

        yield return new WaitForSeconds(time);
        speed = baseSpeed;
    }
}
