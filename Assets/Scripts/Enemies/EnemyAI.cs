using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public GameObject _Projectile;
    [SerializeField] public Transform EmitLocation;

    public bool allowfire = true;

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
    }

    protected virtual IEnumerator FireCooldown()
    {
        Debug.Log("Cooldown activated.");
        yield return new WaitForSeconds(2f);
        allowfire = true;
    }
}
