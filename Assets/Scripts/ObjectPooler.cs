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
            Debug.Log("пулл: " + name + " пуст, создаем новый объект");

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
 Dictionary<TKey, TValue> - коллекция которая хранит данные в виде пар (ключ значение). Каждый ключ уникален и при помощи него можно 
 очень быстро получить значение.
Типы ключа и значений могут быть различными (ключ - string, значение - int).
Как создать словарь:
                    Dictionary<string, int> productQuantities = new Dictionary<string, int>();
                    productQuantities.Add("apples", 100);
                    productQuantities.Add("oranges", 70);
                    productQuantities.Add("bananas", 100);
                    Debug.Log($"На складе осталось {productQuantities["apples"] яблок}");

    Чтобы получить значение по ключу, достаточно обратится к элементу через ключ.
    Если мы попытаемся обратиться к ключу которого нет в словаре, то тогда возникнет ошибка.
    чтобы этого избежать, используй метод TryGetValue
                    if (productQuantities.TryGetValue("Cherry"), out int cherryCount)
                    {
                       Debug.Log($"На складе {cherryCount} вишни")
                    }
                    else
                    {
                       Debug.Log("Вишня отсутвует");   
                    }
   Важные методы словаря:
    1. Add - добавить пару ключ значение
    2. Remove - удаляет элемент по ключу
    3. Clear - удаляет весь список
    4. ContainsKey - Проверяет содержится ли элемент с заданым ключом
    5. TryGetValue - Пытается получить значение по ключу
    6. Count - Возвращает количество всех элементов в словаре
    7. Keys and Values - Возвращает количество всех кдючей или значений  
     
---------------------------------------------------------------------------------------------------------------------------------------------

Queue - Это коллекция по типу "очередь", которая следует принципу FIFO (First in, First out) первый вошел, первый вышел.
        Это озночает, что первый элемент добавленный в очередь, будет первыйм кто выйдет. Представь это как очередь в магазине:
        первый человек в очереди обслуживается первым.
    Когда использовать очередь: 
    1. Когда нужно обробатывать данные по очереди - События или действия, которая выполняется последовательно.
    2. В случии когда важен порядок обработки объектов - первый добавленный объект должен быть первым обработан.
    3. При логике, которая работает по принципу ожидание задач.

Основные методы Queue:
    1.Enqueue - этот метод добовляет элемент в конец очереди.
                    Queue<string> messageQueue = new Queue<string>();
                    messageQueue.Enqueue("игрок 1 присоединился");
                    messageQueue.Enqueue("игрок 2 присоединился");
    2.Dequeue - этот метод достает (извлекает) первый элемент из очереди и удаляет его.
                    string firstMessage = messageQueue.Dequeue();
                    Debug.Log(firstMessage); // вывод: игрок 1 присоединился
    3.Peek - этот метод позволяет посмотреть первый элемент очереди, не удаляя его. Элемент остается в очереди, в отличии от Dequeue.
                    string peekMessage = messageQueue.Peek();
                    Debug.Log(peekMessage); // вывод: игрок 2 присоединился
    4.Count - этот метод возвращает количество текущих объектов в очереди
                    int count = messageQueue.Count(); 
                    Debug.Log(count); // вывод: 1 элемент
    5.Clear - этот метод который удаляет все элементы из очереди.
                    messageQueue.Clear(); 
                    Debug.Log(messageQueue.Count); вывод: 0 элементов

 Основные плюсы Queue:
    Очередь это мощный инструмент для ситуации в которых важен порядок обработки.
    Добовление и удаление элементов происходит эффективно за счет быстрого доступа к памяти по ссылочному типу.



 */
