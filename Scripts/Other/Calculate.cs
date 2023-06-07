using UnityEngine;
using System.Collections.Generic;
using System;

namespace Calc
{
    class Calculate
    {
        /// <summary>
        /// �Q�_�Ԃ̊p�x�����߂郁�\�b�h
        /// </summary>
        /// <param name="origin">���_</param>
        /// <param name="vertex">���_</param>
        /// <returns>�Q�_�Ԃ̊p�x�i�O�`�R�U�O���j</returns>
        public static double Angle_RightTriangle(Vector2 origin, Vector2 vertex)
        {
            Vector2 pos = Vector(origin, vertex);
            double angle = Math.Atan2(pos.y, pos.x) * 180 / Math.PI;
            //�p�x��0�`360���ɒ���
            if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }


        /// <summary>
        /// �Q�_�̃x�N�g�������߂郁�\�b�h
        /// </summary>
        /// <param name="origin">���_</param>
        /// <param name="vertex">���_</param>
        /// <returns>�Q�_�̃x�N�g��</returns>
        public static Vector2 Vector(Vector2 origin, Vector2 vertex)
        {
            return new Vector2(vertex.x - origin.x, vertex.y - origin.y);

        }


        /// <summary>
        /// //�Q�_�Ԃ̋��������߂郁�\�b�h
        /// </summary>
        /// <param name="origin">���_</param>
        /// <param name="vertex">���_</param>
        /// <returns>�Q�_�Ԃ̋���</returns>
        public static double Distance(Vector2 origin, Vector2 vertex)
        {
            Vector2 pos = Vector(origin, vertex);
            return Math.Sqrt(Math.Pow(pos.x, 2) + Math.Pow(pos.y, 2));
        }


        /// <summary>
        /// ���_�𔼌aradius�̍��W�ɒ������\�b�h�A���a�P���Ɛ��K��
        /// </summary>
        /// <param name="origin">���_</param>
        /// <param name="vertex">���_</param>
        /// <param name="radius">���������~�̔��a</param>
        /// <returns>���aradius�ɒ��������W</returns>
        public static Vector2 To_Circle(Vector2 origin, Vector2 vertex, float radius)
        {
            Vector2 pos = Vector(origin, vertex);
            double beforeRadius = Distance(origin, vertex);

            return new Vector2((float)(pos.x / beforeRadius�@* radius), (float)(pos.y / beforeRadius * radius));
        }


        /// <summary>
        /// vertex���ő�angle���̗����ł��炵�����W���v�Z����
        /// </summary>
        /// <param name="origin">�����̍��W</param>
        /// <param name="vertex">�ڕW�̍��W</param>
        /// <param name="angle">���炷�ő�p�x</param>
        /// <returns>���炵����̍��W</returns>
        public static Vector2 ShakeAngle(Vector2 origin, Vector2 vertex, float angle)
        {
            System.Random r = new System.Random();
            int randomValue = r.Next((int)-angle, (int)angle);

            //�e�͍ő�angle�������
            double newAngle = Angle_RightTriangle(origin, vertex) + randomValue;

            //�V�����p�x�����W�A���ɕϊ����ĕԂ�
            int x = 1;
            if (newAngle > 90 && newAngle < 270)
            {
                x = -1;
            }
            double newRadian = x * Math.Tan(newAngle / 180 * Math.PI);

            return new Vector2(x, (float)newRadian);
        }

        /// <summary>
        /// min�`max�܂ł̐����̃��X�g����郁�\�b�h�A���ChooseNumber���\�b�h�Ƒg�ݍ��킹��
        /// </summary>
        /// <param name="min">���X�g�̍ŏ��l</param>
        /// <param name="max">���X�g�̍ő�l</param>
        /// <returns></returns>
        public static List<int> NumList(int min, int max)
        {
            List<int> numList = new List<int>();
            for (int i = min; i <= max; i++)
            {
                numList.Add(i);
            }
            return numList;
        }

        /// <summary>
        /// �����̃��X�g�̒�����A���m���łP�̐�����I�ԃ��\�b�h
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int ChooseNumber(List<int> numberList)
        {
            int num = 0;
            float sumProbability = 0; //�ݐϊm��
            int rand = UnityEngine.Random.Range(1, 101); //�P�`�P�O�O�̊Ԃ̗���

            float eachProbablity = 100.0f / numberList.Count; //�e�e�̏o���m���i���m���j

            //�������ݐϊm���ȉ��ɂȂ����Ƃ��A���̐������߂�l�ƂȂ�
            foreach (int number in numberList)
            {
                sumProbability += eachProbablity;

                if (rand > sumProbability) { continue; }
                
                num = number;
                break;
            }

            return num;
        }
    }
}