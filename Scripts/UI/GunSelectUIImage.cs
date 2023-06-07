using UnityEngine;
using UnityEngine.UI;

//����I����ʂ̏e���Ƃ̉摜���Ǘ�����N���X
public class GunSelectUIImage : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int gunSelectUIAnimeNumber;

    Image myImage;
    RectTransform myRect;
    Vector2 myPos;
    Collider2D myCollider;

    GunSelectUIManager gunSelectUIManagerScript;
    Canvas UICanvas;
    RectTransform UICanvasRect;

    bool isMyDrag;


    void OnEnable()
    {
        gunSelectUIManagerScript = gameManager.gunSelectUIManager;

        UICanvas = gameManager.UICanvas;
        UICanvasRect = gameManager.gunSelectUIRect;

        myImage = gameObject.GetComponent<Image>();
        myRect = gameObject.GetComponent<RectTransform>();

        myCollider = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isMyDrag) { return; } //�����̏e�̉摜���h���b�O���Ȃ瑱����

        //�}�E�X�̃��[�J�����W���擾���A�����̒��̉摜���}�E�X�̈ʒu�ɍ��킹������
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UICanvasRect, Input.mousePosition, UICanvas.worldCamera, out Vector2 mousePos);
        myRect.anchoredPosition = new Vector2(mousePos.x, mousePos.y);

        //�e�̉摜�̃h���b�O����߂���A�e�̉摜�����Ƃ̃|�W�V�����ɖ߂�
        if (Input.GetMouseButtonUp(0))
        {
            myCollider.enabled = true; //�摜�̃R���C�_�[��true��
            gameObject.transform.position = myPos; //�摜�����Ƃ̈ʒu�ɖ߂�
            gunSelectUIManagerScript.TextNone(); //����I����ʂ́A����̖��O�E����̎c�e�E����̐������̕\�����I�t��

            gunSelectUIManagerScript.isDrag = false;
            gunSelectUIManagerScript.dragGunNumber = 0;
            isMyDrag = false;
        }
    }

    void OnMouseOver()
    {

        //�����̏e�̉摜�̏�ō��N���b�N����������A���̎��̉摜�̈ʒu��ۑ����A�h���b�O���Ă��锻��ɂ���
        if (Input.GetMouseButtonDown(0))
        {
            myCollider.enabled = false;
            myPos = gameObject.transform.position;
            gunSelectUIManagerScript.dragGunNumber = gunSelectUIAnimeNumber;
            gunSelectUIManagerScript.isDrag = true;
            isMyDrag = true;
        }

        //�摜�̃h���b�O����߂��Ƃ��A�}�E�X�̈ʒu����������؂�ւ��̈ʒu�Ȃ�A�������̕�����h���b�O��������ɐ؂�ւ���
        if (Input.GetMouseButtonUp(0))
        {
            if (gunSelectUIAnimeNumber > 0) { return; }
            if (gunSelectUIManagerScript.dragGunNumber == 0) { return; }
            gunSelectUIManagerScript.EquipGunChange(gunSelectUIAnimeNumber, gunSelectUIManagerScript.dragGunNumber);
        }

        if (gunSelectUIManagerScript.isDrag && gunSelectUIAnimeNumber > 0) { return; } //���łɃh���b�O������������ɃJ�[�\�����������烊�^�[��
        //����̉摜���󗓂���Ȃ��A�h���b�O���܂��͏�������ɃJ�[�\���������Ă�����A���̕���̗������点��
        gunSelectUIManagerScript.animeNumber = gunSelectUIAnimeNumber; 

        if (gunSelectUIManagerScript.isDrag) { return; }
        //�h���b�O���͕���̐�������؂�ւ��Ȃ�
        gunSelectUIManagerScript.TextChange();
    }

    void OnMouseExit()
    {
        if (myImage.sprite.name == "None") { return; }
        gunSelectUIManagerScript.animeNumber = 0; //�����̏e�̉摜�̃}�E�X�I�[�o�[����߂���A����̗������点��̂���߂�

        if (gunSelectUIManagerScript.isDrag) { return; }
        gunSelectUIManagerScript.TextNone(); //�����̏e�̉摜�̃}�E�X�I�[�o�[����߂���A���������̕\�����I�t��
    }

}
