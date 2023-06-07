using UnityEngine;
using Calc;

/// <summary>
/// プレイヤーと敵の弾丸の設定をするクラス
/// </summary>
public class Bullet : MonoBehaviour
{
    public float damageToPlayer;
    public int damageToEnemy;
    public float force;
    public bool isThrough = false;
    public bool isBound = false;
    public bool isExplosion = false;
    public float explosionDistance = 0;
    public int explosionDamage = 0;
    public float explosionSize = 0;


    /// <summary>
    /// プレイヤーに与えるダメージをセットするメソッド
    /// </summary>
    /// <param name="damage">ダメージの値</param>
    public void SetDamageToPlayer(float damage)
    {
        damageToPlayer = damage;
    }


    /// <summary>
    /// プレイヤーの弾のステータスをセットするメソッド
    /// </summary>
    /// <param name="damage">ダメージの量</param>
    /// <param name="force">ノックバックの値</param>
    /// <param name="isThrough">貫通するかどうか</param>
    /// <param name="isBound">跳ね返るかどうか</param>
    /// <param name="isExplosion">爆発するかどうか</param>
    /// <param name="explosionDistance">爆発距離</param>
    /// <param name="explosionDamage">爆発ダメージ</param> 
    /// <param name="explosionSize">爆発サイズ</param>
    public void SetBulletToEnemy(int damage, float force, bool isThrough, bool isBound, bool isExplosion, float explosionDistance, int explosionDamage, float explosionSize)
    {
        damageToEnemy = damage;
        this.force = force;
        this.isThrough = isThrough;
        this.isBound = isBound;
        this.isExplosion = isExplosion;
        this.explosionDistance = explosionDistance;
        this.explosionDamage = explosionDamage;
        this.explosionSize = explosionSize;

        if (isThrough) //弾をトリガーにして貫通させる
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }


    /// <summary>
    /// 弾がなにかに当たったときの処理
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
	{
        //isExplosionが１なら爆発(Explosionメソッドでノックバックさせる)
        if (isExplosion)
        {
            Explosion(explosionDistance, explosionDamage, explosionSize);
        }
        //当たったのが敵ならノックバック
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            KnockBack(collision.gameObject);
        }

        //isBoundが１なら消滅しないで跳ね返る（弾の物理マテリアルの弾性力は１）
        if (isBound) { return; }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //敵にぶつかったらノックバックさせる
        if (collision.gameObject.CompareTag("Enemy"))
        {
            KnockBack(collision.gameObject);
        }
        //敵以外にぶつかったら自分（弾）をデストロイ
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ノックバックさせるメソッド
    /// </summary>
    /// <param name="collisionObject">衝突した物体</param>
    void KnockBack(GameObject collisionObject)
    {
        Vector2 myPos = gameObject.transform.position;
        Vector2 collisionPos = collisionObject.transform.position;
        Vector2 forceVector = Calculate.To_Circle(myPos, collisionPos, 1.0f);

        //ぶつかった方向と逆方向に力を加える
        Vector2 force = new Vector2(forceVector.x * this.force, forceVector.y * this.force);
        collisionObject.GetComponent<Rigidbody2D>().AddForce(force);
    }

    /// <summary>
    /// 爆発するメソッド
    /// </summary>
    /// <param name="explosionDistance">爆発ダメージをくらう距離</param>
    /// <param name="explosionDamage">爆発ダメージ</param>
    /// <param name="explosionSize">爆発サイズ</param>
    void Explosion(float explosionDistance, int explosionDamage, float explosionSize)
    {
        //全てのエネミーオブジェクトを取得
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.CameraShake(0.1f, 1.0f); //カメラを振動

        PrehubManager prehubManager = gameManager.prehubManager;
        Vector3 myPos = gameObject.transform.position;

        //爆発のプレハブを生成し、３秒後にそのプレハブをデストロイ
        GameObject explosionObj = Instantiate(prehubManager.explosion, myPos, Quaternion.identity);
        gameManager.DestroyObj(explosionObj, 3.0f);

        //爆発の大きさを変える、通常の大きさは0.6
        float normalScale = 0.4f;
        explosionObj.transform.localScale = new Vector2(explosionSize * normalScale, explosionSize * normalScale);

        if (enemys.Length == 0) { return; }
        
        //爆発範囲内にいるエネミーにダメージを与え、ノックバックさせる
        foreach (GameObject enemy in enemys)
        {
            Vector2 enemyPos = enemy.transform.position;
            double distance = Calculate.Distance(myPos, enemyPos);

            if (distance >= explosionDistance) { continue; }

            if (enemy.name == "BossSkull")
            {
                BossSkull bossSkull = enemy.GetComponent<BossSkull>();
                bossSkull.HitBullet(explosionDamage);
            }
            else if(enemy.name == "BossRock")
            {
                BossRock bossRock = enemy.GetComponent<BossRock>();
                bossRock.HitBullet(explosionDamage);
            }
            else if(enemy.name == "BossBunny")
            {
                BossBunny bossBunny = enemy.GetComponent<BossBunny>();
                bossBunny.HitBullet(explosionDamage);
            }
            else
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.HitBullet(explosionDamage);
                KnockBack(enemy);
            }
        }
    }
}