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

    /* Método */
    /* Método Start */
    private void Start()
    {
        // Si pasado el spawnTimeRate no hay ning�n enemigo en la lista, generamos uno nuevo
        InvokeRepeating("SpawnEnemy", spawnTime, spawnTimeRate);
    }

    /* Método CleanDeadEnemies */
    private void CleanDeadEnemies()
    {
        if(!(enemy == null))
        {
            if (!(enemy.activeInHierarchy))
            {
                // Destruimos el enemigo inactivo para que se genere uno nuevo
                Destroy(enemy);
            }
        }
        
    }
    /* Método SpawnEnemy */
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
        }
    }

    /* Método RandomEnemy */
    private void SetRandomEnemy()
    {
        int randomInt = new System.Random().Next(0, (enemyPrefabs.Length));
        enemyPrefab = enemyPrefabs[randomInt];
    }
}
