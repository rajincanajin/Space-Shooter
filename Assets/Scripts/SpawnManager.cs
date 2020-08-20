using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning = false;

    void Start()
    {
      
    }

    public void StartSpawning()
	{
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
	{        
		while (!_stopSpawning)
		{
            yield return new WaitForSeconds(3.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 10, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
	}
    IEnumerator SpawnPowerupRoutine()
	{
		while (!_stopSpawning)
		{
            yield return new WaitForSeconds(3.0f);
            int randomPowerup = Random.Range(0, 4);
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 10, 0);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }        
	}

    public void OnPlayerDeath()
	{
        _stopSpawning = true;
	}
}
