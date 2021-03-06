using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public GameObject _Projectile;
    [SerializeField] public Transform EmitLocation;
    [SerializeField] public float FireCoolDownTime = 2f;

    public bool allowfire = true;
    Health _healthsystem;
    public bool _enemyDiedResetEnemy = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        FireProjectile();
    }

    protected virtual void FireProjectile()
    {
        if (allowfire == true)
        {
            GameObject projectile = Instantiate(_Projectile, EmitLocation.position, EmitLocation.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
            allowfire = false;
            StartCoroutine(FireCooldown());
        }
        else if (_enemyDiedResetEnemy == true)
        {
            allowfire = true;
            _enemyDiedResetEnemy = false;
        }
    }

    protected virtual IEnumerator FireCooldown()
    {
        Debug.Log("Cooldown activated.");
        yield return new WaitForSeconds(FireCoolDownTime);
        allowfire = true;
    }
}
