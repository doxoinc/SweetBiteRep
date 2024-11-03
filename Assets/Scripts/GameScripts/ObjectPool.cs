using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [Header("Pooling Settings")]
    public GameObject piePrefab; // Префаб пирога
    public Sprite[] pieSprites; // Спрайты пирогов: 0 - Strawberry, 1 - Kiwi, 2 - Blueberry
    public int poolSize = 20; // Размер пула

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    private void Awake()
    {
        // Синглтон
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Инициализация пула
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(piePrefab);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    // Получение пирога из пула
    public GameObject GetPooledPie()
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true);
            poolQueue.Enqueue(obj); // Возвращаем объект в очередь для повторного использования
            return obj;
        }
        else
        {
            // Если пул исчерпан, создаём новый пирог
            GameObject obj = Instantiate(piePrefab);
            obj.SetActive(true);
            return obj;
        }
    }
}
