using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MCamera : MonoBehaviour
{
    [Header("カメラ"), SerializeField]
    GameObject m_CameraObj;
    [Header("ターゲット"), SerializeField]
    GameObject m_TargetObj;
    [Header("回転の速さ"), SerializeField]
    private float m_Speed = 1;
    [Header("ズームインズームアウトの速さ"), SerializeField]
    private float m_ZoomSpeed = 2;
    [Header("ターゲットとカメラの距離"), SerializeField]
    private float m_Distance = 2;
    [Header("ターゲットとカメラのズームの近さ"), SerializeField]
    private float m_NearDistance = 1;
    [Header("ターゲットとカメラのズームの遠さ"), SerializeField]
    private float m_FarDistance = 5;

    [Header("ターゲットまで移動する速さ"), SerializeField]
    private float m_TargetSpeed = 0.5f;

    ControllerState m_State;
    ControllerBase m_controller;
    ControllerBase.ControllerButton m_button;

    Transform m_MainCamera;

    static public int m_Camera = 0;       //カメラの番号を格納
    static public int m_Count = 0;
    //補正用
    private float m_Time = 0.0f;
    private float m_Delay = 0.1f;
    //リスト
    private List<GameObject> m_CameraList;
    //ターゲット
    private List<GameObject> m_TargetList;
    //カメラの初期値を格納
    private List<GameObject> m_CameraStorageList;
    //子オブジェクトの数を格納
    private int m_ChildCount;
    //カメラの切り替えを長押しで行わないためのフラグ
    private bool m_CameraFlg = true;
    //カメラの回転
    private Vector2 m_Rotiton;
    //回転させないフラグ
    private bool m_NoRotitonFlg;
    // 現在カメラが向いているターゲット
    private Transform m_CurrentTarget;
    //次に向くターゲット
    private Transform m_NextTarget;
    //ターゲットの座標
    private Vector3 m_TargetPos;
    private bool m_LeapFlg = true;
    //スティックの情報を格納
    Vector2 m_Stick;
    void Start()
    {
        m_State = GetComponent<ControllerState>();
        m_controller = GetComponent<ControllerBase>();
        //リスト初期化
        m_CameraList = new List<GameObject>();
        m_TargetList = new List<GameObject>();
        m_CameraStorageList = new List<GameObject>();
        //子オブジェクトの情報をリストに格納
        foreach (Transform chlid in m_CameraObj.transform)
        {
            m_CameraList.Add(chlid.gameObject);
            m_CameraStorageList.Add(chlid.gameObject);
        }
        foreach (Transform chlid in m_TargetObj.transform)
        {
            m_TargetList.Add(chlid.gameObject); 
        }

        m_MainCamera  = m_CameraList[0].transform.GetChild(0);

        m_ChildCount = m_TargetObj.transform.childCount;
        m_CurrentTarget = m_TargetList[0].transform;
        m_NextTarget = m_TargetList[0].transform;
        m_NoRotitonFlg = true;
    }
    void Update()
    {
        if (m_CameraList == null)
            return;
        m_button = m_controller.GetButton();
        m_Stick = m_controller.GetStick();
        //カピバラが増えたら実行(重くなったら別の方法を考える)
        if(m_TargetObj.transform.childCount > m_ChildCount)
        {
            m_TargetList.Clear();
            foreach (Transform chlid in m_TargetObj.transform)
            {
                m_TargetList.Add(chlid.gameObject);
            }
            m_ChildCount = m_TargetObj.transform.childCount;
        }

        if (m_Delay <= m_Time && m_CameraFlg)
        {
            //移動中なら操作を受け付けない
            if (m_LeapFlg)
            {
                if (m_State.GetButtonR())
                {
                    m_Count++;
                    //カメラの数を超えたら０に補正
                    if (m_Count >= m_CameraList.Count)
                        m_Count = 0;
                    //CameraReSet();
                }
                if (m_State.GetButtonL())
                {
                    m_Count--;
                    //-1になったらカメラの数に補正
                    if (m_Count <= -1)
                        m_Count = m_CameraList.Count - 1;
                    //CameraReSet();
                }
            }
            //次のターゲットを入れる
            m_NextTarget = m_TargetList[m_Count].transform;
            m_Time = 0.0f;
            m_CameraFlg = false;
        }
        //ボタンがDoNotに戻ったらフラグをtrueにする
        if (m_button == ControllerBase.ControllerButton.DoNot)
            m_CameraFlg = true;

       

        //ターゲットまでカメラを移動
        TargetSwitch();

        //カメラを回転させる
        CameraRotation();
        //ズームインズームアウト
        TargetScaling();
        //カメラに切り替え
        //CameraSwitch();
        m_Time += Time.deltaTime;
        Debug.Log(m_Count);
    }
    private void CameraSwitch()
    {
        for (int i = 0; i < m_CameraList.Count; i++)
        {
            m_CameraList[i].SetActive(false);
        }
        m_CameraList[m_Count].SetActive(true);
    }
    private void TargetScaling()
    {
        //ズームイン
        if(m_State.GetButtonX() && m_Distance >= m_NearDistance)
        {
            m_Distance -= m_ZoomSpeed * Time.deltaTime;
        }
        //ズームアウト
        if(m_State.GetButtonY() && m_Distance <= m_FarDistance)
        {
            m_Distance += m_ZoomSpeed * Time.deltaTime;
        }
    }
    private void TargetSwitch()
    {
        //ターゲット切り替え時にスムーズに移動する
        if(m_CurrentTarget != m_NextTarget)
        {
            m_TargetList.Clear();
            foreach (Transform chlid in m_TargetObj.transform)
            {
                m_TargetList.Add(chlid.gameObject);
            }

            //カメラの位置をターゲットに移動
            if (m_LeapFlg)
            {
                m_TargetPos = m_NextTarget.transform.position
                - (Quaternion.Euler(m_Rotiton.x, m_Rotiton.y, 0.0f)
                * Vector3.forward
                /** m_Distance*/);
                m_LeapFlg = false;
            }

            //Vector3 direction = (m_TargetPos - m_CameraList[0].transform.position).normalized;
            //m_CameraList[0].transform.position += direction * m_Speed * Time.deltaTime;

            m_CameraList[0].transform.position = 
                Vector3.MoveTowards(m_CameraList[0].transform.position,
                m_TargetPos, m_TargetSpeed * Time.deltaTime);

            
            //ターゲットに近づいたらターゲットif文をぬける
            if (Vector3.Distance(m_CameraList[0].transform.position, m_TargetPos) <=  0.1f)
            {
                m_CurrentTarget = m_NextTarget;
                m_LeapFlg = true;
            }
        }

    }
    void CameraRotation()
    {
        //移動中なら操作を受け付けない
        if (!m_LeapFlg)
            return;

        if (m_Stick.magnitude > 0.1f)
            Debug.Log("動いてる");
        if(m_NoRotitonFlg)
        {
            m_Rotiton.x += m_Stick.y * m_Speed * Time.deltaTime;
            m_Rotiton.y += m_Stick.x * m_Speed * Time.deltaTime;
        }

        //下限上限を設ける
        m_Rotiton.x = Mathf.Clamp(m_Rotiton.x, -75.0f, 75.0f);
        Quaternion rotationQuaternion = Quaternion.Euler(-m_Rotiton.x, -m_Rotiton.y, 0.0f);
       
        Debug.Log(m_Rotiton);
        m_MainCamera.transform.rotation = rotationQuaternion;

        Vector3 Pos = m_TargetList[m_Count].transform.position -(rotationQuaternion * Vector3.forward * m_Distance);
        //カメラをターゲットに向ける
        m_MainCamera.transform.position = Pos;
        m_MainCamera.transform.LookAt(m_TargetList[m_Count].transform);
    }
    void CameraReSet()
    {
        //カメラの回転情報を初期化
        for (int i = 0; i < m_CameraList.Count; i++)
        {
            m_CameraList[i].gameObject.transform.rotation = m_CameraStorageList[i].gameObject.transform.rotation;
        }
    }

    public void GetCursorCamera(GameObject gameObject)
    {
        for(int i = 0; i < m_TargetList.Count;i++)
        {
            if (m_TargetList[i].gameObject == gameObject)
            {
                m_Count = i;
                //次のターゲットを入れる
                m_NextTarget = m_TargetList[m_Count].transform;
                m_Time = 0.0f;
                m_CameraFlg = false;
            }
            
        }


    }
}


