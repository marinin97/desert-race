using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string PoolName;
    public GameObject Prefab;
    public List<GameObject> Prefabs;
    public int Size;

}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    private bool _poolPaused;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj;
                if (pool.Prefabs == null || pool.Prefabs.Count == 0)
                {
                    obj = Instantiate(pool.Prefab);
                }
                else
                {
                    GameObject randomPrefab = pool.Prefabs[Random.Range(0, pool.Prefabs.Count)];
                    obj = Instantiate(randomPrefab);
                }
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.PoolName, objectPool);
        }


    }

    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {

        if (!poolDictionary.ContainsKey(name))
        {
            return null;
        }

        Queue<GameObject> objectPoolSpawn = poolDictionary[name];

        if (objectPoolSpawn.Count == 0)
        {
            Debug.Log("����: " + name + " ����, ������� ����� ������");

            foreach (Pool pool in pools)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                objectPoolSpawn.Enqueue(obj);
                break;
            }
        }

        GameObject objectToSpawn = objectPoolSpawn.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectPoolSpawn.Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    public GameObject GetRandomObjectFromPool(string name)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[name];
        GameObject[] poolArray = objectPool.ToArray();
        GameObject randomObject = poolArray[Random.Range(0, poolArray.Length)];
        return randomObject;
    }





}

/*
 Dictionary<TKey, TValue> - ��������� ������� ������ ������ � ���� ��� (���� ��������). ������ ���� �������� � ��� ������ ���� ����� 
 ����� ������ �������� ��������.
���� ����� � �������� ����� ���� ���������� (���� - string, �������� - int).
��� ������� �������:
                    Dictionary<string, int> productQuantities = new Dictionary<string, int>();
                    productQuantities.Add("apples", 100);
                    productQuantities.Add("oranges", 70);
                    productQuantities.Add("bananas", 100);
                    Debug.Log($"�� ������ �������� {productQuantities["apples"] �����}");

    ����� �������� �������� �� �����, ���������� ��������� � �������� ����� ����.
    ���� �� ���������� ���������� � ����� �������� ��� � �������, �� ����� ��������� ������.
    ����� ����� ��������, ��������� ����� TryGetValue
                    if (productQuantities.TryGetValue("Cherry"), out int cherryCount)
                    {
                       Debug.Log($"�� ������ {cherryCount} �����")
                    }
                    else
                    {
                       Debug.Log("����� ���������");   
                    }
   ������ ������ �������:
    1. Add - �������� ���� ���� ��������
    2. Remove - ������� ������� �� �����
    3. Clear - ������� ���� ������
    4. ContainsKey - ��������� ���������� �� ������� � ������� ������
    5. TryGetValue - �������� �������� �������� �� �����
    6. Count - ���������� ���������� ���� ��������� � �������
    7. Keys and Values - ���������� ���������� ���� ������ ��� ��������  
     
---------------------------------------------------------------------------------------------------------------------------------------------

Queue - ��� ��������� �� ���� "�������", ������� ������� �������� FIFO (First in, First out) ������ �����, ������ �����.
        ��� ��������, ��� ������ ������� ����������� � �������, ����� ������� ��� ������. ��������� ��� ��� ������� � ��������:
        ������ ������� � ������� ������������� ������.
    ����� ������������ �������: 
    1. ����� ����� ������������ ������ �� ������� - ������� ��� ��������, ������� ����������� ���������������.
    2. � ������ ����� ����� ������� ��������� �������� - ������ ����������� ������ ������ ���� ������ ���������.
    3. ��� ������, ������� �������� �� �������� �������� �����.

�������� ������ Queue:
    1.Enqueue - ���� ����� ��������� ������� � ����� �������.
                    Queue<string> messageQueue = new Queue<string>();
                    messageQueue.Enqueue("����� 1 �������������");
                    messageQueue.Enqueue("����� 2 �������������");
    2.Dequeue - ���� ����� ������� (���������) ������ ������� �� ������� � ������� ���.
                    string firstMessage = messageQueue.Dequeue();
                    Debug.Log(firstMessage); // �����: ����� 1 �������������
    3.Peek - ���� ����� ��������� ���������� ������ ������� �������, �� ������ ���. ������� �������� � �������, � ������� �� Dequeue.
                    string peekMessage = messageQueue.Peek();
                    Debug.Log(peekMessage); // �����: ����� 2 �������������
    4.Count - ���� ����� ���������� ���������� ������� �������� � �������
                    int count = messageQueue.Count(); 
                    Debug.Log(count); // �����: 1 �������
    5.Clear - ���� ����� ������� ������� ��� �������� �� �������.
                    messageQueue.Clear(); 
                    Debug.Log(messageQueue.Count); �����: 0 ���������

 �������� ����� Queue:
    ������� ��� ������ ���������� ��� �������� � ������� ����� ������� ���������.
    ���������� � �������� ��������� ���������� ���������� �� ���� �������� ������� � ������ �� ���������� ����.



 */
