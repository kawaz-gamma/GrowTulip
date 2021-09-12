using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB.Extensions;

namespace Ranking
{
    public enum ScoreType
    {
        Int,
        Float,
    }

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
        private List<RankingNode> userTexts;

        public bool IsValid { get; set; } = true;

        [SerializeField]
        private float reloadIntervalSec = 15.0f;
        [SerializeField]
        private float reloadAccelIntervalSec = 1.0f;
        [SerializeField]
        private float reloadMaxIntervalSec = 120.0f;

        private ReloadTimer reloadTimer;

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
            reloadTimer = new ReloadTimer(reloadIntervalSec, reloadAccelIntervalSec, reloadMaxIntervalSec);
            reloadTimer.ToNearEnd();
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

            reloadTimer.Proc();
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
                text.NoText.text = "";
                text.NameText.text = "";
                text.ScoreText.text = "";
            }

            var board = rankingBoards.GetRankingInfo((int)type);
            var highScore = 0;
            // �n�C�X�R�A���ǂ����v�Z
            bool isHighScore = false;
            highScore = -1;
            {
                var hiScoreCheck = new YieldableNcmbQuery<NCMB.NCMBObject>(board.ClassName);
                hiScoreCheck.WhereEqualTo(OBJECT_ID, ObjectID);
                yield return hiScoreCheck.FindAsync();

                if (hiScoreCheck.Count > 0)
                {
                    // ���_�̌v�Z
                    foreach (var r in hiScoreCheck.Result)
                    {
                        highScore = Mathf.Max(highScore, int.Parse(r[COLUMN_SCORE].ToString()));

                        isHighScore = (scoreGetter.Score > int.Parse(r[COLUMN_SCORE].ToString()));
                        break;
                    }
                }
                else
                {
                    // �ŏ��̃X�R�A
                    isHighScore = true;
                }
            }

            yield return SendScore(board, isHighScore);
            yield return ReceiveScore(board, highScore);
        }

        IEnumerator SendScore(naichilab.RankingInfo board, bool isHighScore)
        {
            var score = new naichilab.NumberScore(scoreGetter.Score, board.CustomFormat);

            if (score.Type != board.Type)
            {
                throw new System.ArgumentException("�X�R�A�̌^���Ⴂ�܂��B");
            }

            if (isHighScore)
            {
                var record = new NCMB.NCMBObject(board.ClassName);
                record.ObjectId = ObjectID;

                record[COLUMN_NAME] = userNameGetter.Name;
                record[COLUMN_SCORE] = scoreGetter.Score;

                NCMB.NCMBException errorResult = null;

                yield return record.YieldableSaveAsync(error => errorResult = error);

                if (errorResult != null)
                {
                    //NCMB�̃R���\�[�����璼�ڍ폜�����ꍇ�ɁA�Y����objectId�������̂Ŕ�������i�炵���j
                    record.ObjectId = null;
                    yield return record.YieldableSaveAsync(error => errorResult = error); //�V�K�Ƃ��đ��M
                }

                //ObjectID��ۑ����Ď��ɔ�����
                ObjectID = record.ObjectId;
            }
        }

        IEnumerator ReceiveScore(naichilab.RankingInfo board, int highScore)
        {
            // �����L���O�̌����擾
            var totalCount = 0;
            // �����̏��ʂ��擾
            var myRank = 0;
            {
                NCMB.NCMBQuery<NCMB.NCMBObject> query = new NCMB.NCMBQuery<NCMB.NCMBObject>(board.ClassName);

                {
                    var fetchEnd = false;
                    query.CountAsync((int count, NCMB.NCMBException e) =>
                    {
                        if (e != null)
                        {
                        }
                        else
                        {
                            totalCount = count;
                            fetchEnd = true;
                        }
                    });

                    yield return new WaitUntil(() => fetchEnd);
                }

                {
                    var fetchEnd = false;
                    query.WhereGreaterThan(COLUMN_SCORE, Mathf.Max(highScore, scoreGetter.Score));
                    query.CountAsync((int count, NCMB.NCMBException e) =>
                    {
                        if (e != null)
                        {
                            Debug.Log("�����̏��ʂ��擾���s");
                        }
                        else
                        {
                            myRank = count;
                            fetchEnd = true;
                        }
                    });

                    yield return new WaitUntil(() => fetchEnd);
                }
            }

            // �����L���O�{�[�h���̎擾 & ������X�V
            var so = new NCMB.Extensions.YieldableNcmbQuery<NCMB.NCMBObject>(board.ClassName);
            so.Limit = 5;
            var skipCount = myRank - 1;

            if(myRank <= 2)
            {
                skipCount = 0;
            }
            if(myRank >= totalCount - 1)
            {
                skipCount = totalCount - 2;
            }
            if(skipCount < 0)
            {
                skipCount = 0;
            }

            //var skipCount = Mathf.Clamp(myRank - 1, 0, totalCount - 2);
            so.Skip = skipCount;
            so.OrderByDescending(COLUMN_SCORE);

            yield return so.FindAsync();

            Debug.Log("�f�[�^�擾 : " + so.Count.ToString() + "��");

            int idx = 0;
            foreach (var r in so.Result)
            {
                if (userTexts.Count == idx)
                {
                    break;
                }


                {
                    var text = (idx + 1 + skipCount).ToString();
                    if (r.ObjectId == ObjectID)
                    {
                        text = "<color=yellow>" + text + "</color>";
                    }
                    userTexts[idx].NoText.text = text;
                }

                {
                    var text = (r[COLUMN_NAME]).ToString();
                    if (r.ObjectId == ObjectID)
                    {
                        text = "<color=yellow>" + text + "</color>";
                    }
                    userTexts[idx].NameText.text = text;
                }

                {
                    var text = (r[COLUMN_SCORE]).ToString();
                    if (scoreGetter.ScoreType == ScoreType.Float)
                    {
                        text = (double.Parse(r[COLUMN_SCORE].ToString()) / 100.0f).ToString("F2");
                    }
                    
                    if (r.ObjectId == ObjectID)
                    {
                        text = "<color=yellow>" + text + "</color>";
                    }
                    userTexts[idx].ScoreText.text = text;
                }

                ++idx;
            }
        }
    }
}