using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerState : MonoBehaviour
{
    ControllerBase m_controller;
    ControllerBase.ControllerButton m_button;

    struct KeyButton
    {
        public bool ButtonA;
        public bool ButtonB;
        public bool ButtonX;
        public bool ButtonY;
        public bool ButtonL;
        public bool ButtonR;
        public bool ButtonLeft;
        public bool ButtonRight;
        public bool ButtonUp;
        public bool ButtonDown;
        public bool ButtonStick;
        public bool ButtonDoNot;
    
    }
    KeyButton m_Key;




    // Start is called before the first frame update
    void Start()
    {
        m_controller = GetComponent<ControllerBase>();
        //ç\ë¢ëÃÇÃèâä˙âª
        InitKeyButton();

        
    }

    // Update is called once per frame
    void Update()
    {
        m_button = m_controller.GetButton();

        switch (m_button)
        {
            case ControllerBase.ControllerButton.Button_A:
                TrueA();
                break;
            case ControllerBase.ControllerButton.Button_B:
                TrueB();
                break;
            case ControllerBase.ControllerButton.Button_X:
                TrueX();
                break;
            case ControllerBase.ControllerButton.Button_Y:
                TrueY();
                break;
            case ControllerBase.ControllerButton.Button_L:
                TrueL();
                break;
            case ControllerBase.ControllerButton.Button_R:
                TrueR();
                break;
            case ControllerBase.ControllerButton.Button_Left:
                TrueLeft();
                break;
            case ControllerBase.ControllerButton.Button_Right:
                TrueRight();
                break;
            case ControllerBase.ControllerButton.Button_Up:
                TrueUp();
                break;
            case ControllerBase.ControllerButton.Button_Down:
                TrueDown();
                break;
            case ControllerBase.ControllerButton.Stick:
                TrueStick();
                break;
            case ControllerBase.ControllerButton.DoNot:
                TrueDoNot();
                break;
        }
        
    }
    public bool GetButtonA()
    {
        return m_Key.ButtonA;
    }
    public bool GetButtonB()
    {
        return m_Key.ButtonB;
    }
    public bool GetButtonX() 
    {
        return m_Key.ButtonX;
    }
    public bool GetButtonY() 
    {
        return m_Key.ButtonY;
    }
    public bool GetButtonL() 
    { 
        return m_Key.ButtonL;
    }
    public bool GetButtonR() 
    { 
        return m_Key.ButtonR;
    }
    public bool GetButtonLeft() 
    {
        return m_Key.ButtonLeft;
    }
    public bool GetButtonRight()
    {
        return m_Key.ButtonRight;
    }
    public bool GetButtonUp()
    {
        return m_Key.ButtonUp;
    }
    public bool GetButtonDown()
    {
        return m_Key.ButtonDown;
    }
    public bool GetButtonStick()
    {
        return m_Key.ButtonStick;
    }
    public bool GetButtonDoNot()
    {
        return m_Key.ButtonDoNot;
    }
    private void InitKeyButton()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueA()
    {
        m_Key.ButtonA = true;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueB()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = true;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueX()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = true;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueY()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = true;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueL()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = true;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueR()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = true;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueLeft()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = true;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueRight()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = true;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueUp()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = true;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueDown()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = true;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = false;
    }
    private void TrueStick()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = true;
        m_Key.ButtonDoNot = false;
    }
    private void TrueDoNot()
    {
        m_Key.ButtonA = false;
        m_Key.ButtonB = false;
        m_Key.ButtonX = false;
        m_Key.ButtonY = false;
        m_Key.ButtonL = false;
        m_Key.ButtonR = false;
        m_Key.ButtonLeft = false;
        m_Key.ButtonRight = false;
        m_Key.ButtonUp = false;
        m_Key.ButtonDown = false;
        m_Key.ButtonStick = false;
        m_Key.ButtonDoNot = true;
    }
}
