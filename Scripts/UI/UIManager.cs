using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// UI���Ǘ�����N���X
/// ��ɁA�Ə��̉摜���}�E�X�̈ʒu�ɍ��킹�A�n�[�g�ƃ{����UI���쐬����
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    PrehubManager prehubManager;

    Canvas myCanvas;
    RectTransform myCanvasRect;

    bool entrance = false;

    //�Q�[���}�l�[�W������ǂݍ��ޕϐ�
    Player playerScript;
    RectTransform aimImageRect;
    GameObject bomb;
    GameObject heart;

    //�v���C���[����ǂݍ��ޕϐ�
    int playerHP;
    int remainBomb;

    //UI���i�[����z��
    GameObject[] heartUI;
    GameObject[] bombUI;


    void Start()
    {
        prehubManager = gameManager.prehubManager;

        entrance = gameManager.entrance;

        myCanvas = gameObject.GetComponent<Canvas>();
        myCanvasRect = myCanvas.GetComponent<RectTransform>();

        playerScript = gameManager.playerScript;
        aimImageRect = gameManager.aimImageRect;
        bomb = prehubManager.bombUIPrehub;
        heart = prehubManager.heartUIPrehub;

        playerHP = playerScript.playerHP;
        remainBomb = playerScript.remainBomb;
        int heartUINumber = playerScript.playerMaxHP / 2; //�ő�HP�̔����̐��A�n�[�g��UI�𐶐�����

        gameManager.DisplayAimImage(); //�J�[�\�����\���ɂ��A�Ə���\��

        //�G���g�����X�Ȃ�A�n�[�g�ƃ{����UI�𐶐����Ȃ�
        if (entrance) { return; }

        RectTransform heartUIParent = gameManager.heartUITransform;
        RectTransform bombUIParent = gameManager.bombUITransform;

        CreateUI(ref heartUI, heart, heartUINumber, 110, 80, -70, heartUIParent); //���l�́AUI�Ԃ̃X�y�[�X�A��ʍ��ォ���X���W��Y���W
        CreateUI(ref bombUI, bomb, playerScript.remainBomb, 110, 80, -180, bombUIParent);
    }

 
    void FixedUpdate()
    {  
        //FixedUpdate����Ȃ���Update���ƏƏ��̉摜���K�N�K�N����
        //aimImage�̈ʒu���A�}�E�X�̈ʒu�ƍ��킹��
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvasRect, Input.mousePosition, myCanvas.worldCamera, out Vector2 mousePos);
        aimImageRect.anchoredPosition = new Vector2(mousePos.x, mousePos.y);
    }

    void Update()
    {
        if (entrance) { return; }

        int currentPlayerHP = playerScript.playerHP;
        int currentRemainBomb = playerScript.remainBomb;

        //�v���C���[��HP���ω�������UI���X�V
        if (currentPlayerHP != playerHP)
        {
            if (currentPlayerHP <= playerScript.playerMaxHP)
            {
                StartCoroutine(UpdateHeartUI(currentPlayerHP));
            }
        }

        //�v���C���[�̃{���̐�����������A�{����UI�����炵�ăt���b�V������
        if (currentRemainBomb < remainBomb)
        {
            DecreaseUI(ref bombUI, remainBomb - currentRemainBomb);
            remainBomb = currentRemainBomb;
            gameManager.Flash();
        }
        //�v���C���[�̃{������������A�{����UI�𑝂₷
        else if (currentRemainBomb > remainBomb)
        {
            IncreaseUI(ref bombUI, bomb, 110, 80, -180, currentRemainBomb - remainBomb);
            remainBomb = currentRemainBomb;
        }
    }

    /// <summary>
    /// �v���n�u��UI���A������A���J�[�Ƃ��AUISpace���Ԋu���J���Ȃ��琶�����郁�\�b�h
    /// </summary>
    /// <param name="gameObjects">���������Q�[���I�u�W�F�N�g���i�[����z��</param>
    /// <param name="gameObject">�����������Q�[���I�u�W�F�N�g</param>
    /// <param name="arrayLength">������������</param>
    /// <param name="UISpace">UI���m�̊Ԋu</param>
    /// <param name="x">��ʍ��ォ��́A�P�ڂ�X���W</param>
    /// <param name="y">��ʍ��ォ��́A�P�ڂ�Y���W</param>
    void CreateUI(ref GameObject[] gameObjects,GameObject gameObject, int arrayLength, int UISpace, int x, int y, RectTransform parent)
    {
        Array.Resize(ref gameObjects, arrayLength);
        float space = UISpace;
        Vector3 UIPos = new Vector3(x, y, 0);

        for (int i = 0; i < arrayLength; i++)
        {
            gameObjects[i] = Instantiate(gameObject);

            //UI�̔z�u�Ƒ傫���̐ݒ�
            RectTransform objectRect = gameObjects[i].GetComponent<RectTransform>();
            objectRect.transform.SetParent(parent.transform);
            objectRect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            objectRect.anchoredPosition = UIPos;

            UIPos.x += space;
        }
    }

    //�O������HP��UI�̍X�V���s�����߂̃��\�b�h
    public void CallUpdateHeartUI()
    {
        StartCoroutine(UpdateHeartUI(playerScript.playerHP));
    }


    /// <summary>
    /// ���݂�HP�ɂ���āA�n�[�g��UI�̍X�V����R���[�`��
    /// </summary>
    /// <param name="currentPlayerHP">���݂�HP</param>
    IEnumerator UpdateHeartUI(int currentPlayerHP)
    {
        int HPFullNumber = currentPlayerHP / 2;
        int HPHalfCheck = currentPlayerHP % 2;
        
        //�z��̂O�`playerHeart�܂ŁA�n�[�g�̃A�j����Full��
        //HP����̏ꍇ�AplayerHeart+1�̃n�[�g�̃A�j����Half��
        //�c��̃n�[�g�̃A�j����Empty�ɂ���
        for (int i = 0; i < HPFullNumber; i++)
        {
            Animator heartAnime = heartUI[i].GetComponent<Animator>();
            heartAnime.SetTrigger("Full");
        }

        if (HPHalfCheck == 1)
        {
            Animator heartAnime = heartUI[HPFullNumber].GetComponent<Animator>();
            heartAnime.SetTrigger("Half");
            HPFullNumber += 1;
        }

        for (int i = HPFullNumber; i < heartUI.Length; i++)
        {
            Animator heartAnime = heartUI[i].GetComponent<Animator>();
            heartAnime.SetTrigger("Empty");
        }

        yield return null;

        playerHP = currentPlayerHP;
    }


    /// <summary>
    /// �\�����Ă���UI�����炷���\�b�h
    /// </summary>
    /// <param name="gameObjects">���炵�����Q�[���I�u�W�F�N�g���i�[����Ă���z��</param>
    /// <param name="number">���炷��</param>
    void DecreaseUI(ref GameObject[] gameObjects, int number)
    {
        //�z��̍Ō�̃Q�[���I�u�W�F�N�g������
        for (int i = 0; i < number; i++)
        {
            if (gameObjects.Length == 0)
            {
                return;
            }
            int arrayLast = gameObjects.Length - 1;
            Destroy(bombUI[arrayLast]);
            Array.Resize(ref gameObjects, arrayLast);
        }
    }


    /// <summary>
    /// �\�����Ă���UI�𑝂₷���\�b�h
    /// </summary>
    /// <param name="gameObjects">���₵�����Q�[���I�u�W�F�N�g���i�[����Ă���z��</param>
    /// <param name="gameObject">���₵�����Q�[���I�u�W�F�N�g</param>
    /// <param name="UISpace">UI���m�̊Ԋu</param>
    /// <param name="x">��ʍ��ォ��́A�P�ڂ�X���W</param>
    /// <param name="y">��ʍ��ォ��́A�P�ڂ�Y���W</param>
    /// <param name="number">���₵������</param>
    void IncreaseUI(ref GameObject[] gameObjects, GameObject gameObject, int UISpace, int x, int y, int number)
    {
        //�z��̍Ō�ɃQ�[���I�u�W�F�N�g��ǉ�
        for (int i = 0; i < number; i++)
        {
            int arrayLast = gameObjects.Length + 1;
            float space = UISpace * arrayLast - UISpace;
            Vector3 UIPos = new Vector3(x + space, y, 0);

            Array.Resize(ref gameObjects, arrayLast);
            gameObjects[arrayLast - 1] = Instantiate(gameObject);
            RectTransform objectRect = gameObjects[arrayLast - 1].GetComponent<RectTransform>();
            RectTransform bombUIParent = gameManager.bombUITransform;
            objectRect.transform.SetParent(bombUIParent.transform);
            objectRect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            objectRect.anchoredPosition = UIPos;
        }
    }
}
