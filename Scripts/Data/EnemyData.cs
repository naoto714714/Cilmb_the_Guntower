using UnityEngine;

namespace Data
{
    //敵のデータを管理するデータ
    class EnemyData : MonoBehaviour
    {
        static float[] enemyData;

        /// <summary>
        /// エネミーナンバーごとにセットする敵のデータを変えるメソッド
        /// </summary>
        /// <param name="classNumber">敵ごとの分類番号</param>
        public static float[] SetEnemyData(int enemyClassNumber)
        {
            switch (enemyClassNumber)
            {
                //敵のHP、歩く速度、歩く時間、歩き終わって止まる時間、
                //攻撃してくる範囲、攻撃してくる間隔、攻撃回数、一度の発射数、攻撃中の発射間隔
                //弾丸の速度、弾のダメージ、玉の大きさ、
                //最大ブレ角度、何秒後に弾を撃つか（アニメーションに合わせるため）、
                //ランダムに移動するか、攻撃中に必ず移動するようにするか、攻撃中に必ず停止するようにするか
                case 1: //トランク
                    enemyData = new float[]
                    { 100, 1.5f, 2.0f, 2.0f, 
                        8, 3.0f, 1, 1, 0,
                        5.0f, 1, 1, 
                        0, 0.6f, 
                        1, 0, 0};
                    break;

                case 2: //ピッグ
                    enemyData = new float[]
                    { 120, 3.5f, 0.3f, 0, 
                        0, 0, 0, 0, 0, 
                        0, 0, 0, 
                        0, 0, 
                        0, 0, 0};
                    break;

                case 3: //プラント
                    enemyData = new float[]
                    { 70, 0.0f, 0.0f, 0.0f, 
                        100, 3.0f, 3, 6, 1.0f,
                        4.0f, 1, 1,
                        30, 1.2f, 
                        0, 0, 0};
                    break;

                case 4: //ライノ
                    enemyData = new float[]
                    { 150, 6.0f, 2.0f, 2.0f,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0, 
                        0, 0, 0};
                    break;

                case 5: //ビー
                    enemyData = new float[]
                    { 70, 1.0f, 2.0f, 3.0f,
                        10, 4.0f, 1, 1, 0,
                        12.0f, 1, 1,
                        0, 1.3f, 
                        1, 0, 0 };
                    break;


                case 6: //タートル
                    enemyData = new float[]
                    { 140, 0.0f, 0.0f, 0.0f,
                        100, 5.0f, 2, 60, 1.5f,
                        4.0f, 1, 1,
                        180, 1.2f, 
                        0, 0, 0};
                    break;

                case 7: //ブルーバード
                    enemyData = new float[]
                    { 100, 1.0f, 0.3f, 0.0f,
                        8, 2.0f, 1, 1, 0,
                        5.0f, 1, 1,
                        10, 0.0f, 
                        0, 0, 0};
                    break;

                case 8: //マッシュルーム
                    enemyData = new float[]
                    { 120, 3.0f, 0.8f, 3.0f,
                        20, 5.0f, 1, 60, 0,
                        2.0f, 1, 1,
                        180, 0.8f, 
                        1, 0, 0};
                    break;

                case 9: //スネイル
                    enemyData = new float[]
                    { 160, 0.8f, 0.3f, 0.0f,
                        10, 4.0f, 10, 1, 0.2f,
                        6.0f, 1, 1,
                        25, 1.0f,
                        0, 0, 1};
                    break;

                case 10: //バット
                    enemyData = new float[]
                    { 80, 2.0f, 90f, 1.0f,
                        10, 4.0f, 4, 1, 0.6f,
                        6.0f, 1, 1,
                        5, 1.0f,
                        0, 1, 0};
                    break;

                case 11: //ビッグスライム
                    enemyData = new float[]
                    { 300,1.0f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 12: //スライム
                    enemyData = new float[]
                    { 150, 2.5f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 13: //ミニスライム
                    enemyData = new float[]
                    { 20, 3.5f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 14: //ラディッシュ
                    enemyData = new float[]
                    { 150, 1.5f, 2.0f, 2.0f,
                        12, 2.0f, 1, 8, 0,
                        5.0f, 1, 1,
                        20, 0.8f,
                        1, 0, 0};
                    break;

                case 15: //ゴースト
                    enemyData = new float[]
                    { 150, 4.5f, 0.8f, 1.5f,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 16: //ファットバード
                    enemyData = new float[]
                    { 250, 0.0f, 0.0f, 0.0f,
                        15, 1.2f, 1, 1, 0.0f,
                        2.0f, 1, 2.5f,
                        0, 0.0f,
                        0, 0, 0};
                    break;

                case 17: //チキン
                    enemyData = new float[]
                    { 150, 6.0f, 2.0f, 2.0f,
                        10, 3, 3, 1, 0.3f,
                        5, 1, 1,
                        10, 0,
                        0, 0, 0};
                    break;

                case 18: //ダック
                    enemyData = new float[]
                    { 100, 6.0f, 2.0f, 2.0f,
                        10, 3, 1, 1, 0,
                        5, 1, 1,
                        10, 0,
                        0, 0, 0};
                    break;

                case 19: //カメレオン
                    enemyData = new float[]
                    { 500, 6.0f, 1.5f, 2.0f,
                        30, 3, 3, 3, 0.3f,
                        7, 1, 1.2f,
                        15, 0,
                        1, 0, 0};
                    break;

                case 20: //ロック
                    enemyData = new float[]
                    { 250,1.0f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 21: //ミニロック
                    enemyData = new float[]
                    { 50, 3.5f, 0.3f, 0,
                        0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0,
                        0, 0, 0};
                    break;

                case 101: //チュートリアル用
                    enemyData = new float[]
                    { 100, 1.5f, 2.0f, 2.0f,
                        0, 3.0f, 1, 1, 0,
                        5.0f, 1, 1,
                        0, 0.6f, 
                        1, 0, 0};
                    break;
            }
            return enemyData;
        }
    }
}

