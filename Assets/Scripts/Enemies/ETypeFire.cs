using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeFire : EnemyAI
{

    protected override void FireProjectile()
    {
        if (allowfire == true)
        {
            allowfire = false;
            GameObject projectile = Instantiate(_Projectile, EmitLocation.position, EmitLocation.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
            
            StartCoroutine(FireCooldown());
            StartCoroutine(FireProjectileDelay());
        }
    }

    IEnumerator FireProjectileDelay()
    {
        yield return new WaitForSeconds(.05f);
        Vector3 offset = new Vector3(0, .25f, 0);
        GameObject projectile = Instantiate(_Projectile, EmitLocation.position + offset, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(_Projectile, EmitLocation.position, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(_Projectile, EmitLocation.position - offset, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
    }


}
