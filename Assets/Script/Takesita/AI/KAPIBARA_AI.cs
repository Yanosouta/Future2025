using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class KAPIBARA_AI : MonoBehaviour
{
    // --- AIState用変数
    public enum AIState { Idle, Walking, Eating } // AIState::AIの状態を定義
                                                  // Idle::待機
                                                  // Walking::歩く
                                                  // Eating::食べる
    private AIState currentState;                 // 現在のステート格納用

    // ステートの出現確率
    [Range(0, 100)] public int idleWeight = 50; // Idleの重み
    [Range(0, 100)] public int walkingWeight = 30; // Walkingの重み
    [Range(0, 100)] public int eatingWeight = 20; // Eatingの重み


    public float idleDuration = 2.0f;// Idleステートの待機時間
    public float eatingDuration = 3.0f;// Eatingステートのアニメーション再生時間

    public float  IdleRotationSpeed = 10f;
    private float IdletargetRotation; // 次の目標回転角度
    private float IdlenextRotationTime = 0f; // 次のランダムな方向決定時間

    private NavMeshAgent agent; // NavMeshコンポーネント
    private Animator animator;  // Animatorコンポーネント

    private Vector3 currentDestination;
    public float minRange = 5f; // Walking時の目的地までの最小範囲
    public float maxRange = 10f; // Walking時の目的地までの最大範囲

    [SerializeField] private GameObject destinationMarkerPrefab; // 目印Prefabを指定する
    private GameObject currentMarker; // 現在の目印オブジェクトを保持


    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgentコンポーネントの取得
        animator = GetComponent<Animator>();  // Animatorコンポーネントの取得
        agent.stoppingDistance = 0;
        agent.autoBraking = true;
        agent.updateRotation = false;
        currentState = AIState.Idle; // 初期ステートをIdleに設定

        if (agent == null)
        {
            Debug.LogError("NavMeshAgentがアタッチされていません");
        }
        else if (!agent.enabled)
        {
            Debug.LogError("NavMeshAgentが有効化されていません");
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
        }
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case AIState.Idle:
                    Debug.Log("状態: Idle");
                    yield return new WaitForSeconds(idleDuration);
                    // ランダムに次のステートを決定
                    currentState = GetWeightedRandomState(); // 重み付けランダムで次のステートを決定
                    break;

                case AIState.Walking:
                    Debug.Log("状態: Walking");
                    animator.SetInteger("state", 1); // Walkingアニメーション
                    SetNewDestination(); // 目的地を設定する
                    yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.5f); // 目的地到着まで待機
                    currentState = AIState.Idle; // WalkingからIdleへ遷移
                    break;

                case AIState.Eating:
                    Debug.Log("状態: Eating");
                    // Eatingが終わったらIdleに戻る
                    yield return new WaitForSeconds(eatingDuration);
                    currentState = AIState.Idle; // EatingからIdleへ遷移
                    break;
            }
        }
    }

    public void Idle()
    {
        animator.SetInteger("state", 0); // Idleアニメーション

        // 一定時間ごとにランダムな方向を決める
        if (Time.time > IdlenextRotationTime)
        {
            // 新しいランダムな方向を決定
            IdletargetRotation = Random.Range(-180f, 180f); // ランダムな角度（-180°から180°）
            IdlenextRotationTime = Time.time + Random.Range(1f, 3f); // 次回のランダムな方向決定までの時間を設定（1～3秒ごと）
        }

        // ゆっくりその方向に向かって回転
        float step = IdleRotationSpeed * Time.deltaTime;
        float currentYRotation = transform.eulerAngles.y; // 現在のY軸回転角度
        float newYRotation = Mathf.MoveTowardsAngle(currentYRotation, IdletargetRotation, step); // 徐々に目標角度に向かう

        // 回転を適用
        transform.rotation = Quaternion.Euler(0, newYRotation, 0);
    }
    public void Walking()
    {
        animator.SetInteger("state", 1); // Walkingアニメーションに切り替え
        // 現在の位置から指定範囲内でランダムな目的地を設定
        if (agent.remainingDistance < 0.1f)
        {
            currentDestination = RandomNavSphere(transform.position, Random.Range(minRange, maxRange));
            agent.SetDestination(currentDestination);

            Debug.Log("New Destination Set: " + currentDestination); // デバッグ用
        }

        // 目的地に向かう方向を向く
        Vector3 direction = (currentDestination - transform.position).normalized;
        if (direction.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    public void Eating()
    {
        animator.SetInteger("state", 2); // Eatingアニメーションを再生
    }

    private AIState GetWeightedRandomState()
    {
        // 重みの合計を計算
        int totalWeight = idleWeight + walkingWeight + eatingWeight;
        int randomValue = Random.Range(0, totalWeight);

        // ランダムな値に基づいてステートを決定
        if (randomValue < idleWeight)
        {
            return AIState.Idle;
        }
        else if (randomValue < idleWeight + walkingWeight)
        {
            return AIState.Walking;
        }
        else
        {
            return AIState.Eating;
        }
    }

    private void SetNewDestination()
    {
        // ランダムな目的地を生成
        currentDestination = RandomNavSphere(transform.position, Random.Range(minRange, maxRange));
        agent.SetDestination(currentDestination);
        Debug.Log("New Destination Set: " + currentDestination);

        // 既存のマーカーがある場合は削除
        if (currentMarker != null)
        {
            Destroy(currentMarker);
        }

        // 新しい目的地マーカーを生成して配置
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
}
