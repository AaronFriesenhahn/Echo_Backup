using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPart : CollectPartsBase
{
    bool IsOnPart = false;

    public bool IsWeaponTutorial = false;
    public bool IsLegTutorial = false;

    public int HealthIncrease = 10;

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
        CollectPart(player);
    }

    private void CollectPart(PlayerController player)
    {
        var powerupMesh = gameObject.GetComponent<MeshRenderer>();
        var powerupCollider = gameObject.GetComponent<Collider>();
        //gameObject.SetActive(false);
        powerupMesh.enabled = false;
        powerupCollider.enabled = false;
        //give player health
        player.IncreaseHealth(HealthIncrease);
        Debug.Log("Increase player health by " + HealthIncrease);
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
