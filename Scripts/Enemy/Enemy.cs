using UnityEngine;
using System.Collections;
using Calc;
using Data;

/// <summary>
/// 敵キャラクターを管理するクラス
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 敵の種類
    /// </summary>
    [SerializeField] int enemyClassNumber;

    GameManager gameManager;
    SEManager SEManager;
    EnemyMethod enemyMethod;

    Rigidbody2D myRigidBody;
    Animator myAnime;
    SpriteRenderer mySprite;

    int enemyHP;
    float walkSpeed;
    float walkTime;
    float walkStopTime;
    float attackRange;
    float attackInterval;
    int attackNumber;
    int bulletCreateNumber;
    float bulletInterval;
    float bulletSpeed;
    int bulletDamage;
    float bulletScale;
    float bulletShakeAngle;
    float bulletCreateTime;
    int isWalkRandom;
    float isAttackAndMove;
    float isAttackAndStop;

    Vector3 playerPos;
    Vector3 myPos;

    float keyRight = 0;
    float keyUp = 0;

    bool isMoving = false;
    bool dead = false;

    Coroutine move;

    AudioClip enemyDownSE;
    AudioSource audioSource;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SEManager = gameManager.SEManager;
        enemyMethod = gameManager.enemyMethod;
        audioSource = gameManager.audioSourceSE;

        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myAnime = gameObject.GetComponent<Animator>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();

        float[] enemyData = EnemyData.SetEnemyData(enemyClassNumber);
        enemyHP = (int)enemyData[0];
        walkSpeed = enemyData[1];
        walkTime = enemyData[2];
        walkStopTime = enemyData[3];
        attackRange = enemyData[4];
        attackInterval = enemyData[5];
        attackNumber = (int)enemyData[6];
        bulletCreateNumber = (int)enemyData[7];
        bulletInterval = enemyData[8];
        bulletSpeed = enemyData[9];
        bulletDamage = (int)enemyData[10];
        bulletScale = enemyData[11];
        bulletShakeAngle = enemyData[12];
        bulletCreateTime = enemyData[13];
        isWalkRandom = (int)enemyData[14];
        isAttackAndMove = enemyData[15];
        isAttackAndStop = enemyData[16];

        //敵は1～1.5秒の間に動き出し、動き出しから0～1.5秒の間に攻撃を開始
        float enemyMoveStartTime = Random.Range(10, 16) * 0.1f;
        float enemyShootStartTime = enemyMoveStartTime + Random.Range(0, 16) * 0.1f;

        myAnime.SetBool("Idle", true);
        Invoke("CallEnemyWalk", enemyMoveStartTime);
        Invoke("CallEnemyAttack", enemyShootStartTime);

        enemyDownSE = SEManager.SE_EnemyDown;
    }

    void Update()
    {
        playerPos = gameManager.playerPos;
        myPos = gameObject.transform.position;

        //移動
        myRigidBody.velocity = new Vector2(walkSpeed * keyRight, walkSpeed * keyUp);

        //プレイヤーの方向によって敵（自分）の向きを変える
        //弾を撃たない突進キャラは移動中に向きを変えない
        if (isMoving && bulletCreateNumber == 0) { return; }

        enemyMethod.SpriteFlip(1, mySprite, myPos);
    }


    /// <summary>
    /// 敵が移動するコルーチンを呼び出すメソッド
    /// </summary>
    void CallEnemyWalk()
    {
        if (!gameObject.activeSelf) { return; } //非アクティブになったら呼ばないようにする
        if (isAttackAndMove == 1) { return; } //攻撃中に必ず動くキャラは、ここで動くコルーチンを作動しない
        move = StartCoroutine(EnemyWalk(walkTime, walkStopTime));
    }

    /// <summary>
    /// 敵を一定時間同じ方向に移動させ、一定時間停止させるコルーチン
    /// </summary>
    /// <param name="walkTime">移動する時間</param>
    /// <param name="WalkStopTime">停止する時間</param>
    IEnumerator EnemyWalk(float walkTime, float WalkStopTime)
    {
        if (walkTime == 0) { yield break; }

        //移動（walkTime秒）→停止（walkStopTime秒)→移動（walkTime秒）→停止（walkStopTime秒）→・・・
        while (true)
        {
            EnemyMoveStart();

            yield return new WaitForSeconds(walkTime);

            isMoving = false;

            yield return null;

            if (WalkStopTime < 0.1) { continue; }

            EnemyMoveStop();

            yield return new WaitForSeconds(WalkStopTime);
        }
    }


    /// <summary>
    /// 敵が移動する方向を決めて移動するメソッド、移動方向はkey配列の中で決める
    /// </summary>
    void EnemyMoveStart()
    {
        enemyMethod.EnemyMoveStart(ref keyRight, ref keyUp, myPos, isWalkRandom);
        myAnime.SetBool("Idle", false);
        myAnime.SetBool("Walk", true);
        isMoving = true;
    }


    /// <summary>
    /// 移動を停止させるメソッド
    /// </summary>
    void EnemyMoveStop()
    {
        keyRight = 0;
        keyUp = 0;
        myAnime.SetBool("Walk", false);
        myAnime.SetBool("Idle", true);
    }


    /// <summary>
    /// 敵が攻撃するコルーチンを呼び出すメソッド
    /// </summary>
    void CallEnemyAttack()
    {
        if (!gameObject.activeSelf) { return; } //非アクティブになったら呼ばないようにする
        StartCoroutine(EnemyAttack());
    }

    /// <summary>
    /// 敵を、一定時間攻撃させ、一定時間攻撃を停止するコルーチン
    /// </summary>
    IEnumerator EnemyAttack()
    {
        if (bulletCreateNumber == 0) { yield break; }

        while (true)
        {
            //プレイヤーがattackRange内にいるなら攻撃開始
            if (Calculate.Distance(myPos, playerPos) < attackRange)
            {
                myAnime.SetTrigger("Attack");
                
                //攻撃中に必ず移動するキャラはここで動くコルーチンを作動
                if (isAttackAndMove == 1)
                {
                    move = StartCoroutine(EnemyWalk(walkTime, walkStopTime));
                }
                //攻撃中に必ず止まるキャラはここで動くコルーチンを停止
                if (isAttackAndStop == 1)
                {
                    StopCoroutine(move);
                    EnemyMoveStop();
                }

                //アニメーションと攻撃開始のタイミングを合わせるため、bulletCreateTime秒停止してから攻撃
                yield return new WaitForSeconds(bulletCreateTime);

                //bulletInterval秒の間隔をあけ、attackNumberの数だけ連続で攻撃
                for (int i = 0; i < attackNumber; i++)
                {
                    enemyMethod.CreateBullet(myPos, bulletCreateNumber, bulletSpeed, bulletDamage, bulletScale, bulletShakeAngle);
                    yield return new WaitForSeconds(bulletInterval);
                }
                    
                //攻撃中に必ず移動するキャラはここで動くコルーチンを停止
                if (isAttackAndMove == 1)
                {
                    StopCoroutine(move);
                    EnemyMoveStop();
                }
                
                //攻撃中に必ず止まるキャラはここで動くコルーチンを作動
                if (isAttackAndStop == 1)
                {
                    yield return new WaitForSeconds(1.0f); //少し待ってから動き出す、引数を増やしすぎないようにここの時間は固定
                    move = StartCoroutine(EnemyWalk(walkTime, walkStopTime));
                }
            }

            //攻撃が終わったらattackInterval秒攻撃しない
            yield return new WaitForSeconds(attackInterval);
        }
    }

    /// <summary>
    /// 通常の弾に命中したときに呼ぶメソッド
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HitBullet(enemyMethod.GetDamage(collision.gameObject));
        }
    }


    /// <summary>
    /// 貫通弾に命中したときに呼ぶメソッド
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HitBullet(enemyMethod.GetDamage(collision.gameObject));
        }   
    }


    /// <summary>
    /// 弾に命中したとき、白く光らせ、ダメージを喰らうメソッド
    /// </summary>
    public void HitBullet(int damage)
    {
        enemyHP = enemyHP - damage;
        if (enemyHP <= 0)
        {
            if (dead) { return; }
            DeadEnemy();
        }

        StartCoroutine(enemyMethod.DoWhite(mySprite.material));
    }

    /// <summary>
    /// 敵を倒したら効果音を再生し、キルカウントを増やし、自分をデストロイするメソッド
    /// </summary>
    void DeadEnemy()
    {
        audioSource.PlayOneShot(enemyDownSE); //敵が倒れたときのSE
        gameManager.KillCountUp();
        Destroy(gameObject);
        dead = true;
    }
}
