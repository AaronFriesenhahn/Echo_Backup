using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPart : CollectPartsBase
{
    bool IsOnPart = false;

    public bool IsWeaponTutorial = false;
    public bool IsLegTutorial = false;

    protected override void OnTriggerStay(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("PlayerNeaby!");
            IsOnPart = true;
            Collect(player);
            //get mesh of Collectable
            //spawn particles & sfx because we need to disable object
            Feedback();
        }
    }

    protected override void Collect(PlayerController player)
    {
        if (IsWeaponTutorial == true && IsLegTutorial == false)
        {
            //if w is pressed, grab arm powerup
            if (Input.GetKey(KeyCode.W))
            {
                Debug.Log("W pressed");
                player.DetectArmUpgrade(0);

                var powerupMesh = gameObject.GetComponent<MeshRenderer>();
                var powerupCollider = gameObject.GetComponent<Collider>();
                //gameObject.SetActive(false);
                powerupMesh.enabled = false;
                powerupCollider.enabled = false;
            }
            Debug.Log("Collect called.");
        }
        else if (IsWeaponTutorial == false && IsLegTutorial == true)
        {
            //if s is pressed, grab leg powerup
            if (Input.GetKey(KeyCode.S))
            {
                Debug.Log("S pressed");
                player.DetectLegUpgrade(0);

                var powerupMesh = gameObject.GetComponent<MeshRenderer>();
                var powerupCollider = gameObject.GetComponent<Collider>();
                //gameObject.SetActive(false);
                powerupMesh.enabled = false;
                powerupCollider.enabled = false;
            }
        }
        else
        {
            CollectPart(player);
        }
    }

    private void CollectPart(PlayerController player)
    {
        //give the Player Normal Ability
        //if s is pressed, grab leg powerup
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("S pressed");
            player.DetectLegUpgrade(0);

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
            Debug.Log("W pressed");
            player.DetectArmUpgrade(0);

            var powerupMesh = gameObject.GetComponent<MeshRenderer>();
            var powerupCollider = gameObject.GetComponent<Collider>();
            //gameObject.SetActive(false);
            powerupMesh.enabled = false;
            powerupCollider.enabled = false;
            //give player health
            player.IncreaseHealth(10);
            Debug.Log("Increase player health by 10.");
        }
        Debug.Log("Collect called.");
    }

    protected override void Movement(Rigidbody rb)
    {
        //calculate rotation
        Quaternion turnOffset = Quaternion.Euler
            (MovementSpeed, MovementSpeed, 0);
        rb.MoveRotation(rb.rotation * turnOffset);
    }
}
