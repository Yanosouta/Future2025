

//制作者　矢野
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerBase : MonoBehaviour
{
    //コントローラー構造体
    public enum ControllerButton
    { 
        Button_A,
        Button_B, 
        Button_X,
        Button_Y,
        Button_Menu,
        Button_L,
        Button_R,
        Stick,
        Button_Up,
        Button_Down,
        Button_Left,
        Button_Right,
        DoNot,
    }
    private Vector2 Stick_Left;        //スティックの値を格納
    //変数
    private ControllerButton m_button = ControllerButton.DoNot;
    private void Start()
    {
        //初期化
        Stick_Left = Vector2.zero;
    }
    private void Update()
    {
    }
    //=======================================
    //Zキー、Enterキーを押した時に呼び出す
    //Aボタンを押した時に呼び出す
    //=======================================
    public void Button_A(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_A);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
        
        Debug.Log("Zキー、Enterキー,Aボタン");
    }
    //=======================================
    //Xキー、BackSpaceキーを押した時に呼び出す
    //Bボタンを押した時に呼び出す
    //=======================================
    public void Button_B(InputAction.CallbackContext context)
    {
        
       
        Debug.Log("Xキー、BackSpaceキー,Bボタン");
    }
    //=======================================
    //Cキーを押した時に呼び出す
    //Xボタンを押した時に呼び出す
    //=======================================
    public void Button_X(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_X);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
        
        Debug.Log("Cキー,Xボタン");
    }
    //=======================================
    //Vキーを押した時に呼び出す
    //Yボタンを押した時に呼び出す
    //=======================================
    public void Button_Y(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_Y);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
        
        Debug.Log("Vキー、Yボタン");
    }
    //=======================================
    //Escキーを押した時に呼び出す
    //メニューボタンを押した時に呼び出す
    //=======================================
    public void Button_Menu(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_Menu);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
        
        Debug.Log("Escキー、,メニューボタン");
    }
    //=======================================
    //Qキーを押した時に呼び出す
    //Lボタンを押した時に呼び出す
    //=======================================
    public void Button_L(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_L);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
        
        Debug.Log("Qキー、,Lボタン");
    }
    //=======================================
    //Eキーを押した時に呼び出す
    //Rボタンを押した時に呼び出す
    //=======================================
    public void Button_R(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_R);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
        
        Debug.Log("Eキー、Rボタン");
    }
    //=======================================
    //矢印キーを押した時に呼び出す
    //左スティックを動かしたときに呼び出す
    //=======================================
    public void Stick(InputAction.CallbackContext context)
    {
        //長押しの実装
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Stick);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
        //移動アクションの入力取得
        Stick_Left = context.ReadValue<Vector2>();
        //Debug.Log("矢印,左スティック");

    }
    //=======================================
    //Wキーを押した時に呼び出す
    //上ボタンを押した時に呼び出す
    //=======================================
    public void Button_Up(InputAction.CallbackContext context)
    {
      
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_Up);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
        Debug.Log("Wキー、上ボタン");
    }
    //=======================================
    //Sキーを押した時に呼び出す
    //下ボタンを押した時に呼び出す
    //=======================================
    public void Button_Down(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_Down);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
       

        Debug.Log("Sキー、下ボタン");
    }
    //=======================================
    //Aキーを押した時に呼び出す
    //左ボタンを押した時に呼び出す
    //=======================================
    public void Button_Left(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_Left);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
        Debug.Log("Aキー、左ボタン");
    }
    //=======================================
    //Dキーを押した時に呼び出す
    //右ボタンを押した時に呼び出す
    //=======================================
    public void Button_Right(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //ボタンが押された
                ButtonChange(ControllerButton.Button_Right);
                break;
            case InputActionPhase.Canceled:
                //ボタンが離された
                ButtonChange(ControllerButton.DoNot);
                break;
        }
       
        Debug.Log("Dキー、右ボタン");
    }

    public void ButtonChange(ControllerButton newbotton)
    {
        m_button = newbotton;
    }
    public Vector2 GetStick()
    {
        return Stick_Left; 
    }
    public ControllerButton GetButton()
    {
        return m_button;
    }
}
