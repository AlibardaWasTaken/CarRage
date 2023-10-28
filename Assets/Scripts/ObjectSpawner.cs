using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn;


    [SerializeField]
    private List<Transform> spawnpoints;

    [SerializeField]
    private float MaxRadiusCheck =3000f;


    [SerializeField]
    private int TakePointsToSpawn = 3;

    private TopDownPlayerCar topDownPlayerCar;

    public float spawnInterval = 1.0f; // Time between spawning prefabs

    public GameObject PrefabToSpawn { get => prefabToSpawn; }
    public List<Transform> Spawnpoints { get => spawnpoints; }

    private void Start()
    {
        topDownPlayerCar = CarInputHandler.Instance.topDownCarController;
        StartCoroutine(SpawnPrefabsRandomly());
    }

    private IEnumerator SpawnPrefabsRandomly()
    {
        while (true)
        {


            yield return new WaitForSeconds(spawnInterval);

            if (!Condition())
                continue;


            //var Points = spawnpoints.OrderBy(x => (x.position - topDownPlayerCar.gameObject.transform.position).sqrMagnitude < radiusCheck )
            //               .Take(TakePointsToSpawn).ToList();



           // Debug.Log((spawnpoints[0].position - topDownPlayerCar.gameObject.transform.position).sqrMagnitude.ToString() + " " + ((spawnpoints[0].position - topDownPlayerCar.gameObject.transform.position).sqrMagnitude > MaxRadiusCheck).ToString());

            var Points = spawnpoints.FindAll(x => (x.position - topDownPlayerCar.gameObject.transform.position).sqrMagnitude > MaxRadiusCheck).OrderBy(x=>(x.position - topDownPlayerCar.gameObject.transform.position).sqrMagnitude).Take(TakePointsToSpawn).ToList();


            if (Points.Count == 0)
                continue;

            var RandomPointNum = Random.Range(0, Points.Count);
           var distantSpawnPoint = Points[RandomPointNum];

            //Debug.Log((distantSpawnPoint.position - topDownPlayerCar.gameObject.transform.position).sqrMagnitude);
            




            var spawnedobj = Instantiate(PrefabToSpawn, distantSpawnPoint.position, Quaternion.identity, this.gameObject.transform);
            OnSpawn(spawnedobj);
            


        }
    }


    protected virtual bool Condition()
    {
        return true;
    }

    protected virtual void OnSpawn(GameObject SpawnedObj)
    {
        
    }
}
