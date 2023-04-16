using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    /* Atributos */
    // About InvokeRepeating SpawnEnemy
    private float spawnTime = 0.01f;
    private float spawnTimeRate = 5f;
    // About Enemies
    public GameObject[] enemyPrefabs;
    private GameObject enemyPrefab;
    private GameObject enemy;

    /* M�todos */
    /* M�todo Start */
    private void Start()
    {
        // Si pasado el spawnTimeRate no hay ning�n enemigo en la lista, generamos uno nuevo
        InvokeRepeating("SpawnEnemy", spawnTime, spawnTimeRate);
    }

    /* M�todo CleanDeadEnemies */
    private void CleanDeadEnemies()
    {
        if(!(enemy == null))
        {
            Debug.Log("CleanDeadEnemies --> -- enemy.activeInHierarchyt --> " + enemy.activeInHierarchy);
            if (!(enemy.activeInHierarchy))
            {
                // Destruimos el enemigo inactivo para que se genere uno nuevo
                Destroy(enemy);
            }
        }
        
    }
    /* M�todo SpawnEnemy */
    private void SpawnEnemy()
    {
        // Limpiamos la lista de enemigos eliminados cada deleteTime
        CleanDeadEnemies();
        // Si no hay ning�n enemigo instanciado
        if (enemy == null)
        {
            //Asignamos el enemigo de forma aleatoria
            SetRandomEnemy();
            if (!Object.ReferenceEquals(enemyPrefab, null))
            {
                // Instanciamos el enemigo
                enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            }
            Debug.Log("SpawnEnemy --> -- enemy.activeInHierarchyt --> " + enemy.activeInHierarchy);
        }
    }

    /* M�todo RandomEnemy */
    private void SetRandomEnemy()
    {
        Debug.Log("enemyPrefabs.Length" + enemyPrefabs.Length);
        int randomInt = new System.Random().Next(0, (enemyPrefabs.Length));
        Debug.Log("Debug.Log(randomInt): " + randomInt);
        enemyPrefab = enemyPrefabs[randomInt];
    }
}
