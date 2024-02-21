using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider timerSlider;
    public float sliderTimer;
    public bool stopTimer = false;
    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
    }

    public void StartTimer(float max)
    {
        timerSlider.value = 0;
        timerSlider.maxValue = max;
        sliderTimer = 0;
        stopTimer = false;
        StartCoroutine(StartTheTimerTicker());
    }

    IEnumerator StartTheTimerTicker()
    {
        while(stopTimer == false)
        {
            if (!gameController.isGamePlaying) yield return null;
            sliderTimer += Time.deltaTime;
            yield return new WaitForSeconds(0.001f);

            if (sliderTimer > 5)
            {
                stopTimer = true;
            }

            if (stopTimer == false) 
            { 
                timerSlider.value = sliderTimer; 
            
            }
        }
    }

    public void StopTimer()
    {
        stopTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
