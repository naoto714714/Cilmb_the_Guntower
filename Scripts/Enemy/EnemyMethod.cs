using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class EnemyMethod : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    GameObject bullet;
    Transform enemyBulletManagerTransform;
    AudioClip attackSE;
    AudioSource audioSource;

    int[,] key = new int[,] { { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 } };
    /*  KeyTable            (0, 1)
     *              (-1 , 1)       (1, 1)
     *      (-1 , 0)                        (1, 0)
     *              (-1, -1)        (1, -1)
     *                     (0, -1)
     */

    void Start()
    {
        bullet = gameManager.prehubManager.enemyBulletPrehub;
        enemyBulletManagerTransform = gameManager.enemyBulletManagerTransform;
        attackSE = gameManager.SEManager.SE_EnemyShot;
        audioSource = gameManager.audioSourceSE;
    }

    public void SpriteFlip(int type, SpriteRenderer mySprite, Vector2 myPos)
    {
        Vector2 playerPos = gameManager.playerPos;

        if (playerPos.x - myPos.x > 0)
        {
            if (type == 1) { mySprite.flipX = true; }
            else if (type == 2) { mySprite.flipX = false; }   
        }
        else
        {
            if (type == 1) { mySprite.flipX = false; }
            else if(type == 2) { mySprite.flipX = true; }
        }
    }

    /// <summary>
    /// 敵が移動する方向を決めて移動するメソッド、移動方向はkey配列の中で決める
    /// </summary>
    public void EnemyMoveStart(ref float keyRight, ref float keyUp, Vector2 myPos, int isWalkRandom)
    {
        Vector2 playerPos = gameManager.playerPos;

        double anglePlayer = Calculate.Angle_RightTriangle(myPos, playerPos);
        int keyRandom = 0;

        if (isWalkRandom == 1)
        {
            //40%の確率で、直進ではなくkeyを１つずらす
            int randomEnemyMove = Random.Range(0, 10);

            if (randomEnemyMove < 2)
            {
                keyRandom = 1;
            }
            else if (randomEnemyMove > 7)
            {
                keyRandom = -1;
            }
        }

        float angle = 22.5f;

        //プレイヤーの方向によって移動する向きを決める
        //keyRandomを追加してもkey配列の範囲外に出ないようにする
        for (int i = 0; i < key.GetLength(0); i++)
        {
            if (anglePlayer <= angle)
            {
                if (i + keyRandom < 0)
                {
                    keyRandom = key.GetLength(0) - 2;
                }
                if (i + keyRandom >= key.GetLength(0))
                {
                    keyRandom = -1 * (key.GetLength(0) - 2);
                }

                keyRight = key[i + keyRandom, 0];
                keyUp = key[i + keyRandom, 1];

                break;
            }
            angle += 45;
        }

        //斜め移動の場合は、速度を揃えるために 1 / √2
        if (keyRight != 0 && keyUp != 0)
        {
            keyRight /= 1.41f;
            keyUp /= 1.41f;
        }
    }

        /// <summary>
        /// 弾丸を生成するメソッド
        /// </summary>
        /// <param name="myPos">自分（敵）の場所</param>
        /// <param name="bulletCreateNumber">弾丸生成数</param>
        /// <param name="bulletSpeed">弾丸の速度</param>
        /// <param name="bulletDamage">与えるダメージ</param>
        /// <param name="bulletScale">弾丸の大きさ</param>
        /// <param name="bulletShakeAngle">最大ブレ角度</param>
        public void CreateBullet(Vector2 myPos, int bulletCreateNumber, float bulletSpeed, int bulletDamage, float bulletScale, float bulletShakeAngle)
    {
        Vector2 playerPos = gameManager.playerPos;

        audioSource.PlayOneShot(attackSE); //攻撃時のSEを再生

        //一度にbulletCreateNumberの数だけ弾を生成
        for (int i = 0; i < bulletCreateNumber; i++)
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

            //発射方向から、最大bulletShakeAngle°ずらし、その方向に速度を掛け、弾に速度を与える
            Vector2 origin = new Vector2(0, 0);
            Vector2 shootDirection = Calculate.To_Circle(origin, Calculate.ShakeAngle(myPos, playerPos, bulletShakeAngle), 1.0f);
            bulletClone.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
        }
    }

    /// <summary>
    /// 攻撃名の配列からランダムで一つ選び、攻撃名を返すメソッド
    /// </summary>
    /// <param name="attack">攻撃名の配列</param>
    /// <returns>攻撃名</returns>
    public string ChooseBossAttack(string[] attack)
    {
        int pattern = Calculate.ChooseNumber(Calculate.NumList(0, attack.Length - 1));
        return attack[pattern];
    }

    /// <summary>
    /// ヒットした弾のダメージを受け取り返すメソッド
    /// </summary>
    /// <param name="bulletObj">ヒットした弾</param>
    /// <returns>弾のダメージ</returns>
    public int GetDamage(GameObject bulletObj)
    {
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        int damage = bullet.damageToEnemy;
        return damage;
    }

    /// <summary>
    /// 弾に命中したとき、白く光らせるコルーチン
    /// </summary>
    public IEnumerator DoWhite(Material myMaterial)
    {
        myMaterial.color = new Color(1.0f, 1.0f, 1.0f); //白くする

        float hitWhiteTime = 0.05f;
        yield return new WaitForSeconds(hitWhiteTime);

        myMaterial.color = new Color(0.5f, 0.5f, 0.5f); //通常色
    }

    public void DeadBoss(GameObject gameObject, GameObject explosion)
    {
        GameObject bossHPSlider = gameManager.bossHPSlider.gameObject;
        GameObject bossNameText = gameManager.bossNameText.gameObject; 

        StartCoroutine(gameManager.stagingManager.DeadBoss(gameObject, explosion)); //ボスを撃破したときの演出を再生
        gameManager.DestroyEnemyBullet(); //敵の弾を全て消す

        bossHPSlider.gameObject.SetActive(false); //ボスのHPバーを非表示
        bossNameText.gameObject.SetActive(false); //ボスの名前を非表示
        gameManager.KillCountUp();
    }
}
