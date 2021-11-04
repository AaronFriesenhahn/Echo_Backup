using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeBoss : EnemyAI
{
    [Tooltip("Type 'None' for normal jump, or 'Electric', 'Fire', 'Ice', and 'Ground' for respective elemental jumps.")]
    [SerializeField] string _JumpType;
    Health _healthsystem;
    Rigidbody _rigidbody = null;
    [SerializeField] Transform _PlayerPosition;
    public bool _isFacingLeft = true;
    public bool _PlayerHasEntered  = false;

    //for jumping
    int x = 0;
    //for supernova ability
    int y = 0;
    bool _canFireSupernova = true;
    [SerializeField] GameObject _BossSecondaryProjectile;
    [SerializeField] ParticleSystem _SupernovaChargingParticles;
    [SerializeField] Transform[] _Supernovas;

    [SerializeField] GameObject _ObjectToPowerUponDeath;


    // Start is called before the first frame update
    void Start()
    {
        _healthsystem = GetComponent<Health>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_PlayerHasEntered == false)
        {
            allowfire = false;
        }

        //checks to see if player is in range of Boss (TODO needs a better check than number, probably)
        if (_PlayerHasEntered == true)
        {
            if (_PlayerPosition.transform.position.x < transform.position.x && _isFacingLeft == false)
            {
                //Face Left
                transform.Rotate(0, 180, 0);
                _isFacingLeft = true;
            }
            else if (_PlayerPosition.transform.position.x > transform.position.x && _isFacingLeft == true)
            {
                //Face Right
                transform.Rotate(0, -180, 0);
                _isFacingLeft = false;
            }
            //check for health and update Boss Tactics based on health
            if (_healthsystem._currentHealth < 150 && _healthsystem._currentHealth > 50 && x == 0)
            {
                x = 1;
                StartCoroutine(JumpDelay());
            }
            if (_healthsystem._currentHealth < 100 && y == 0)
            {
                y = 1;
                StartCoroutine(SuperNovaAbility());
            }
            if (_healthsystem._currentHealth == 0)
            {
                _ObjectToPowerUponDeath.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered = true;
            }
        }
    }

    //Ice Projectile Section
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
        Vector3 offset = new Vector3(0, .25f, 0);
        GameObject projectile = Instantiate(_Projectile, EmitLocation.position - offset, EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        if (_healthsystem._currentHealth < 150)
        {
            projectile = Instantiate(_Projectile, EmitLocation.position + offset, EmitLocation.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        }
        //fire a projectile downwards
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(_Projectile, EmitLocation.position - (offset * 3), EmitLocation.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        if (_healthsystem._currentHealth < 150)
        {
            projectile = Instantiate(_Projectile, EmitLocation.position + (offset * 3), EmitLocation.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        }
        yield return new WaitForSeconds(.05f);
    }

    //how often the Boss Jumps.
    IEnumerator JumpDelay()
    {
        //TODO add more jump types (in the future)
        if (_JumpType == "Fire")
        {
            //jump
            _rigidbody.AddForce(Vector3.up * (400));
            yield return new WaitForSeconds(1f);
            //jump again
            _rigidbody.AddForce(Vector3.up * (350));
            Debug.Log("Boss Jumping!");
        }
        yield return new WaitForSeconds(5f);
        x = 0;
    }

    //supernova ability
    IEnumerator SuperNovaAbility()
    {
        Debug.Log("Supernova!");
        if (_canFireSupernova == true)
        {
            _canFireSupernova = false;

            ParticleSystem SupernovaCharging = Instantiate(_SupernovaChargingParticles, gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);

            //spawns fire projectiles at each spawn location
            for (int i = 0; i < _Supernovas.Length; i++)
            {
                GameObject _SupernovaToSpawn;
                _SupernovaToSpawn = Instantiate(_BossSecondaryProjectile, _Supernovas[i].position, _Supernovas[i].rotation);
                if (i == 0)
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.right * -500);
                }
                else if (i == 1)
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.right * -500);
                }
                else if (i == 2)
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.up * 250);
                }
                else if (i == 3)
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.right * 500);
                }
                else
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.right * 500);
                }
            }
            yield return new WaitForSeconds(0.5f);
            //spawns fire projectiles at each spawn location
            for (int i = 0; i < _Supernovas.Length; i++)
            {
                GameObject _SupernovaToSpawn;
                _SupernovaToSpawn = Instantiate(_BossSecondaryProjectile, _Supernovas[i].position, _Supernovas[i].rotation);
                if (i == 0)
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.right * -500);
                }
                else if (i == 1)
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.right * -500);
                }
                else if (i == 2)
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.up * 250);
                }
                else if (i == 3)
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.right * 500);
                }
                else
                {
                    _SupernovaToSpawn.GetComponent<Rigidbody>().AddForce(transform.right * 500);
                }
            }
            yield return new WaitForSeconds(2.5f);
            _canFireSupernova = true;
            y = 0;
        }
    }
}
