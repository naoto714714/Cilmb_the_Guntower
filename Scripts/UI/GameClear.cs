using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// �Q�[���I�[�o�[�̃��j���[���Ǘ�����N���X
/// </summary>
public class GameClear : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI killText;
    Player player;

    SEManager SEManager;
    AudioSource audioSource;
    AudioClip openSE;
    AudioClip clickSE;

    void Awake()
    {
        player = gameManager.playerScript;

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        openSE = SEManager.SE_GameClearDisplay;
        clickSE = SEManager.SE_GameOverClick;
    }

    /// <summary>
    /// �N�������Ƃ��̃��\�b�h
    /// </summary>
    void OnEnable()
    {
        audioSource.PlayOneShot(openSE);
        gameManager.DisplayCursor();
        
        //�Q�[���N���A�����Ƃ��̎��ԁE�R�C���E�X�e�[�W���E���j�����擾
        int[] times = gameManager.ElapsedTime();
        timeText.text = times[0].ToString("0") + ":" + times[1].ToString("00") + ":" + times[2].ToString("00");
        coinText.text = player.holdCoin.ToString();
        stageText.text = gameManager.stageName.text;
        killText.text =  player.killCount.ToString();
    }

    /// <summary>
    /// �G���g�����X�֑J�ڂ��郁�\�b�h
    /// </summary>
    public void ToEntrance()
    {
        audioSource.PlayOneShot(clickSE);
        StartCoroutine(BlackOutAndTransition(1));
    }

    /// <summary>
    /// �Ó]������A�����҂��Ă���V�[���J�ڂ���R���[�`��
    /// </summary>
    /// <param name="transitionNumber"></param>
    /// <returns></returns>
    public IEnumerator BlackOutAndTransition(int transitionNumber)
    {
        //���я������ւ��A���p�l�����őO�ʂ�
        gameObject.transform.SetAsFirstSibling();

        yield return new WaitForSeconds(4.0f);

        //�����҂��Ă����ʑJ��
        gameManager.TransitionStage(transitionNumber);
    }
}
