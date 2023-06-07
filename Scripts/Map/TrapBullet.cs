using UnityEngine;
using System.Collections;
using Calc;

/// <summary>
/// ������ɒe�𔭎˂�������g���b�v�̃N���X
/// </summary>
public class TrapBullet : MonoBehaviour
{
    GameManager gameManager;
    PrehubManager prehubManager;

    /// <summary>
    /// �e�̔��ˊԊu
    /// </summary>
    public float bulletInterval;
    /// <summary>
    /// �e�𔭎˂������
    /// �P�F��A�Q�F���A�R�F�k�A�S�F��
    /// </summary>
    public int bulletDirection;
    /// <summary>
    /// ���˂���e�̑傫��
    /// </summary>
    public float bulletScale;
    /// <summary>
    /// ���˂���e�̃X�s�[�h
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
    /// �G���U������R���[�`�����Ăяo�����\�b�h
    /// </summary>
    void CallTrapAttack()
    {
        if (!gameObject.activeSelf) { return; } //��A�N�e�B�u�ɂȂ�����Ă΂Ȃ��悤�ɂ���
        StartCoroutine(TrapAttack());
    }

    /// <summary>
    /// �G���A��莞�ԍU�������A��莞�ԍU�����~����R���[�`��
    /// </summary>
    IEnumerator TrapAttack()
    {
        yield return null; //������yirld return�����Ȃ��ƁA���W(0,0)�ɒe�����������

        while (true)
        {
            //�v���C���[��attackRange���ɂ���Ȃ�U���J�n
            if (Calculate.Distance(myPos, playerPos) < attackRange)
            {
                CreateBullet();
            }
            yield return new WaitForSeconds(bulletInterval); //bulletInterval�b�̊Ԋu���J���A�ĂэU��
        }
    }

    /// <summary>
    /// �e�ۂ𐶐����郁�\�b�h
    /// </summary>
    void CreateBullet()
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

        //���˕�����ς���
        Vector2 shootDirection = new Vector2(0, 0);
        switch (bulletDirection)
        {
            case 1: //��Ɍ���
                shootDirection = new Vector2(0, -1);
                break;

            case 2: //���Ɍ���
                shootDirection = new Vector2(1, 0);
                break;

            case 3: //�k�Ɍ���
                shootDirection = new Vector2(0, 1);
                break;

            case 4: //���Ɍ���
                shootDirection = new Vector2(-1, 0);
                break;
        }
        bulletClone.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
    }
}
