using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class KAPIBARA_AI : MonoBehaviour
{
    // --- AIState用変数
    public enum AIState { Idle, Walking, Eating,GoroGoro } // AIState::AIの状態を定義
                                                           // Idle::待機
                                                           // Walking::歩く
                                                           // Eating::食べる
                                                           // GoroGoro::ゴロゴロ
    private AIState currentState;                 // 現在のステート格納用

    // ステートの出現確率
    [Range(0, 100)] public int idleWeight = 50; // Idleの重み
    [Range(0, 100)] public int walkingWeight = 30; // Walkingの重み
    //[Range(0, 100)] public int GoroGoroWeight = 30; // GoroGoroの重み
    //[Range(0, 100)] public int eatingWeight = 20; // Eatingの重み


    public float idleDuration = 2.0f;// Idleステートの待機時間
    public float eatingDuration = 3.0f;// Eatingステートのアニメーション再生時間
    public float grogroDuration = 3.0f;// Eatingステートのアニメーション再生時間

    public float  IdleRotationSpeed = 10f;
    private float IdletargetRotation; // 次の目標回転角度
    private float IdlenextRotationTime = 0f; // 次のランダムな方向決定時間
    public float walkingMaxDuration = 5.0f; // Walkingステートの最大持続時間（秒）
    private float walkingTimer = 0.0f; // Walkingステート用のタイマー
    public float normalSpeed = 3.5f;   // 通常時歩行速度
    public float waterSpeed = 1.5f;    // 水中時歩行速度

    public LayerMask waterLayer; // 水のレイヤー

    private NavMeshAgent agent; // NavMeshコンポーネント
    private Animator animator;  // Animatorコンポーネント

    private Vector3 currentDestination;
    public float minRange = 5f; // Walking時の目的地までの最小範囲
    public float maxRange = 10f; // Walking時の目的地までの最大範囲

    [SerializeField] private GameObject destinationMarkerPrefab; // 目印Prefabを指定する
    private GameObject currentMarker; // 現在の目印オブジェクトを保持

    public LayerMask foodLayer;  // エサのレイヤー
    public float detectionRadius = 5f;  // エサを探す半径

    private GameObject currentFood; // 現在食べているエサを保持する変数
    public float eatingDistance = 1.5f; // エサを食べる距離のしきい値
    private float timeToEat = 2.0f; // 食べるまでの遅延時間（秒）
    private float eatTimer = 0.0f; // タイマー

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
                    Debug.Log("状態: Idle");
                    // エサが近くにあるかをチェック
                    if (IsFoodNearby())
                    {
                        Debug.Log("エサが近くにあります。Eating状態に遷移します。");
                        // エサが近くにあればEatingステートに遷移
                        currentState = AIState.Eating;
                    }
                    else
                    {
                        Debug.Log("エサが近くにありません。ランダムに次のステートを選びます。");
                        // エサが近くになければランダムに次のステートを決定
                        yield return new WaitForSeconds(idleDuration);
                        currentState = GetWeightedRandomState();
                    }
                    break;

                case AIState.Walking:
                    Debug.Log("状態: Walking");
                    SetNewDestination(); // 目的地を設定する
                    walkingTimer = 0.0f; // タイマーリセット

                    while (currentState == AIState.Walking)
                    {
                        walkingTimer += Time.deltaTime; // タイマーを進める

                        // 経過時間が制限を超えた場合
                        if (walkingTimer >= walkingMaxDuration)
                        {
                            Debug.Log("Walkingの制限時間を超えたためIdleステートに遷移します。");
                            currentState = AIState.Idle;
                            break;
                        }

                        // 目的地に到着した場合
                        if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        {
                            Debug.Log("目的地に到着したためIdleステートに遷移します。");
                            currentState = AIState.Idle;
                            break;
                        }

                        yield return null; // 次のフレームまで待つ
                    }
                    break;

                case AIState.Eating:
                    Debug.Log("状態: Eating");
                    // Eatingが終わったらIdleに戻る
                    //DestroyNearbyFood();
                    yield return new WaitForSeconds(eatingDuration);
                    currentState = AIState.Idle; // EatingからIdleへ遷移
                    break;

                //case AIState.GoroGoro:
                //    Debug.Log("状態: GoroGoro");
                //    yield return new WaitForSeconds(grogroDuration);
                //    break;
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
        // 水中判定
        if (IsInWater())
        {
            agent.speed = waterSpeed; // 水中時の速度を設定
        }
        else
        {
            agent.speed = normalSpeed; // 通常速度を設定
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

        if (currentFood != null)
        {
            // エサの方向を向く
            Vector3 direction = (currentFood.transform.position - transform.position).normalized;
            if (direction.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // エサに向かって移動する
            float distanceToFood = Vector3.Distance(transform.position, currentFood.transform.position);

            if (distanceToFood > eatingDistance)
            {
                // エサに向かって移動（NavMeshAgentを使用）
                agent.SetDestination(currentFood.transform.position);
            }
            else
            {
                // 食べるタイミングの遅延
                if (eatTimer < timeToEat)
                {
                    eatTimer += Time.deltaTime; // タイマーを進める
                }
                else
                {
                    // タイマーが経過した後に食べる
                    Destroy(currentFood);
                    Debug.Log("エサを食べて削除しました");

                    // エサを食べた後、ターゲットをクリア
                    currentFood = null;

                    // タイマーのリセット
                    eatTimer = 0.0f;
                    // Eatingステートを終了し、Idleステートに戻る
                    currentState = AIState.Idle;
                }
            }
        }
    }
    public void GoroGoro()
    {
        animator.SetInteger("state", 3); // Eatingアニメーションを再生
    
        
    }
    private AIState GetWeightedRandomState()
    {
        // 重みの合計を計算
        int totalWeight = idleWeight + walkingWeight; //+ GoroGoroWeight;
        int randomValue = Random.Range(0, totalWeight);

        // ランダムな値に基づいてステートを決定
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

    private bool IsFoodNearby()
    {
        // 指定された半径内にエサオブジェクトが存在するかをチェック
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, foodLayer);
        Debug.Log($"検出したエサの数: {colliders.Length}");

        // 食べる対象を設定（最初に見つかったエサ）
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
            // 現在ターゲットにしているエサをDestroy
            Destroy(currentFood);
            Debug.Log("エサオブジェクトをDestroyしました");

            // 食べ終わったらターゲットをクリア
            currentFood = null;
        }
    }
    // 水中にいるかを判定するメソッド
    private bool IsInWater()
    {
        // プレイヤーの足元にSphereCastを行い、水レイヤーに触れているか確認
        float sphereRadius = 0.5f; // SphereCastの半径
        Vector3 sphereOrigin = transform.position + Vector3.up * 0.5f; // AIの位置から少し上
        float sphereDistance = 1f; // 判定する距離

        return Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out RaycastHit hit, sphereDistance, waterLayer);
    }
}
