using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementIsometric : MonoBehaviour
{
    private CharacterController controller;

    private bool groundedPlayer;
    private bool doOnce = true;

    private float playerSpeed = 5.0f, gravityValue = -40f;

    private Vector3 northSouthDir, eastWestDir, playerVelocity;
    private Vector3 move;

    private void Start()
    {
        //////////////////////////////////////////////////////////// Get Character Controller Off the Player
        controller = gameObject.GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {

        //////////////////////////////////////////////////////////// Player Grounded & Speed
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            //////////////////////////////////////////////////////////// Sprinting Speed
            playerVelocity.y = 0f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (doOnce)
                {
                    doOnce = false;
                }
                playerSpeed = 15.0f;
            }
            else
            {
                //////////////////////////////////////////////////////////// Walking Speed
                doOnce = true;
                playerSpeed = 8.0f;
            }
        }
        else
        {
            //////////////////////////////////////////////////////////// In air Speed
            playerSpeed = 5.0f;
        }

        //////////////////////////////////////////////////////////// Player Movement X & Z


        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            northSouthDir = Camera.main.transform.forward;
            northSouthDir += new Vector3(0, .7f, 0);
            northSouthDir = Vector3.Normalize(northSouthDir);
            eastWestDir = Quaternion.Euler(new Vector3(0, 90, 0)) * northSouthDir;
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            move = Quaternion.Euler(new Vector3(0, 90, 0)) * northSouthDir;

            Vector3 rightMovement = eastWestDir * playerSpeed * Input.GetAxis("Horizontal");
            Vector3 upMovement = northSouthDir * playerSpeed * Input.GetAxis("Vertical");

            move = Vector3.Normalize(rightMovement + upMovement);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }
        else
        {
            move = Vector3.zero;
        }

        //////////////////////////////////////////////////////////// Player Movement Y
        /*if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            if (jumpCount == 0 && groundedPlayer)
            {
                //////////////////////////////////////////////////////////// 1st Jump
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -5f * gravityValue);
            }
            else if (jumpCount == 1)
            {
                //////////////////////////////////////////////////////////// 2nd Jump
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -5f * gravityValue);
            }
            jumpCount++;
        }*/

        //////////////////////////////////////////////////////////// Final Movement
        if (playerVelocity.y >= 15) playerVelocity.y = 15;
        playerVelocity.y += (gravityValue * Time.fixedDeltaTime);
        Vector3 finalMovementVector = new Vector3(move.x * playerSpeed, playerVelocity.y, move.z * playerSpeed);
        controller.Move(finalMovementVector * Time.fixedDeltaTime);
    }
}
