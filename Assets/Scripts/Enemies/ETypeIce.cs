using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeIce : EnemyAI
{
    protected override void FireProjectile()
    {
        if (allowfire == true)
        {
            GameObject projectile = Instantiate(_Projectile, EmitLocation.position, EmitLocation.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
            allowfire = false;
            StartCoroutine(FireCooldown());
            StartCoroutine(FireProjectileDelay());
        }
    }

    IEnumerator FireProjectileDelay()
    {
        //fire a projectile upwards
        yield return new WaitForSeconds(.05f);
        Vector3 offset = new Vector3(0, .5f, 0);
        GameObject projectile = Instantiate(_Projectile, EmitLocation.position + offset, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        //fire a projectile downwards
        //yield return new WaitForSeconds(.05f);
        projectile = Instantiate(_Projectile, EmitLocation.position - offset, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        yield return new WaitForSeconds(.05f);
    }
}
