using UnityEngine;

namespace Data
{
    /// <summary>
    /// �e�̐��������Ǘ�����N���X
    /// </summary>
    class GunData : MonoBehaviour
    {
        static string gunExplanation;

        /// <summary>
        /// �e�̐��������Z�b�g���郁�\�b�h
        /// </summary>
        /// <param name="classNumber">�e�̕��ޔԍ�</param>
        /// <returns>�e�̐�����</returns>
        public static string SetGunExplanation(int classNumber)
        {
            switch (classNumber)
            {
                case 1: //�n���h�K��
                    gunExplanation =
                        @"�V���v���ȃn���h�K���B
�e��͖������������Â炢�B";
                    break;

                case 2: //�\�[�h�I�t�V���b�g�K��
                    gunExplanation = 
                        @"�˒����Z���V���b�g�K���B
�G�ɋ߂Â��Č��ĂΈЗ͔��Q�B";
                    break;

                case 3: //�ђʃK��
                    gunExplanation = 
                        @"�e���G���ђʂ���n���h�K���B";
                    break;

                case 4: //�o�E���h�K��
                    gunExplanation = 
                        @"�e���ǂŒ��˕Ԃ�n���h�K���B";
                    break;

                case 5: //�O���l�[�h�K��
                    gunExplanation = 
                        @"�O���l�[�h�𔭎˂���e�B
�����ŋ߂��̓G�ɂ��_���[�W��^����B";
                    break;

                case 6: //���{���o�[
                    gunExplanation =
                        @"���Ȃ�З͂̍������e�B
�u�������Ȃ����Ɉ����₷���B";
                    break;

                case 7: //�}�[�N�X�}�����C�t��
                    gunExplanation =
                        @"�˒������������x���C�t���B
�G���ђʂ��Ȃ����A������x�̘A�˂��\�B";
                    break;

                case 8: //�X�i�C�p�[���C�t��
                    gunExplanation =
                        @"�������ւ̍U�����ł��A�ђʂ�����X�i�C�p�[���C�t���B
�����[�h�͒��������ɍ��З́B";
                    break;

                case 9: //�V���b�g�K��
                    gunExplanation =
                        @"�\�[�h�I�t�V���b�g�K����萸�x���З͂��i�i�ɍ����V���b�g�K���B
�������̓G�ɂ��\���L���B";
                    break;

                case 10: //�~�j�}�V���K��
                    gunExplanation =
                        @"�З͂͒Ⴂ���A�˂������}�V���K���B
�e�̏��Ղ��������B";
                    break;

                case 11: //�T�u�}�V���K��
                    gunExplanation =
                        @"���x�����������ŘA�˂����ɑ����}�V���K���B
�e�؂�ɂ͗v���ӁB";
                    break;

                case 12: //�����`���[
                    gunExplanation =
                        @"�P�����������ĂȂ����З͂͐��B
�����ōL���͈͂̓G�ɂ����_���[�W�B";
                    break;

                case 13: //�S�[���f���n���h�K��
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��n���h�K���B
��x�ɕ������˂��A���ˊԊu�����ɒZ���B";
                    break;

                case 14: //�S�[���f���s�X�g��
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��s�X�g���B
�e���o�E���h�E���剻���A��x�ɂQ�����˂���B";
                    break;

                case 15: //�S�[���f���\�[�h�I�t
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��\�[�h�I�t�V���b�g�K���B
���ˊԊu�����ɒZ���B";
                    break;

                case 16: //�S�[���f���V���b�g�K��
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��V���b�g�K���B
�����x���e�����˕Ԃ�A�������̓G�ւ��L���B";
                    break;

                case 17: //�S�[���f��M�}�V���K��
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��~�j�}�V���K���B
��x�ɑ�ʂ̒e�𔭎˂��A���ˊԊu���Z���B";
                    break;

                case 18: //�S�[���f��S�}�V���K��
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��T�u�}�V���K���B
��x�ɑ����̒e�𔭎˂��A�e�����˕Ԃ�B";
                    break;

                case 19: //�S�[���f�����{���o�[
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂����{���o�[�B
���ɍ��_���[�W�ŁA�G���ђʂ���B";
                    break;

                case 20: //�S�[���f���}�O�i��
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��}�O�i���B
�e���o�E���h�E���剻���A�З͂����ɍ����B";
                    break;

                case 21: //�S�[���f�����C�t��
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��}�[�N�X�}�����C�t���B
�e�������ɑ����A�e�����˕Ԃ�B";
                    break;

                case 22: //�S�[���f���X�i�C�p�[
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��X�i�C�p�[���C�t���B
�G���ђʂ��A�j��I�ȈЗ͂��ւ�B";
                    break;

                case 23: //�S�[���f���O������
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂��O���l�[�h�����`���[�B
��x�ɕ����̃O���l�[�h�𔭎˂���B";
                    break;

                case 24: //�S�[���f�������`���[
                    gunExplanation =
                        @"�e���ƈ��������ɐ��Ȑ��\�𓾂������`���[�B
�����ɂ���G����|�ł���B";
                    break;

                case 25: //�^�N�e�B�J���A�T���g
                    gunExplanation =
                        @"�e�����˕Ԃ�A�T���g���C�t���B
���x�����������₷���B";
                    break;

                case 26: //�A�T���g���C�t��
                    gunExplanation =
                        @"��ʓI�ȃA�T���g���C�t���B
���x�͕s���肾���З͂͂��������B";
                    break;

                case 27: //�o�[�X�g�A�T���g
                    gunExplanation =
                        @"�R���̒e��A���Ŕ��˂���A�T���g���C�t���B
���������ꂾ�����͂ȕ���B";
                    break;

                case 28: //�t���I�[�g�s�X�g��
                    gunExplanation =
                        @"�t���I�[�g�ɑΉ������s�X�g���B
���x���������ˊԊu���Z�����A�˒����Z���B";
                    break;

                case 29: //�s�X�g��
                    gunExplanation =
                        @"��ʓI�ȃs�X�g���B
�n���h�K���������ˊԊu���Z���B";
                    break;

                case 30: //�R���p�N�g�A�T���g
                    gunExplanation =
                        @"�ʏ�̃A�T���g���C�t�������ߋ��������̃A�T���g���C�t���B
�˒����Z���Ȃ������A���x���������ˊԊu���Z���B";
                    break;

                case 31: //�w�r�[�A�T���g
                    gunExplanation =
                        @"�A�T���g���C�t�����������������̃A�T���g���C�t���B
�˒��E���x�E�З͂��オ�������A���ˊԊu�������Ȃ����B";
                    break;

                case 32: //�c�C���A�T���g
                    gunExplanation =
                        @"�P�x�ɂQ���̒e�𔭎˂���A�T���g���C�t���B
��肭�g�����Ƃ��ł�������ɍ��Η͂ȕ���ƂȂ邾�낤�B";
                    break;

                case 33: //�}�V���K��
                    gunExplanation =
                        @"��ʓI�ȃ}�V���K���B
���x�͒Ⴂ���e���ŕ₨���B";
                    break;

                case 34: //�w�r�[�X�i�C�p�[
                    gunExplanation =
                        @"���З͂Ƒ��x���������X�i�C�p�[���C�t���B
�����[�h���Ԃ������Ȃ������Ƃɒ��ӁB";
                    break;

                case 35: //�S�A�������`���[
                    gunExplanation =
                        @"�P�x�ɂS�̃O���l�[�h�𔭎˂���O���l�[�h�����`���[�B
���ɋ��́B";
                    break;

                case 36: //���C�g�}�V���K��
                    gunExplanation =
                        @"��舵���₷���Ȃ����}�V���K���B
�ʏ�̃}�V���K�������ߋ��������B";
                    break;

                case 37: //�R���p�N�gSMG
                    gunExplanation =
                        @"�ʏ�̃T�u�}�V���K�������ߋ��������̃T�u�}�V���K���B
�e�؂�ɒ��ӂ��悤�B";
                    break;

                case 38: //�o�[�X�gSMG
                    gunExplanation =
                        @"�T���̒e��A���Ŕ��˂���T�u�}�V���K���B
���������ꂾ�����͂ȕ���B";
                    break;

                case 39: //�^�N�e�B�J��SMG
                    gunExplanation =
                        @"�e�����˕Ԃ�T�u�}�V���K���B
���x�������Ȃ������₷���B";
                    break;

                case 100: //�f�o�b�O�K��
                    gunExplanation = 
                        @"�f�o�b�O�p";
                    break;

            }
            return gunExplanation;
        }
    }
}

