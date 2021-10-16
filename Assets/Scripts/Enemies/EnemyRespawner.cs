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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator RespawnDelay()
    {
        _EnemyToSpawn.SetActive(false);
        yield return new WaitForSeconds(10f);
        
        _EnemyToSpawn.SetActive(true);
        _EnemyToSpawn.GetComponent<Health>()._currentHealth = _EnemyToSpawn.GetComponent<Health>()._maxHealth;
        _EnemyToSpawn.GetComponent<EnemyAI>().allowfire = true;
    }
}
