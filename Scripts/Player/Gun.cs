using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Calc;

/// <summary>
/// プレイヤーの銃を管理するクラス
/// </summary>
public class Gun : MonoBehaviour
{
	GameManager gameManager;
	PrehubManager prehubManager;
	SEManager SEManager;
	
	public int gunClassNumber;
	[SerializeField] int gunSENumber; //銃の効果音の種類
	public int magazineSize; //マガジンサイズ
	public int maxBullet; //最大の残弾
	[SerializeField] float reloadTime; //リロード時間
	[SerializeField] int bulletCreateNumber; //一度の発射数
	[SerializeField] float bulletInterval; //発射間隔
	[SerializeField] float bulletSpeed; //弾丸の速度
	[SerializeField] float bulletScale; //弾丸の大きさ
	[SerializeField] int bulletDamage; //ダメージ
	[SerializeField] float bulletKnockBack; //ノックバックの大きさ
	[SerializeField] float bulletShakeAngle; //最大ブレ角度
	[SerializeField] float bulletDestroyTime; //射程
	[SerializeField] float cameraShakeTime; //カメラの揺れの時間
	[SerializeField] float cameraShakeSize; //カメラの揺れの大きさ
	[SerializeField] float muzzleFlashPosX; //マズルフラッシュの位置
	[SerializeField] float muzzleFlashScale; //マズルフラッシュの大きさ
	[SerializeField] float explosionDistance; //爆発範囲
	[SerializeField] int explosionDamage; //爆風ダメージ
	[SerializeField] float explosionSize;
	[SerializeField] bool isBulletThrough; //弾が貫通するかどうか
	[SerializeField] bool isBulletBound; //弾が跳ね返るかどうか
	[SerializeField] bool isBulletExplosion; //爆発するかどうか
	[SerializeField] bool consecutive; //弾の当たり判定は連続的かどうか
	public string gunName;

	Transform myTransform;
	SpriteRenderer mySprite;


	//ゲームマネージャから読み込む変数
	Player playerScript;
	GameObject bullet;
	Image reloadCircle;

	//ゲームマネージャから読み込み常に更新する変数
	Vector3 playerPos;
	Vector3 aimImagePos;

	//自分の子オブジェクトのマズルフラッシュ
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
		mySprite.flipX = true; //銃の向きは通常左向き

		//ゲームマネージャから読み込む
		playerScript = gameManager.playerScript;
		reloadCircle = gameManager.reloadCircle;

		//consecutiveがfalseなら衝突判定が非連続的な弾、trueなら衝突判定が連続的な弾
		if (consecutive)
        {
			bullet = prehubManager.playerConsecutiveBulletPrehub;
		}
		else
		{
			bullet = prehubManager.playerBulletPrehub;
		}
		//マズルフラッシュの設定
		muzzleFlash = myTransform.Find("MuzzleFlash");
		muzzleFlash.localPosition = new Vector2(muzzleFlashPosX, 0);
		muzzleFlash.localScale = new Vector2(muzzleFlashScale, muzzleFlashScale);

		//効果音の種類を設定
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
		//ゲームマネージャから読み込み常に更新する
		playerPos = gameManager.playerPos;
		aimImagePos = gameManager.aimImagePos;
		
		//プレイヤーのHPが０になったら銃を非表示に
		if (playerScript.playerHP == 0)
		{
			gameObject.SetActive(false);
		}

		GunTransform(); //銃のTransformを変えるメソッド
		GunRotation(); //銃のRotationを変えるメソッド

		if (gameManager.noShoot) { return; } //ゲームマネージャーのノーシュートがtrueなら撃てないように

		Shoot(); //弾丸を生成するメソッドを呼び出すメソッド

		//所持中の弾数が最大弾数を超えないように
		if (remainBullet > maxBullet)
        {
			remainBullet = maxBullet;
        }
		
		//ローリング中は銃を透明に
		mySprite.color = playerScript.rollingCheck ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);

		//リロード中はリロードできないように
		if (reloadCheck) { return; }
		
		//残弾が０ならリロードできないように
		if (remainBullet == 0) { return; }

		//Rキーでリロード
		if (Input.GetKeyDown(KeyCode.R) && magazineBullet != magazineSize)
		{
			magazineBullet = 0;
		}
	}

    /// <summary>
    /// 銃のTransformを変えるメソッド
    /// </summary>
    void GunTransform()
	{
		//円の半径は1.0
		const float circleRadius = 1.0f;

		//プレイヤーの位置からマウスの位置に向け、半径circleRadiusの円を描くように移動する
		myTransform.localPosition = Calculate.To_Circle(playerPos, aimImagePos, circleRadius);
	}

	/// <summary>
	/// 銃のRotationを変えるメソッド
	/// </summary>
	void GunRotation()
    {
		//照準の向きに銃の角度を変える
		double angleAimImage = Calculate.Angle_RightTriangle(playerPos, aimImagePos);
		myTransform.rotation = Quaternion.Euler(0, 0, (float)angleAimImage);

		//銃の位置がプレイヤーの位置より右なら通常、左なら上下反転
		bool flipCheck = myTransform.localPosition.x >= 0;
		mySprite.flipY = flipCheck ? false : true;
	}


	/// <summary>
	/// 弾丸の生成とリロードを管理するメソッド
	/// </summary>
	void Shoot()
	{
		//マガジンの残弾が０になったらリロード
		if (magazineBullet == 0)
		{	
			Reload();
		}

		//マウスの左クリックが押されていて、前回の射撃からbulletInterval秒以上経過していたら"CreateBullet"メソッド呼び出し
		else if (Input.GetMouseButton(0) && shotTimeCountUp >= bulletInterval)
		{
			//ローリング中は無効
			if (playerScript.rollingCheck) { return; }

			CreateBullet(); //発射する弾丸生成
			useBulletCount += 1;
			gameManager.CameraShake(cameraShakeTime, cameraShakeSize); //カメラの振動
			StartCoroutine (SetActiveInterval(muzzleFlash.gameObject, muzzleFlashTime)); //マズルフラッシュ
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
	/// 弾丸を生成するメソッド
	/// </summary>
	void CreateBullet()
	{
		for (int i = 0; i < bulletCreateNumber; i++)
		{
			// 弾（ゲームオブジェクト）の生成//弾（ゲームオブジェクトの生成）
			Vector3 bulletPos = new Vector3(myTransform.position.x, myTransform.position.y, 0);
			GameObject bulletClone = Instantiate(bullet, bulletPos, Quaternion.identity);;
			bulletClone.transform.parent = gameManager.playerTransform;

			//弾の大きさを変える、通常の大きさは0.4
			float normalScale = 0.4f;
			bulletClone.transform.localScale = new Vector2(bulletScale * normalScale, bulletScale * normalScale);

			//弾にダメージとノックバックの値を与える
			Bullet bulletScript = bulletClone.GetComponent<Bullet>();
			bulletScript.SetBulletToEnemy(bulletDamage, bulletKnockBack, isBulletThrough, isBulletBound, isBulletExplosion, explosionDistance, explosionDamage, explosionSize);

			//発射方向から、最大bulletShakeAngle°ブラし、その方向に速度を掛け、弾に速度を与える
			Vector2 origin = new Vector2(0, 0);
			Vector2 shootDirection = Calculate.To_Circle(origin, Calculate.ShakeAngle(playerPos, aimImagePos, bulletShakeAngle), 1.0f);
			bulletClone.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;

			//bulletDestroyTime秒後に消滅（射程）
			Destroy(bulletClone, bulletDestroyTime);
		}
	}

	
	/// <summary>
	/// リロードするメソッド
	/// </summary>
	void Reload()
    {
		if (remainBullet == 0 && !reloadCheck) { return; }

		float sumBullet = remainBullet + (magazineSize - useBulletCount);
		if (!reloadCheck)
        {
			//残弾がマガジンサイズより少ないときにリロードしたら残弾を０に（マイナスにならないように）
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

		//リロードの時間を示す円オブジェクトを表示
		reloadCircle.gameObject.SetActive(true);
		reloadTimeCountUP += Time.deltaTime;
		reloadCircle.fillAmount = reloadTimeCountUP / reloadTime;

		//始めの１回に効果音を鳴らす
		if (!reloadCheck) { audioSource.PlayOneShot(reloadSE); }
		
		//リロード中にリロードできないように
		reloadCheck = true;

		//reloadTimeCountUpがreloadタイムを超えたら終了
		if (reloadTimeCountUP > reloadTime)
		{
			//残弾がマガジンサイズより少ないときにリロードしたら残弾を全てマガジンに（マガジンサイズまで回復しないように）
			if (remainBulletNone)
			{
				magazineBullet = reloadBullet;
			}
			else
			{
				magazineBullet = magazineSize;
			}
			
			shotTimeCountUp = bulletInterval; //リロード後すぐに撃てるように
			reloadTimeCountUP = 0;
			reloadCircle.gameObject.SetActive(false);
			reloadCheck = false;
		}
	}


	/// <summary>
	/// gameObjectをinterval秒だけアクティブにするコルーチン
	/// </summary>
	/// <param name="gameObject">アクティブにしたいゲームオブジェクト</param>
	/// <param name="interval">アクティブにする秒数</param>
	/// <returns></returns>
	IEnumerator SetActiveInterval(GameObject gameObject, float interval)
    {
		gameObject.SetActive(true);

		yield return new WaitForSeconds(interval);

		gameObject.SetActive(false);
    }
}