using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Utils;
using UnityEngine;

namespace DontLetItFall
{
    public class BoxSpawner : MonoBehaviour
    {
        [SerializeField] 
        private LootTable _lootTable;
        
        [SerializeField]
        private Transform[] _spawnPoints;
        
        [SerializeField]
        private Vector2 _timeBetweenSpawns;

        private void Start()
        {
            StartCoroutine(SpawnBox());
        }

        private IEnumerator SpawnBox()
        {
            yield return new WaitForSeconds(Random.Range(_timeBetweenSpawns.x, _timeBetweenSpawns.y));
            Vector3 spawnPoint = new Vector3(Random.Range(_spawnPoints[0].position.x, _spawnPoints[1].position.x), transform.position.y, Random.Range(_spawnPoints[0].position.z, _spawnPoints[1].position.z));
            Instantiate(_lootTable.GetRandom(), spawnPoint, transform.rotation);
            
            StartCoroutine(SpawnBox());
        }
    }
}
