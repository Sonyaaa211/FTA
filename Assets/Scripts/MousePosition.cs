using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Joystick joystick;
    private Rigidbody rb;
    public float speed = 15;

    [SerializeField] Transform objTransform;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        //{
        //    transform.position = raycastHit.point ;
        //}

        if (gameController.isThrowingGrenade)
        {
            if (joystick.joystickVec != Vector2.zero)
            {
                rb.velocity = new Vector3(joystick.joystickVec.x, 0, joystick.joystickVec.y) * speed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
