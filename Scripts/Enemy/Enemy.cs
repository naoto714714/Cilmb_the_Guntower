using UnityEngine;
using System.Collections;
using Calc;
using Data;

/// <summary>
/// �G�L�����N�^�[���Ǘ�����N���X
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// �G�̎��
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

        //�G��1�`1.5�b�̊Ԃɓ����o���A�����o������0�`1.5�b�̊ԂɍU�����J�n
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

        //�ړ�
        myRigidBody.velocity = new Vector2(walkSpeed * keyRight, walkSpeed * keyUp);

        //�v���C���[�̕����ɂ���ēG�i�����j�̌�����ς���
        //�e�������Ȃ��ːi�L�����͈ړ����Ɍ�����ς��Ȃ�
        if (isMoving && bulletCreateNumber == 0) { return; }

        enemyMethod.SpriteFlip(1, mySprite, myPos);
    }


    /// <summary>
    /// �G���ړ�����R���[�`�����Ăяo�����\�b�h
    /// </summary>
    void CallEnemyWalk()
    {
        if (!gameObject.activeSelf) { return; } //��A�N�e�B�u�ɂȂ�����Ă΂Ȃ��悤�ɂ���
        if (isAttackAndMove == 1) { return; } //�U�����ɕK�������L�����́A�����œ����R���[�`�����쓮���Ȃ�
        move = StartCoroutine(EnemyWalk(walkTime, walkStopTime));
    }

    /// <summary>
    /// �G����莞�ԓ��������Ɉړ������A��莞�Ԓ�~������R���[�`��
    /// </summary>
    /// <param name="walkTime">�ړ����鎞��</param>
    /// <param name="WalkStopTime">��~���鎞��</param>
    IEnumerator EnemyWalk(float walkTime, float WalkStopTime)
    {
        if (walkTime == 0) { yield break; }

        //�ړ��iwalkTime�b�j����~�iwalkStopTime�b)���ړ��iwalkTime�b�j����~�iwalkStopTime�b�j���E�E�E
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
    /// �G���ړ�������������߂Ĉړ����郁�\�b�h�A�ړ�������key�z��̒��Ō��߂�
    /// </summary>
    void EnemyMoveStart()
    {
        enemyMethod.EnemyMoveStart(ref keyRight, ref keyUp, myPos, isWalkRandom);
        myAnime.SetBool("Idle", false);
        myAnime.SetBool("Walk", true);
        isMoving = true;
    }


    /// <summary>
    /// �ړ����~�����郁�\�b�h
    /// </summary>
    void EnemyMoveStop()
    {
        keyRight = 0;
        keyUp = 0;
        myAnime.SetBool("Walk", false);
        myAnime.SetBool("Idle", true);
    }


    /// <summary>
    /// �G���U������R���[�`�����Ăяo�����\�b�h
    /// </summary>
    void CallEnemyAttack()
    {
        if (!gameObject.activeSelf) { return; } //��A�N�e�B�u�ɂȂ�����Ă΂Ȃ��悤�ɂ���
        StartCoroutine(EnemyAttack());
    }

    /// <summary>
    /// �G���A��莞�ԍU�������A��莞�ԍU�����~����R���[�`��
    /// </summary>
    IEnumerator EnemyAttack()
    {
        if (bulletCreateNumber == 0) { yield break; }

        while (true)
        {
            //�v���C���[��attackRange���ɂ���Ȃ�U���J�n
            if (Calculate.Distance(myPos, playerPos) < attackRange)
            {
                myAnime.SetTrigger("Attack");
                
                //�U�����ɕK���ړ�����L�����͂����œ����R���[�`�����쓮
                if (isAttackAndMove == 1)
                {
                    move = StartCoroutine(EnemyWalk(walkTime, walkStopTime));
                }
                //�U�����ɕK���~�܂�L�����͂����œ����R���[�`�����~
                if (isAttackAndStop == 1)
                {
                    StopCoroutine(move);
                    EnemyMoveStop();
                }

                //�A�j���[�V�����ƍU���J�n�̃^�C�~���O�����킹�邽�߁AbulletCreateTime�b��~���Ă���U��
                yield return new WaitForSeconds(bulletCreateTime);

                //bulletInterval�b�̊Ԋu�������AattackNumber�̐������A���ōU��
                for (int i = 0; i < attackNumber; i++)
                {
                    enemyMethod.CreateBullet(myPos, bulletCreateNumber, bulletSpeed, bulletDamage, bulletScale, bulletShakeAngle);
                    yield return new WaitForSeconds(bulletInterval);
                }
                    
                //�U�����ɕK���ړ�����L�����͂����œ����R���[�`�����~
                if (isAttackAndMove == 1)
                {
                    StopCoroutine(move);
                    EnemyMoveStop();
                }
                
                //�U�����ɕK���~�܂�L�����͂����œ����R���[�`�����쓮
                if (isAttackAndStop == 1)
                {
                    yield return new WaitForSeconds(1.0f); //�����҂��Ă��瓮���o���A�����𑝂₵�����Ȃ��悤�ɂ����̎��Ԃ͌Œ�
                    move = StartCoroutine(EnemyWalk(walkTime, walkStopTime));
                }
            }

            //�U�����I�������attackInterval�b�U�����Ȃ�
            yield return new WaitForSeconds(attackInterval);
        }
    }

    /// <summary>
    /// �ʏ�̒e�ɖ��������Ƃ��ɌĂԃ��\�b�h
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HitBullet(enemyMethod.GetDamage(collision.gameObject));
        }
    }


    /// <summary>
    /// �ђʒe�ɖ��������Ƃ��ɌĂԃ��\�b�h
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HitBullet(enemyMethod.GetDamage(collision.gameObject));
        }   
    }


    /// <summary>
    /// �e�ɖ��������Ƃ��A�������点�A�_���[�W����炤���\�b�h
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
    /// �G��|��������ʉ����Đ����A�L���J�E���g�𑝂₵�A�������f�X�g���C���郁�\�b�h
    /// </summary>
    void DeadEnemy()
    {
        audioSource.PlayOneShot(enemyDownSE); //�G���|�ꂽ�Ƃ���SE
        gameManager.KillCountUp();
        Destroy(gameObject);
        dead = true;
    }
}
