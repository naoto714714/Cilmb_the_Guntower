using UnityEngine;

/// <summary>
/// �v���C���[���G�ꂽ��_���[�W���󂯂�悤�ȃg���b�v�̃N���X
/// </summary>
public class Trap : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        //�Փ˂����̂��v���C���[�Ȃ�A�v���C���[�Ƀ_���[�W��^����
        //���G�̃v���C���[�ɂ̓_���[�W��^���Ȃ��悤�ɁA���C���[�Ŕ��f����
        int layerNumber = (LayerMask.NameToLayer("Player"));
        if (collision.gameObject.layer == layerNumber)
        {
            Player playerScript = collision.gameObject.GetComponent<Player>();
            playerScript.Damaged(1);
        }
    }
}
