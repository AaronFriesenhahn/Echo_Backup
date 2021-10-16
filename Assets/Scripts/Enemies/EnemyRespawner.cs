using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    [SerializeField] GameObject _EnemyToSpawn;
    [SerializeField] EnemyAI _enemyAI;
    [SerializeField] ETypeFire _enemyTypeFire;
    [SerializeField] ETypeElectric _enemyTypeElectric;
    [SerializeField] ETypeIce _enemyTypeIce;
    [SerializeField] ETypeGround _enemyTypeGround;

    bool Respawned = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_EnemyToSpawn.activeSelf == false && Respawned == false)
        {
            Debug.Log("activating "+ _EnemyToSpawn);
            _EnemyToSpawn.GetComponent<EnemyAI>().allowfire = true;
            StartCoroutine(RespawnDelay());
        }
    }

    IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(10f);
        _EnemyToSpawn.GetComponent<Health>()._currentHealth = _EnemyToSpawn.GetComponent<Health>()._maxHealth;
        _EnemyToSpawn.SetActive(true);
        Respawned = true;
    }
}
