using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPart : CollectPartsBase
{
    bool IsOnPart = false;

    protected override void OnTriggerStay(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            IsOnPart = true;
            Collect(player);
            //get mesh of Collectable
            //spawn particles & sfx because we need to disable object
            Feedback();
        }
    }

    protected override void Collect(PlayerController player)
    {
        //give the Player Ground Ability
        //if s is pressed, grab leg powerup
        if (Input.GetKey(KeyCode.S))
        {
            player.DetectLegUpgrade(3);

            var powerupMesh = gameObject.GetComponent<MeshRenderer>();
            var powerupCollider = gameObject.GetComponent<Collider>();
            //gameObject.SetActive(false);
            powerupMesh.enabled = false;
            powerupCollider.enabled = false;
            //give player health
            player.IncreaseHealth(10);
            Debug.Log("Increase player health by 10.");
        }
        //if w is pressed, grab arm powerup
        else if (Input.GetKey(KeyCode.W))
        {
            player.DetectArmUpgrade(3);

            var powerupMesh = gameObject.GetComponent<MeshRenderer>();
            var powerupCollider = gameObject.GetComponent<Collider>();
            //gameObject.SetActive(false);
            powerupMesh.enabled = false;
            powerupCollider.enabled = false;
            //give player health
            player.IncreaseHealth(10);
            Debug.Log("Increase player health by 10.");
        }
    }

    protected override void Movement(Rigidbody rb)
    {
        //calculate rotation
        Quaternion turnOffset = Quaternion.Euler
            (MovementSpeed, MovementSpeed, 0);
        rb.MoveRotation(rb.rotation * turnOffset);
    }
}
