using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// �v���C���[�������Ă���e���Ǘ�����N���X
/// </summary>
public class GunManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    PrehubManager prehubManager;

    public GameObject gun1;
    public GameObject gun2;

    Gun gun1Script;
    Gun gun2Script;
    public int equipGunNumber = 1;

    public GameObject muzzleFlash1;
    public GameObject muzzleFlash2;

    Image reloadCircle;

    float timeCountUp;
    bool autoReloadCheck;


    void Awake()
    {
        prehubManager = gameManager.prehubManager;

        reloadCircle = gameManager.reloadCircle;

        StartCoroutine(StartSetUpEquipGun()); //�������̕�����Z�b�g���A�P�t���[���J���Ă���c�e���Z�b�g����
        StartCoroutine(StartSetUpHoldGun()); //�P�t���[���J���Ă���T���̕�����Z�b�g����
    }


    void Update()
    {
        //�E�N���b�N�ő�������؂�ւ�
        if (Input.GetMouseButtonDown(1))
        {
            GunChange();
        }

        if (autoReloadCheck) { return; }

        //�����؂�ւ��Ă���R�b�o������T���̕�������������[�h
        AutoReload();
    }

    /// <summary>
    /// �P�t���[���J���Ă���A�������̕���̃f�[�^��ǂݍ���Ő������A�P�t���[���J���Ă���c�e���Z�b�g����R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator StartSetUpEquipGun()
    {
        //�f�[�^��ǂݍ���ŏe�𐶐�����
        int[] gunClassNumber = GameData.equipGunNumbers;
        GameObject[] _gun = new GameObject[] { prehubManager.SetGun(gunClassNumber[0]), prehubManager.SetGun(gunClassNumber[1]) };
        gun1 = Instantiate(_gun[0]);
        gun1.transform.SetParent(gameObject.transform);
        gun2 = Instantiate(_gun[1]);
        gun2.transform.SetParent(gameObject.transform);

        //������������̏����ݒ������
        gun1.SetActive(true);
        gun2.SetActive(false);

        gun1Script = gun1.GetComponent<Gun>();
        gun2Script = gun2.GetComponent<Gun>();

        gun1Script.enabled = true;
        gun2Script.enabled = true;

        gun1.transform.SetSiblingIndex(0);
        gun2.transform.SetSiblingIndex(1);

        muzzleFlash1 = gun1.transform.Find("MuzzleFlash").gameObject;
        muzzleFlash2 = gun2.transform.Find("MuzzleFlash").gameObject;

        gun1Script.Awake();
        gun2Script.Awake();

        yield return null;

        gun1Script.remainBullet = GameData.equipGunRemainBullets[0];
        gun2Script.remainBullet = GameData.equipGunRemainBullets[1];
    }


    /// <summary>
    /// �P�t���[���J���Ă���A�T���̕���̃f�[�^��ǂݍ���Ő������A�c�e���Z�b�g����R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator StartSetUpHoldGun()
    {
        if (GameData.holdGunNumbers[0] == 0) { yield break; } //�T���̕��킪�Ȃ��Ȃ�u���C�N

        yield return null;

        int i = 0; 

        //�������̕���̈ꗗ��\������
        foreach (int number in GameData.holdGunNumbers)
        {
            if (number == 0) { yield break; } //�T���̕��킪����ȏ�Ȃ��Ȃ�u���C�N

            //�f�[�^��ǂݍ���ŏe�𐶐����A�����ݒ������
            GameObject _gun = prehubManager.SetGun(number);
            GameObject gun = Instantiate(_gun);
            gun.transform.SetParent(gameObject.transform);
            gun.SetActive(false);
            gun.transform.SetSiblingIndex(i + 2); //�������̕��킪0�ԂƂP�Ԃ�����A�T���̕���͂Q�Ԃ���

            Gun gunScript = gun.GetComponent<Gun>();
            gunScript.enabled = true;
            gunScript.Awake();

            gunScript.remainBullet = GameData.holdGunRemainBullets[i];

            i += 1;
        }
    }

    
    /// <summary>
    /// �������镐���؂�ւ��郁�\�b�h
    /// </summary>
    void GunChange()
    {
        reloadCircle.gameObject.SetActive(false);
        timeCountUp = 0;
        autoReloadCheck = false;

        //�P�Ԃ̕���������Ă���Ȃ�Q�Ԃ̕���ɐ؂�ւ�
        if (equipGunNumber == 1)
        {
            equipGunNumber = 2;
            gun2.transform.position = gun1.transform.position;
            gun2.transform.rotation = gun1.transform.rotation;
            gun1.SetActive(false);
            gun2.SetActive(true);
            muzzleFlash1.SetActive(false);
            gun1Script.reloadTimeCountUP = 0;
        }
        //�Q�Ԃ̕���������Ă���Ȃ�P�Ԃ̕���ɐ؂�ւ�
        else if (equipGunNumber == 2)
        {
            equipGunNumber = 1;
            gun1.transform.position = gun2.transform.position;
            gun1.transform.rotation = gun2.transform.rotation;
            gun1.SetActive(true);
            gun2.SetActive(false);
            muzzleFlash2.SetActive(false);
            gun2Script.reloadTimeCountUP = 0;
        }
    }


    /// <summary>
    /// �R�b��������T���̕���������Ń����[�h���郁�\�b�h
    /// </summary>
    void AutoReload()
    {
        timeCountUp += Time.deltaTime;

        if (timeCountUp < 3.0f) { return; }
        
        autoReloadCheck = true;

        if (equipGunNumber == 1)
        {
            float sumBullet = gun2Script.magazineBullet + gun2Script.remainBullet;
            if (sumBullet < gun2Script.magazineSize)
            {
                gun2Script.magazineBullet = sumBullet;
                gun2Script.remainBullet = 0;
            }
            else
            {
                gun2Script.magazineBullet = gun2Script.magazineSize;
                gun2Script.remainBullet -= gun2Script.useBulletCount;
            }
            gun2Script.useBulletCount = 0;
        }
        else if (equipGunNumber == 2)
        {
            float sumBullet = gun1Script.magazineBullet + gun1Script.remainBullet;
            if (sumBullet < gun1Script.magazineSize)
            {
                gun1Script.magazineBullet = sumBullet;
                gun1Script.remainBullet = 0;
            }
            else
            {
                gun1Script.magazineBullet = gun1Script.magazineSize;
                gun1Script.remainBullet -= gun1Script.useBulletCount;
                gun1Script.useBulletCount = 0;
            }
        }
    }
    
    /// <summary>
    /// ����I����ʂő������̕����ς����瑕������̏����ݒ�����郁�\�b�h
    /// </summary>
    public void GunSetUp()
    {
        foreach (Transform gun in gameObject.transform)
        {
            gun.gameObject.SetActive(false);
        }

        if (equipGunNumber == 1)
        {
            gun1.SetActive(true);
        }
        else
        {
            gun2.SetActive(true);
        }
        GunUIManager gunUIManager = gameManager.gunUIManager.GetComponent<GunUIManager>();
        gunUIManager.GunUISetUp();

        gun1Script = gun1.GetComponent<Gun>();
        gun2Script = gun2.GetComponent<Gun>();

        gun1.transform.SetSiblingIndex(0);
        gun2.transform.SetSiblingIndex(1);

        muzzleFlash1 = gun1.transform.Find("MuzzleFlash").gameObject;
        muzzleFlash2 = gun2.transform.Find("MuzzleFlash").gameObject;

        reloadCircle.gameObject.SetActive(false);
    }

    /// <summary>
    /// GameData�Ɍ��݂̏e�̃f�[�^��ۑ����郁�\�b�h
    /// ��ɃV�[���Ԃ̃f�[�^�����p���p
    /// </summary>
    public void SetGunData()
    {
        int[] equipGunNumbers = new int[2];
        int[] holdGunNumbers = new int[20];
        int[] equipGunRemainBullets = new int[2];
        int[] holdGunRemainBullets = new int[20];

        int i = 0;
        int equipNumber = 2; //�������̕���̐�
        int holdNumber = 20; //�T���̕���̐��̍ő�l

        foreach (Transform gun in gameObject.transform)
        {
             Gun gunScript = gun.GetComponent<Gun>();
            int gunClassNumber = gunScript.gunClassNumber;
            int remainBullet = (int)gunScript.remainBullet;

            //�莝���̕���̃f�[�^�̃Z�b�g
            if (i < equipNumber)
            {
                equipGunNumbers[i] = gunClassNumber; //����̕��ޔԍ�
                equipGunRemainBullets[i] = remainBullet; //����̎c�e
            }
            //�T���̕���̃f�[�^�̃Z�b�g
            else if (i < (equipNumber + holdNumber))
            {
                int holdArrayNumber = i - equipNumber;
                holdGunNumbers[holdArrayNumber] = gunClassNumber; //����̕��ޔԍ�
                holdGunRemainBullets[holdArrayNumber] = remainBullet; //����̎c�e
            }
            i += 1;
        }

        GameData.equipGunNumbers = equipGunNumbers;
        GameData.equipGunRemainBullets = equipGunRemainBullets;
        GameData.holdGunNumbers = holdGunNumbers;
        GameData.holdGunRemainBullets = holdGunRemainBullets;
    }
}


