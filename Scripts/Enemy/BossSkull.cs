using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// �X�J���L���O�i�{�X�j���Ǘ�����N���X
/// </summary>
public class BossSkull : MonoBehaviour
{
    GameManager gameManager;

    Rigidbody2D myRigidBody;
    Animator myAnime;
    SpriteRenderer mySprite;

    [SerializeField] ParticleSystem particle; //�{�X���U������Ƃ��̃p�[�e�B�N��
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
    AudioClip attack1SE;
    AudioSource audioSource;

    //�U���p�^�[���̎��
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

        //�G�̃X�e�[�^�X��ݒ�
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

        mySprite.material.color = new Color(0.5f, 0.5f, 0.5f); //�ʏ�F

        bossHPSlider = gameManager.bossHPSlider;
        bossNameText = gameManager.bossNameText;

        bossHPSlider.value = 1;
        bossNameText.text = bossName;

        attack1SE = SEManager.SE_BossSkullAttack1;
    }

    void Update()
    {
        //�����i�{�X�j�̈ʒu���擾
        myPos = gameObject.transform.position;

        //�ړ�
        myRigidBody.velocity = new Vector2(walkSpeed * keyRight, walkSpeed * keyUp);

        //�v���C���[�̕����ɂ���ēG�i�����j�̌�����ς���
        if (isMoving) { return; }

        enemyMethod.SpriteFlip(2, mySprite, myPos);

        //�{�X�A�^�b�N�A�j�����I���ɂȂ�����U���A�j�����Đ��i���o�p�j
        if (gameManager.bossAttackAnime)
        {
            myAnime.SetBool("Attack", true);
            audioSource.PlayOneShot(attack1SE); //�U���A�j���Đ�����SE���Đ�
            particle.Play();
            gameManager.bossAttackAnime = false;
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
            //�G��1�b��ɓ����o���A�S�b��ɍU�����J�n
            float enemyMoveStartTime = 1;
            float enemyShootStartTime = 4;

            Invoke("CallEnemyWalk", enemyMoveStartTime);
            Invoke("CallEnemyAttack", enemyShootStartTime);

            gameManager.bossMoveStart = false;
        }
    }


    /// <summary>
    /// �G���ړ�����R���[�`�����Ăяo�����\�b�h
    /// </summary>
    void CallEnemyWalk()
    {
        if (!gameObject.activeSelf) { return; } //��A�N�e�B�u�ɂȂ�����Ă΂Ȃ��悤�ɂ���
        StartCoroutine(EnemyWalk(walkTime, walkStopTime));
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

        //�U���A�j���J�n�A�U�����ʉ��Đ��A�p�[�e�B�N���Đ�
        myAnime.SetBool("Attack", true);
        audioSource.PlayOneShot(attack1SE);
        particle.Play();

        //�U���A�j���ƃ^�C�~���O�����킹�邽�߁A������~���Ă���U���J�n
        yield return new WaitForSeconds(attackInterval / 3);
        
        //�z��attack�̒����烉���_���ōU����I��
        //�I�΂ꂽ�U������attackName�Ɋi�[
        while (true)
        {
            string attackName = enemyMethod.ChooseBossAttack(attack);

            //attackName�Ɋi�[���ꂽ�U�����J�n����
            Invoke(attackName, 0);

            //�A�j���[�V�����ƍU���J�n�̃^�C�~���O�����킹�邽�߁AbulletCreateTime�b��~���Ă���U��
            yield return new WaitForSeconds(bulletCreateTime);

            //bulletInterval�b�̊Ԋu�������AattackNumber�̐������A���ōU��
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

            //�U�����I�������attackInterval�b�U�����Ȃ�
            yield return new WaitForSeconds(attackInterval / 3 * 2);

            if (dead) { yield break; }

            //�U���A�j���J�n�A�U�����ʉ��Đ��A�p�[�e�B�N���Đ�
            myAnime.SetBool("Attack", true);
            audioSource.PlayOneShot(attack1SE);
            particle.Play();

            yield return new WaitForSeconds(attackInterval / 3);
        }
    }

    //�΂���e�ۂ�A���Ŕ���
    void Attack1()
    {
        attackNumber = 60;
        bulletCreateNumber = 1;
        bulletInterval = 0.10f;
        bulletSpeed = 10;
        bulletScale = 1;
        bulletShakeAngle = 20;
    }

    //360���U��
    void Attack2()
    {
        attackNumber = 3;
        bulletCreateNumber = 100;
        bulletInterval = 2.5f;
        bulletSpeed = 5;
        bulletScale = 1;
        bulletShakeAngle = 180;
    }

    //�傫�Ȓe���R�A��
    void Attack3()
    {
        attackNumber = 3;
        bulletCreateNumber = 1;
        bulletInterval = 0.8f;
        bulletSpeed = 10;
        bulletScale = 5;
        bulletShakeAngle = 0;
    }

    //���ɍU��
    void Attack4()
    {
        attackNumber = 2;
        bulletCreateNumber = 60;
        bulletInterval = 2.0f;
        bulletSpeed = 8;
        bulletScale = 1;
        bulletShakeAngle = 45;
    }

    //�A�j�����I�t�ɂ��郁�\�b�h
    public void MyAnimeOff()
    {
        myAnime.SetBool("Attack", false);
        particle.Stop();
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
