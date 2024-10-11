using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject[] Camera;
    int camNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            camNum++;
            if(camNum >= Camera.Length)
            {
                camNum = 0;
            }
            for (int i = 0; i < Camera.Length; i++)
            {
                Camera[i].SetActive(false);
            }
            Camera[camNum].SetActive(true);
        }      
    }
}
