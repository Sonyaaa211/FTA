using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;

public class CountdownText : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public float time;
    private float timeRemain;
    public float timeStep = 0.1f;
    // Start is called before the first frame update
    void Awake()
    {

        InvokeRepeating("StartCountdown", 0, timeStep);
    }

    private void OnEnable()
    {
        timeRemain = time;
    }

    public void StartCountdown()
    {
        if(timeRemain > 0)
        {
            text.text = (Mathf.Round(timeRemain*10)/10).ToString();
            timeRemain -= timeStep;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
