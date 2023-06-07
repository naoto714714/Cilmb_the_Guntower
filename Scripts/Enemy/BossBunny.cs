using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// �J�[�X�o�j�[�i�{�X�j���Ǘ�����N���X
/// </summary>
public class BossBunny : MonoBehaviour
{
    GameManager gameManager;

    Rigidbody2D myRigidBody;
    Animator myAnime;
    SpriteRenderer mySprite;

    [SerializeField] GameObject explosion; //�{�X���j���̔����̃v���n�u

    //�퓬���̃f�[�^�m�F�p�ɃV���A���C�Y���i�퓬���Ƀf�[�^�͕ς�邽�߁j
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
    AudioClip appearSE;
    AudioSource audioSource;

    Coroutine move;

    //�U���p�^�[���̎��
    string[] attack = new string[]
    {
        "Attack1",
        "Attack2",
        "Attack3",
        "Attack4",
        "Attack5"
    };

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //�G�̃X�e�[�^�X��ݒ�
        enemyMaxHP = 3500;
        enemyHP = (int)enemyMaxHP;
        attackInterval = 1.4f;
        bulletDamage = 1;
        bulletCreateTime = 1;

        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myAnime = gameObject.GetComponent<Animator>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();

        enemyMethod = gameManager.enemyMethod;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        myAnime.SetBool("Attack", false);

        mySprite.material.color = new Color(0.5f, 0.5f, 0.5f); //�ʏ�F

        bossHPSlider = gameManager.bossHPSlider;
        bossNameText = gameManager.bossNameText;

        bossHPSlider.value = 1;
        bossNameText.text = bossName;

        appearSE = SEManager.SE_BossBunny;
    }

    void Update()
    {
        //�����i�{�X�j�̈ʒu���擾
        myPos = gameObject.transform.position;

        //�ړ�
        myRigidBody.velocity = new Vector2(walkSpeed * keyRight, walkSpeed * keyUp);

        //�v���C���[�̕����ɂ���ēG�i�����j�̌�����ς���
        if (isMoving) { return; }

        enemyMethod.SpriteFlip(1, mySprite, myPos);

        //�{�X�A�^�b�N�A�j�����I���ɂȂ�����U���A�j�����Đ��i���o�p�j
        if (gameManager.bossAttackAnime)
        {
            myAnime.SetBool("Attack", true);
            gameManager.bossAttackAnime = false;
            audioSource.PlayOneShot(appearSE);
            Invoke("MyAnimeOff", 3);
        }

        //�{�X�A�^�b�N�A�j���I�t���I���ɂȂ�����U���A�j�����~�i���o�p�j
        if (gameManager.bossAttackAnimeOff)
        {
            MyAnimeOff();
            gameManager.bossAttackAnimeOff = false;
        }

        //���[�u�X�^�[�g���I���ɂȂ�����ړ��ƍU�����J�n
        if (gameManager.bossMoveStart)
        {
            //�R�b��ɍU�����J�n
            float enemyShootStartTime = 3;

            Invoke("CallEnemyAttack", enemyShootStartTime);

            gameManager.bossMoveStart = false;
        }
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
            if (dead) { yield break; }

            //�ړ��J�n
            enemyMethod.EnemyMoveStart(ref keyRight, ref keyUp, myPos, 0);
            isMoving = true;

            yield return new WaitForSeconds(walkTime); //walkTime�̎��ԕ��ړ�����

            isMoving = false;

            yield return null;

            if (WalkStopTime < 0.1) { continue; }

            //�ړ���~
            EnemyMoveStop();

            yield return new WaitForSeconds(WalkStopTime); //walkStopTime�̎��ԕ���~����
        }
    }

    /// <summary>
    /// �ړ����~�����郁�\�b�h
    /// </summary>
    void EnemyMoveStop()
    {
        keyRight = 0;
        keyUp = 0;
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
        if (dead) { yield break; }

        //�U���A�j���ƃ^�C�~���O�����킹�邽�߁A������~���Ă���U���J�n
        yield return new WaitForSeconds(attackInterval / 3);
        
        //�z��attack�̒����烉���_���ōU����I��
        //�I�΂ꂽ�U������attackName�Ɋi�[
        while (true)
        {
            walkSpeed = 0;

            string attackName = enemyMethod.ChooseBossAttack(attack);            

            //attackName�Ɋi�[���ꂽ�U�����J�n����
            Invoke(attackName, 0);
            yield return null;

            //�U���A�j���J�n�A�U�����ʉ��Đ��A�p�[�e�B�N���Đ�
            myAnime.SetBool("Attack", true);
            move = StartCoroutine(EnemyWalk(walkTime, walkStopTime));

            //�A�j���[�V�����ƍU���J�n�̃^�C�~���O�����킹�邽�߁AbulletCreateTime�b��~���Ă���U��
            yield return new WaitForSeconds(bulletCreateTime);

            //bulletInterval�b�̊Ԋu�������AattackNumber�̐������A���ōU��
            for (int i = 0; i < attackNumber; i++)
            {
                if (dead) { yield break; }
                enemyMethod.CreateBullet(myPos, bulletCreateNumber, bulletSpeed, bulletDamage, bulletScale, bulletShakeAngle);
                yield return new WaitForSeconds(bulletInterval);  
            }

            yield return new WaitForSeconds(1.0f);

            myAnime.SetBool("Attack", false);
            StopCoroutine(move);
            EnemyMoveStop();

            //�U�����I�������attackInterval�b�U�����Ȃ�
            yield return new WaitForSeconds(attackInterval / 3 * 2);

            if (dead) { yield break; }

            yield return new WaitForSeconds(attackInterval / 3);
        }
    }

    //�f�����ړ����Ȃ���v���C���[�����ɔ͈͍U��
    void Attack1()
    {
        attackNumber = 2;
        bulletCreateNumber = 20;
        bulletInterval = 0.3f;
        bulletSpeed = 6;
        bulletScale = 1;
        bulletShakeAngle = 25;
        walkSpeed = 10;
        walkTime = 0.5f;
        walkStopTime = 10;
    }

    //�v���C���[�ɋ߂Â��Ȃ���S�̍U��
    void Attack2()
    {
        attackNumber = 3;
        bulletCreateNumber = 100;
        bulletInterval = 2.5f;
        bulletSpeed = 5;
        bulletScale = 1;
        bulletShakeAngle = 180;
        walkSpeed = 4;
        walkTime = 0.3f;
        walkStopTime = 0;
    }

    //�~�܂��Ēe���T���U�炷
    void Attack3()
    {
        attackNumber = 40;
        bulletCreateNumber = 3;
        bulletInterval = 0.1f;
        bulletSpeed = 5;
        bulletScale = 1;
        bulletShakeAngle = 180;
        walkSpeed = 0;
        walkTime = 10f;
        walkStopTime = 0;
        bulletCreateTime = 1;
    }

    //�傫���Ēx���e�𔭎�
    void Attack4()
    {
        attackNumber = 5;
        bulletCreateNumber = 5;
        bulletInterval = 0.5f;
        bulletSpeed = 1;
        bulletScale = 3;
        bulletShakeAngle = 180;
        walkSpeed = 2;
        walkTime = 10f;
        walkStopTime = 0;
        bulletCreateTime = 1;
    }

    //�U�������ɑf�����ړ�
    void Attack5()
    {
        attackNumber = 0;
        bulletCreateNumber = 0;
        bulletInterval = 0.0f;
        bulletSpeed = 0;
        bulletScale = 0;
        bulletShakeAngle = 0;
        walkSpeed = 10;
        walkTime = 2f;
        walkStopTime = 10f;
        bulletCreateTime = 1;
    }

    //�A�j�����I�t�ɂ��郁�\�b�h
    public void MyAnimeOff()
    {
        myAnime.SetBool("Attack", false);
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
    /// �e�ɖ��������Ƃ��A�����𔒂����点�A�_���[�W����炤���\�b�h
    /// </summary>
    public void HitBullet(int damage)
    {
        enemyHP -= damage; //�_���[�W�̕������_���[�W��H�炤 

        //�{�X�����j
        if (enemyHP <= 0)
        {
            dead = true;
            EnemyMoveStop(); //�ړ����~
            MyAnimeOff(); //�A�j�����~
            enemyMethod.DeadBoss(gameObject, explosion);
        }

        bossHPSlider.value = enemyHP / enemyMaxHP;

        StartCoroutine(enemyMethod.DoWhite(mySprite.material));
    }
}
