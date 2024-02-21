using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;
    private float Dif;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        Dif = -12;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(playerTransform.position.x/2, transform.position.y, playerTransform.position.z + Dif);
    }
}
