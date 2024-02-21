using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateUIItem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject item;
    public Transform parent;
    float posX=0, posY=0;
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            var obj = Instantiate(item, parent);
            obj.GetComponent<RectTransform>().rect.Set(posX, posY,100 ,150 );
            posY += 150;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
