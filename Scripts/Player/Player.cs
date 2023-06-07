using Calc;
using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// プレイヤーを管理するクラス
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    Rigidbody2D myRigidBody;
    Animator myAnime;
    SpriteRenderer mySprite;

    public int playerMaxHP;
    public int playerHP;
    public int maxBomb;
    public int remainBomb;
    public int holdCoin = 0;
    const float walkSpeed = 6.0f; //歩く速度
    public float walkSpeedAddition = 1.0f; //歩く速度を上げるときに使う変数
    const float rollingTime = 0.6f; //ローリングの時間
    const float noDamageTime = 3.0f; //ダメージを受けたときの無敵時間
    public int killCount;

    float keyRight;
    float keyUp;

    //ゲームマネージャから読み込む変数
    Transform enemyBulletManagerTransform;

    //ゲームマネージャから読み込み常に更新する変数
    Vector3 playerPos;
    Vector3 aimImagePos;

    public bool rollingCheck = false;
    bool damagedCheck = false;
    bool bombCheck = false;
    bool entrance = false;
    bool tutorial = false;
    bool dead = false;

    //歩きのアニメーションを切り替えるための変数
    string[] walkDirection = new string[] { "Walk_Right", "Walk_UpRight", "Walk_Up", "Walk_UpLeft", "Walk_Left", "Walk_DownLeft", "Walk_Down", "Walk_DownRight", "Walk_Right" };

    SEManager SEManager;
    AudioClip damageSE;
    AudioClip bombSE;
    AudioClip rollingSE;
    AudioSource audioSource;

    TextMeshProUGUI coin;


    void Start()
    {
        StartCoroutine(SetPlayerHP());
        holdCoin = GameData.holdCoin;
        killCount = GameData.killCount;

        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myAnime = gameObject.GetComponent<Animator>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();

        //ゲームマネージャから読み込み
        if (gameManager.enemyBulletManagerTransform != null) { enemyBulletManagerTransform = gameManager.enemyBulletManagerTransform; }
        if (gameManager.coin != null) { coin = gameManager.coin; }

        entrance = gameManager.entrance;
        tutorial = gameManager.tutorial;

        remainBomb = maxBomb;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;
        damageSE = SEManager.SE_Damage;
        bombSE = SEManager.SE_Bomb;
        rollingSE = SEManager.SE_Rolling;
    }


    void Update()
    {
        if (!entrance && !tutorial)
        {
            coin.text = holdCoin.ToString(); //エントランスなら所持コインを表示しない
        }

        //ゲームマネージャから読み込み常に更新する
        playerPos = gameManager.playerPos;
        aimImagePos = gameManager.aimImagePos;

        //HPが上限を超えないようにする
        if (playerHP > playerMaxHP)
        {
            playerHP = playerMaxHP;
        }

        //HPがマイナスにならないようにする
        if (playerHP < 0)
        {
            playerHP = 0;
        }

        if (dead) { return; }

        //HPが0になったときの処理
        if (playerHP == 0)
        {
            myAnime.SetTrigger("Death");
            Dead();
        }

        PlayerDirection(); //プレイヤーの向いてる方向（アニメーション）を変えるメソッド
    }

    /// <summary>
    /// プレイヤーの移動とローリングをするメソッド、keyInputManagerから呼び出す
    /// </summary>
    public void MovePlayer()
    {
        //WASDキーで移動
        keyRight = Input.GetKey(KeyCode.D) ? 1 :
                    Input.GetKey(KeyCode.A) ? -1 :
                    0;

        keyUp = Input.GetKey(KeyCode.W) ? 1 :
                 Input.GetKey(KeyCode.S) ? -1 :
                 0;

        //斜め移動の場合は、速度を揃えるために 1 / √2
        if (keyRight != 0 && keyUp != 0)
        {
            keyRight /= 1.41f;
            keyUp /= 1.41f;
        }
        if (rollingCheck) { return; } //ローリング中は移動方向を変えられないようにする

        //プレイヤーの移動
        //walkSpeedAdditionで、非戦闘中は移動速度アップ
        myRigidBody.velocity = new Vector2(walkSpeed * keyRight * walkSpeedAddition, walkSpeed * keyUp * walkSpeedAddition);
    }

    /// <summary>
    /// プレイヤーを停止させるメソッド
    /// </summary>
    public void StopPlayer()
    {
        myRigidBody.velocity = new Vector2(0, 0);
    }

    /// <summary>
    /// プレイヤーが止まってなかったらローリングするメソッド（一定時間無敵かつ移動速度アップだが、銃を使えなくなり一方向しか移動できない）
    /// keyInputManagerから呼び出し
    /// </summary>
    public void RollingPlayer()
    {
        if (rollingCheck) { return; } //ローリング中はローリングできないように
        if (keyRight == 0 && keyUp == 0) { return; } //プレイヤーが止まってたらローリングできないようにする

        //左方向にローリングするなら、スプライトを反転（アニメーションが左に向かってローリングするように）
        if (keyRight < 0)
        {
            mySprite.flipX = true;
        }

        audioSource.PlayOneShot(rollingSE);
        StartCoroutine(Rolling(gameObject, rollingTime)); //ローリング
    }

    /// <summary>
    /// 武器を切り替えるUIを出すメソッド
    /// keyInputManagerから呼び出し
    /// </summary>
    public void ActiveGunSelectUI()
    {
        gameManager.gunSelectUIMGR.SetActive(true);
    }

    /// <summary>
    /// 操作確認画面を出すメソッド
    /// keyInputManagerから呼び出し
    /// </summary>
    public void ActiveOperationsConfirm()
    {
        gameManager.operationsConfirm.SetActive(true);
    }

    /// <summary>
    /// remainBombが1以上ならボムを発動するメソッド、ボム使用後少しの間使えないようにする（連発を防ぐ）
    /// keyInputManagerから呼び出し
    /// </summary>
    public void UseBomb()
    {
        if (remainBomb <= 0)
        {
            return;
        }
        if (bombCheck) { return; } //残りのボムがなかったり、連発しようとしているならリターン

        bombCheck = true;
        gameManager.CameraShake(0.1f, 0.5f); //カメラを揺らすメソッド
        audioSource.PlayOneShot(bombSE); //効果音を鳴らす
        remainBomb -= 1; //ボムを減らす

        StartCoroutine(BombWait()); //ボムを使用したら少しの間使用できないようにするコルーチン
        StartCoroutine(DestroyEnemyBullet()); //敵の弾を一時的に消すコルーチン
    }

    /// <summary>
    /// ボムを使用したら少しの間ボムを使用できないようにするコルーチン
    /// </summary>
    IEnumerator BombWait()
    {
        float bombWaitTime = 1.0f;
        yield return new WaitForSeconds(bombWaitTime);
        bombCheck = false;
    }

    /// <summary>
    /// ボムを使ったら敵の弾を全て消し、少しの間消し続けるコルーチン
    /// </summary>
    IEnumerator DestroyEnemyBullet()
    {
        for (float i = 0; i < 100; i += 1.0f)
        {
            foreach (Transform childTransform in enemyBulletManagerTransform) //全ての敵の弾オブジェクトをDestroy
            {
                Destroy(childTransform.gameObject);
            }
            yield return null;
        }
    }

    /// <summary>
    /// プレイヤーとマウスの座標の1/3の位置にカメラを移動するメソッド
    /// keyInputManagerから呼び出し
    /// </summary>
    public void MoveCamera()
    {
        gameManager.cameraTransform.position = new Vector3((playerPos.x * 2 + aimImagePos.x) / 3, (playerPos.y * 2 + aimImagePos.y) / 3, -100f);
    }

    /// <summary>
    /// プレイヤーの向く角度（アニメーション）を指定するメソッド
    /// </summary>
    void PlayerDirection()
    {
        //エイムの画像の位置ととプレイヤーの位置の角度を求める
        double angleAimImage = Calculate.Angle_RightTriangle(playerPos, aimImagePos);

        //22.5度から、45度ずつで角度を変える（337.5°～360°は右向き）
        string direction = null;
        float angle = 22.5f;

        for (int i = 0; i < walkDirection.Length; i++)
        {
            if (angleAimImage <= angle)
            {
                direction = walkDirection[i];
                break;
            }
            angle += 45;
        }

        WalkAnimeOn(direction); //照準の向きのアニメーションを再生
    }


    /// <summary>
    /// 歩きアニメーションの切り替えメソッド
    /// </summary>
    /// <param name="direction">向きたい方向（アニメーション名）</param>
    void WalkAnimeOn(string direction)
    {
        for (int i = 0; i < walkDirection.Length; i++)
        {
            myAnime.SetBool(walkDirection[i], false);
        }
        //directionに格納されたbool名のアニメーションのみtrue
        myAnime.SetBool(direction, true);
    }

    /// <summary>
    /// ダメージを受けたとき、アニメを再生し、無敵にするコルーチンと点滅させるコルーチンを呼び出すメソッド
    /// </summary>
    public void Damaged(int damage)
    {
        if (damagedCheck) { return; } //連続でダメージを受けないように
        playerHP -= damage;
        audioSource.PlayOneShot(damageSE); //ダメージを食らったときのSE

        if (playerHP <= 0) { return; } //HPが０になったらダメージのアニメーションを再生しないように
        myAnime.SetTrigger("Hit");
        StartCoroutine(NoHit(gameObject, noDamageTime)); //noDamageTime秒無敵
        StartCoroutine(Flashing(gameObject, noDamageTime - 0.5f)); //noDamageTime - 0.5秒点滅
        damagedCheck = true;
    }


    /// <summary>
    /// 敵の弾が衝突したときに呼び出すメソッド
    /// </summary>
    /// <param name="collision">衝突した相手</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        //弾が衝突したらダメージを受ける
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            int damage = (int)bullet.damageToPlayer;
            Damaged(damage);
        }

        if (damagedCheck) { return; }

        //敵に衝突してもダメージを受ける
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Damaged(1);
        }
    }


    /// <summary>
    /// ローリングアニメーションを再生し、一定時間無敵かつ、一方向にしか歩けない(ローリングチェックをTrue)にするコルーチン
    /// </summary>
    /// <param name="gameObject">ローリングするゲームオブジェクト</param>
    /// <param name="rollingTime">ローリングする時間</param>
    IEnumerator Rolling(GameObject gameObject, float rollingTime)
    {
        myAnime.SetTrigger("Rolling");
        if (!damagedCheck) { StartCoroutine(NoHit(gameObject, rollingTime)); } //ダメージ受けてるときにローリングすると無敵が短くなるのを防ぐ
        rollingCheck = true; //trueのとき、移動ができなく、ローリングも出せない
        myRigidBody.velocity = new Vector2(walkSpeed * keyRight * 1.1f, walkSpeed * keyUp * 1.1f); //ローリング中は少し加速する

        yield return new WaitForSeconds(rollingTime);

        rollingCheck = false;
        mySprite.flipX = false;
    }


    /// <summary>
    /// 一定時間レイヤーを変え、ゲームオブジェクトに弾を当たらなくするコルーチン
    /// </summary>
    /// <param name="gameObject">弾を当たらなくするゲームオブジェクト</param>
    /// <param name="NoHitTime">弾を当たらなくする時間</param>
    IEnumerator NoHit(GameObject gameObject, float NoHitTime)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerNoHit");

        yield return new WaitForSeconds(NoHitTime);

        gameObject.layer = LayerMask.NameToLayer("Player");
        damagedCheck = false;
    }


    /// <summary>
    /// 一定時間ゲームオブジェクトを点滅させるコルーチン
    /// </summary>
    /// <param name="gameObject">点滅させるゲームオブジェクト</param>
    /// <param name="flashTime">点滅させる時間</param>
    IEnumerator Flashing(GameObject gameObject, float flashTime)
    {
        SpriteRenderer gameObjectSprite = gameObject.GetComponent<SpriteRenderer>();

        for (int i = 0; i < flashTime * 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            gameObjectSprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            gameObjectSprite.enabled = true;
        }
    }

    //開始から1フレーム開けてからプレイヤーのHPをセットするコルーチン
    IEnumerator SetPlayerHP()
    {
        yield return null;

        playerHP = GameData.playerHP;
    }

    /// <summary>
    /// HPが０になったときのメソッド
    /// </summary>
    void Dead()
    {
        dead = true;

        //レイヤーを変えて被弾しないように
        gameObject.layer = LayerMask.NameToLayer("PlayerNoHit");

        //動けないようにして、カメラをプレイヤーの位置に固定
        gameManager.keyInputManager.noControl = true;
        gameManager.cameraTransform.position = new Vector3(playerPos.x, playerPos.y, gameManager.cameraTransform.position.z);

        gameManager.Flash(); //画面をフラッシュ
        Invoke("OpenGameOverMenu", 8.0f); //時間を開けてからゲームオーバーのメニューを表示
        gameManager.UIMGR.gameObject.SetActive(false); //UIを非表示
        gameManager.reloadCircle.gameObject.SetActive(false);
        if (gameManager.miniMap != null) { gameManager.miniMap.MapOff(); }

        //BGMを停止する
        AudioSource audioSource = gameManager.audioSourceBGM;
        audioSource.Stop();

        //すべての敵を非アクティブに
        //デストロイだと部屋が開いてしまう
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemys.Length == 0) { return; }

        foreach (GameObject enemy in enemys)
        {
            enemy.SetActive(false);
        }
    }

    /// <summary>
    /// ゲームオーバーメニューを開くメソッド
    /// </summary>
    void OpenGameOverMenu()
    {
        gameManager.DisplayCursor(); //照準を非表示にし、カーソルを表示

        GameObject gameOverMenu = gameManager.gameOverMenu;
        gameOverMenu.SetActive(true);
    }
}