using UnityEngine;
using System.Collections.Generic;
using System;

namespace Calc
{
    class Calculate
    {
        /// <summary>
        /// ２点間の角度を求めるメソッド
        /// </summary>
        /// <param name="origin">原点</param>
        /// <param name="vertex">頂点</param>
        /// <returns>２点間の角度（０～３６０°）</returns>
        public static double Angle_RightTriangle(Vector2 origin, Vector2 vertex)
        {
            Vector2 pos = Vector(origin, vertex);
            double angle = Math.Atan2(pos.y, pos.x) * 180 / Math.PI;
            //角度を0～360°に直す
            if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }


        /// <summary>
        /// ２点のベクトルを求めるメソッド
        /// </summary>
        /// <param name="origin">原点</param>
        /// <param name="vertex">頂点</param>
        /// <returns>２点のベクトル</returns>
        public static Vector2 Vector(Vector2 origin, Vector2 vertex)
        {
            return new Vector2(vertex.x - origin.x, vertex.y - origin.y);

        }


        /// <summary>
        /// //２点間の距離を求めるメソッド
        /// </summary>
        /// <param name="origin">原点</param>
        /// <param name="vertex">頂点</param>
        /// <returns>２点間の距離</returns>
        public static double Distance(Vector2 origin, Vector2 vertex)
        {
            Vector2 pos = Vector(origin, vertex);
            return Math.Sqrt(Math.Pow(pos.x, 2) + Math.Pow(pos.y, 2));
        }


        /// <summary>
        /// 頂点を半径radiusの座標に直すメソッド、半径１だと正規化
        /// </summary>
        /// <param name="origin">原点</param>
        /// <param name="vertex">頂点</param>
        /// <param name="radius">直したい円の半径</param>
        /// <returns>半径radiusに直した座標</returns>
        public static Vector2 To_Circle(Vector2 origin, Vector2 vertex, float radius)
        {
            Vector2 pos = Vector(origin, vertex);
            double beforeRadius = Distance(origin, vertex);

            return new Vector2((float)(pos.x / beforeRadius　* radius), (float)(pos.y / beforeRadius * radius));
        }


        /// <summary>
        /// vertexを最大angle°の乱数でずらした座標を計算する
        /// </summary>
        /// <param name="origin">自分の座標</param>
        /// <param name="vertex">目標の座標</param>
        /// <param name="angle">ずらす最大角度</param>
        /// <returns>ずらした後の座標</returns>
        public static Vector2 ShakeAngle(Vector2 origin, Vector2 vertex, float angle)
        {
            System.Random r = new System.Random();
            int randomValue = r.Next((int)-angle, (int)angle);

            //弾は最大angle°ずれる
            double newAngle = Angle_RightTriangle(origin, vertex) + randomValue;

            //新しい角度をラジアンに変換して返す
            int x = 1;
            if (newAngle > 90 && newAngle < 270)
            {
                x = -1;
            }
            double newRadian = x * Math.Tan(newAngle / 180 * Math.PI);

            return new Vector2(x, (float)newRadian);
        }

        /// <summary>
        /// min～maxまでの数字のリストを作るメソッド、主にChooseNumberメソッドと組み合わせる
        /// </summary>
        /// <param name="min">リストの最小値</param>
        /// <param name="max">リストの最大値</param>
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
        /// 数字のリストの中から、等確率で１つの数字を選ぶメソッド
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int ChooseNumber(List<int> numberList)
        {
            int num = 0;
            float sumProbability = 0; //累積確率
            int rand = UnityEngine.Random.Range(1, 101); //１～１００の間の乱数

            float eachProbablity = 100.0f / numberList.Count; //各銃の出現確率（等確率）

            //乱数が累積確率以下になったとき、その数字が戻り値となる
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