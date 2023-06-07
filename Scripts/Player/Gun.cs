using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Calc;

/// <summary>
/// �v���C���[�̏e���Ǘ�����N���X
/// </summary>
public class Gun : MonoBehaviour
{
	GameManager gameManager;
	PrehubManager prehubManager;
	SEManager SEManager;
	
	public int gunClassNumber;
	[SerializeField] int gunSENumber; //�e�̌��ʉ��̎��
	public int magazineSize; //�}�K�W���T�C�Y
	public int maxBullet; //�ő�̎c�e
	[SerializeField] float reloadTime; //�����[�h����
	[SerializeField] int bulletCreateNumber; //��x�̔��ː�
	[SerializeField] float bulletInterval; //���ˊԊu
	[SerializeField] float bulletSpeed; //�e�ۂ̑��x
	[SerializeField] float bulletScale; //�e�ۂ̑傫��
	[SerializeField] int bulletDamage; //�_���[�W
	[SerializeField] float bulletKnockBack; //�m�b�N�o�b�N�̑傫��
	[SerializeField] float bulletShakeAngle; //�ő�u���p�x
	[SerializeField] float bulletDestroyTime; //�˒�
	[SerializeField] float cameraShakeTime; //�J�����̗h��̎���
	[SerializeField] float cameraShakeSize; //�J�����̗h��̑傫��
	[SerializeField] float muzzleFlashPosX; //�}�Y���t���b�V���̈ʒu
	[SerializeField] float muzzleFlashScale; //�}�Y���t���b�V���̑傫��
	[SerializeField] float explosionDistance; //�����͈�
	[SerializeField] int explosionDamage; //�����_���[�W
	[SerializeField] float explosionSize;
	[SerializeField] bool isBulletThrough; //�e���ђʂ��邩�ǂ���
	[SerializeField] bool isBulletBound; //�e�����˕Ԃ邩�ǂ���
	[SerializeField] bool isBulletExplosion; //�������邩�ǂ���
	[SerializeField] bool consecutive; //�e�̓����蔻��͘A���I���ǂ���
	public string gunName;

	Transform myTransform;
	SpriteRenderer mySprite;


	//�Q�[���}�l�[�W������ǂݍ��ޕϐ�
	Player playerScript;
	GameObject bullet;
	Image reloadCircle;

	//�Q�[���}�l�[�W������ǂݍ��ݏ�ɍX�V����ϐ�
	Vector3 playerPos;
	Vector3 aimImagePos;

	//�����̎q�I�u�W�F�N�g�̃}�Y���t���b�V��
	Transform muzzleFlash;

	float shotTimeCountUp;
	public float reloadTimeCountUP;
	public float magazineBullet;
	public float remainBullet;
	public float useBulletCount = 0;
	const float muzzleFlashTime = 0.15f;
	float reloadBullet;

	bool reloadCheck = false;
	bool remainBulletNone = false;

	AudioClip shootSE;
	AudioClip reloadSE;
	AudioSource audioSource;

    public void Awake()
    {
		shotTimeCountUp = bulletInterval;
		magazineBullet = magazineSize;
		remainBullet = maxBullet;
	}


    void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		prehubManager = gameManager.prehubManager;
		SEManager = gameManager.SEManager;
		audioSource = gameManager.audioSourceSE;

		myTransform = gameObject.GetComponent<Transform>();
		mySprite = gameObject.GetComponent<SpriteRenderer>();
		mySprite.flipX = true; //�e�̌����͒ʏ퍶����

		//�Q�[���}�l�[�W������ǂݍ���
		playerScript = gameManager.playerScript;
		reloadCircle = gameManager.reloadCircle;

		//consecutive��false�Ȃ�Փ˔��肪��A���I�Ȓe�Atrue�Ȃ�Փ˔��肪�A���I�Ȓe
		if (consecutive)
        {
			bullet = prehubManager.playerConsecutiveBulletPrehub;
		}
		else
		{
			bullet = prehubManager.playerBulletPrehub;
		}
		//�}�Y���t���b�V���̐ݒ�
		muzzleFlash = myTransform.Find("MuzzleFlash");
		muzzleFlash.localPosition = new Vector2(muzzleFlashPosX, 0);
		muzzleFlash.localScale = new Vector2(muzzleFlashScale, muzzleFlashScale);

		//���ʉ��̎�ނ�ݒ�
		switch (gunSENumber)
        {
			case 1:
				shootSE = SEManager.SE_ShotNormal;
				break;

			case 2:
				shootSE = SEManager.SE_ShotLight;
				break;

			case 3:
				shootSE = SEManager.SE_ShotHeavy;
				break;
        }
		reloadSE = SEManager.SE_Reload;
		
	}


	void Update()
	{
		//�Q�[���}�l�[�W������ǂݍ��ݏ�ɍX�V����
		playerPos = gameManager.playerPos;
		aimImagePos = gameManager.aimImagePos;
		
		//�v���C���[��HP���O�ɂȂ�����e���\����
		if (playerScript.playerHP == 0)
		{
			gameObject.SetActive(false);
		}

		GunTransform(); //�e��Transform��ς��郁�\�b�h
		GunRotation(); //�e��Rotation��ς��郁�\�b�h

		if (gameManager.noShoot) { return; } //�Q�[���}�l�[�W���[�̃m�[�V���[�g��true�Ȃ猂�ĂȂ��悤��

		Shoot(); //�e�ۂ𐶐����郁�\�b�h���Ăяo�����\�b�h

		//�������̒e�����ő�e���𒴂��Ȃ��悤��
		if (remainBullet > maxBullet)
        {
			remainBullet = maxBullet;
        }
		
		//���[�����O���͏e�𓧖���
		mySprite.color = playerScript.rollingCheck ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);

		//�����[�h���̓����[�h�ł��Ȃ��悤��
		if (reloadCheck) { return; }
		
		//�c�e���O�Ȃ烊���[�h�ł��Ȃ��悤��
		if (remainBullet == 0) { return; }

		//R�L�[�Ń����[�h
		if (Input.GetKeyDown(KeyCode.R) && magazineBullet != magazineSize)
		{
			magazineBullet = 0;
		}
	}

    /// <summary>
    /// �e��Transform��ς��郁�\�b�h
    /// </summary>
    void GunTransform()
	{
		//�~�̔��a��1.0
		const float circleRadius = 1.0f;

		//�v���C���[�̈ʒu����}�E�X�̈ʒu�Ɍ����A���acircleRadius�̉~��`���悤�Ɉړ�����
		myTransform.localPosition = Calculate.To_Circle(playerPos, aimImagePos, circleRadius);
	}

	/// <summary>
	/// �e��Rotation��ς��郁�\�b�h
	/// </summary>
	void GunRotation()
    {
		//�Ə��̌����ɏe�̊p�x��ς���
		double angleAimImage = Calculate.Angle_RightTriangle(playerPos, aimImagePos);
		myTransform.rotation = Quaternion.Euler(0, 0, (float)angleAimImage);

		//�e�̈ʒu���v���C���[�̈ʒu���E�Ȃ�ʏ�A���Ȃ�㉺���]
		bool flipCheck = myTransform.localPosition.x >= 0;
		mySprite.flipY = flipCheck ? false : true;
	}


	/// <summary>
	/// �e�ۂ̐����ƃ����[�h���Ǘ����郁�\�b�h
	/// </summary>
	void Shoot()
	{
		//�}�K�W���̎c�e���O�ɂȂ����烊���[�h
		if (magazineBullet == 0)
		{	
			Reload();
		}

		//�}�E�X�̍��N���b�N��������Ă��āA�O��̎ˌ�����bulletInterval�b�ȏ�o�߂��Ă�����"CreateBullet"���\�b�h�Ăяo��
		else if (Input.GetMouseButton(0) && shotTimeCountUp >= bulletInterval)
		{
			//���[�����O���͖���
			if (playerScript.rollingCheck) { return; }

			CreateBullet(); //���˂���e�ې���
			useBulletCount += 1;
			gameManager.CameraShake(cameraShakeTime, cameraShakeSize); //�J�����̐U��
			StartCoroutine (SetActiveInterval(muzzleFlash.gameObject, muzzleFlashTime)); //�}�Y���t���b�V��
			audioSource.PlayOneShot(shootSE);
			
			magazineBullet -= 1;
			shotTimeCountUp = 0.0f;
		}
		else
		{
			shotTimeCountUp += Time.deltaTime;
		}
	}

	/// <summary>
	/// �e�ۂ𐶐����郁�\�b�h
	/// </summary>
	void CreateBullet()
	{
		for (int i = 0; i < bulletCreateNumber; i++)
		{
			// �e�i�Q�[���I�u�W�F�N�g�j�̐���//�e�i�Q�[���I�u�W�F�N�g�̐����j
			Vector3 bulletPos = new Vector3(myTransform.position.x, myTransform.position.y, 0);
			GameObject bulletClone = Instantiate(bullet, bulletPos, Quaternion.identity);;
			bulletClone.transform.parent = gameManager.playerTransform;

			//�e�̑傫����ς���A�ʏ�̑傫����0.4
			float normalScale = 0.4f;
			bulletClone.transform.localScale = new Vector2(bulletScale * normalScale, bulletScale * normalScale);

			//�e�Ƀ_���[�W�ƃm�b�N�o�b�N�̒l��^����
			Bullet bulletScript = bulletClone.GetComponent<Bullet>();
			bulletScript.SetBulletToEnemy(bulletDamage, bulletKnockBack, isBulletThrough, isBulletBound, isBulletExplosion, explosionDistance, explosionDamage, explosionSize);

			//���˕�������A�ő�bulletShakeAngle���u�����A���̕����ɑ��x���|���A�e�ɑ��x��^����
			Vector2 origin = new Vector2(0, 0);
			Vector2 shootDirection = Calculate.To_Circle(origin, Calculate.ShakeAngle(playerPos, aimImagePos, bulletShakeAngle), 1.0f);
			bulletClone.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;

			//bulletDestroyTime�b��ɏ��Łi�˒��j
			Destroy(bulletClone, bulletDestroyTime);
		}
	}

	
	/// <summary>
	/// �����[�h���郁�\�b�h
	/// </summary>
	void Reload()
    {
		if (remainBullet == 0 && !reloadCheck) { return; }

		float sumBullet = remainBullet + (magazineSize - useBulletCount);
		if (!reloadCheck)
        {
			//�c�e���}�K�W���T�C�Y��菭�Ȃ��Ƃ��Ƀ����[�h������c�e���O�Ɂi�}�C�i�X�ɂȂ�Ȃ��悤�Ɂj
			if (sumBullet < magazineSize)
            {
				reloadBullet = sumBullet;
				remainBullet = 0;
				remainBulletNone = true;
            }
            else
            {
				remainBullet -= useBulletCount;
            }
        }

		useBulletCount = 0;

		//�����[�h�̎��Ԃ������~�I�u�W�F�N�g��\��
		reloadCircle.gameObject.SetActive(true);
		reloadTimeCountUP += Time.deltaTime;
		reloadCircle.fillAmount = reloadTimeCountUP / reloadTime;

		//�n�߂̂P��Ɍ��ʉ���炷
		if (!reloadCheck) { audioSource.PlayOneShot(reloadSE); }
		
		//�����[�h���Ƀ����[�h�ł��Ȃ��悤��
		reloadCheck = true;

		//reloadTimeCountUp��reload�^�C���𒴂�����I��
		if (reloadTimeCountUP > reloadTime)
		{
			//�c�e���}�K�W���T�C�Y��菭�Ȃ��Ƃ��Ƀ����[�h������c�e��S�ă}�K�W���Ɂi�}�K�W���T�C�Y�܂ŉ񕜂��Ȃ��悤�Ɂj
			if (remainBulletNone)
			{
				magazineBullet = reloadBullet;
			}
			else
			{
				magazineBullet = magazineSize;
			}
			
			shotTimeCountUp = bulletInterval; //�����[�h�シ���Ɍ��Ă�悤��
			reloadTimeCountUP = 0;
			reloadCircle.gameObject.SetActive(false);
			reloadCheck = false;
		}
	}


	/// <summary>
	/// gameObject��interval�b�����A�N�e�B�u�ɂ���R���[�`��
	/// </summary>
	/// <param name="gameObject">�A�N�e�B�u�ɂ������Q�[���I�u�W�F�N�g</param>
	/// <param name="interval">�A�N�e�B�u�ɂ���b��</param>
	/// <returns></returns>
	IEnumerator SetActiveInterval(GameObject gameObject, float interval)
    {
		gameObject.SetActive(true);

		yield return new WaitForSeconds(interval);

		gameObject.SetActive(false);
    }
}