using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    [SerializeField] int DamageToPlayer;
    [SerializeField] Transform PlayerRespawn;
    [SerializeField] GameObject Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Hurts player and teleports player to a 'respawn' point
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IDamageable hit = (IDamageable)Player.gameObject.GetComponent(typeof(IDamageable));
            hit.TakeDamage(DamageToPlayer);
            Debug.Log("damaged player.");
            Player.transform.position = PlayerRespawn.position;
        }  
    }
}
