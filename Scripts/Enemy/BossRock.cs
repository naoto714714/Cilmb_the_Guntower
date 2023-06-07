using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// ���K���g���b�N�i�{�X�j���Ǘ�����N���X
/// </summary>
public class BossRock : MonoBehaviour
{
    GameManager gameManager;

    Animator myAnime;
    SpriteRenderer mySprite;

    [SerializeField] GameObject rock;
    [SerializeField] GameObject miniRock;
    [SerializeField] GameObject explosion; //�{�X���j���̔����̃v���n�u

    [SerializeField] Transform trapManager;
    const int trapNumber = 24;
    GameObject[] traps = new GameObject[trapNumber];
    TrapBullet[] trapBullets = new TrapBullet[trapNumber];

    Vector2 rockCreatePosLeft;
    Vector2 rockCreatePosRight;

    //�퓬���̃f�[�^�m�F�p�ɃV���A���C�Y���i�퓬���Ƀf�[�^�͕ς�邽�߁j
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

    //�U���p�^�[���̎��
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

        //�G�̃X�e�[�^�X��ݒ�
        enemyMaxHP = 3500;
        enemyHP = enemyMaxHP;
        attackInterval = 2;
        bulletCreateTime = 1.5f;

        myAnime = gameObject.GetComponent<Animator>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();

        enemyMethod = gameManager.enemyMethod;
        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        mySprite.material.color = new Color(0.5f, 0.5f, 0.5f); //�ʏ�F

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
        //�{�X�A�^�b�N�A�j�����I���ɂȂ�����U���A�j�����Đ��i���o�p�j
        if (gameManager.bossAttackAnime)
        {
            myAnime.SetTrigger("Attack");
            audioSource.PlayOneShot(attack1SE); //�U���A�j���Đ�����SE���Đ�
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
            float enemyShootStartTime = 2;

            Invoke("CallEnemyAttack", enemyShootStartTime);

            gameManager.bossMoveStart = false;
        }
    }

    /// <summary>
    /// �G���U������R���[�`�����Ăяo�����\�b�h
    /// </summary>
    void CallEnemyAttack()
    {
        if (!gameObject.activeSelf) { return; } //��A�N�e�B�u�ɂȂ�����Ă΂Ȃ��悤�ɂ���
        coroutine =  StartCoroutine(EnemyAttack());
    }

    /// <summary>
    /// �G���A��莞�ԍU�������A��莞�ԍU�����~����R���[�`��
    /// </summary>
    IEnumerator EnemyAttack()
    {
        if (dead) { yield break; }

        //�U���A�j���J�n�A�U�����ʉ��Đ��A�p�[�e�B�N���Đ�
        myAnime.SetTrigger("Attack");
        audioSource.PlayOneShot(attack1SE);

        //�z��attack�̒����烉���_���ōU����I��
        //�I�΂ꂽ�U������attackName�Ɋi�[
        while (true)
        {
            string attackName = enemyMethod.ChooseBossAttack(attack);

            //�A�j���[�V�����ƍU���J�n�̃^�C�~���O�����킹�邽�߁AbulletCreateTime�b��~���Ă���U��
            yield return new WaitForSeconds(bulletCreateTime);

            //attackName�Ɋi�[���ꂽ�U�����J�n����
            Invoke(attackName, 0);

            yield return null;

            //attackContinueTime�̎��Ԃ����U���𑱂���
            yield return new WaitForSeconds(attackContinueTime);

            TrapOff();

            //�U�����I�������attackInterval�b�U�����Ȃ�
            yield return new WaitForSeconds(attackInterval);

            if (dead) { yield break; }

            //�U���A�j���J�n�A�U�����ʉ��Đ��A�p�[�e�B�N���Đ�
            myAnime.SetTrigger("Attack");
            audioSource.PlayOneShot(attack1SE);
        }
    }
    
    //�����S�̂ɐ���U��
    void Attack1()
    {
        TrapOn();
        ChangeTrapStatus(3f, 1, 3);
        attackContinueTime = 4.0f;
    }

    //�����̍�����e�Ŗ��ߐs�����A�E�ɓG������
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

    //�����̉E����e�Ŗ��ߐs�����A���ɓG������
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

    //���S�ɑf�����U��
    void Attack4()
    {
        for (int i = 8; i < 16; i++)
        {
            traps[i].SetActive(true);
        }
        ChangeTrapStatus(10, 1, 6);
        attackContinueTime = 2.0f;
    }

    //���E�Ƀ~�j���b�N������
    void Attack5()
    {
        Instantiate(miniRock, rockCreatePosLeft, Quaternion.identity);
        Instantiate(miniRock, rockCreatePosRight, Quaternion.identity);
        attackContinueTime = 2.0f;
    }

    //���E�Ƀ��b�N������
    void Attack6()
    {
        Instantiate(rock, rockCreatePosLeft, Quaternion.identity);
        Instantiate(rock, rockCreatePosRight, Quaternion.identity);
        attackContinueTime = 6.0f;
    }


    //�A�j�����I�t�ɂ��郁�\�b�h
    public void MyAnimeOff()
    {
        //�X�e�[�W�}�l�[�W���[����Ăԃ{�X������
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
    public void HitBullet(float damage)
    {
        enemyHP -= damage; //�_���[�W�̕������_���[�W��H�炤 

        //�{�X�����j
        if (enemyHP <= 0)
        {
            DeadBoss();
        }

        bossHPSlider.value = enemyHP / enemyMaxHP;

        StartCoroutine(enemyMethod.DoWhite(mySprite.material));
    }

    /// <summary>
    /// �S�Ẵg���b�v���I���ɂ��郁�\�b�h
    /// </summary>
    void TrapOn()
    {
        for (int i = 0; i < trapNumber; i++)
        {
            traps[i].SetActive(true);
        }
    }

    /// <summary>
    /// �S�Ẵg���b�v���I�t�ɂ��郁�\�b�h
    /// </summary>
    void TrapOff()
    {
        for (int i = 0; i < trapNumber; i++)
        {
            traps[i].SetActive(false);
        }
    }

    /// <summary>
    /// �g���b�v�̃X�e�[�^�X��ς��郁�\�b�h
    /// </summary>
    /// <param name="interval">���ˊԊu</param>
    /// <param name="scale">�e�̑傫��</param>
    /// <param name="speed">�e�̃X�s�[�h</param>
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
    /// �{�X�����j�����Ƃ��̃��\�b�h
    /// </summary>
    void DeadBoss()
    {
        dead = true;
        enemyMethod.DeadBoss(gameObject, explosion);

        MyAnimeOff(); //�A�j�����~
        TrapOff();
        StopCoroutine(coroutine);

        //���ׂĂ̓G���A�N�e�B�u��
        //�f�X�g���C���ƕ������J���Ă��܂�
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemys.Length == 0) { return; }

        //�����ȊO�̓G���A�N�e�B�u
        foreach (GameObject enemy in enemys)
        {
            if (enemy.name == "BossRock") { continue; }
            enemy.SetActive(false);
        }
    }
}
