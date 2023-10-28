using UnityEngine;

public class CarInputHandler : MonoBehaviour
{

    public bool isUIInput = false;

    public static CarInputHandler Instance;

    Vector2 inputVector = Vector2.zero;

    //Components
    public TopDownPlayerCar topDownCarController;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        topDownCarController = GetComponent<TopDownPlayerCar>();


    }


    // Update is called once per frame and is frame dependent
    void Update()
    {
        if (isUIInput)
        {

        }
        else
        {
            //    inputVector = Vector2.zero;

            //            //Get input from Unity's input system.


                        inputVector.x = Input.GetAxis("Horizontal");
                        inputVector.y = Input.GetAxis("Vertical");


            //}
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.position.x < Screen.width / 2)
                {
                    Debug.Log("Left click");
                    inputVector.x = -1;
                }
                else if (touch.position.x > Screen.width / 2)
                {
                    Debug.Log("Right click");
                    inputVector.x = 1;
                }
            }



            inputVector.y = 1;
        }
        topDownCarController.SetInputVector(inputVector);
        inputVector.x = 0;
    }

    public void SetInput(Vector2 newInput)
    {
        inputVector = newInput;
    }
}
