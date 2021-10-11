using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeFire : EnemyAI
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
        yield return new WaitForSeconds(.05f);
        GameObject projectile = Instantiate(_Projectile, EmitLocation.position, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(_Projectile, EmitLocation.position, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(_Projectile, EmitLocation.position, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
    }


}
