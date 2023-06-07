using UnityEngine;
using TMPro;
using Calc;
using Text;

/// <summary>
/// �Ŕ��Ǘ�����N���X
/// </summary>
public class Signboard : MonoBehaviour
{
    GameManager gameManager;
    SEManager SEManager;
    [SerializeField] int textNumber;

    Transform myTransform;
    SpriteRenderer mySpriteRenderer;

    const float canReadRange = 3.0f; //�Ŕ�ǂނ��Ƃ��ł���͈�

    //�Q�[���}�l�[�W������ǂݍ��ޕϐ�
    Material outline;
    Material defaultMaterial;
    GameObject signboardTextBackground;
    TextMeshProUGUI signboardText;
    GameObject miniMap;
    GameObject mapPoint;

    AudioClip signboardOpenSE;
    AudioClip signboardCloseSE;
    AudioSource audioSource;

    string text;
    bool isReading = false;


    void Start()
    {
        GameObject gameMGR = GameObject.Find("GameManager");
        gameManager = gameMGR.GetComponent<GameManager>();

        SEManager = gameManager.SEManager;
        audioSource = gameManager.audioSourceSE;

        myTransform = gameObject.transform;
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        outline = gameManager.outline; //���t�`
        defaultMaterial = gameManager.defaultMaterial; //���t�`�Ȃ�

        signboardTextBackground = gameManager.signboardTextBackground;
        signboardText = gameManager.signboardText;

        miniMap = gameManager.miniMap.gameObject;
        mapPoint = gameManager.mapPoint;

        signboardOpenSE = SEManager.SE_SignboardOpen;
        signboardCloseSE = SEManager.SE_SignboardClose;

        text = TextData.setText(textNumber);
    }


    void Update()
    {
        Vector3 myPos = myTransform.position;
        Vector3 playerPos = gameManager.playerPos;

        mySpriteRenderer.material = defaultMaterial;

        //�Ŕ�ǂ�ł�Ƃ��A�v���C���[������邩�AE�L�[�������ꂽ��Ŕ����
        if (isReading)
        {
            if (Calculate.Distance(myPos, playerPos) > canReadRange)
            {
                HiddenText();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                HiddenText();
                mySpriteRenderer.material = outline;
                return;
            }
        }

        //�v���C���[��canOpenRange���ɂ��邩���肵�A�����甒�t�`������
        if (Calculate.Distance(myPos, playerPos) > canReadRange) { return; }

        mySpriteRenderer.material = outline;

        //�󔠂̋߂��ɂ���Ƃ��AE�L�[�������ꂽ��Ŕ̕�����\������
        if (Input.GetKeyDown(KeyCode.E))
        {
            DisplayText();
        }
    }

    /// <summary>
    /// �Ŕ̕�����\�����郁�\�b�h
    /// </summary>
    void DisplayText()
    {
        audioSource.PlayOneShot(signboardOpenSE);

        //�Ŕ̕�����\��
        signboardTextBackground.SetActive(true);
        signboardText.text = text;

        //UI����������\��
        miniMap.SetActive(false);
        mapPoint.SetActive(false);

        isReading = true;
    }

    /// <summary>
    /// �Ŕ̕�������郁�\�b�h
    /// </summary>
    void HiddenText()
    {
        audioSource.PlayOneShot(signboardCloseSE);

        //�Ŕ̕������\��
        signboardTextBackground.SetActive(false);

        //��\���ɂ���UI��\��
        miniMap.SetActive(true);
        mapPoint.SetActive(true);

        isReading = false;
    }
}
