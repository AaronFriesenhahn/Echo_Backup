using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorScript : MonoBehaviour
{
    [SerializeField]
    Animator EnemyAnimations;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimations = GetComponent<Animator>();
    }

    private void Awake()
    {
        EnemyAnimations.SetTrigger("IdleTrigger");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
