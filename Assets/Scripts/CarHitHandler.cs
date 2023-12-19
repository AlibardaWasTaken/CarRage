using UnityEngine;

public class CarHitHandler : MonoBehaviour
{
    [SerializeField] private CarSFXHandler carSfxHandler;

    public CarSFXHandler CarSfxHandler { get => carSfxHandler; }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnHit(collision);
    }
    public virtual void OnHit(Collision2D collision)
    {
        CarSfxHandler.PlayHitSound(collision.relativeVelocity.magnitude);

        
        if (collision.gameObject.TryGetComponent(out IHittable Hitable))
            Hitable.OnHit();
        
    }
}
