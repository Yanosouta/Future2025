using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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

    //一度だけ実行フラグ
    private bool m_OneFlg;
    //メニューフラグ
    // 前フレームで遮蔽物として扱われていたゲームオブジェクトを格納。
    public GameObject[] m_PrevRaycast;
    public List<GameObject> m_RaycastHitsList = new List<GameObject>();
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
        m_Rotiton = new Vector2(-34.0f,60.0f);
        m_ChildCount = m_TargetObj.transform.childCount;
        m_CurrentTarget = m_TargetList[0].transform;
        m_NextTarget = m_TargetList[0].transform;
        m_NoRotitonFlg = true;
        m_OneFlg = true;
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
                    if (m_Count >= m_TargetList.Count)
                        m_Count = 0;
                    //TargetOneAdd();
                }
                if (m_State.GetButtonL())
                {
                    m_Count--;
                    //-1になったらカメラの数に補正
                    if (m_Count <= -1)
                        m_Count = m_TargetList.Count - 1;
                    //TargetOneAdd();
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

        //障害物を透明にする
        ObjectGlasschange();

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
            //m_TargetList.Clear();
            //foreach (Transform chlid in m_TargetObj.transform)
            //{
            //    m_TargetList.Add(chlid.gameObject);
            //}

            //カメラの位置をターゲットに移動
            if (m_LeapFlg)
            {
                m_TargetPos = m_NextTarget.transform.localPosition
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
    //カメラとターゲットの間のオブジェクトを透明にする
    void ObjectGlasschange()
    {
        //オブジェクト間のベクトルを得る
        Vector3 difference = (m_TargetList[m_Count].transform.position - m_MainCamera.transform.position);
        //normalizedベクトルの正規化を行う
        Vector3 direction = difference.normalized;
        //Ray(開始地点、進む方向)
        //Ray ray = new Ray(m_MainCamera.transform.position, difference);
        //RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        
        // BoxCast の幅、高さ、奥行きを設定
        Vector3 boxHalfExtents = new Vector3(0.5f, 0.5f, 0.5f); // 半径を設定
        // BoxCast を実行
        RaycastHit[] boxCastHits = Physics.BoxCastAll(
        m_MainCamera.transform.position,  // BoxCast の開始位置
        boxHalfExtents,                   // Box の半径
        direction,                        // Box の進行方向
        Quaternion.identity,              // Box の回転 (今回はデフォルト)
        difference.magnitude              // BoxCast の距離
        );

        Debug.DrawRay(m_MainCamera.transform.position, difference, Color.white, 1.0f, false);

        //前フレームで障害物であった全てのGameObjectを保持
        m_PrevRaycast = m_RaycastHitsList.ToArray();
        m_RaycastHitsList.Clear();


        foreach(RaycastHit hit in boxCastHits)
        {
            
            if(hit.collider.CompareTag("stage"))
            {
                GlassMaterial glassMaterial = hit.collider.GetComponent<GlassMaterial>();
                if (glassMaterial != null)
                {
                    glassMaterial.GlassMaterialInvoke();
                }
                //次のフレームで使いたいため、不透明にしたオブジェクトを追加する
                m_RaycastHitsList.Add(hit.collider.gameObject);
            }
            
        }
        foreach(GameObject gameObject in m_PrevRaycast.Except<GameObject>(m_RaycastHitsList))
        {
            if (gameObject != null)
            {
                GlassMaterial noglassMaterial = gameObject.GetComponent<GlassMaterial>();
                //障害物でなくなったGameObjectを不透明に戻す
                if (noglassMaterial != null)
                {
                    noglassMaterial.NotGlassMaterialInvoke();
                }
            }
        }
    }
    void InitRotation()
    {
        if(m_OneFlg)
        {
            m_MainCamera.transform.rotation = new Quaternion(0.0f, -41.925f,0.0f,0.0f);
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


