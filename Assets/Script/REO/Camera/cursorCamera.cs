using UnityEngine;
using UnityEngine.InputSystem;

public class CursorCameraController : MonoBehaviour
{
    ControllerState m_State;
    ControllerBase m_Stick;
    MCamera m_Camera;

    public GameObject cursor; // カーソルとして使用するUIオブジェクト
    public float cursorSpeed = 100f; // カーソルの移動速度
    public GameObject mainCamera; // メインカメラ
    public float cameraDistance = 2f; // オブジェクトの目の前の距離
    private GameObject selectedObject = null; // 選択されたオブジェクト
    private bool wasBButtonPressed = false; // 前フレームのBボタンの状態を記録

    private bool isCursorVisible = false;
    private Vector2 cursorPosition;

    private GameObject ParentObj;

    void Start()
    {
        // カーソルを画面中央に配置
        cursorPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        if (cursor != null)
        {
            cursor.transform.position = cursorPosition;
            cursor.SetActive(false); // 初期状態で非表示
        }
        else
        {
            Debug.LogError("Cursor が設定されていません！");
        }
        ParentObj = mainCamera.transform.root.gameObject;
        m_Camera = ParentObj.GetComponent<MCamera>();
        m_State = ParentObj.GetComponent<ControllerState>();

    }

    void Update()
    {
        // ゲームパッドが接続されているか確認
        if (Gamepad.current == null)
        {
            Debug.LogWarning("ゲームパッドが接続されていません！");
            return;
        }

        bool isBButtonPressed = m_State.GetButtonB(); // Bボタンの現在の状態

        // Bボタンでカーソルの表示・非表示を切り替え
        if (isBButtonPressed && !wasBButtonPressed) // Bボタン
        {
            isCursorVisible = !isCursorVisible;
            cursor.SetActive(isCursorVisible);
        }

        wasBButtonPressed = isBButtonPressed; // Bボタンの状態を更新

        // カーソルが表示されている場合、左スティックで移動
        if (isCursorVisible)
        {
            Vector2 dpadInput = Gamepad.current.dpad.ReadValue();
            MoveCursor(dpadInput);
            // ここでカーソルの位置に基づいてレイを更新
            SelectObjectUnderCursor();
            // AボタンまたはEnterキーが押された場合、カメラを選択されたオブジェクトの前に移動
            if (selectedObject != null && m_State.GetButtonA())
            {
                MoveCameraToSelectedObject();
                DisableCursorControl(); // カーソル操作を無効にする
            }
        }
    }

    void MoveCursor(Vector2 dpadInput)
    {
        // 左スティックの入力に応じてカーソルを移動
        cursorPosition += dpadInput * cursorSpeed * Time.deltaTime;

        // 画面の境界内にカーソルを制限
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0, Screen.width);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0, Screen.height);

        // カーソルの位置をUIオブジェクトに反映
        cursor.transform.position = cursorPosition;
    }

    void SelectObjectUnderCursor()
    {
        if (mainCamera == null)
        {
            Debug.LogError("mainCamera が設定されていません！");
            return;
        }

        // カーソル位置をワールド座標に変換
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, cameraDistance));

        // カメラ位置とカーソル位置から方向ベクトルを計算
        Vector3 direction = (cursorWorldPosition - Camera.main.transform.position).normalized;

        // BoxCastの中心位置（レイの出現位置を左に1.0f、上に1.0fずらす）
        Vector3 boxOrigin = Camera.main.transform.position + Camera.main.transform.right * -0.8f + Camera.main.transform.up * 1.0f;

        // BoxCastの半径（ボックスサイズの半分）
        Vector3 boxHalfExtents = new Vector3(1.0f, 1.0f, 1.0f);

        // BoxCastの結果を取得
        RaycastHit hit;
        bool isHit = Physics.BoxCast(boxOrigin, boxHalfExtents, direction, out hit, Quaternion.identity, 100f);

        // デバッグ描画
        Debug.DrawRay(boxOrigin, direction * 100f, isHit ? Color.red : Color.green);

        if (isHit)
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
        if (selectedObject != null && selectedObject.CompareTag("KAPIBARA"))
        {
            // MCameraのGetCursorCameraを使用してターゲットを更新
            m_Camera.GetCursorCamera(selectedObject);

            // カメラを即座に移動
            Vector3 targetPosition = selectedObject.transform.position - (selectedObject.transform.forward * cameraDistance);
            mainCamera.transform.position = targetPosition;
            mainCamera.transform.LookAt(selectedObject.transform.position);

            // カーソルを非表示にして操作を無効化
            DisableCursorControl();

            selectedObject = null; // 選択解除
        }
    }

    void DisableCursorControl()
    {
        // カーソルを非表示にして操作を無効化
        isCursorVisible = false;
        cursor.SetActive(false);
    }
}

//using UnityEngine;
//using UnityEngine.InputSystem;
//using System.Collections;

//public class cursorCamera : MonoBehaviour
//{
//    ControllerState m_State;
//    ControllerBase m_Stick;
//    MCamera m_Camera;

//    public GameObject cursor; // カーソルとして使用するUIオブジェクト
//    public float cursorSpeed = 100f; // カーソルの移動速度
//    public GameObject mainCamera; // メインカメラ
//    public float cameraDistance = 2f; // オブジェクトの目の前の距離

//    private bool isCursorVisible = false;
//    private bool isCursorEnabled = true; // カーソルの操作を有効にするかどうかのフラグ
//    private Vector2 cursorPosition;
//    private GameObject selectedObject = null; // 選択されたオブジェクト

//    private GameObject ParentObj;
//    void Start()
//    {
//        cursor.SetActive(false); // ゲーム開始時はカーソルを非表示に
//        cursorPosition = new Vector2(Screen.width / 2, Screen.height / 2); // カーソルの初期位置を画面中央に設定
//                                                                           // cursorが設定されているか確認
//        m_Stick = GetComponent<ControllerBase>();

//        ParentObj = mainCamera.transform.root.gameObject;
//        m_Camera = ParentObj.GetComponent<MCamera>();
//        m_State = ParentObj.GetComponent<ControllerState>();
//    }

//    void Update()
//    {
//        if (mainCamera == null)
//        {
//            Debug.LogError("mainCamera が設定されていません！");
//        }

//        if (cursor == null)
//        {
//            Debug.LogError("cursor が設定されていません！");
//        }

//        // ゲームパッドのBボタンが押されたらカーソルの表示・非表示を切り替える
//        if (m_State.GetButtonB())//|| Keyboard.current.spaceKey.wasPressedThisFrame)
//        {
//            isCursorVisible = !isCursorVisible;
//            cursor.SetActive(isCursorVisible);
//            isCursorEnabled = true;
//        }

//        // カーソルが表示されている場合、左スティックで移動
//        if (isCursorEnabled && isCursorVisible)
//        {
//            Vector2 leftStickInput = Vector2.zero;

//            if (Gamepad.current != null)
//            {
//                leftStickInput = m_Stick.GetStick();
//            }

//            // キーボードのWASD入力
//            if (Keyboard.current != null)
//            {
//                if (m_State.GetButtonUp())
//                    leftStickInput.y += 1;
//                if (m_State.GetButtonDown())
//                    leftStickInput.y -= 1;
//                if (m_State.GetButtonLeft())
//                    leftStickInput.x -= 1;
//                if (m_State.GetButtonRight())
//                    leftStickInput.x += 1;
//            }

//            MoveCursor(leftStickInput);

//            // ここでカーソルの位置に基づいてレイを更新
//            SelectObjectUnderCursor();

//            // AボタンまたはEnterキーが押された場合、カメラを選択されたオブジェクトの前に移動
//            if (selectedObject != null && (isCursorEnabled && Gamepad.current != null && m_State.GetButtonA() || Keyboard.current.enterKey.wasPressedThisFrame))
//            {
//                MoveCameraToSelectedObject();
//                DisableCursorControl(); // カーソル操作を無効にする
//            }
//        }
//    }

//    void MoveCursor(Vector2 leftStickInput)
//    {
//        // 左スティックの入力に応じてカーソルを移動
//        cursorPosition += leftStickInput * cursorSpeed * Time.deltaTime;

//        // 画面の境界内にカーソルを制限
//        cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0, Screen.width);
//        cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0, Screen.height);

//        // カーソルの位置をUIオブジェクトに反映
//        cursor.transform.position = cursorPosition;
//    }


//    void MoveCameraToSelectedObject()
//    {
//        if (selectedObject != null && selectedObject.CompareTag("KAPIBARA"))
//        {
//            // MCameraのGetCursorCameraを使用してターゲットを更新
//            m_Camera.GetCursorCamera(selectedObject);

//            // カメラを即座に移動
//            Vector3 targetPosition = selectedObject.transform.position - (selectedObject.transform.forward * cameraDistance);
//            mainCamera.transform.position = targetPosition;
//            mainCamera.transform.LookAt(selectedObject.transform.position);

//            // カーソルを非表示にして操作を無効化
//            DisableCursorControl();

//            selectedObject = null; // 選択解除
//        }
//    }


//    //IEnumerator MoveCameraCoroutine()
//    //{
//    //    Vector3 startPosition = mainCamera.transform.position;
//    //    Vector3 direction = (mainCamera.transform.position - selectedObject.transform.position).normalized;
//    //    Vector3 targetPosition = selectedObject.transform.position + direction * cameraDistance;
//    //
//    //    float elapsedTime = 0f;
//    //    float duration = 5f; // 5秒間かけて移動
//    //
//    //    //while (elapsedTime < duration)
//    //    //{
//    //    //    mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
//    //    //    mainCamera.transform.LookAt(selectedObject.transform.position); // オブジェクトを注視
//    //    //    elapsedTime += Time.deltaTime;
//    //    //    yield return null;
//    //    //}
//    //    Quaternion startRotation = mainCamera.transform.rotation;
//    //    Quaternion targetRotation = Quaternion.LookRotation(selectedObject.transform.position - mainCamera.transform.position);
//    //
//    //    while (elapsedTime < duration)
//    //    {
//    //        mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
//    //        mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
//    //        elapsedTime += Time.deltaTime;
//    //        yield return null;
//    //    }
//    //    mainCamera.transform.position = targetPosition;
//    //    mainCamera.transform.rotation = targetRotation;
//    //
//    //    //mainCamera.transform.position = targetPosition; // 最終位置を確実に設定
//    //
//    //    selectedObject = null;
//    //}

//    void DisableCursorControl()
//    {
//        // カーソルを非表示にして操作を無効化
//        isCursorVisible = false;
//        cursor.SetActive(false);
//        isCursorEnabled = false; // カーソルの操作を無効にする
//    }
//}
