using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIOpen : MonoBehaviour
{
    public List<GameObject> objsToClose = new List<GameObject>();
    public List<GameObject> objsToOpen = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenGUI()
    {
        for(int i = 0; i < objsToClose.Count; i++)
        {
            objsToClose[i].SetActive(false);
        }
        for(int i = 0;i < objsToOpen.Count; i++)
        {
            objsToOpen[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
