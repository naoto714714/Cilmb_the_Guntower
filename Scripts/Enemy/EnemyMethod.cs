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
    /// �G���ړ�������������߂Ĉړ����郁�\�b�h�A�ړ�������key�z��̒��Ō��߂�
    /// </summary>
    public void EnemyMoveStart(ref float keyRight, ref float keyUp, Vector2 myPos, int isWalkRandom)
    {
        Vector2 playerPos = gameManager.playerPos;

        double anglePlayer = Calculate.Angle_RightTriangle(myPos, playerPos);
        int keyRandom = 0;

        if (isWalkRandom == 1)
        {
            //40%�̊m���ŁA���i�ł͂Ȃ�key���P���炷
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

        //�v���C���[�̕����ɂ���Ĉړ�������������߂�
        //keyRandom��ǉ����Ă�key�z��͈̔͊O�ɏo�Ȃ��悤�ɂ���
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

        //�΂߈ړ��̏ꍇ�́A���x�𑵂��邽�߂� 1 / ��2
        if (keyRight != 0 && keyUp != 0)
        {
            keyRight /= 1.41f;
            keyUp /= 1.41f;
        }
    }

        /// <summary>
        /// �e�ۂ𐶐����郁�\�b�h
        /// </summary>
        /// <param name="myPos">�����i�G�j�̏ꏊ</param>
        /// <param name="bulletCreateNumber">�e�ې�����</param>
        /// <param name="bulletSpeed">�e�ۂ̑��x</param>
        /// <param name="bulletDamage">�^����_���[�W</param>
        /// <param name="bulletScale">�e�ۂ̑傫��</param>
        /// <param name="bulletShakeAngle">�ő�u���p�x</param>
        public void CreateBullet(Vector2 myPos, int bulletCreateNumber, float bulletSpeed, int bulletDamage, float bulletScale, float bulletShakeAngle)
    {
        Vector2 playerPos = gameManager.playerPos;

        audioSource.PlayOneShot(attackSE); //�U������SE���Đ�

        //��x��bulletCreateNumber�̐������e�𐶐�
        for (int i = 0; i < bulletCreateNumber; i++)
        {
            //�e�i�Q�[���I�u�W�F�N�g�̐����j
            Vector3 bulletPos = new Vector3(myPos.x, myPos.y, 0);
            GameObject bulletClone = Instantiate(bullet, bulletPos, Quaternion.identity);
            bulletClone.transform.parent = enemyBulletManagerTransform;

            //�e�̑傫����ς���A�ʏ�̑傫����0.6
            float normalScale = 0.6f;
            bulletClone.transform.localScale = new Vector2(bulletScale * normalScale, bulletScale * normalScale);

            //�e�Ƀ_���[�W��^����
            Bullet bulletScript = bulletClone.GetComponent<Bullet>();
            bulletScript.SetDamageToPlayer(bulletDamage);

            //���˕�������A�ő�bulletShakeAngle�����炵�A���̕����ɑ��x���|���A�e�ɑ��x��^����
            Vector2 origin = new Vector2(0, 0);
            Vector2 shootDirection = Calculate.To_Circle(origin, Calculate.ShakeAngle(myPos, playerPos, bulletShakeAngle), 1.0f);
            bulletClone.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
        }
    }

    /// <summary>
    /// �U�����̔z�񂩂烉���_���ň�I�сA�U������Ԃ����\�b�h
    /// </summary>
    /// <param name="attack">�U�����̔z��</param>
    /// <returns>�U����</returns>
    public string ChooseBossAttack(string[] attack)
    {
        int pattern = Calculate.ChooseNumber(Calculate.NumList(0, attack.Length - 1));
        return attack[pattern];
    }

    /// <summary>
    /// �q�b�g�����e�̃_���[�W���󂯎��Ԃ����\�b�h
    /// </summary>
    /// <param name="bulletObj">�q�b�g�����e</param>
    /// <returns>�e�̃_���[�W</returns>
    public int GetDamage(GameObject bulletObj)
    {
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        int damage = bullet.damageToEnemy;
        return damage;
    }

    /// <summary>
    /// �e�ɖ��������Ƃ��A�������点��R���[�`��
    /// </summary>
    public IEnumerator DoWhite(Material myMaterial)
    {
        myMaterial.color = new Color(1.0f, 1.0f, 1.0f); //��������

        float hitWhiteTime = 0.05f;
        yield return new WaitForSeconds(hitWhiteTime);

        myMaterial.color = new Color(0.5f, 0.5f, 0.5f); //�ʏ�F
    }

    public void DeadBoss(GameObject gameObject, GameObject explosion)
    {
        GameObject bossHPSlider = gameManager.bossHPSlider.gameObject;
        GameObject bossNameText = gameManager.bossNameText.gameObject; 

        StartCoroutine(gameManager.stagingManager.DeadBoss(gameObject, explosion)); //�{�X�����j�����Ƃ��̉��o���Đ�
        gameManager.DestroyEnemyBullet(); //�G�̒e��S�ď���

        bossHPSlider.gameObject.SetActive(false); //�{�X��HP�o�[���\��
        bossNameText.gameObject.SetActive(false); //�{�X�̖��O���\��
        gameManager.KillCountUp();
    }
}
