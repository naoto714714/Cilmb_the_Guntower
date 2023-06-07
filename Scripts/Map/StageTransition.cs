using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Calc;

/// <summary>
/// �v���C���[���ʂ蔲�����Ƃ��A��ʂ�J�ڂ�����N���X
/// </summary>
public class StageTransition : MonoBehaviour
{
    /// <summary>
    /// ��������������A�k�F�P�C���F�Q�C��F�R�A���F�S
    /// </summary>
    [SerializeField] int directionNumber;
    /// <summary>
    /// ��ʑJ�ڐ���w��
    /// </summary>
    [SerializeField] int transitionStageNumber;

    GameManager gameManager;
    SEManager SEManager;

    Transform myTransform;

    AudioClip stairsSE;
    AudioSource audioSourceSE;
    AudioSource audioSourceBGM;


    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        SEManager = gameManager.SEManager;
        audioSourceSE = gameManager.audioSourceSE;
        audioSourceBGM = gameManager.audioSourceBGM;

        myTransform = gameObject.transform;

        stairsSE = SEManager.SE_Stairs;
    }

    /// <summary>
    /// �v���C���[���ʂ蔲�������ʑJ�ڂ��郁�\�b�h���Ăԃ��\�b�h
    /// </summary>
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        Vector2 vector = Calculate.Vector(myTransform.position, collision.transform.position);

        //�v���C���[���w�肵�������ɒʂ蔲�����ꍇ��BlackOut���\�b�h���Ă�
        switch (directionNumber)
        {
            case 1: //�k
                if (vector.y > 0)
                {
                    StartCoroutine(BlackOut());
                }
                break;


            case 2: //��
                if (vector.x > 0)
                {
                    StartCoroutine(BlackOut());
                }
                break;

            case 3: //��
                if (vector.y < 0)
                {
                    StartCoroutine(BlackOut());
                }
                break;

            case 4: //��
                if (vector.x < 0)
                {
                    StartCoroutine(BlackOut());
                }
                break;
        }
    }


    /// <summary>
    /// ���X�ɉ�ʂ��Ó]���A��ʂ�J�ڂ��郁�\�b�h���ĂԃR���[�`��
    /// </summary>
    IEnumerator BlackOut()
    {
        //�Q�[���I�����j���[��\������Ȃ�A�Ó]�����Ƀ��j���[��\��
        if (transitionStageNumber == 0)
        {
            gameManager.TransitionStage(transitionStageNumber);
            yield break;
        }

        //�K�i��o��SE��炵�ABGM���t�F�[�h�A�E�g����
        audioSourceSE.PlayOneShot(stairsSE);
        StartCoroutine(VolumeDown());

        //�v���C���[�𓮂��Ȃ�����
        gameManager.keyInputManager.noControl = true;
        Time.timeScale = 0;

        //�~�j�}�b�v��\��
        if (gameManager.miniMap != null) { gameManager.miniMap.MapOff(); }

        //���p�l����\�����A�����ɂ���
        GameObject blackPanelObj = gameManager.blackPanel.gameObject;
        blackPanelObj.SetActive(true);

        Image blackPanel = gameManager.blackPanel;
        
        blackPanel.color = new Color(1, 1, 1, 0);

        //���p�l���̓����x�����X�ɉ�����
        for (float i = 0; i < 1; i += 0.008f)
        {
            blackPanel.color = new Color(1, 1, 1, i);
            yield return null;
        }

        blackPanel.color = new Color(1, 1, 1, 1);
       
        //�R���[�`���𓮂������ߎ��Ԃ�߂�
        Time.timeScale = 1;

        yield return new WaitForSeconds(3.0f);

        //������~���Ă����ʂ�J�ڂ���
        gameManager.TransitionStage(transitionStageNumber);

        //������悤�ɂ���
        gameManager.keyInputManager.noControl = false;
    }

    /// <summary>
    /// ���X��BGM�̃{�����[����������R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator VolumeDown()
    {
        while (audioSourceBGM.volume > 0)
        {
            audioSourceBGM.volume -= 0.0015f;
            yield return null;
        }
    }
}
