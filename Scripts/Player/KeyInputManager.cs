using UnityEngine;

/// <summary>
/// �L�[���͂��Ǘ�����N���X
/// </summary>
public class KeyInputManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    Player player;
    MiniMap miniMap;

    /// <summary>
    /// �O������noControl��true�ɂ��邱�Ƃɂ���đ���ł��Ȃ�����ϐ�
    /// </summary>
    public bool noControl = false;
    bool entrance;

    void Start()
    {
        player = gameManager.playerScript;
        miniMap = gameManager.miniMap;
        entrance = gameManager.entrance;
    }

    //E�L�[�̃A�C�e���擾��Item�X�N���v�g�AR�L�[�̃����[�h��Gun�X�N���v�g�̂܂�
    //�A�C�e���Əe�͂������X�N���v�g�������Ď擾����ς�����
    void Update()
    {
        if (noControl)
        {
            player.StopPlayer(); //noControl��true�Ȃ�v���C���[���~�A���ꂪ�Ȃ��ƈ�����Ɉړ���������
        }

        if (noControl) { return; } //noControl��true�Ȃ瑀��ł��Ȃ��悤��

        player.MovePlayer(); //�v���C���[���ړ����郁�\�b�h
        player.MoveCamera(); //�J�������ړ����郁�\�b�h

        //�X�y�[�X�L�[�����t�g�V�t�g�L�[�������ꂽ�烍�[�����O
        //������PC���� A,W,Space �� A,S,Space�̑g�ݍ��킹�������Ȃ�����
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            player.RollingPlayer(); //���[�����O���郁�\�b�h
        }

        //�G���g�����X�Ȃ�A�}�b�v�A�{���A����I���A����m�F�𖳌�
        if (entrance) { return; }

        //M�őS�̃}�b�v���J��
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.ChangeMapCamera(); //�S�̃}�b�v�ƃ~�j�}�b�v��؂�ւ��郁�\�b�h
        }

        //�z�C�[���N���b�N�Ń{���g�p
        if (Input.GetMouseButtonDown(2))
        {
            player.UseBomb(); //�{�����g�p���郁�\�b�h
        }

        //�^�u�N���b�N�ŕ���I����ʂ��J��
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            player.ActiveGunSelectUI(); //����I����ʂ��J��/���郁�\�b�h
        }

        //�G�X�P�[�v�N���b�N�ő���m�F��ʂ��J��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.ActiveOperationsConfirm();�@//����m�F��ʂ��J��/���郁�\�b�h
        }
    }
}
