using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KapibaraSpawner : MonoBehaviour
{
    public List<GameObject> aiPrefabs; // ��������AI�I�u�W�F�N�g��Prefab���X�g
    public float spawnInterval = 5f;   // ��������Ԋu�i�b�j
    public int maxObjects = 10;       // ��������AI�I�u�W�F�N�g�̍ő吔
    public float nextSpawnTime;       // ���ɐ������鎞��
    private List<GameObject> availablePrefabs; // �c��̐����\��Prefab���X�g
    public Transform parentObject;    // �e�I�u�W�F�N�g���C���X�y�N�^�[����w��

    public float minScale = 0.5f; // �ŏ��X�P�[���i�C���X�y�N�^�[�Őݒ�\�j
    public float maxScale = 2f;  // �ő�X�P�[���i�C���X�y�N�^�[�Őݒ�\�j

    public CameraChange camChange;
    KapiDicData KapiData;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval; // �ŏ��̐������Ԃ�ݒ�
        availablePrefabs = new List<GameObject>(aiPrefabs); // �v���n�u���X�g�𕡐����Ďg�p
        if (camChange == null)
        {
            camChange = FindObjectOfType<CameraChange>(); // CameraChange�X�N���v�g�������Ō�����
        }
    }

    void Update()
    {
        if (KapiData == null)
            KapiData = FindObjectOfType<KapiDicData>();

        if (camChange == null)
        {
            camChange = FindObjectOfType<CameraChange>(); // CameraChange�X�N���v�g�������Ō�����
        }
        // "KAPIBARA"�^�O�̂����I�u�W�F�N�g��maxObjects�ȉ��̏ꍇ�ɐ������s��
        if (Time.time >= nextSpawnTime && GameObject.FindGameObjectsWithTag("KAPIBARA").Length < maxObjects)
        {
            SpawnAIObject();
            nextSpawnTime = Time.time + spawnInterval; // ���̐������Ԃ��X�V
        }
    }

    void SpawnAIObject()
    {
        // �����\��Prefab���Ȃ��ꍇ�͉������Ȃ�
        if (availablePrefabs.Count == 0)
        {
            Debug.Log("No more unique prefabs available to spawn.");
            return;
        }

        Vector3 randomPosition = new Vector3(
                   Random.Range(-32.3f, -35.0f), // X���͈̔�
                   -12.64f,  // Y����0�ɌŒ�
                   Random.Range(10.3f, 12f)  // Z���͈̔�
               );

        // ��������ʒu��n�ʂɃX�i�b�v
        RaycastHit hit;
        if (Physics.Raycast(randomPosition + Vector3.up * 10, Vector3.down, out hit, 100f))
        {
            randomPosition.y = hit.point.y; // �n�ʂ̍����ɃX�i�b�v
        }

        // �����_����Prefab��I�����Đ������A�g�p�ς݂Ƃ��ă��X�g����폜
        int randomIndex = Random.Range(0, availablePrefabs.Count);
        GameObject selectedPrefab = availablePrefabs[randomIndex];
        GameObject spawnedObject = Instantiate(selectedPrefab, randomPosition, Quaternion.identity);

        // �����_���ȃX�P�[�����v�Z���A�S���ɓK�p
        float randomScale = Random.Range(minScale, maxScale);
        spawnedObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        Debug.Log("AI object spawned at: " + randomPosition + " with uniform scale: " + randomScale + " using prefab: " + selectedPrefab.name);

        // ���������I�u�W�F�N�g�̐e��ݒ�
        spawnedObject.transform.SetParent(parentObject);

        // �g�p����Prefab�����X�g����폜
        availablePrefabs.RemoveAt(randomIndex);

        // �������ꂽ�I�u�W�F�N�g���ɃJ���������݂���ꍇ�ACameraChange�ɓo�^
        Camera spawnedCamera = spawnedObject.GetComponentInChildren<Camera>();
        if (spawnedCamera != null)
        {
            camChange.AddCamera(spawnedCamera.gameObject);
        }

        // ���������I�u�W�F�N�g��Id���Q��
        KapiID kapiID = spawnedObject.GetComponent<KapiID>();
        if (kapiID != null)
        {
            Debug.Log("���ۂ���������������������������ID:" + kapiID.KapiId);
        }

        KapiData.MarkAsKapi(kapiID.KapiId);
    }
}
