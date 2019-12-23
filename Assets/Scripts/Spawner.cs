using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    // Start is called before the first frame update
    [Header("Enemies")]
    public int maxEnemy;
    public float spawnFrequency;

    [Header("Waypoints")]
    [Tooltip("Les positions que vont prendre les ennemis")]
    public GameObject[] targets;

    private GameObject enemyContainer;
    private int enemyRemaining;

    bool enemiesSpawning;

    private float chrono = 0f;

    void Start()
    {
        enemiesSpawning = false;
        enemyRemaining = maxEnemy;
        enemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer");
        /*
        if (!enemiesSpawning && _MGR_Night_Day.Instance.cycleEnum == _MGR_Night_Day.CYCLE.NIGHT)
        {
            enemiesSpawning = true;
            StartCoroutine(WaitAfterSpawn());
        }*/
    }

    private void Update()
    {
        /*if (!enemiesSpawning && _MGR_Night_Day.Instance.cycleEnum == _MGR_Night_Day.CYCLE.NIGHT)
        {
            StartCoroutine(WaitAfterSpawn());
            enemiesSpawning = true;
        }
        */

        if (_MGR_Night_Day.Instance.cycleEnum == _MGR_Night_Day.CYCLE.NIGHT)
        {
            chrono += Time.deltaTime;
            if (chrono >= spawnFrequency)
            {
                if (enemyRemaining > 0)
                {
                    SpawnEnemy();
                    chrono = 0f;
                }
            }
        }
    }

    // IEnumerator WaitAfterSpawn()
    // {
    //     Debug.Log(enemyRemaining);
    //     if (enemyRemaining > 0 && enemiesSpawning)
    //     {
    //         while (enemyRemaining > 0)
    //         {
    //             //Fait spawn un enemy
    //             enemyRemaining--;
    //             Debug.Log("enemy =" + enemyToInstantiate);
    //             Debug.Log("targets length " + targets.Length);
    //             enemyToInstantiate.GetComponent<Enemy>().arr_target = targets;
    //             Instantiate(enemyToInstantiate).transform.parent = enemyContainer.transform;
    //             enemyToInstantiate.GetComponent<Enemy>().arr_target = targets;
    //             yield return new WaitForSeconds(spawnFrequency);
    //         }
    //     }
    //     else
    //     {
    //         enemiesSpawning = false;
    //         StopCoroutine(WaitAfterSpawn());
    //     }
    // }

    private void SpawnEnemy()
    {
        //Fait spawn un enemy
        enemyRemaining--;

        GameObject enemytoInstantiate;

        enemytoInstantiate = Instantiate(enemyPrefab);

        enemytoInstantiate.GetComponent<Enemy>().arr_target = targets;
        enemytoInstantiate.transform.position = gameObject.transform.localPosition;
        enemytoInstantiate.transform.parent = enemyContainer.transform;

    }
}
