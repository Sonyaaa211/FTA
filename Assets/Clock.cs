using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public TMP_Text text;
    private int clockCount = 0;
    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeClock", 0, 1);
        if(gameController == null) gameController = GameController.Instance.GetComponent<GameController>();
    }

    private void ChangeClock()
    {
        if (!gameController.isGamePlaying) return;
        int sec = clockCount % 60;
        int min = clockCount++ / 60;
        text.text = $"{min}:{sec}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
