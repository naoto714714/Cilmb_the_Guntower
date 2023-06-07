using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// スカルキング（ボス）を管理するクラス
/// </summary>
public class BossSkull : MonoBehaviour
{
    GameManager gameManager;

    Rigidbody2D myRigidBody;
    Animator myAnime;
    SpriteRenderer mySprite;

    [SerializeField] ParticleSystem particle; //ボスが攻撃するときのパーティクル
    [SerializeField] GameObject explosion; //ボス撃破時の爆発のプレハブ

    //戦闘中のデータ確認用にシリアライズ化（戦闘中にデータは変わるため）
    [SerializeField] int enemyHP;
    [SerializeField] float walkSpeed;
    [SerializeField] float walkTime;
    [SerializeField] float walkStopTime;
    [SerializeField] float attackInterval;
    [SerializeField] int attackNumber;
    [SerializeField] int bulletCreateNumber;
    [SerializeField] float bulletInterval;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletScale;
    [SerializeField] int bulletDamage;
    [SerializeField] float bulletShakeAngle;
    [SerializeField] float bulletCreateTime;

    Slider bossHPSlider;
    TextMeshProUGUI bossNameText;

    Vector3 myPos;

    float enemyMaxHP;
    float keyRight = 0;
    float keyUp = 0;

    [SerializeField] string bossName;
    bool isMoving = false;
    bool dead = false;

    EnemyMethod enemyMethod;
    SEManager SEManager;
    AudioClip attack1SE;
    AudioSource audioSource;

    //攻撃パターンの種類
    string[] attack = new string[]
    {
        "Attack1",
        "Attack2",
        "Attack3",
        "Attack4"
    };

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //敵のステータスを設定
        enemyMaxHP = 2500;
        enemyHP = (int)enemyMaxHP;
        walkSpeed = 1.5f;
        walkTime = 0.3f;
        walkStopTime = 0;
        attackInterval = 4;
        bulletDamage = 1;
        bulletCreateTime = 0.1f;

        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myAnime = gameObject.GetComponent<Animator>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();

        enemyMethod = gameManager.enemyMethod;
        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        myAnime.SetBool("Attack", false);
        particle.Stop();

        mySprite.material.color = new Color(0.5f, 0.5f, 0.5f); //通常色

        bossHPSlider = gameManager.bossHPSlider;
        bossNameText = gameManager.bossNameText;

        bossHPSlider.value = 1;
        bossNameText.text = bossName;

        attack1SE = SEManager.SE_BossSkullAttack1;
    }

    void Update()
    {
        //自分（ボス）の位置を取得
        myPos = gameObject.transform.position;

        //移動
        myRigidBody.velocity = new Vector2(walkSpeed * keyRight, walkSpeed * keyUp);

        //プレイヤーの方向によって敵（自分）の向きを変える
        if (isMoving) { return; }

        enemyMethod.SpriteFlip(2, mySprite, myPos);

        //ボスアタックアニメがオンになったら攻撃アニメを再生（演出用）
        if (gameManager.bossAttackAnime)
        {
            myAnime.SetBool("Attack", true);
            audioSource.PlayOneShot(attack1SE); //攻撃アニメ再生時のSEを再生
            particle.Play();
            gameManager.bossAttackAnime = false;
            Invoke("MyAnimeOff", 3);
        }

        //ボスアタックアニメオフがオンになったら攻撃アニメを停止（演出用）
        if (gameManager.bossAttackAnimeOff)
        {
            MyAnimeOff();
            gameManager.bossAttackAnimeOff = false;
        }

        //ムーブスタートがオンになったら移動と攻撃を開始
        if (gameManager.bossMoveStart)
        {
            //敵は1秒後に動き出し、４秒後に攻撃を開始
            float enemyMoveStartTime = 1;
            float enemyShootStartTime = 4;

            Invoke("CallEnemyWalk", enemyMoveStartTime);
            Invoke("CallEnemyAttack", enemyShootStartTime);

            gameManager.bossMoveStart = false;
        }
    }


    /// <summary>
    /// 敵が移動するコルーチンを呼び出すメソッド
    /// </summary>
    void CallEnemyWalk()
    {
        if (!gameObject.activeSelf) { return; } //非アクティブになったら呼ばないようにする
        StartCoroutine(EnemyWalk(walkTime, walkStopTime));
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
            if (dead) { yield break; }

            //移動開始
            enemyMethod.EnemyMoveStart(ref keyRight, ref keyUp, myPos, 0);
            isMoving = true;

            yield return new WaitForSeconds(walkTime); //walkTimeの時間分移動する

            isMoving = false;

            yield return null;

            if (WalkStopTime < 0.1) { continue; }

            //移動停止
            EnemyMoveStop();

            yield return new WaitForSeconds(WalkStopTime); //walkStopTimeの時間分停止する
        }
    }

    /// <summary>
    /// 移動を停止させるメソッド
    /// </summary>
    void EnemyMoveStop()
    {
        keyRight = 0;
        keyUp = 0;
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
        if (dead) { yield break; }

        //攻撃アニメ開始、攻撃効果音再生、パーティクル再生
        myAnime.SetBool("Attack", true);
        audioSource.PlayOneShot(attack1SE);
        particle.Play();

        //攻撃アニメとタイミングを合わせるため、少し停止してから攻撃開始
        yield return new WaitForSeconds(attackInterval / 3);
        
        //配列attackの中からランダムで攻撃を選ぶ
        //選ばれた攻撃名はattackNameに格納
        while (true)
        {
            string attackName = enemyMethod.ChooseBossAttack(attack);

            //attackNameに格納された攻撃を開始する
            Invoke(attackName, 0);

            //アニメーションと攻撃開始のタイミングを合わせるため、bulletCreateTime秒停止してから攻撃
            yield return new WaitForSeconds(bulletCreateTime);

            //bulletInterval秒の間隔をあけ、attackNumberの数だけ連続で攻撃
            for (int i = 0; i < attackNumber; i++)
            {
                if (dead) { yield break; }
                enemyMethod.CreateBullet(myPos, bulletCreateNumber, bulletSpeed, bulletDamage, bulletScale, bulletShakeAngle);
                yield return new WaitForSeconds(bulletInterval);

                if (i == attackNumber - 1)
                {
                    particle.Stop();
                    myAnime.SetBool("Attack", false);
                }
            }

            //攻撃が終わったらattackInterval秒攻撃しない
            yield return new WaitForSeconds(attackInterval / 3 * 2);

            if (dead) { yield break; }

            //攻撃アニメ開始、攻撃効果音再生、パーティクル再生
            myAnime.SetBool("Attack", true);
            audioSource.PlayOneShot(attack1SE);
            particle.Play();

            yield return new WaitForSeconds(attackInterval / 3);
        }
    }

    //ばらつく弾丸を連続で発射
    void Attack1()
    {
        attackNumber = 60;
        bulletCreateNumber = 1;
        bulletInterval = 0.10f;
        bulletSpeed = 10;
        bulletScale = 1;
        bulletShakeAngle = 20;
    }

    //360°攻撃
    void Attack2()
    {
        attackNumber = 3;
        bulletCreateNumber = 100;
        bulletInterval = 2.5f;
        bulletSpeed = 5;
        bulletScale = 1;
        bulletShakeAngle = 180;
    }

    //大きな弾を３連発
    void Attack3()
    {
        attackNumber = 3;
        bulletCreateNumber = 1;
        bulletInterval = 0.8f;
        bulletSpeed = 10;
        bulletScale = 5;
        bulletShakeAngle = 0;
    }

    //扇状に攻撃
    void Attack4()
    {
        attackNumber = 2;
        bulletCreateNumber = 60;
        bulletInterval = 2.0f;
        bulletSpeed = 8;
        bulletScale = 1;
        bulletShakeAngle = 45;
    }

    //アニメをオフにするメソッド
    public void MyAnimeOff()
    {
        myAnime.SetBool("Attack", false);
        particle.Stop();
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
    /// 弾に命中したとき、自分を白く光らせ、ダメージを喰らうメソッド
    /// </summary>
    public void HitBullet(int damage)
    {
        enemyHP -= damage; //ダメージの分だけダメージを食らう 

        //ボスを撃破
        if (enemyHP <= 0)
        {
            dead = true;
            EnemyMoveStop(); //移動を停止
            MyAnimeOff(); //アニメを停止
            enemyMethod.DeadBoss(gameObject, explosion);
        }

        bossHPSlider.value = enemyHP / enemyMaxHP;

        StartCoroutine(enemyMethod.DoWhite(mySprite.material));
    }
}
