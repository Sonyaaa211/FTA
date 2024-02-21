using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController gameController;
    public Joystick joystickManagement;
    // Start is called before the first frame update
    void Awake()
    {
        gameController = GameController.Instance.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.isThrowingGrenade && gameController.isGamePlaying)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, joystickManagement.joystickRotation / Mathf.PI * 180, transform.eulerAngles.z);
        }
    }
}
