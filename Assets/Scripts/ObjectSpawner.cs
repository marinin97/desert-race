using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class ObjectSpawner : MonoBehaviour
{
    public Transform player;
    public float roadLenght;
    public int activeRoadsCount;
    public float roadSpeed;
    public float returnDistance;

    private List<GameObject> curentRoads = new List<GameObject>();
    private const string _carPoolName = "Cars";

    [Header("carsSettings")]
    public List<float> CarsPositions = new List<float>();
    public float MinCarsSpawnTime, MaxCarsSpawnTime;
    public float CarSpeed;


    [Header("barrelSettings")]
    public float leftBarrelPositionX, rightBarrelPositionX;
    public float MinSpawnTime;
    public float MaxSpawnTime;

    private const string _barrelName = "barrel";

    private Coroutine _spawnRoad;
    private Coroutine _spawnCar;
    private Coroutine _spawnBarrel;
    private Renderer _barrelRenderer;

    public void Awake()
    {
        PauseManager.OnGamePaused += PauseSpawn;
        PauseManager.OnGameResumed += ResumeSpawn;
        PlayerController.OnCarDestroyed += PauseSpawn;
    }

    private void Start()
    {
        for (int i = 0; i < activeRoadsCount; i++)
        {
            SpawnRoadAtPosition(player.position.z + i * roadLenght);
        }

        _spawnRoad = StartCoroutine(CheckAndSpawnRoads());
        _spawnCar = StartCoroutine(SpawnCars());
        _spawnBarrel = StartCoroutine(SpawnBarrels());


    }


    private IEnumerator CheckAndSpawnRoads()
    {
        while (true)
        {
            if (curentRoads.Count > 0)
            {
                GameObject firstRoad = curentRoads[0];
                float distanceBehind = player.position.z - firstRoad.transform.position.z;
                if (distanceBehind >= roadLenght)
                {
                    firstRoad.SetActive(false);
                    curentRoads.RemoveAt(0);
                }
            }
            while (curentRoads.Count < activeRoadsCount)
            {
                float nextPositionZ = curentRoads.Count > 0 ?
                curentRoads[curentRoads.Count - 1].transform.position.z + roadLenght : player.position.z;
                SpawnRoadAtPosition(nextPositionZ);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SpawnRoadAtPosition(float zPosition)
    {
        GameObject road = ObjectPooler.Instance.SpawnFromPool("MainRoad", new Vector3(0, 0, zPosition), Quaternion.identity);
        curentRoads.Add(road);
    }

    private IEnumerator SpawnCars()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinCarsSpawnTime, MaxCarsSpawnTime));
            float spawnPositionX = CarsPositions[Random.Range(0, CarsPositions.Count)];
            GameObject obj = ObjectPooler.Instance.SpawnFromPool(_carPoolName, new Vector3(spawnPositionX,
            transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, spawnPositionX > 0 ? 0 : 180, 0));
            if (obj != null)
            {
                ObjectMover mover = obj.GetComponent<ObjectMover>();
                float speed = spawnPositionX > 0 ?
                CarSpeed - roadSpeed :
                CarSpeed + roadSpeed;
                mover.SetSpeed(speed);
            }
            StartCoroutine(ReturnCarsPool(obj));
        }
    }
    private IEnumerator ReturnCarsPool(GameObject car)
    {
        float disactiveDistance = 8f;
        while (car.activeSelf && car.transform.position.z >= player.position.z - disactiveDistance)
        {
            yield return null;
        }
        car.SetActive(false);
    }

    private IEnumerator SpawnBarrels()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinSpawnTime, MaxSpawnTime));
            Vector3 spawnPosition = new Vector3(Random.Range(rightBarrelPositionX, leftBarrelPositionX), transform.position.y + 0.1f, transform.position.z);
            GameObject barrel = ObjectPooler.Instance.SpawnFromPool(_barrelName, spawnPosition, Quaternion.Euler(-90, 0, 0));
            ObjectMover mover = barrel.GetComponent<ObjectMover>();
            mover.SetSpeed(60);
            StartCoroutine(ReturnBarrelPool(barrel));
            _barrelRenderer = barrel.GetComponent<Renderer>();
        }
    }

    private IEnumerator ReturnBarrelPool(GameObject barrel)
    {
        float disactiveDistancebarrel = 8f;
        while (barrel.transform.position.z >= player.position.z - disactiveDistancebarrel)
        {
            yield return null;
        }
        _barrelRenderer.enabled = true;
        barrel.SetActive(false);
    }
    private void PauseSpawn()
    {
        StopCoroutine(_spawnRoad);
        StopCoroutine(_spawnCar);
        StopCoroutine(_spawnBarrel);
    }
    private void ResumeSpawn()
    {
        StartCoroutine(CheckAndSpawnRoads());
        _spawnCar = StartCoroutine(SpawnCars());
        _spawnBarrel = StartCoroutine(SpawnBarrels());
    }
    private void OnDestroy()
    {
        PauseManager.OnGameResumed -= ResumeSpawn;
        PauseManager.OnGamePaused -= PauseSpawn;
        PlayerController.OnCarDestroyed -= PauseSpawn;
        

    }

}

