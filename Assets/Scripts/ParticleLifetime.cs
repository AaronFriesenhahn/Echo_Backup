using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifetime : MonoBehaviour
{
    [SerializeField] float _ParticleLifetime;

    bool StartCountdown = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StartCountdown == false)
        {
            StartCountdown = true;
            StartCoroutine(LifetimeCountdown(_ParticleLifetime));
        }
    }

    IEnumerator LifetimeCountdown(float value)
    {
        yield return new WaitForSeconds(value);
        Destroy(gameObject);
    }
}
