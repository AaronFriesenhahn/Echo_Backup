using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public float _fireRate = 1f;
    public float _coolDown = 1f;
    public bool _canFire = true;
    public bool _onCeiling = false;
    public bool ObjectPowered = true;
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform EmitLocation;
    [SerializeField] ObjectsHitByProjectileBehavior _hitByProjectile;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_canFire == true && ObjectPowered == true)
        {
            if (_hitByProjectile != null)
            {
                if (_hitByProjectile.HitByIce == false)
                {
                    if (_onCeiling == true)
                    {
                        FireProjectileDown();
                        _canFire = false;
                        StartCoroutine(OnFireCooldown(_fireRate));
                    }
                    else
                    {
                        FireProjectileRight();
                        _canFire = false;
                        StartCoroutine(OnFireCooldown(_fireRate));
                    }
                }
                //add hitbyElectric condition?
            }
            else if (_hitByProjectile == null)
            {
                if (_onCeiling == true)
                {
                    FireProjectileDown();
                    _canFire = false;
                    StartCoroutine(OnFireCooldown(_fireRate));
                }
                else
                {
                    FireProjectileRight();
                    _canFire = false;
                    StartCoroutine(OnFireCooldown(_fireRate));
                }
            }  
        }
    }

    //Fires several projectiles at specified firerate
    IEnumerator OnFireCooldown(float value)
    {
        yield return new WaitForSeconds(value);
        if (_onCeiling == true)
        {
            FireProjectileDown();
            yield return new WaitForSeconds(value);
            FireProjectileDown();
            yield return new WaitForSeconds(value);
            FireProjectileDown();
            yield return new WaitForSeconds(value);
        }
        else
        {
            FireProjectileRight();
            yield return new WaitForSeconds(value);
            FireProjectileRight();
            yield return new WaitForSeconds(value);
            FireProjectileRight();
            yield return new WaitForSeconds(value);
        }
        yield return new WaitForSeconds(_coolDown);
        _canFire = true;
    }

    void FireProjectileDown()
    {
        GameObject projectile = Instantiate(_projectile, EmitLocation.position, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.up * -500);
    }
    void FireProjectileRight()
    {
        GameObject projectile = Instantiate(_projectile, EmitLocation.position, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * 500);
    }
}
