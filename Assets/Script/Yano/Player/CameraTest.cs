using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{

    ControllerState m_State;
    // Start is called before the first frame update
    void Start()
    {
        m_State = GetComponent<ControllerState>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_State.GetButtonA())
        {
            Debug.Log("AÉ{É^ÉìâüÇµÇΩÇÊÅ[");
        }
    }
}
