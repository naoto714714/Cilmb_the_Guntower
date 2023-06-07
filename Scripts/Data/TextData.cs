using UnityEngine;

namespace Text
{
    /// <summary>
    /// テキストのデータを管理するクラス
    /// </summary>
    class TextData : MonoBehaviour
    {
        static string text;

        /// <summary>
        /// テキストナンバーのテキストをセットするメソッド
        /// </summary>
        /// <param name="textNumber">取得したいテキストの番号</param>
        public static string setText(int textNumber)
        {
            switch (textNumber)
            {
                //１～１００はチュートリアル用
                case 1:
                    text =
                        @"チュートリアルへようこそ！
ここでは基本的な操作を説明します

↑ エントランスに戻る";
                    break;

                case 2:
                    text =
                        @"左クリックを長押しでも射撃することができます
マガジンの残弾が０になると自動的にリロードします
控えの武器も数秒後に自動的にリロードされます";
                    break;

                case 3:
                    text =
                        @"ローリング中は敵の弾が当たらなくなります
ローリングで弾を避けながら進んでみましょう！
※キーボードによっては反応しないキーの組み合わせがある可能性があります（キーロールオーバー）";
                    break;

                case 4:
                    text =
                        @"ボムを使うと敵の弾をすべて消すことができます
使ってから少しの間、効果が続きます";
                    break;

                case 5:
                    text =
                        @"次は実践形式の練習です
今までの知識を活用して進みましょう！";
                    break;

                case 6:
                    text =
                        @"チュートリアルは以上です
階段を上るとエントランスに戻ります
第３階層にあるゴールを目指しましょう！";
                    break;

            }

            return text;
        }
    }
}

