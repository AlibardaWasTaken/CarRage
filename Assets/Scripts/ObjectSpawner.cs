using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn;

    

    [SerializeField]
    protected int limiter = 100;

    public bool IsEnabled = true;

    [SerializeField]
    private List<Transform> spawnpoints;

    [SerializeField]
    private float maxRadiusCheck =3000f;


    [SerializeField]
    private int takePointsToSpawn = 3;

    private TopDownPlayerCar topDownPlayerCar;

    [SerializeField]
    private float spawnInterval = 1.0f; 

    public GameObject PrefabToSpawn { get => prefabToSpawn; }
    public List<Transform> Spawnpoints { get => spawnpoints; }
    public float MaxRadiusCheck { get => maxRadiusCheck; protected set => maxRadiusCheck = value; }
    public int TakePointsToSpawn { get => takePointsToSpawn; protected set => takePointsToSpawn = value; }
    public float SpawnInterval { get => spawnInterval; protected set => spawnInterval = value; }

    private int ObjectAmount;

    protected ObjectPool<GameObject> objectPool;

    private WaitForSeconds PooledYield;

    private void Start()
    {
        topDownPlayerCar = CarInputHandler.Instance.topDownCarController;

        PooledYield = new WaitForSeconds(SpawnInterval);
        objectPool = new ObjectPool<GameObject>(createFunc: CreateObject, actionOnGet: OnPoolGet, actionOnRelease: OnPoolRelease, actionOnDestroy: OnPoolDestroy, maxSize: GetLimiter());

        GameManager.instance.OnGameStateChanged += OnGameStateChanged;

        OnStart();

        
        StartCoroutine(SpawnPrefabsRandomly());
    }

    private void OnGameStateChanged(GameStates obj)
    {
        if (obj == GameStates.raceOver)
            IsEnabled = false;
    }


    void OnDestroy()
    {
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }


    protected virtual void OnStart()
    {
    }


    protected virtual void OnPoolGet(GameObject obj)
    {
       
        var Points = spawnpoints.FindAll(x => (x.position - topDownPlayerCar.gameObject.transform.position).sqrMagnitude > MaxRadiusCheck).OrderBy(x => (x.position - topDownPlayerCar.gameObject.transform.position).sqrMagnitude).Take(TakePointsToSpawn).ToList();


        if (Points.Count == 0)
            objectPool.Release(obj);


        var RandomPointNum = Random.Range(0, Points.Count);
        var distantSpawnPoint = Points[RandomPointNum];

        obj.transform.position = distantSpawnPoint.position;

       if(obj.TryGetComponent(out IPoolLinked LinkedObj))
            LinkedObj.LinkPool(objectPool);

        obj.SetActive(true);
        ObjectAmount++;
    }




    protected virtual void OnPoolRelease(GameObject obj)
    {
        ObjectAmount--;
        obj.SetActive(false);
    }


    protected virtual void OnPoolDestroy(GameObject obj)
    {
        ObjectAmount--;
        Destroy(obj);
    }




    private IEnumerator SpawnPrefabsRandomly()
    {
        while (IsEnabled)
        {


            yield return PooledYield;

            if (!Condition())
                continue;

            var obj = objectPool.Get();
            OnSpawn(obj);

        }
    }


   private GameObject CreateObject()
    {

        GameObject obj = Instantiate(prefabToSpawn, this.transform);
        return obj;
    }


    public virtual int GetLimiter()
    {
        return limiter;
    }
    protected virtual bool Condition()
    {
        return (GetLimiter() > ObjectAmount);
    }

    protected virtual void OnSpawn(GameObject SpawnedObj)
    {
        
    }
}
