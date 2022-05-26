using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Utils;
using DontLetItFall.Variables;
using UnityEngine;

namespace DontLetItFall
{
    public class BoxSpawner : MonoBehaviour
    {
        [SerializeField] 
        private LootTable _lootTable;
        
        [SerializeField] 
        private LootTable _rocksList;
        
        [SerializeField]
        private Transform[] _spawnPoints;
        
        [SerializeField]
        private Vector2 _dayTimeBetweenSpawns;
        
        [SerializeField]
        private Vector2 _nightTimeBetweenSpawns;
        
        [SerializeField]
        private Value<float> _timeOfDay;

        private void Start()
        {
            StartCoroutine(SpawnBox());
        }

        private IEnumerator SpawnBox()
        {
            Vector3 spawnPoint = new Vector3(Random.Range(_spawnPoints[0].position.x, _spawnPoints[1].position.x), transform.position.y, Random.Range(_spawnPoints[0].position.z, _spawnPoints[1].position.z));
            if (_timeOfDay.value >= .5f)
            {
                Instantiate(_lootTable.GetRandom(), spawnPoint, new Quaternion(0,0,0,0));
                yield return new WaitForSeconds(Random.Range(_dayTimeBetweenSpawns.x, _dayTimeBetweenSpawns.y));
            }
            else
            {
                Instantiate(_rocksList.GetRandom(), spawnPoint, new Quaternion(0,0,0,0));
                yield return new WaitForSeconds(Random.Range(_nightTimeBetweenSpawns.x, _nightTimeBetweenSpawns.y));
            }
            
            StartCoroutine(SpawnBox());
        }
    }
}
