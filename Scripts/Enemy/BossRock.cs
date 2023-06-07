using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// メガントロック（ボス）を管理するクラス
/// </summary>
public class BossRock : MonoBehaviour
{
    GameManager gameManager;

    Animator myAnime;
    SpriteRenderer mySprite;

    [SerializeField] GameObject rock;
    [SerializeField] GameObject miniRock;
    [SerializeField] GameObject explosion; //ボス撃破時の爆発のプレハブ

    [SerializeField] Transform trapManager;
    const int trapNumber = 24;
    GameObject[] traps = new GameObject[trapNumber];
    TrapBullet[] trapBullets = new TrapBullet[trapNumber];

    Vector2 rockCreatePosLeft;
    Vector2 rockCreatePosRight;

    //戦闘中のデータ確認用にシリアライズ化（戦闘中にデータは変わるため）
    [SerializeField] float enemyHP;
    [SerializeField] float bulletCreateTime;
    [SerializeField] float attackContinueTime;
    [SerializeField] float attackInterval;

    Slider bossHPSlider;
    TextMeshProUGUI bossNameText;

    float enemyMaxHP;

    [SerializeField] string bossName;
    bool dead = false;

    Coroutine coroutine;

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
        "Attack4",
        "Attack5",
        "Attack6"
    };

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //敵のステータスを設定
        enemyMaxHP = 3500;
        enemyHP = enemyMaxHP;
        attackInterval = 2;
        bulletCreateTime = 1.5f;

        myAnime = gameObject.GetComponent<Animator>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();

        enemyMethod = gameManager.enemyMethod;
        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        mySprite.material.color = new Color(0.5f, 0.5f, 0.5f); //通常色

        bossHPSlider = gameManager.bossHPSlider;
        bossNameText = gameManager.bossNameText;

        bossHPSlider.value = 1;
        bossNameText.text = bossName;

        attack1SE = SEManager.SE_BossRock;

        int i = 0;
        foreach (Transform trap in trapManager)
        {
            GameObject trapObj = trap.gameObject;
            traps[i] = trapObj;
            trapBullets[i] = trapObj.GetComponent<TrapBullet>();
            i += 1;
        }

        Vector2 myPos = gameObject.transform.position;
        rockCreatePosLeft = new Vector2(myPos.x - 10, myPos.y);
        rockCreatePosRight = new Vector2(myPos.x + 10, myPos.y);
    }

    void Update()
    {
        //ボスアタックアニメがオンになったら攻撃アニメを再生（演出用）
        if (gameManager.bossAttackAnime)
        {
            myAnime.SetTrigger("Attack");
            audioSource.PlayOneShot(attack1SE); //攻撃アニメ再生時のSEを再生
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
            float enemyShootStartTime = 2;

            Invoke("CallEnemyAttack", enemyShootStartTime);

            gameManager.bossMoveStart = false;
        }
    }

    /// <summary>
    /// 敵が攻撃するコルーチンを呼び出すメソッド
    /// </summary>
    void CallEnemyAttack()
    {
        if (!gameObject.activeSelf) { return; } //非アクティブになったら呼ばないようにする
        coroutine =  StartCoroutine(EnemyAttack());
    }

    /// <summary>
    /// 敵を、一定時間攻撃させ、一定時間攻撃を停止するコルーチン
    /// </summary>
    IEnumerator EnemyAttack()
    {
        if (dead) { yield break; }

        //攻撃アニメ開始、攻撃効果音再生、パーティクル再生
        myAnime.SetTrigger("Attack");
        audioSource.PlayOneShot(attack1SE);

        //配列attackの中からランダムで攻撃を選ぶ
        //選ばれた攻撃名はattackNameに格納
        while (true)
        {
            string attackName = enemyMethod.ChooseBossAttack(attack);

            //アニメーションと攻撃開始のタイミングを合わせるため、bulletCreateTime秒停止してから攻撃
            yield return new WaitForSeconds(bulletCreateTime);

            //attackNameに格納された攻撃を開始する
            Invoke(attackName, 0);

            yield return null;

            //attackContinueTimeの時間だけ攻撃を続ける
            yield return new WaitForSeconds(attackContinueTime);

            TrapOff();

            //攻撃が終わったらattackInterval秒攻撃しない
            yield return new WaitForSeconds(attackInterval);

            if (dead) { yield break; }

            //攻撃アニメ開始、攻撃効果音再生、パーティクル再生
            myAnime.SetTrigger("Attack");
            audioSource.PlayOneShot(attack1SE);
        }
    }
    
    //室内全体に数回攻撃
    void Attack1()
    {
        TrapOn();
        ChangeTrapStatus(3f, 1, 3);
        attackContinueTime = 4.0f;
    }

    //部屋の左側を弾で埋め尽くし、右に敵を召喚
    void Attack2()
    {
        for (int i = 0; i < 16; i++)
        {
            traps[i].SetActive(true);
        }
        ChangeTrapStatus(0.3f, 1, 2.5f);
        Instantiate(miniRock, rockCreatePosRight, Quaternion.identity);
        attackContinueTime = 4.0f;
    }

    //部屋の右側を弾で埋め尽くし、左に敵を召喚
    void Attack3()
    {
        for (int i = 8; i < 24; i++)
        {
            traps[i].SetActive(true);
        }
        ChangeTrapStatus(0.3f, 1, 2.5f);
        Instantiate(miniRock, rockCreatePosLeft, Quaternion.identity);
        attackContinueTime = 4.0f;
    }

    //中心に素早い攻撃
    void Attack4()
    {
        for (int i = 8; i < 16; i++)
        {
            traps[i].SetActive(true);
        }
        ChangeTrapStatus(10, 1, 6);
        attackContinueTime = 2.0f;
    }

    //左右にミニロックを召喚
    void Attack5()
    {
        Instantiate(miniRock, rockCreatePosLeft, Quaternion.identity);
        Instantiate(miniRock, rockCreatePosRight, Quaternion.identity);
        attackContinueTime = 2.0f;
    }

    //左右にロックを召喚
    void Attack6()
    {
        Instantiate(rock, rockCreatePosLeft, Quaternion.identity);
        Instantiate(rock, rockCreatePosRight, Quaternion.identity);
        attackContinueTime = 6.0f;
    }


    //アニメをオフにするメソッド
    public void MyAnimeOff()
    {
        //ステージマネージャーから呼ぶボスもある
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
    public void HitBullet(float damage)
    {
        enemyHP -= damage; //ダメージの分だけダメージを食らう 

        //ボスを撃破
        if (enemyHP <= 0)
        {
            DeadBoss();
        }

        bossHPSlider.value = enemyHP / enemyMaxHP;

        StartCoroutine(enemyMethod.DoWhite(mySprite.material));
    }

    /// <summary>
    /// 全てのトラップをオンにするメソッド
    /// </summary>
    void TrapOn()
    {
        for (int i = 0; i < trapNumber; i++)
        {
            traps[i].SetActive(true);
        }
    }

    /// <summary>
    /// 全てのトラップをオフにするメソッド
    /// </summary>
    void TrapOff()
    {
        for (int i = 0; i < trapNumber; i++)
        {
            traps[i].SetActive(false);
        }
    }

    /// <summary>
    /// トラップのステータスを変えるメソッド
    /// </summary>
    /// <param name="interval">発射間隔</param>
    /// <param name="scale">弾の大きさ</param>
    /// <param name="speed">弾のスピード</param>
    void ChangeTrapStatus(float interval, float scale, float speed)
    {
        for (int i = 0; i < trapNumber; i++)
        {
            trapBullets[i].bulletInterval = interval;
            trapBullets[i].bulletScale = scale;
            trapBullets[i].bulletSpeed = speed;
        }
    }

    /// <summary>
    /// ボスを撃破したときのメソッド
    /// </summary>
    void DeadBoss()
    {
        dead = true;
        enemyMethod.DeadBoss(gameObject, explosion);

        MyAnimeOff(); //アニメを停止
        TrapOff();
        StopCoroutine(coroutine);

        //すべての敵を非アクティブに
        //デストロイだと部屋が開いてしまう
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemys.Length == 0) { return; }

        //自分以外の敵を非アクティブ
        foreach (GameObject enemy in enemys)
        {
            if (enemy.name == "BossRock") { continue; }
            enemy.SetActive(false);
        }
    }
}
