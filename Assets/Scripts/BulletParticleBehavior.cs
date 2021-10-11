using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticleBehavior : MonoBehaviour
{

    private ParticleSystem bulletParticles;

    // Start is called before the first frame update
    void Start()
    {
        bulletParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletParticles)
        {
            if (!bulletParticles.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
