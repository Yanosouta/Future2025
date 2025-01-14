using UnityEngine;
using UnityEngine.InputSystem;

public class CursorCameraController : MonoBehaviour
{
    ControllerState m_State;
    ControllerBase m_Stick;
    MCamera m_Camera;

    public GameObject cursor; // �J�[�\���Ƃ��Ďg�p����UI�I�u�W�F�N�g
    public float cursorSpeed = 100f; // �J�[�\���̈ړ����x
    public GameObject mainCamera; // ���C���J����
    public float cameraDistance = 2f; // �I�u�W�F�N�g�̖ڂ̑O�̋���
    private GameObject selectedObject = null; // �I�����ꂽ�I�u�W�F�N�g
    private bool wasBButtonPressed = false; // �O�t���[����B�{�^���̏�Ԃ��L�^

    private bool isCursorVisible = false;
    private Vector2 cursorPosition;

    private GameObject ParentObj;

    void Start()
    {
        // �J�[�\������ʒ����ɔz�u
        cursorPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        if (cursor != null)
        {
            cursor.transform.position = cursorPosition;
            cursor.SetActive(false); // ������ԂŔ�\��
        }
        else
        {
            Debug.LogError("Cursor ���ݒ肳��Ă��܂���I");
        }
        ParentObj = mainCamera.transform.root.gameObject;
        m_Camera = ParentObj.GetComponent<MCamera>();
        m_State = ParentObj.GetComponent<ControllerState>();

    }

    void Update()
    {
        // �Q�[���p�b�h���ڑ�����Ă��邩�m�F
        if (Gamepad.current == null)
        {
            Debug.LogWarning("�Q�[���p�b�h���ڑ�����Ă��܂���I");
            return;
        }

        bool isBButtonPressed = m_State.GetButtonB(); // B�{�^���̌��݂̏��

        // B�{�^���ŃJ�[�\���̕\���E��\����؂�ւ�
        if (isBButtonPressed && !wasBButtonPressed) // B�{�^��
        {
            isCursorVisible = !isCursorVisible;
            cursor.SetActive(isCursorVisible);
        }

        wasBButtonPressed = isBButtonPressed; // B�{�^���̏�Ԃ��X�V

        // �J�[�\�����\������Ă���ꍇ�A���X�e�B�b�N�ňړ�
        if (isCursorVisible)
        {
            Vector2 dpadInput = Gamepad.current.dpad.ReadValue();
            MoveCursor(dpadInput);
            // �����ŃJ�[�\���̈ʒu�Ɋ�Â��ă��C���X�V
            SelectObjectUnderCursor();
            // A�{�^���܂���Enter�L�[�������ꂽ�ꍇ�A�J������I�����ꂽ�I�u�W�F�N�g�̑O�Ɉړ�
            if (selectedObject != null && m_State.GetButtonA())
            {
                MoveCameraToSelectedObject();
                DisableCursorControl(); // �J�[�\������𖳌��ɂ���
            }
        }
    }

    void MoveCursor(Vector2 dpadInput)
    {
        // ���X�e�B�b�N�̓��͂ɉ����ăJ�[�\�����ړ�
        cursorPosition += dpadInput * cursorSpeed * Time.deltaTime;

        // ��ʂ̋��E���ɃJ�[�\���𐧌�
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0, Screen.width);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0, Screen.height);

        // �J�[�\���̈ʒu��UI�I�u�W�F�N�g�ɔ��f
        cursor.transform.position = cursorPosition;
    }

    void SelectObjectUnderCursor()
    {
        if (mainCamera == null)
        {
            Debug.LogError("mainCamera ���ݒ肳��Ă��܂���I");
            return;
        }

        // �J�[�\���ʒu�����[���h���W�ɕϊ�
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, cameraDistance));

        // �J�����ʒu�ƃJ�[�\���ʒu��������x�N�g�����v�Z
        Vector3 direction = (cursorWorldPosition - Camera.main.transform.position).normalized;

        // BoxCast�̒��S�ʒu�i���C�̏o���ʒu������1.0f�A���1.0f���炷�j
        Vector3 boxOrigin = Camera.main.transform.position + Camera.main.transform.right * -0.8f + Camera.main.transform.up * 1.0f;

        // BoxCast�̔��a�i�{�b�N�X�T�C�Y�̔����j
        Vector3 boxHalfExtents = new Vector3(1.0f, 1.0f, 1.0f);

        // BoxCast�̌��ʂ��擾
        RaycastHit hit;
        bool isHit = Physics.BoxCast(boxOrigin, boxHalfExtents, direction, out hit, Quaternion.identity, 100f);

        // �f�o�b�O�`��
        Debug.DrawRay(boxOrigin, direction * 100f, isHit ? Color.red : Color.green);

        if (isHit)
        {
            // �q�b�g�����R���C�_�[���J�v�Z���R���C�_�[���ǂ����m�F 
            if (hit.collider is CapsuleCollider)
            {
                if (hit.collider.gameObject.CompareTag("KAPIBARA"))
                {
                    selectedObject = hit.collider.gameObject;
                    Debug.Log("KAPIBARA�^�O�̃J�v�Z���R���C�_�[��I�����܂���: " + selectedObject.name);
                }
                else
                {
                    selectedObject = null;
                    Debug.Log("�J�v�Z���R���C�_�[����KAPIBARA�^�O������܂���: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                selectedObject = null;
                Debug.Log("�J�v�Z���R���C�_�[�ł͂���܂���: " + hit.collider.gameObject.name);
            }
        }
        else
        {
            selectedObject = null;
            Debug.Log("�����I������Ă��܂���");
        }
    }

    void MoveCameraToSelectedObject()
    {
        if (selectedObject != null && selectedObject.CompareTag("KAPIBARA"))
        {
            // MCamera��GetCursorCamera���g�p���ă^�[�Q�b�g���X�V
            m_Camera.GetCursorCamera(selectedObject);

            // �J�����𑦍��Ɉړ�
            Vector3 targetPosition = selectedObject.transform.position - (selectedObject.transform.forward * cameraDistance);
            mainCamera.transform.position = targetPosition;
            mainCamera.transform.LookAt(selectedObject.transform.position);

            // �J�[�\�����\���ɂ��đ���𖳌���
            DisableCursorControl();

            selectedObject = null; // �I������
        }
    }

    void DisableCursorControl()
    {
        // �J�[�\�����\���ɂ��đ���𖳌���
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

//    public GameObject cursor; // �J�[�\���Ƃ��Ďg�p����UI�I�u�W�F�N�g
//    public float cursorSpeed = 100f; // �J�[�\���̈ړ����x
//    public GameObject mainCamera; // ���C���J����
//    public float cameraDistance = 2f; // �I�u�W�F�N�g�̖ڂ̑O�̋���

//    private bool isCursorVisible = false;
//    private bool isCursorEnabled = true; // �J�[�\���̑����L���ɂ��邩�ǂ����̃t���O
//    private Vector2 cursorPosition;
//    private GameObject selectedObject = null; // �I�����ꂽ�I�u�W�F�N�g

//    private GameObject ParentObj;
//    void Start()
//    {
//        cursor.SetActive(false); // �Q�[���J�n���̓J�[�\�����\����
//        cursorPosition = new Vector2(Screen.width / 2, Screen.height / 2); // �J�[�\���̏����ʒu����ʒ����ɐݒ�
//                                                                           // cursor���ݒ肳��Ă��邩�m�F
//        m_Stick = GetComponent<ControllerBase>();

//        ParentObj = mainCamera.transform.root.gameObject;
//        m_Camera = ParentObj.GetComponent<MCamera>();
//        m_State = ParentObj.GetComponent<ControllerState>();
//    }

//    void Update()
//    {
//        if (mainCamera == null)
//        {
//            Debug.LogError("mainCamera ���ݒ肳��Ă��܂���I");
//        }

//        if (cursor == null)
//        {
//            Debug.LogError("cursor ���ݒ肳��Ă��܂���I");
//        }

//        // �Q�[���p�b�h��B�{�^���������ꂽ��J�[�\���̕\���E��\����؂�ւ���
//        if (m_State.GetButtonB())//|| Keyboard.current.spaceKey.wasPressedThisFrame)
//        {
//            isCursorVisible = !isCursorVisible;
//            cursor.SetActive(isCursorVisible);
//            isCursorEnabled = true;
//        }

//        // �J�[�\�����\������Ă���ꍇ�A���X�e�B�b�N�ňړ�
//        if (isCursorEnabled && isCursorVisible)
//        {
//            Vector2 leftStickInput = Vector2.zero;

//            if (Gamepad.current != null)
//            {
//                leftStickInput = m_Stick.GetStick();
//            }

//            // �L�[�{�[�h��WASD����
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

//            // �����ŃJ�[�\���̈ʒu�Ɋ�Â��ă��C���X�V
//            SelectObjectUnderCursor();

//            // A�{�^���܂���Enter�L�[�������ꂽ�ꍇ�A�J������I�����ꂽ�I�u�W�F�N�g�̑O�Ɉړ�
//            if (selectedObject != null && (isCursorEnabled && Gamepad.current != null && m_State.GetButtonA() || Keyboard.current.enterKey.wasPressedThisFrame))
//            {
//                MoveCameraToSelectedObject();
//                DisableCursorControl(); // �J�[�\������𖳌��ɂ���
//            }
//        }
//    }

//    void MoveCursor(Vector2 leftStickInput)
//    {
//        // ���X�e�B�b�N�̓��͂ɉ����ăJ�[�\�����ړ�
//        cursorPosition += leftStickInput * cursorSpeed * Time.deltaTime;

//        // ��ʂ̋��E���ɃJ�[�\���𐧌�
//        cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0, Screen.width);
//        cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0, Screen.height);

//        // �J�[�\���̈ʒu��UI�I�u�W�F�N�g�ɔ��f
//        cursor.transform.position = cursorPosition;
//    }


//    void MoveCameraToSelectedObject()
//    {
//        if (selectedObject != null && selectedObject.CompareTag("KAPIBARA"))
//        {
//            // MCamera��GetCursorCamera���g�p���ă^�[�Q�b�g���X�V
//            m_Camera.GetCursorCamera(selectedObject);

//            // �J�����𑦍��Ɉړ�
//            Vector3 targetPosition = selectedObject.transform.position - (selectedObject.transform.forward * cameraDistance);
//            mainCamera.transform.position = targetPosition;
//            mainCamera.transform.LookAt(selectedObject.transform.position);

//            // �J�[�\�����\���ɂ��đ���𖳌���
//            DisableCursorControl();

//            selectedObject = null; // �I������
//        }
//    }


//    //IEnumerator MoveCameraCoroutine()
//    //{
//    //    Vector3 startPosition = mainCamera.transform.position;
//    //    Vector3 direction = (mainCamera.transform.position - selectedObject.transform.position).normalized;
//    //    Vector3 targetPosition = selectedObject.transform.position + direction * cameraDistance;
//    //
//    //    float elapsedTime = 0f;
//    //    float duration = 5f; // 5�b�Ԃ����Ĉړ�
//    //
//    //    //while (elapsedTime < duration)
//    //    //{
//    //    //    mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
//    //    //    mainCamera.transform.LookAt(selectedObject.transform.position); // �I�u�W�F�N�g�𒍎�
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
//    //    //mainCamera.transform.position = targetPosition; // �ŏI�ʒu���m���ɐݒ�
//    //
//    //    selectedObject = null;
//    //}

//    void DisableCursorControl()
//    {
//        // �J�[�\�����\���ɂ��đ���𖳌���
//        isCursorVisible = false;
//        cursor.SetActive(false);
//        isCursorEnabled = false; // �J�[�\���̑���𖳌��ɂ���
//    }
//}
