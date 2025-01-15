using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class KAPIBARA_AI : MonoBehaviour
{
    // --- AIState�p�ϐ�
    public enum AIState { Idle, Walking, Eating,GoroGoro } // AIState::AI�̏�Ԃ��`
                                                           // Idle::�ҋ@
                                                           // Walking::����
                                                           // Eating::�H�ׂ�
                                                           // GoroGoro::�S���S��
    private AIState currentState;                 // ���݂̃X�e�[�g�i�[�p

    // �X�e�[�g�̏o���m��
    [Range(0, 100)] public int idleWeight = 50; // Idle�̏d��
    [Range(0, 100)] public int walkingWeight = 30; // Walking�̏d��
    //[Range(0, 100)] public int GoroGoroWeight = 30; // GoroGoro�̏d��
    //[Range(0, 100)] public int eatingWeight = 20; // Eating�̏d��


    public float idleDuration = 2.0f;// Idle�X�e�[�g�̑ҋ@����
    public float eatingDuration = 3.0f;// Eating�X�e�[�g�̃A�j���[�V�����Đ�����
    public float grogroDuration = 3.0f;// Eating�X�e�[�g�̃A�j���[�V�����Đ�����

    public float  IdleRotationSpeed = 10f;
    private float IdletargetRotation; // ���̖ڕW��]�p�x
    private float IdlenextRotationTime = 0f; // ���̃����_���ȕ������莞��
    public float walkingMaxDuration = 5.0f; // Walking�X�e�[�g�̍ő厝�����ԁi�b�j
    private float walkingTimer = 0.0f; // Walking�X�e�[�g�p�̃^�C�}�[
    public float normalSpeed = 3.5f;   // �ʏ펞���s���x
    public float waterSpeed = 1.5f;    // ���������s���x

    public LayerMask waterLayer; // ���̃��C���[

    private NavMeshAgent agent; // NavMesh�R���|�[�l���g
    private Animator animator;  // Animator�R���|�[�l���g

    private Vector3 currentDestination;
    public float minRange = 5f; // Walking���̖ړI�n�܂ł̍ŏ��͈�
    public float maxRange = 10f; // Walking���̖ړI�n�܂ł̍ő�͈�

    [SerializeField] private GameObject destinationMarkerPrefab; // �ڈ�Prefab���w�肷��
    private GameObject currentMarker; // ���݂̖ڈ�I�u�W�F�N�g��ێ�

    public LayerMask foodLayer;  // �G�T�̃��C���[
    public float detectionRadius = 5f;  // �G�T��T�����a

    private GameObject currentFood; // ���ݐH�ׂĂ���G�T��ێ�����ϐ�
    public float eatingDistance = 1.5f; // �G�T��H�ׂ鋗���̂������l
    private float timeToEat = 2.0f; // �H�ׂ�܂ł̒x�����ԁi�b�j
    private float eatTimer = 0.0f; // �^�C�}�[

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent�R���|�[�l���g�̎擾
        animator = GetComponent<Animator>();  // Animator�R���|�[�l���g�̎擾
        agent.stoppingDistance = 0;
        agent.autoBraking = true;
        agent.updateRotation = false;
        currentState = AIState.Idle; // �����X�e�[�g��Idle�ɐݒ�

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent���A�^�b�`����Ă��܂���");
        }
        else if (!agent.enabled)
        {
            Debug.LogError("NavMeshAgent���L��������Ă��܂���");
        }
        StartCoroutine(StateMachine());
    }
    void Update()
    {
        switch (currentState)
        {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Walking:
                Walking();
                break;
            case AIState.Eating:
                Eating();
                break;
            //case AIState.GoroGoro:
            //    GoroGoro();
            //    break;
        }
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case AIState.Idle:
                    Debug.Log("���: Idle");
                    // �G�T���߂��ɂ��邩���`�F�b�N
                    if (IsFoodNearby())
                    {
                        Debug.Log("�G�T���߂��ɂ���܂��BEating��ԂɑJ�ڂ��܂��B");
                        // �G�T���߂��ɂ����Eating�X�e�[�g�ɑJ��
                        currentState = AIState.Eating;
                    }
                    else
                    {
                        Debug.Log("�G�T���߂��ɂ���܂���B�����_���Ɏ��̃X�e�[�g��I�т܂��B");
                        // �G�T���߂��ɂȂ���΃����_���Ɏ��̃X�e�[�g������
                        yield return new WaitForSeconds(idleDuration);
                        currentState = GetWeightedRandomState();
                    }
                    break;

                case AIState.Walking:
                    Debug.Log("���: Walking");
                    SetNewDestination(); // �ړI�n��ݒ肷��
                    walkingTimer = 0.0f; // �^�C�}�[���Z�b�g

                    while (currentState == AIState.Walking)
                    {
                        walkingTimer += Time.deltaTime; // �^�C�}�[��i�߂�

                        // �o�ߎ��Ԃ������𒴂����ꍇ
                        if (walkingTimer >= walkingMaxDuration)
                        {
                            Debug.Log("Walking�̐������Ԃ𒴂�������Idle�X�e�[�g�ɑJ�ڂ��܂��B");
                            currentState = AIState.Idle;
                            break;
                        }

                        // �ړI�n�ɓ��������ꍇ
                        if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        {
                            Debug.Log("�ړI�n�ɓ�����������Idle�X�e�[�g�ɑJ�ڂ��܂��B");
                            currentState = AIState.Idle;
                            break;
                        }

                        yield return null; // ���̃t���[���܂ő҂�
                    }
                    break;

                case AIState.Eating:
                    Debug.Log("���: Eating");
                    // Eating���I�������Idle�ɖ߂�
                    //DestroyNearbyFood();
                    yield return new WaitForSeconds(eatingDuration);
                    currentState = AIState.Idle; // Eating����Idle�֑J��
                    break;

                //case AIState.GoroGoro:
                //    Debug.Log("���: GoroGoro");
                //    yield return new WaitForSeconds(grogroDuration);
                //    break;
            }
        }
    }

    public void Idle()
    {
        animator.SetInteger("state", 0); // Idle�A�j���[�V����

        // ��莞�Ԃ��ƂɃ����_���ȕ��������߂�
        if (Time.time > IdlenextRotationTime)
        {
            // �V���������_���ȕ���������
            IdletargetRotation = Random.Range(-180f, 180f); // �����_���Ȋp�x�i-180������180���j
            IdlenextRotationTime = Time.time + Random.Range(1f, 3f); // ����̃����_���ȕ�������܂ł̎��Ԃ�ݒ�i1�`3�b���Ɓj
        }

        // ������肻�̕����Ɍ������ĉ�]
        float step = IdleRotationSpeed * Time.deltaTime;
        float currentYRotation = transform.eulerAngles.y; // ���݂�Y����]�p�x
        float newYRotation = Mathf.MoveTowardsAngle(currentYRotation, IdletargetRotation, step); // ���X�ɖڕW�p�x�Ɍ�����

        // ��]��K�p
        transform.rotation = Quaternion.Euler(0, newYRotation, 0);
    }
    public void Walking()
    {
        animator.SetInteger("state", 1); // Walking�A�j���[�V�����ɐ؂�ւ�
        // ���݂̈ʒu����w��͈͓��Ń����_���ȖړI�n��ݒ�
        if (agent.remainingDistance < 0.1f)
        {
            currentDestination = RandomNavSphere(transform.position, Random.Range(minRange, maxRange));
            agent.SetDestination(currentDestination);

            Debug.Log("New Destination Set: " + currentDestination); // �f�o�b�O�p
        }
        // ��������
        if (IsInWater())
        {
            agent.speed = waterSpeed; // �������̑��x��ݒ�
        }
        else
        {
            agent.speed = normalSpeed; // �ʏ푬�x��ݒ�
        }
        // �ړI�n�Ɍ���������������
        Vector3 direction = (currentDestination - transform.position).normalized;
        if (direction.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    public void Eating()
    {
        animator.SetInteger("state", 2); // Eating�A�j���[�V�������Đ�

        if (currentFood != null)
        {
            // �G�T�̕���������
            Vector3 direction = (currentFood.transform.position - transform.position).normalized;
            if (direction.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // �G�T�Ɍ������Ĉړ�����
            float distanceToFood = Vector3.Distance(transform.position, currentFood.transform.position);

            if (distanceToFood > eatingDistance)
            {
                // �G�T�Ɍ������Ĉړ��iNavMeshAgent���g�p�j
                agent.SetDestination(currentFood.transform.position);
            }
            else
            {
                // �H�ׂ�^�C�~���O�̒x��
                if (eatTimer < timeToEat)
                {
                    eatTimer += Time.deltaTime; // �^�C�}�[��i�߂�
                }
                else
                {
                    // �^�C�}�[���o�߂�����ɐH�ׂ�
                    Destroy(currentFood);
                    Debug.Log("�G�T��H�ׂč폜���܂���");

                    // �G�T��H�ׂ���A�^�[�Q�b�g���N���A
                    currentFood = null;

                    // �^�C�}�[�̃��Z�b�g
                    eatTimer = 0.0f;
                    // Eating�X�e�[�g���I�����AIdle�X�e�[�g�ɖ߂�
                    currentState = AIState.Idle;
                }
            }
        }
    }
    public void GoroGoro()
    {
        animator.SetInteger("state", 3); // Eating�A�j���[�V�������Đ�
    
        
    }
    private AIState GetWeightedRandomState()
    {
        // �d�݂̍��v���v�Z
        int totalWeight = idleWeight + walkingWeight; //+ GoroGoroWeight;
        int randomValue = Random.Range(0, totalWeight);

        // �����_���Ȓl�Ɋ�Â��ăX�e�[�g������
        if (randomValue < idleWeight)
        {
            return AIState.Idle;
        }
        else //if (randomValue < idleWeight + walkingWeight)
        {
            return AIState.Walking;
        }
        //else
        //{
        //    return AIState.GoroGoro;
        //}
    }

    private void SetNewDestination()
    {
        // �����_���ȖړI�n�𐶐�
        currentDestination = RandomNavSphere(transform.position, Random.Range(minRange, maxRange));
        agent.SetDestination(currentDestination);
        Debug.Log("New Destination Set: " + currentDestination);

        // �����̃}�[�J�[������ꍇ�͍폜
        if (currentMarker != null)
        {
            Destroy(currentMarker);
        }

        // �V�����ړI�n�}�[�J�[�𐶐����Ĕz�u
        currentMarker = Instantiate(destinationMarkerPrefab, currentDestination, Quaternion.identity);
    }

    public Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);

        return navHit.position;
    }

    private bool IsFoodNearby()
    {
        // �w�肳�ꂽ���a���ɃG�T�I�u�W�F�N�g�����݂��邩���`�F�b�N
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, foodLayer);
        Debug.Log($"���o�����G�T�̐�: {colliders.Length}");

        // �H�ׂ�Ώۂ�ݒ�i�ŏ��Ɍ��������G�T�j
        if (colliders.Length > 0 && currentFood == null)
        {
            currentFood = colliders[0].gameObject;
        }

        return colliders.Length > 0;
    }


    private void DestroyNearbyFood()
    {
        if (currentFood != null)
        {
            // ���݃^�[�Q�b�g�ɂ��Ă���G�T��Destroy
            Destroy(currentFood);
            Debug.Log("�G�T�I�u�W�F�N�g��Destroy���܂���");

            // �H�׏I�������^�[�Q�b�g���N���A
            currentFood = null;
        }
    }
    // �����ɂ��邩�𔻒肷�郁�\�b�h
    private bool IsInWater()
    {
        // �v���C���[�̑�����SphereCast���s���A�����C���[�ɐG��Ă��邩�m�F
        float sphereRadius = 0.5f; // SphereCast�̔��a
        Vector3 sphereOrigin = transform.position + Vector3.up * 0.5f; // AI�̈ʒu���班����
        float sphereDistance = 1f; // ���肷�鋗��

        return Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out RaycastHit hit, sphereDistance, waterLayer);
    }
}
