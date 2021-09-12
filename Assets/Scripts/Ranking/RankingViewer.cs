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
                throw new System.ArgumentException("�X�R�A�̌^���Ⴂ�܂��B");
            }

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

        IEnumerator ReceiveScore(naichilab.RankingInfo board)
        {
            // �����L���O�{�[�h���̎擾 & ������X�V
            var so = new NCMB.Extensions.YieldableNcmbQuery<NCMB.NCMBObject>(board.ClassName);
            so.Limit = 10;
            // so.Skip = 10; // �擾�J�n�ꏊ���w��B�v���C���[�̏��ʂ̂ЂƂO����n�߂���
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