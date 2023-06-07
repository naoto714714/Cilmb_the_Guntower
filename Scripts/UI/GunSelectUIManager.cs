using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;

/// <summary>
/// ����I����ʂ��Ǘ�����N���X
/// </summary>
public class GunSelectUIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    KeyInputManager keyInputManager;

    const int equipGunNumber = 2;
    const int holdGunNumber = 10;

    [SerializeField] Image[] equipGunImages = new Image[equipGunNumber];
    [SerializeField] Image[] holdGunImages = new Image[holdGunNumber];
    [SerializeField] TextMeshProUGUI gunNameText;
    [SerializeField] TextMeshProUGUI remainBulletText;
    [SerializeField] TextMeshProUGUI gunExplanation;
    [SerializeField] GunSelectUIArrow arrowUp;
    [SerializeField] GunSelectUIArrow arrowDown;

    GameObject[] holdGunsObj = new GameObject[holdGunNumber]; 

    Animator myAnime;
    public int animeNumber = 0;
    public int dragGunNumber = 0;
    public bool isDrag = false;
    bool wasNoControl = false;
    bool wasNoShoot = false;

    //�Q�[���}�l�[�W������ǂݍ��ޕϐ�
    GunManager gunManager;

    //�K���}�l�[�W������ǂݍ��ޕϐ�
    GameObject[] equipGuns = new GameObject[equipGunNumber];
    GameObject[] holdGuns = new GameObject[holdGunNumber];

    string[] equipGunNames = new string[equipGunNumber];
    string[] holdGunNames = new string[holdGunNumber];

    float[] equipRemainBullets = new float[equipGunNumber];
    float[] holdRemainBullets = new float[holdGunNumber];

    string[] equipGunExplanations = new string[equipGunNumber];
    string[] holdGunExplanations = new string[holdGunNumber];

    GameObject gunUIManager;
    GameObject miniMap;
    GameObject mapPoint;

    SEManager SEManager;
    AudioClip openSE;
    AudioClip closeSE;
    AudioClip cursorSE;
    AudioClip decideSE;
    AudioSource audioSource;
    bool SECheck;


    void Awake()
    {
        keyInputManager = gameManager.keyInputManager;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GunSelectOpen;
        closeSE = SEManager.SE_GunSelectClose;
        cursorSE = SEManager.SE_GunSelectCursor;
        decideSE = SEManager.SE_GunSelectDecide;

        gunUIManager = gameManager.gunUIManager;
        miniMap = gameManager.miniMap.gameObject;
        mapPoint = gameManager.mapPoint;

        int i = 0;
        foreach (Image holdGun in holdGunImages)
        {
            holdGunsObj[i] = holdGun.gameObject;
            holdGunsObj[i].SetActive(false);
            i += 1;
        }
    }


    /// <summary>
    /// ����I����ʂ��N�����̃��\�b�h
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE); //�e�I����ʂ��J�����Ƃ���SE

        wasNoControl = false;
        wasNoShoot = false;

        //�����Ȃ��悤��
        Time.timeScale = 0;
        //���ɓ����Ȃ���ԂȂ������������Ȃ��悤��
        if (keyInputManager.noControl) { wasNoControl = true; }
        else { keyInputManager.noControl = true; }
        //���Ɍ��ĂȂ���ԂȂ����������ĂȂ��悤��
        if (gameManager.noShoot) { wasNoShoot = true; }
        else { gameManager.noShoot = true; }

        //UI����������\��
        gunUIManager.SetActive(false);
        miniMap.SetActive(false);
        mapPoint.SetActive(false);

        gameObject.transform.SetAsLastSibling();
        gameManager.DisplayCursor(); //�Ə��������ăJ�[�\����\��

        myAnime = gameObject.GetComponent<Animator>();

        gunManager = gameManager.gunManager;

        gunManager.muzzleFlash1.SetActive(false);
        gunManager.muzzleFlash2.SetActive(false);

        int i = 0;

        //�������̕���̈ꗗ��\������
        foreach (Transform gun in gunManager.transform)
        {
            SpriteRenderer gunImage = gun.GetComponent<SpriteRenderer>();
            Gun gunScript = gun.GetComponent<Gun>();
            int gunClassNumber = gunScript.gunClassNumber;

            //�n�߂̂Q�͑������̕���̗��ɕ\��
            if (i < equipGunNumber)
            {
                equipGuns[i] = gun.gameObject;
                equipGunImages[i].sprite = gunImage.sprite;
                equipGunNames[i] = gunScript.gunName;
                equipRemainBullets[i] = gunScript.remainBullet;
                equipGunExplanations[i] = GunData.SetGunExplanation(gunClassNumber);
            }
            //�n�߂̂Q�ȍ~�͏������̕���̗��ɕ\������
            else if(i < (equipGunNumber + holdGunNumber))
            {
                int holdArrayNumber = i - equipGunNumber;
                holdGuns[holdArrayNumber] = gun.gameObject;
                holdGunImages[holdArrayNumber].sprite = gunImage.sprite;
                holdGunNames[holdArrayNumber] = gunScript.gunName;
                holdRemainBullets[holdArrayNumber] = gunScript.remainBullet;
                holdGunExplanations[holdArrayNumber] = GunData.SetGunExplanation(gunClassNumber);
                holdGunsObj[holdArrayNumber].SetActive(true);
            }
            //�������킪�P�O�𒴂�����A�����I���ɂ��A�Q�y�[�W�ڂɂ�����悤�ɂ���
            else
            {
                arrowDown.ActiveArrow();
                break;
            }
            i += 1;
        }
    }


    void Update()
    {
        myAnime.SetInteger("SelectUIStatus", animeNumber);
        
        if (isDrag) { return; } //�h���b�O���͕���Ȃ��悤��
        //Tab�L�[�������ꂽ�畐��I����ʂ��\����
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameManager.DisplayAimImage(); //�J�[�\���������ďƏ���\��
            audioSource.PlayOneShot(closeSE); //�e�I����ʂ�����Ƃ��̉�

            //�������̕���̗��ɂ��镐����A��������悤�ɐ؂�ւ��A�����ݒ���s��
            gunManager.gun1 = equipGuns[0];
            gunManager.gun2 = equipGuns[1];
            gunManager.GunSetUp();

            //��\���ɂ���UI��\��
            gunUIManager.SetActive(true);
            miniMap.SetActive(true);
            mapPoint.SetActive(true);

            //����I����ʂ��\���ɂ��A������悤�ɂ���
            gameObject.SetActive(false);
            Time.timeScale = 1;
            if (!wasNoControl) { keyInputManager.noControl = false; }
            if (!wasNoShoot) { gameManager.noShoot = false; }
        }
    }

    /// <summary>
    /// ����ɃJ�[�\�������킹����A���̕���̖��O�E�c�e�E��������\�����郁�\�b�h
    /// </summary>
    public void TextChange()
    {   
        //�J�[�\����SE�A�Ȃɂ��Ȃ��Ƃ���ɃJ�[�\�������킹����SECheck��false�ɂ���
        if (!SECheck)
        {
            audioSource.PlayOneShot(cursorSE);
            SECheck = true;
        }

        float remainBullet = 0;
        //�������̕���ɃJ�[�\�������킹����A���̕��햼�Ɛ�������\��
        if (animeNumber > 0)
        {
            int arrayNumber = animeNumber - 1;
            gunNameText.text = holdGunNames[arrayNumber];
            remainBullet = holdRemainBullets[arrayNumber];
            gunExplanation.text = holdGunExplanations[arrayNumber];

        }
        //�������̕���ɃJ�[�\������������A���̕��햼�Ɛ�������\��
        else if (animeNumber < 0)
        {
            int arrayNumber = (animeNumber * -1) - 1;
            gunNameText.text = equipGunNames[arrayNumber];
            remainBullet = equipRemainBullets[arrayNumber];
            gunExplanation.text = equipGunExplanations[arrayNumber];
        }

        //�c�e�̕\��
        //�c�e��2000���ȏ�Ȃ�A���ƕ\������
        if (remainBullet > 2000)
        {
            remainBulletText.text = "�i�c�F�� ���j";
        }
        //2000�������Ȃ�c�e��\��
        else
        {
            remainBulletText.text = "�i�c�F" + remainBullet + " ���j";
        }
    }

    /// <summary>
    /// ���햼�A�c�e�A���������\���ɂ��郁�\�b�h
    /// </summary>
    public void TextNone()
    {
        SECheck = false;
        gunNameText.text = null;
        remainBulletText.text = null;
        gunExplanation.text = null;
    }

    /// <summary>
    /// �������̕����؂�ւ��郁�\�b�h
    /// </summary>
    /// <param name="equipGunNumber">���ݑ������Ă��镐��̔ԍ�</param>
    /// <param name="holdGunNumber">��������������̔ԍ�</param>
    public void EquipGunChange(int equipGunNumber, int holdGunNumber)
    {
        audioSource.PlayOneShot(decideSE); //��������ւ����Ƃ���SE

        int equipArrayNumber = equipGunNumber * -1 - 1;

        //���ݑ������Ă��镐��̐ݒ���ꎞ�I�ɕۑ��i����ւ��邽�߁j
        GameObject tmpObject = equipGuns[equipArrayNumber];
        Sprite tmpSprite = equipGunImages[equipArrayNumber].sprite;
        string tmpGunNameText = equipGunNames[equipArrayNumber];
        float tmpRemainBulletText = equipRemainBullets[equipArrayNumber];
        string tmpGunExplanationText = equipGunExplanations[equipArrayNumber];

        //�������̕���P�Ƒ������̕���Q�̓���ւ����s���ꍇ
        if (holdGunNumber < 0)
        {
            int holdArrayNumber = holdGunNumber * -1 - 1;
            //�I�u�W�F�N�g�̔�������ւ�
            equipGuns[equipArrayNumber] = equipGuns[holdArrayNumber];
            equipGuns[holdArrayNumber] = tmpObject;

            //����̉摜�����ւ�
            equipGunImages[equipArrayNumber].sprite = equipGunImages[holdArrayNumber].sprite;
            equipGunImages[holdArrayNumber].sprite = tmpSprite;

            //�c�e�����ւ�
            equipRemainBullets[equipArrayNumber] = equipRemainBullets[holdArrayNumber];
            equipRemainBullets[holdArrayNumber] = tmpRemainBulletText;

            //���햼�����ւ�
            equipGunNames[equipArrayNumber] = equipGunNames[holdArrayNumber];
            equipGunNames[holdArrayNumber] = tmpGunNameText;

            //�����������ւ�
            equipGunExplanations[equipArrayNumber] = equipGunExplanations[holdArrayNumber];
            equipGunExplanations[holdArrayNumber] = tmpGunExplanationText;
        }
        //�������̕���Ƒ������̕���̓���ւ����s���ꍇ
        else
        {
            int holdArrayNumber = holdGunNumber - 1;
            //�I�u�W�F�N�g�̔�������ւ�
            equipGuns[equipArrayNumber] = holdGuns[holdArrayNumber];
            holdGuns[holdArrayNumber] = tmpObject;

            //����̉摜�����ւ�
            equipGunImages[equipArrayNumber].sprite = holdGunImages[holdArrayNumber].sprite;
            holdGunImages[holdArrayNumber].sprite = tmpSprite;

            //����̎c�e�����ւ�
            equipRemainBullets[equipArrayNumber] = holdRemainBullets[holdArrayNumber];
            holdRemainBullets[holdArrayNumber] = tmpRemainBulletText;

            //���햼�����ւ�
            equipGunNames[equipArrayNumber] = holdGunNames[holdArrayNumber];
            holdGunNames[holdArrayNumber] = tmpGunNameText;

            //�����������ւ�
            equipGunExplanations[equipArrayNumber] = holdGunExplanations[holdArrayNumber];
            holdGunExplanations[holdArrayNumber] = tmpGunExplanationText;
        }
    }

    void OffHoldGuns()
    {
        foreach (GameObject obj in holdGunsObj)
        {
            obj.SetActive(false);
        }

        for (int i = 0; i < holdGunNumber; i++)
        {
            holdGuns[i] = null;
            holdGunImages[i].sprite = null;
            holdGunNames[i] = null;
            holdRemainBullets[i] = 0;
            holdGunExplanations[i] = null;
        }
    }

    public void Page1()
    {
        arrowUp.NormalArrow();
        arrowDown.ActiveArrow();
        OffHoldGuns();

        int i = 0;

        //�������̕���̈ꗗ��\������
        foreach (Transform gun in gunManager.transform)
        {
            if (i < equipGunNumber)
            {
                i += 1;
                continue;
            }
            SpriteRenderer gunImage = gun.GetComponent<SpriteRenderer>();
            Gun gunScript = gun.GetComponent<Gun>();
            int gunClassNumber = gunScript.gunClassNumber;

            //1�y�[�W�ڂɎ��܂�Ȃ�����͂Q�y�[�W�ڂ�
            if (i < (equipGunNumber + holdGunNumber))
            {
                int holdArrayNumber = i - equipGunNumber;
                holdGuns[holdArrayNumber] = gun.gameObject;
                holdGunImages[holdArrayNumber].sprite = gunImage.sprite;
                holdGunNames[holdArrayNumber] = gunScript.gunName;
                holdRemainBullets[holdArrayNumber] = gunScript.remainBullet;
                holdGunExplanations[holdArrayNumber] = GunData.SetGunExplanation(gunClassNumber);
                holdGunsObj[holdArrayNumber].SetActive(true);
            }
            i += 1;
        }
    }

    public void Page2()
    {
        arrowUp.ActiveArrow();
        arrowDown.NormalArrow();
        OffHoldGuns();

        int i = 0;

        //�������̕���̈ꗗ��\������
        foreach (Transform gun in gunManager.transform)
        {
            if (i < equipGunNumber + holdGunNumber)
            {
                i += 1;
                continue;
            }
            SpriteRenderer gunImage = gun.GetComponent<SpriteRenderer>();
            Gun gunScript = gun.GetComponent<Gun>();
            int gunClassNumber = gunScript.gunClassNumber;

            //1�y�[�W�ڂɎ��܂�Ȃ�����͂Q�y�[�W�ڂ�
            if (i < (equipGunNumber + holdGunNumber + holdGunNumber))
            {
                int holdArrayNumber = i - (equipGunNumber + holdGunNumber);
                holdGuns[holdArrayNumber] = gun.gameObject;
                holdGunImages[holdArrayNumber].sprite = gunImage.sprite;
                holdGunNames[holdArrayNumber] = gunScript.gunName;
                holdRemainBullets[holdArrayNumber] = gunScript.remainBullet;
                holdGunExplanations[holdArrayNumber] = GunData.SetGunExplanation(gunClassNumber);
                holdGunsObj[holdArrayNumber].SetActive(true);
            }
            i += 1;
        }
    }
}
