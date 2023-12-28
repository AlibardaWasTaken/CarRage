
using System.Collections;
using UnityEngine;

public class TopDownPlayerCar : TopDownCarController, IHittable 
{


    [SerializeField] private ParticleSystem _bloodParticleSystem;
    private bool IsInvincibleTime;




    public ParticleSystem BloodParticleSystem { get => _bloodParticleSystem; }

    public override void DoDamage(int amount)
    {
        BloodManager.Instance.RemoveBlood(amount);
    }

    public void Eat()
    {


        BloodParticleSystem.Play();


    }



    public void Jump(float jumpHeightScale, float jumpPushScale, int carColliderLayerBeforeJump)
    {
        if (!IsJumping)
            StartCoroutine(JumpCo(jumpHeightScale, jumpPushScale, carColliderLayerBeforeJump));
    }



    public void OnHit()
    {
        if (IsInvincibleTime == false)
        {
            IsInvincibleTime = true;
            var randHit = Random.Range(1, 7);
            StartCoroutine(InvicibilityTime(randHit));
        }



   

    }

    private IEnumerator InvicibilityTime(int damage, float time = 0.2f)
    {
        

        BloodManager.Instance.RemoveBlood(Mathf.Clamp(damage - GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.Armor], 1, 99));

        CarSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(time);
        CarSpriteRenderer.color = Color.white;
        IsInvincibleTime = false;
    }


    private IEnumerator JumpCo(float jumpHeightScale, float jumpPushScale, int carColliderLayerBeforeJump)
    {
        IsJumping = true;

        float jumpStartTime = Time.time;
        float jumpDuration = CarRigidbody2D.velocity.magnitude * 0.05f;

        jumpHeightScale = jumpHeightScale * CarRigidbody2D.velocity.magnitude * 0.05f;
        jumpHeightScale = Mathf.Clamp(jumpHeightScale, 0.0f, 1.0f);

        //Change the layer of the car, as we have jumped we are now flying
        CarCollider.gameObject.layer = LayerMask.NameToLayer("ObjectFlying");

        CarSfxHandler.PlayJumpSfx();

        //Change sorting layer to flying
        CarSpriteRenderer.sortingLayerName = "Flying";
        CarShadowRenderer.sortingLayerName = "Flying";

        //Push the object forward as we passed a jump
        CarRigidbody2D.AddForce(10 * jumpPushScale * CarRigidbody2D.velocity.normalized, ForceMode2D.Impulse);

        while (IsJumping)
        {
            //Percentage 0 - 1.0 of where we are in the jumping process
            float jumpCompletedPercentage = (Time.time - jumpStartTime) / jumpDuration;
            jumpCompletedPercentage = Mathf.Clamp01(jumpCompletedPercentage);

            //Take the base scale of 1 and add how much we should increase the scale with. 
            CarSpriteRenderer.transform.localScale = Vector3.one + JumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale * Vector3.one;

            //Change the shadow scale also but make it a bit smaller. In the real world this should be the opposite, the higher an object gets the larger its shadow gets but this looks better in my opinion
            CarShadowRenderer.transform.localScale = CarSpriteRenderer.transform.localScale * 0.75f;

            //Offset the shadow a bit. This is not 100% correct either but works good enough in our game. 
            CarShadowRenderer.transform.localPosition = 3 * JumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale * new Vector3(1, -1, 0.0f);

            //When we reach 100% we are done
            if (jumpCompletedPercentage == 1.0f)
                break;

            yield return null;
        }


        //Handle landing, scale back the object
        CarSpriteRenderer.transform.localScale = Vector3.one;

        //reset the shadows position and scale
        CarShadowRenderer.transform.localPosition = Vector3.zero;
        CarShadowRenderer.transform.localScale = CarSpriteRenderer.transform.localScale;

        //We are safe to land, so enable change the collision layer back to what it was before we jumped
        CarCollider.gameObject.layer = carColliderLayerBeforeJump;


        //Change sorting layer to regular layer
        CarSpriteRenderer.sortingLayerName = "Default";
        CarShadowRenderer.sortingLayerName = "Default";

        //Play the landing particle system if it is a bigger jump
        if (jumpHeightScale > 0.2f)
        {
            LandingParticleSystem.Play();

            CarSfxHandler.PlayLandingSfx();
        }

        //Change state
        IsJumping = false;
        //}
    }



}
