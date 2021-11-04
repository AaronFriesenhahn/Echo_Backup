using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float thrust = 500f;
    public Rigidbody rb;
    public float timeLeft = 2f;
    public int weaponDamage = 10;

    [SerializeField] ParticleSystem impactParticles;
    //[SerializeField] ParticleSystem bulletParticles;

    int addforce = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        DestroyObject();
    }

    //projectile destroys upon collision
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            //does damage to hit object
            IDamageable hit = (IDamageable)collision.gameObject.GetComponent(typeof(IDamageable));
            if (hit != null)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                hit.TakeDamage(weaponDamage);                
            }
            if (impactParticles != null)
            {
                impactParticles = Instantiate(impactParticles,
                    transform.position, Quaternion.identity);
            }
            //AudioHelper.PlayClip2D(impactSound, 1f);
            
        }
        else if (collision.collider.tag != "Player")
        {
            //does damage to hit object
            IDamageable hit = (IDamageable)collision.gameObject.GetComponent(typeof(IDamageable));
            if (hit != null)
            {
                hit.TakeDamage(weaponDamage);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            if (impactParticles != null)
            {
                impactParticles = Instantiate(impactParticles,
                    transform.position, Quaternion.identity);
            }
            //AudioHelper.PlayClip2D(impactSound, 1f);
        }
        else if (collision.collider.tag == "Player")
        {
            Debug.Log("Hit Player.");
            //does damage to hit object
            IDamageable hit = (IDamageable)collision.gameObject.GetComponent(typeof(IDamageable));
            if (hit != null)
            {
                hit.TakeDamage(weaponDamage);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            if (impactParticles != null)
            {
                impactParticles = Instantiate(impactParticles,
                    transform.position, Quaternion.identity);
            }
            //AudioHelper.PlayClip2D(impactSound, 1f);
        }
        Debug.Log("Hit an object.");
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    void DestroyObject()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(gameObject);
        }
    }
}
