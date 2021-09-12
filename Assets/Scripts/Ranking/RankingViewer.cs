using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB.Extensions;

namespace Ranking
{
    public class RankingViewer : MonoBehaviour
    {
        enum RankingType
        {
            TotalKyuukon,
            KyuukonRate,
        }

        private const string OBJECT_ID = "objectId";
        private const string COLUMN_SCORE = "score";
        private const string COLUMN_NAME = "name";

        [SerializeField]
        private RankingType type;

        [SerializeField]
        private BaseScore scoreGetter;
        [SerializeField]
        private UserName userNameGetter;

        [SerializeField]
        private naichilab.RankingBoards rankingBoards;

        [SerializeField]
        private TMPro.TextMeshProUGUI titleText;

        [SerializeField]
        private List<TMPro.TextMeshProUGUI> userTexts;

        public bool IsValid { get; set; } = true;

        [SerializeField]
        private float reloadIntervalSec = 15.0f;

        private TadaLib.Timer reloadTimer;

        private string ClassName => rankingBoards.GetRankingInfo((int)type).ClassName;
        private string _objectid = null;
        private string ObjectID
        {
            get { return _objectid ?? (_objectid = PlayerPrefs.GetString(BoardIdPlayerPrefsKey, null)); }
            set
            {
                if (_objectid == value)
                    return;
                PlayerPrefs.SetString(BoardIdPlayerPrefsKey, _objectid = value);
            }
        }
        private string BoardIdPlayerPrefsKey
        {
            get { return string.Format("{0}_{1}_{2}", "board", ClassName, OBJECT_ID); }
        }

        // Start is called before the first frame update
        void Start()
        {
            reloadTimer = new TadaLib.Timer(reloadIntervalSec);
            StartCoroutine(ReloadRanking());
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsValid)
            {
                titleText.enabled = false;
                foreach(var text in userTexts)
                {
                    text.enabled = false;
                }
            }

            if (reloadTimer.IsTimeout())
            {
                StartCoroutine(ReloadRanking());
                reloadTimer.TimeReset();
            }
        }

        IEnumerator ReloadRanking()
        {
            foreach(var text in userTexts)
            {
                text.text = "";
            }

            var board = rankingBoards.GetRankingInfo((int)type);
            yield return SendScore(board);
            yield return ReceiveScore(board);
        }

        IEnumerator SendScore(naichilab.RankingInfo board)
        {
            var score = new naichilab.NumberScore(scoreGetter.Score, board.CustomFormat);

            if (score.Type != board.Type)
            {
                throw new System.ArgumentException("スコアの型が違います。");
            }

            var record = new NCMB.NCMBObject(board.ClassName);
            record.ObjectId = ObjectID;

            record[COLUMN_NAME] = userNameGetter.Name;
            record[COLUMN_SCORE] = scoreGetter.Score;

            NCMB.NCMBException errorResult = null;

            yield return record.YieldableSaveAsync(error => errorResult = error);

            if (errorResult != null)
            {
                //NCMBのコンソールから直接削除した場合に、該当のobjectIdが無いので発生する（らしい）
                record.ObjectId = null;
                yield return record.YieldableSaveAsync(error => errorResult = error); //新規として送信
            }

            //ObjectIDを保存して次に備える
            ObjectID = record.ObjectId;
        }

        IEnumerator ReceiveScore(naichilab.RankingInfo board)
        {
            // ランキングボード情報の取得 & 文字列更新
            var so = new NCMB.Extensions.YieldableNcmbQuery<NCMB.NCMBObject>(board.ClassName);
            so.Limit = 10;
            // so.Skip = 10; // 取得開始場所を指定。プレイヤーの順位のひとつ前から始めたい
            so.OrderByDescending(COLUMN_SCORE);

            yield return so.FindAsync();

            Debug.Log("データ取得 : " + so.Count.ToString() + "件");

            int idx = 0;
            foreach (var r in so.Result)
            {
                if (userTexts.Count == idx)
                {
                    break;
                }


                string text = $"{idx + 1}  {r[COLUMN_NAME].ToString()}  {r[COLUMN_SCORE]}";
                if (r.ObjectId == ObjectID)
                {
                    text = "<color=yellow>" + text + "</color>";
                }

                userTexts[idx].text = text;
                ++idx;
            }
        }
    }
}