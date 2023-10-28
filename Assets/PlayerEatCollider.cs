using UnityEngine;

public class PlayerEatCollider : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (collider2d.CompareTag("Interactable"))
        {

            collider2d.GetComponent<Iinteractable>().Interact();

        }



    }
}
