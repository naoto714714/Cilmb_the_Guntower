using UnityEngine;
using Calc;

/// <summary>
/// �v���C���[�ƓG�̒e�ۂ̐ݒ������N���X
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
    /// �v���C���[�ɗ^����_���[�W���Z�b�g���郁�\�b�h
    /// </summary>
    /// <param name="damage">�_���[�W�̒l</param>
    public void SetDamageToPlayer(float damage)
    {
        damageToPlayer = damage;
    }


    /// <summary>
    /// �v���C���[�̒e�̃X�e�[�^�X���Z�b�g���郁�\�b�h
    /// </summary>
    /// <param name="damage">�_���[�W�̗�</param>
    /// <param name="force">�m�b�N�o�b�N�̒l</param>
    /// <param name="isThrough">�ђʂ��邩�ǂ���</param>
    /// <param name="isBound">���˕Ԃ邩�ǂ���</param>
    /// <param name="isExplosion">�������邩�ǂ���</param>
    /// <param name="explosionDistance">��������</param>
    /// <param name="explosionDamage">�����_���[�W</param> 
    /// <param name="explosionSize">�����T�C�Y</param>
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

        if (isThrough) //�e���g���K�[�ɂ��Ċђʂ�����
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }


    /// <summary>
    /// �e���Ȃɂ��ɓ��������Ƃ��̏���
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
	{
        //isExplosion���P�Ȃ甚��(Explosion���\�b�h�Ńm�b�N�o�b�N������)
        if (isExplosion)
        {
            Explosion(explosionDistance, explosionDamage, explosionSize);
        }
        //���������̂��G�Ȃ�m�b�N�o�b�N
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            KnockBack(collision.gameObject);
        }

        //isBound���P�Ȃ���ł��Ȃ��Œ��˕Ԃ�i�e�̕����}�e���A���̒e���͂͂P�j
        if (isBound) { return; }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //�G�ɂԂ�������m�b�N�o�b�N������
        if (collision.gameObject.CompareTag("Enemy"))
        {
            KnockBack(collision.gameObject);
        }
        //�G�ȊO�ɂԂ������玩���i�e�j���f�X�g���C
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �m�b�N�o�b�N�����郁�\�b�h
    /// </summary>
    /// <param name="collisionObject">�Փ˂�������</param>
    void KnockBack(GameObject collisionObject)
    {
        Vector2 myPos = gameObject.transform.position;
        Vector2 collisionPos = collisionObject.transform.position;
        Vector2 forceVector = Calculate.To_Circle(myPos, collisionPos, 1.0f);

        //�Ԃ����������Ƌt�����ɗ͂�������
        Vector2 force = new Vector2(forceVector.x * this.force, forceVector.y * this.force);
        collisionObject.GetComponent<Rigidbody2D>().AddForce(force);
    }

    /// <summary>
    /// �������郁�\�b�h
    /// </summary>
    /// <param name="explosionDistance">�����_���[�W�����炤����</param>
    /// <param name="explosionDamage">�����_���[�W</param>
    /// <param name="explosionSize">�����T�C�Y</param>
    void Explosion(float explosionDistance, int explosionDamage, float explosionSize)
    {
        //�S�ẴG�l�~�[�I�u�W�F�N�g���擾
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.CameraShake(0.1f, 1.0f); //�J������U��

        PrehubManager prehubManager = gameManager.prehubManager;
        Vector3 myPos = gameObject.transform.position;

        //�����̃v���n�u�𐶐����A�R�b��ɂ��̃v���n�u���f�X�g���C
        GameObject explosionObj = Instantiate(prehubManager.explosion, myPos, Quaternion.identity);
        gameManager.DestroyObj(explosionObj, 3.0f);

        //�����̑傫����ς���A�ʏ�̑傫����0.6
        float normalScale = 0.4f;
        explosionObj.transform.localScale = new Vector2(explosionSize * normalScale, explosionSize * normalScale);

        if (enemys.Length == 0) { return; }
        
        //�����͈͓��ɂ���G�l�~�[�Ƀ_���[�W��^���A�m�b�N�o�b�N������
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