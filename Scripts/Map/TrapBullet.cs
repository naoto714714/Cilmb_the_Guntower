using UnityEngine;
using System.Collections;
using Calc;

/// <summary>
/// 一方向に弾を発射し続けるトラップのクラス
/// </summary>
public class TrapBullet : MonoBehaviour
{
    GameManager gameManager;
    PrehubManager prehubManager;

    /// <summary>
    /// 弾の発射間隔
    /// </summary>
    public float bulletInterval;
    /// <summary>
    /// 弾を発射する方向
    /// １：南、２：東、３：北、４：西
    /// </summary>
    public int bulletDirection;
    /// <summary>
    /// 発射する弾の大きさ
    /// </summary>
    public float bulletScale;
    /// <summary>
    /// 発射する弾のスピード
    /// </summary>
    public float bulletSpeed;

    Transform enemyBulletManagerTransform;

    const float attackRange = 50;
    const float bulletDamage = 1;

    GameObject bullet;

    Vector3 playerPos;
    Vector3 myPos;

    bool firstCheck = false;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        prehubManager = gameManager.prehubManager;

        bullet = prehubManager.enemyBulletPrehub;
        enemyBulletManagerTransform = gameManager.enemyBulletManagerTransform;

        CallTrapAttack();
        firstCheck = true;
    }

    void OnEnable()
    {
        if (!firstCheck) { return; }
        CallTrapAttack();
    }

    void Update()
    {
        playerPos = gameManager.playerPos;
        myPos = gameObject.transform.position;
    }

    /// <summary>
    /// 敵が攻撃するコルーチンを呼び出すメソッド
    /// </summary>
    void CallTrapAttack()
    {
        if (!gameObject.activeSelf) { return; } //非アクティブになったら呼ばないようにする
        StartCoroutine(TrapAttack());
    }

    /// <summary>
    /// 敵を、一定時間攻撃させ、一定時間攻撃を停止するコルーチン
    /// </summary>
    IEnumerator TrapAttack()
    {
        yield return null; //ここでyirld returnを入れないと、座標(0,0)に弾が生成される

        while (true)
        {
            //プレイヤーがattackRange内にいるなら攻撃開始
            if (Calculate.Distance(myPos, playerPos) < attackRange)
            {
                CreateBullet();
            }
            yield return new WaitForSeconds(bulletInterval); //bulletInterval秒の間隔を開け、再び攻撃
        }
    }

    /// <summary>
    /// 弾丸を生成するメソッド
    /// </summary>
    void CreateBullet()
    {
        //弾（ゲームオブジェクトの生成）
        Vector3 bulletPos = new Vector3(myPos.x, myPos.y, 0);
        GameObject bulletClone = Instantiate(bullet, bulletPos, Quaternion.identity);
        bulletClone.transform.parent = enemyBulletManagerTransform;

        //弾の大きさを変える、通常の大きさは0.6
        float normalScale = 0.6f;
        bulletClone.transform.localScale = new Vector2(bulletScale * normalScale, bulletScale * normalScale);

        //弾にダメージを与える
        Bullet bulletScript = bulletClone.GetComponent<Bullet>();
        bulletScript.SetDamageToPlayer(bulletDamage);

        //発射方向を変える
        Vector2 shootDirection = new Vector2(0, 0);
        switch (bulletDirection)
        {
            case 1: //南に撃つ
                shootDirection = new Vector2(0, -1);
                break;

            case 2: //東に撃つ
                shootDirection = new Vector2(1, 0);
                break;

            case 3: //北に撃つ
                shootDirection = new Vector2(0, 1);
                break;

            case 4: //西に撃つ
                shootDirection = new Vector2(-1, 0);
                break;
        }
        bulletClone.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
    }
}
