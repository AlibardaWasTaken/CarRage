using UnityEngine;

public class JumpData : MonoBehaviour, Iinteractable
{
    [Header("Jump info")]
    //The scale effects how long the cars will stay airborn when they hit a jump
    public float jumpHeightScale = 1.0f;

    //How much should the cars be pushed forward when they hit the jump
    public float jumpPushScale = 1.0f;

    public void Interact()
    {
        CarInputHandler.Instance.topDownCarController.Jump(jumpHeightScale, jumpPushScale, CarInputHandler.Instance.topDownCarController.gameObject.layer);
    }
}
