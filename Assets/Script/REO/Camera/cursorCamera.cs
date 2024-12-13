using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class cursorCamera : MonoBehaviour
{
    ControllerState m_State;
    ControllerBase m_Stick;
    MCamera m_Camera;

    public GameObject cursor; // カーソルとして使用するUIオブジェクト
    public float cursorSpeed = 100f; // カーソルの移動速度
    public GameObject mainCamera; // メインカメラ
    public float cameraDistance = 2f; // オブジェクトの目の前の距離

    private bool isCursorVisible = false;
    private bool isCursorEnabled = true; // カーソルの操作を有効にするかどうかのフラグ
    private Vector2 cursorPosition;
    private GameObject selectedObject = null; // 選択されたオブジェクト

    private GameObject ParentObj;
    void Start()
    {
        cursor.SetActive(false); // ゲーム開始時はカーソルを非表示に
        cursorPosition = new Vector2(Screen.width / 2, Screen.height / 2); // カーソルの初期位置を画面中央に設定
                                                                           // cursorが設定されているか確認
        m_Stick = GetComponent<ControllerBase>();

        ParentObj = mainCamera.transform.root.gameObject;
        m_Camera = ParentObj.GetComponent<MCamera>();
        m_State = ParentObj.GetComponent<ControllerState>();
    }

    void Update()
    {
        if (mainCamera == null)
        {
            Debug.LogError("mainCamera が設定されていません！");
        }

        if (cursor == null)
        {
            Debug.LogError("cursor が設定されていません！");
        }

        // ゲームパッドのBボタンが押されたらカーソルの表示・非表示を切り替える
        if (Gamepad.current != null && m_State.GetButtonB() || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isCursorVisible = !isCursorVisible;
            cursor.SetActive(isCursorVisible); 
            isCursorEnabled = true; 
        }

        // カーソルが表示されている場合、左スティックで移動
        if (isCursorEnabled && isCursorVisible)
        {
            Vector2 leftStickInput = Vector2.zero;

            if (Gamepad.current != null)
            {
                leftStickInput = m_Stick.GetStick();
            }

            // キーボードのWASD入力
            if (Keyboard.current != null)
            {
                if (m_State.GetButtonUp())
                    leftStickInput.y += 1;
                if (m_State.GetButtonDown())
                    leftStickInput.y -= 1;
                if (m_State.GetButtonLeft())
                    leftStickInput.x -= 1;
                if (m_State.GetButtonRight())
                    leftStickInput.x += 1;
            }

            MoveCursor(leftStickInput);

            // ここでカーソルの位置に基づいてレイを更新
            SelectObjectUnderCursor();

            // AボタンまたはEnterキーが押された場合、カメラを選択されたオブジェクトの前に移動
            if (selectedObject != null && (isCursorEnabled && Gamepad.current != null && m_State.GetButtonA() || Keyboard.current.enterKey.wasPressedThisFrame))  
            {
                MoveCameraToSelectedObject();
                DisableCursorControl(); // カーソル操作を無効にする
            }
        }
    }

    void MoveCursor(Vector2 leftStickInput)
    {
        // 左スティックの入力に応じてカーソルを移動
        cursorPosition += leftStickInput * cursorSpeed * Time.deltaTime;

        // 画面の境界内にカーソルを制限
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0, Screen.width);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0, Screen.height);

        // カーソルの位置をUIオブジェクトに反映
        cursor.transform.position = cursorPosition;
    }

    void SelectObjectUnderCursor()
    {
        // カーソル位置からレイを飛ばしてオブジェクトを選択
        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
        ray.origin += Vector3.up * 0.8f; // レイの発射位置を上に調整

        Vector3 boxHalfExtents = new Vector3(1.0f, 1.0f, 1.0f); // BoxCastの半径
        RaycastHit hit;

        // デバッグ描画
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);

        if (Physics.BoxCast(ray.origin, boxHalfExtents, ray.direction, out hit, Quaternion.identity, 10000f))
        {
            // ヒットしたコライダーがカプセルコライダーかどうか確認
            if (hit.collider is CapsuleCollider)
            {
                if (hit.collider.gameObject.CompareTag("KAPIBARA"))
                {
                    selectedObject = hit.collider.gameObject;
                    Debug.Log("KAPIBARAタグのカプセルコライダーを選択しました: " + selectedObject.name);
                }
                else
                {
                    selectedObject = null;
                    Debug.Log("カプセルコライダーだがKAPIBARAタグがありません: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                selectedObject = null;
                Debug.Log("カプセルコライダーではありません: " + hit.collider.gameObject.name);
            }
        }
        else
        {
            selectedObject = null;
            Debug.Log("何も選択されていません");
        }
    }



    void MoveCameraToSelectedObject()
    {
        // selectedObject が null でなく、「KAPIBARA」タグが付いたオブジェクトの場合のみ移動
        if (selectedObject != null && selectedObject.CompareTag("KAPIBARA"))
        {
            m_Camera.GetCursorCamera(selectedObject);




            StartCoroutine(MoveCameraCoroutine());
        }
    }

    IEnumerator MoveCameraCoroutine()
    {
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 direction = (mainCamera.transform.position - selectedObject.transform.position).normalized;
        Vector3 targetPosition = selectedObject.transform.position + direction * cameraDistance;

        float elapsedTime = 0f;
        float duration = 5f; // 5秒間かけて移動

        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            mainCamera.transform.LookAt(selectedObject.transform.position); // オブジェクトを注視
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition; // 最終位置を確実に設定

        selectedObject = null;
    }

    void DisableCursorControl()
    {
        // カーソルを非表示にして操作を無効化
        isCursorVisible = false;
        cursor.SetActive(false);
        isCursorEnabled = false; // カーソルの操作を無効にする
    }

    //void OnDrawGizmos()
    //{
    //    if (Camera.main != null)
    //    {
    //        // カーソル位置からレイを飛ばして描画
    //        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
    //
    //        // レイの始点と向きに基づいてGizmoのラインを描画
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawRay(ray.origin, ray.direction * 100f);
    //    }
    //}
}
