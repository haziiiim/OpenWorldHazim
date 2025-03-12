using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerMovement : MonoBehaviour
{
    public float jumpup;
    public float speed;
    private Rigidbody rb;
    private PlayerInput playerInput;
    PlayerInputActions playerInputActions;
    
    public void Awake()
    {


        rb = GetComponent<Rigidbody>();
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += jump_performed;
        playerInputActions.Player.Move.performed += Move_performed;

    }

    private void Move_performed(InputAction.CallbackContext context)
    {
      Debug.Log(context.performed);
        Vector2 inputVector = context.ReadValue<Vector2>();
        rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);

    }

    //private void Jump_performed(InputAction.CallbackContext context)
    //{
    //    Debug.Log(context);
    //    //        if (context.performed)
    //    //        {

    //    //            //jumpp = jumpup;
    //    //            rb.AddForce(Vector3.up * jumpup, ForceMode.Impulse);

    //    //            Debug.Log("jump" + context.phase);
    //    //        }


    //}

    public void jump_performed(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        



        //jumpp = jumpup;
        rb.AddForce(Vector3.up * jumpup, ForceMode.Impulse);

        Debug.Log("jump" + context.phase);
    }
    



    //    }
    ////    // Start is called before the first frame update
    //    void Start()
    //    {

    //    }


}
