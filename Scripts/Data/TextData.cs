using UnityEngine;

namespace Text
{
    /// <summary>
    /// �e�L�X�g�̃f�[�^���Ǘ�����N���X
    /// </summary>
    class TextData : MonoBehaviour
    {
        static string text;

        /// <summary>
        /// �e�L�X�g�i���o�[�̃e�L�X�g���Z�b�g���郁�\�b�h
        /// </summary>
        /// <param name="textNumber">�擾�������e�L�X�g�̔ԍ�</param>
        public static string setText(int textNumber)
        {
            switch (textNumber)
            {
                //�P�`�P�O�O�̓`���[�g���A���p
                case 1:
                    text =
                        @"�`���[�g���A���ւ悤�����I
�����ł͊�{�I�ȑ����������܂�

�� �G���g�����X�ɖ߂�";
                    break;

                case 2:
                    text =
                        @"���N���b�N�𒷉����ł��ˌ����邱�Ƃ��ł��܂�
�}�K�W���̎c�e���O�ɂȂ�Ǝ����I�Ƀ����[�h���܂�
�T���̕�������b��Ɏ����I�Ƀ����[�h����܂�";
                    break;

                case 3:
                    text =
                        @"���[�����O���͓G�̒e��������Ȃ��Ȃ�܂�
���[�����O�Œe������Ȃ���i��ł݂܂��傤�I
���L�[�{�[�h�ɂ���Ă͔������Ȃ��L�[�̑g�ݍ��킹������\��������܂��i�L�[���[���I�[�o�[�j";
                    break;

                case 4:
                    text =
                        @"�{�����g���ƓG�̒e�����ׂď������Ƃ��ł��܂�
�g���Ă��班���̊ԁA���ʂ������܂�";
                    break;

                case 5:
                    text =
                        @"���͎��H�`���̗��K�ł�
���܂ł̒m�������p���Đi�݂܂��傤�I";
                    break;

                case 6:
                    text =
                        @"�`���[�g���A���͈ȏ�ł�
�K�i�����ƃG���g�����X�ɖ߂�܂�
��R�K�w�ɂ���S�[����ڎw���܂��傤�I";
                    break;

            }

            return text;
        }
    }
}

