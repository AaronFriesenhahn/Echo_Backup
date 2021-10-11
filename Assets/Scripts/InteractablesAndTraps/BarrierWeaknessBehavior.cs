using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierWeaknessBehavior : MonoBehaviour
{
    [Tooltip("Type 'None' for player projectile, or 'Electric', 'Fire', 'Ice', and 'Ground' for respective elemental projectiles.")]
    [SerializeField] string _BarrierWeakness;    

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerProjectile(Clone)" && _BarrierWeakness == "None")
        {
            Debug.Log("Hit by " + collision.gameObject.name + ". Destroying Barrier.");
            Destroy(gameObject);
        }
        else if (collision.gameObject.name == "PlayerProjectileElectric(Clone)" && _BarrierWeakness == "Electric")
        {
            Debug.Log("Hit by " + collision.gameObject.name + ". Destroying Barrier.");
            Destroy(gameObject);
        }
        else if (collision.gameObject.name == "PlayerProjectileFire(Clone)" && _BarrierWeakness == "Fire")
        {
            Debug.Log("Hit by " + collision.gameObject.name + ". Destroying Barrier.");
            Destroy(gameObject);
        }
        else if (collision.gameObject.name == "PlayerProjectileIce(Clone)" && _BarrierWeakness == "Ice")
        {
            Debug.Log("Hit by " + collision.gameObject.name + ". Destroying Barrier.");
            Destroy(gameObject);
        }
        else if (collision.gameObject.name == "PlayerProjectileGround(Clone)" && _BarrierWeakness == "Ground")
        {
            Debug.Log("Hit by " + collision.gameObject.name + ". Destroying Barrier.");
            Destroy(gameObject);
        }
    }
}
