using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeElectric : EnemyAI
{
    protected override void FireProjectile()
    {
        if (allowfire == true)
        {
            GameObject projectile = Instantiate(_Projectile, EmitLocation.position, EmitLocation.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(transform.right * -600);
            allowfire = false;
            StartCoroutine(FireCooldown());
            StartCoroutine(FireProjectileDelay());
        }
    }

    IEnumerator FireProjectileDelay()
    {
        yield return new WaitForSeconds(.05f);
    }
}
