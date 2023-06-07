using UnityEngine;

namespace Data
{
    /// <summary>
    /// 銃の説明文を管理するクラス
    /// </summary>
    class GunData : MonoBehaviour
    {
        static string gunExplanation;

        /// <summary>
        /// 銃の説明文をセットするメソッド
        /// </summary>
        /// <param name="classNumber">銃の分類番号</param>
        /// <returns>銃の説明文</returns>
        public static string SetGunExplanation(int classNumber)
        {
            switch (classNumber)
            {
                case 1: //ハンドガン
                    gunExplanation =
                        @"シンプルなハンドガン。
弾薬は無限だが扱いづらい。";
                    break;

                case 2: //ソードオフショットガン
                    gunExplanation = 
                        @"射程が短いショットガン。
敵に近づいて撃てば威力抜群。";
                    break;

                case 3: //貫通ガン
                    gunExplanation = 
                        @"弾が敵を貫通するハンドガン。";
                    break;

                case 4: //バウンドガン
                    gunExplanation = 
                        @"弾が壁で跳ね返るハンドガン。";
                    break;

                case 5: //グレネードガン
                    gunExplanation = 
                        @"グレネードを発射する銃。
爆風で近くの敵にもダメージを与える。";
                    break;

                case 6: //リボルバー
                    gunExplanation =
                        @"かなり威力の高い拳銃。
ブレも少なく非常に扱いやすい。";
                    break;

                case 7: //マークスマンライフル
                    gunExplanation =
                        @"射程が長い高精度ライフル。
敵を貫通しないが、ある程度の連射が可能。";
                    break;

                case 8: //スナイパーライフル
                    gunExplanation =
                        @"遠距離への攻撃ができ、貫通もするスナイパーライフル。
リロードは長いが非常に高威力。";
                    break;

                case 9: //ショットガン
                    gunExplanation =
                        @"ソードオフショットガンより精度も威力も格段に高いショットガン。
中距離の敵にも十分有効。";
                    break;

                case 10: //ミニマシンガン
                    gunExplanation =
                        @"威力は低いが連射が早いマシンガン。
弾の消耗が激しい。";
                    break;

                case 11: //サブマシンガン
                    gunExplanation =
                        @"精度もそこそこで連射が非常に早いマシンガン。
弾切れには要注意。";
                    break;

                case 12: //ランチャー
                    gunExplanation =
                        @"１発ずつしか撃てないが威力は絶大。
爆風で広い範囲の敵にも高ダメージ。";
                    break;

                case 13: //ゴールデンハンドガン
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たハンドガン。
一度に複数発射し、発射間隔も非常に短い。";
                    break;

                case 14: //ゴールデンピストル
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たピストル。
弾がバウンド・巨大化し、一度に２発発射する。";
                    break;

                case 15: //ゴールデンソードオフ
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たソードオフショットガン。
発射間隔が非常に短い。";
                    break;

                case 16: //ゴールデンショットガン
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たショットガン。
高精度かつ弾が跳ね返り、遠距離の敵へも有効。";
                    break;

                case 17: //ゴールデンMマシンガン
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たミニマシンガン。
一度に大量の弾を発射し、発射間隔も短い。";
                    break;

                case 18: //ゴールデンSマシンガン
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たサブマシンガン。
一度に多くの弾を発射し、弾が跳ね返る。";
                    break;

                case 19: //ゴールデンリボルバー
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たリボルバー。
非常に高ダメージで、敵を貫通する。";
                    break;

                case 20: //ゴールデンマグナム
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たマグナム。
弾がバウンド・巨大化し、威力も非常に高い。";
                    break;

                case 21: //ゴールデンライフル
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たマークスマンライフル。
弾速が非常に早く、弾が跳ね返る。";
                    break;

                case 22: //ゴールデンスナイパー
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たスナイパーライフル。
敵を貫通しつつ、破壊的な威力を誇る。";
                    break;

                case 23: //ゴールデングレラン
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たグレネードランチャー。
一度に複数のグレネードを発射する。";
                    break;

                case 24: //ゴールデンランチャー
                    gunExplanation =
                        @"弾数と引き換えに絶大な性能を得たランチャー。
室内にいる敵を一掃できる。";
                    break;

                case 25: //タクティカルアサルト
                    gunExplanation =
                        @"弾が跳ね返るアサルトライフル。
精度も高く扱いやすい。";
                    break;

                case 26: //アサルトライフル
                    gunExplanation =
                        @"一般的なアサルトライフル。
精度は不安定だが威力はそこそこ。";
                    break;

                case 27: //バーストアサルト
                    gunExplanation =
                        @"３発の弾を連続で発射するアサルトライフル。
扱いが特殊だが強力な武器。";
                    break;

                case 28: //フルオートピストル
                    gunExplanation =
                        @"フルオートに対応したピストル。
精度も高く発射間隔も短いが、射程が短い。";
                    break;

                case 29: //ピストル
                    gunExplanation =
                        @"一般的なピストル。
ハンドガンよりも発射間隔が短い。";
                    break;

                case 30: //コンパクトアサルト
                    gunExplanation =
                        @"通常のアサルトライフルよりも近距離向けのアサルトライフル。
射程が短くなったが、精度が高く発射間隔も短い。";
                    break;

                case 31: //ヘビーアサルト
                    gunExplanation =
                        @"アサルトライフルよりも遠距離向けのアサルトライフル。
射程・精度・威力が上がったが、発射間隔が長くなった。";
                    break;

                case 32: //ツインアサルト
                    gunExplanation =
                        @"１度に２発の弾を発射するアサルトライフル。
上手く使うことができたら非常に高火力な武器となるだろう。";
                    break;

                case 33: //マシンガン
                    gunExplanation =
                        @"一般的なマシンガン。
精度は低いが弾数で補おう。";
                    break;

                case 34: //ヘビースナイパー
                    gunExplanation =
                        @"より威力と速度が増したスナイパーライフル。
リロード時間が長くなったことに注意。";
                    break;

                case 35: //４連式ランチャー
                    gunExplanation =
                        @"１度に４つのグレネードを発射するグレネードランチャー。
非常に強力。";
                    break;

                case 36: //ライトマシンガン
                    gunExplanation =
                        @"より扱いやすくなったマシンガン。
通常のマシンガンよりも近距離向け。";
                    break;

                case 37: //コンパクトSMG
                    gunExplanation =
                        @"通常のサブマシンガンよりも近距離向けのサブマシンガン。
弾切れに注意しよう。";
                    break;

                case 38: //バーストSMG
                    gunExplanation =
                        @"５発の弾を連続で発射するサブマシンガン。
扱いが特殊だが強力な武器。";
                    break;

                case 39: //タクティカルSMG
                    gunExplanation =
                        @"弾が跳ね返るサブマシンガン。
精度も悪くなく扱いやすい。";
                    break;

                case 100: //デバッグガン
                    gunExplanation = 
                        @"デバッグ用";
                    break;

            }
            return gunExplanation;
        }
    }
}

