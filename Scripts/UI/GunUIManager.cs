using UnityEngine;
using TMPro;
using Data;

//�������̕����\������UI���Ǘ�����N���X
public class GunUIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] SpriteRenderer gunImage;
    [SerializeField] TextMeshProUGUI magazineBulletText;
    [SerializeField] TextMeshProUGUI remainBulletText;
    [SerializeField] TextMeshProUGUI gunName1Text;
    [SerializeField] TextMeshProUGUI gunName2Text;

    Animator myAnime;

    //�Q�[���}�l�[�W������ǂݍ��ޕϐ�
    GunManager gunManager;

    //�K���}�l�[�W������ǂݍ��ޕϐ�
    int equipGunNumber;
    float magazineBullet;
    float remainBullet;
    int currentEquipGunNumber;
    float currentMagazineBullet;
    float currentRemainBullet;

    //gun1, gun2�̃R���|�[�l���g������ϐ�
    Sprite gun1Sprite;
    Sprite gun2Sprite;
    Gun gun1Script;
    Gun gun2Script;


    void Start()
    {
        myAnime = gameObject.GetComponent<Animator>();

        gunManager = gameManager.gunManager;

        GameObject gun1 = gunManager.gun1;
        GameObject gun2 = gunManager.gun2;
        gun1Sprite = gun1.GetComponent<SpriteRenderer>().sprite;
        gun2Sprite = gun2.GetComponent<SpriteRenderer>().sprite;
        gun1Script = gun1.GetComponent<Gun>();
        gun2Script = gun2.GetComponent<Gun>();

        equipGunNumber = gunManager.equipGunNumber;

        gunName1Text.text = gun1Script.gunName;
        gunName2Text.text = gun2Script.gunName;

        gunImage.sprite = gun1Sprite;

        myAnime.SetBool("GunAnimeChange", false);
    }


    void Update()
    {  
        //���݂̑������̕���ԍ�����Ɏ擾
        currentEquipGunNumber = gunManager.equipGunNumber;

        //�������̕���̔ԍ��ɂ���ă}�K�W���̒e���Ǝc�e���̕ϐ���؂�ւ���
        //UI��ō��̕��킪�P�ԁA�E�̕��킪�Q��
        SetCurrentBullet();

        //�������̕���̔ԍ����ς������AUI�̃A�j���A�摜�A�}�K�W���̒e���A�c�e����؂�ւ�
        if (currentEquipGunNumber != equipGunNumber)
        {
            EquipGunNumberChange();
        }

        //�}�K�W���̒e�����ς������UI�̃}�K�W���̒e���e�L�X�g�ύX
        if (currentMagazineBullet != magazineBullet)
        {
            MagazineBulletTextChange();
        }

        //�c�e�����ς������UI�̎c�e���e�L�X�g�ύX
        //�c�e��2000�ȏ�́�����
        if (currentRemainBullet != remainBullet)
        {
            RemainBulletTextChange();
        }

        //�����ɃA�j���؂�ւ������Ȃ��ƁA2�𑕔�����GunSelectUIManager�ŕ����ς����Ƃ��A�j�������������Ȃ�
        if (equipGunNumber == 1)
        {
            myAnime.SetBool("GunAnimeChange", false);
        }
        else if (equipGunNumber == 2)
        {
            myAnime.SetBool("GunAnimeChange", true);
        }
    }


    /// <summary>
    /// �������̕���̔ԍ��ɂ���ă}�K�W���̒e���Ǝc�e���̕ϐ���؂�ւ��郁�\�b�h
    /// </summary>
    void SetCurrentBullet()
    {
        //�P�Ԃ𑕔����Ȃ�A�P�Ԃ̕���̃}�K�W���̒e���Ǝc�e����\��
        if (equipGunNumber == 1)
        {
            currentMagazineBullet = gun1Script.magazineBullet;
            currentRemainBullet = gun1Script.remainBullet;
        }
        //�Q�Ԃ𑕔����Ȃ�A�Q�Ԃ̕���̃}�K�W���̒e���Ǝc�e����\��
        else if (equipGunNumber == 2)
        {
            currentMagazineBullet = gun2Script.magazineBullet;
            currentRemainBullet = gun2Script.remainBullet;
        }
    }


    /// <summary>
    /// �������̕���̔ԍ����ς������A�摜�A�}�K�W���̒e���A�c�e����؂�ւ��郁�\�b�h
    /// </summary>
    void EquipGunNumberChange()
    {
        equipGunNumber = currentEquipGunNumber;
        
        //�������̕��킪�P�Ȃ�P�̕���̉摜�A�}�K�W���̒e���A�c�e����\��
        if (equipGunNumber == 1)
        {
            gunImage.sprite = gun1Sprite;
            magazineBullet = gun1Script.magazineBullet;
            remainBullet = gun1Script.remainBullet;
        }
        //�������̕��킪�Q�Ȃ�Q�̕���̉摜�A�}�K�W���̒e���A�c�e����\��
        else if (equipGunNumber == 2)
        {
            gunImage.sprite = gun2Sprite;
            magazineBullet = gun2Script.magazineBullet;
            remainBullet = gun2Script.remainBullet;
        }
    }


    /// <summary>
    /// �}�K�W���̒e�����ς������UI�̃}�K�W���̒e���e�L�X�g�ύX���郁�\�b�h
    /// </summary>
    void MagazineBulletTextChange()
    {
        magazineBullet = currentMagazineBullet;
        magazineBulletText.text = magazineBullet.ToString();
    }


    /// <summary>
    /// �c�e�����ς������UI�̎c�e���e�L�X�g�ύX���郁�\�b�h
    /// </summary>
    void RemainBulletTextChange()
    {
        remainBullet = currentRemainBullet;
        //�c�e��2000�ȏ�̏ꍇ�́��ƕ\��
        if (remainBullet > 2000)
        {
            remainBulletText.text = "��";
        }
        else
        {
            remainBulletText.text = remainBullet.ToString();
        }
    }

    /// <summary>
    /// �����UI�̐؂�ւ����܂Ƃ߂čs�����\�b�h�i�O������̌Ăяo���p�j
    /// </summary>
    public void GunUISetUp()
    {
        GameObject gun1 = gunManager.gun1;
        GameObject gun2 = gunManager.gun2;
        gun1Sprite = gun1.GetComponent<SpriteRenderer>().sprite;
        gun2Sprite = gun2.GetComponent<SpriteRenderer>().sprite;
        gun1Script = gun1.GetComponent<Gun>();
        gun2Script = gun2.GetComponent<Gun>();

        currentEquipGunNumber = gunManager.equipGunNumber;

        gunName1Text.text = gun1Script.gunName;
        gunName2Text.text = gun2Script.gunName;

        EquipGunNumberChange();
        MagazineBulletTextChange();
        RemainBulletTextChange();
    }

}
