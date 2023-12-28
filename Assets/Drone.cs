using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class Drone : MonoBehaviour, IPoolLinked
{

    public float followSpeed = 5f;
    public float ExplosiveTime = 5f;
    public float RotateSpeed = 5f;
    private Rigidbody2D rb;
    private bool isActivated = false;

    [SerializeField]
    private Transform _rotatingObject;
    public float Radius;

    public Transform RotatingObject { get => _rotatingObject; private set => _rotatingObject = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
      
        Vector2 directionToTarget = CarInputHandler.Instance.topDownCarController.transform.position - transform.position;


        directionToTarget.Normalize();


        rb.AddForce(directionToTarget * followSpeed);


        RotatingObject.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated == true) return;

        if (collision.GetComponent<TopDownCarController>())
        {
            isActivated = true;
            StartCoroutine(SelfDestruct());
        }

    }



    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }



    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(ExplosiveTime);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, Radius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.TryGetComponent<TopDownCarController>(out var Comp))
            {
                Comp.DoDamage(Random.Range(9, 15));
                Instantiate(ParticleHolder.ExpParticle, Comp.transform.position, Quaternion.identity, null);
            }

        }


        Instantiate(ParticleHolder.ExpParticle, this.transform.position, Quaternion.identity, null);
        CommonSoundManager.Instance.ExpContainer.PlayRandom();
        ((IPoolLinked)this).ReturnPool();
    }

    ObjectPool<GameObject> IPoolLinked.LinkedPool { get; set; }


    public GameObject ThisGameObject()
    {
        return this.gameObject;
    }


}
