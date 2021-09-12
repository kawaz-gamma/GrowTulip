using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool loadSaveData = true;
    [SerializeField]
    private float saveIntervalSec = 10.0f;
    private TadaLib.Timer saveTimer;

    // ��
    AudioSource audioSource;
    [SerializeField]
    AudioClip getSe;
    [SerializeField]
    AudioClip buySe;
    public static GameManager instance;
    public static float landBaseWidth = 1.25f;
    public static float landBaseHeight = 1f;
    public static float landWidth = 2.5f;
    public static float landHeight = 2f;
    [SerializeField]
    Tulip tulipPrefab;
    [SerializeField]
    Soujiki soujikiPrefab;
    [SerializeField]
    Drone dronePrefab;
    [SerializeField]
    RectTransform kyuukonImage;
    [SerializeField]
    RectTransform kyuukonIcon;
    [SerializeField]
    TMP_Text kyuukonCountText;
    int plantTulipCount = 0;
    int getTulipCount = 0;
    public int kyuukonCount = 0;
    public static int totalKyuukonCount { get; private set; } = 0;
    int kyuukonPerTulip = 2;
    public TulipList tulipList = new TulipList();
    public List<Soujiki> soujikiList = new List<Soujiki>();
    public List<Drone> droneList = new List<Drone>();
    int landPrice = 5;
    float landMag = 1.5f;
    int soujikiPrice = 10;
    float soujikiMag = 1.5f;
    int dronePrice = 20;
    float droneMag = 1.5f;
    int sSpeedPrice = 30;
    float sSpeedMag = 1.5f;
    int dSpeedPrice = 40;
    float dSpeedMag = 1.5f;
    int tSpeedPrice = 50;
    float tSpeedMag = 1.5f;
    int kPerPrice = 100;
    float kPerMag = 1.5f;
    [SerializeField]
    Transform walls;
    [SerializeField]
    TMP_Text landText;
    [SerializeField]
    TMP_Text soujikiText;
    [SerializeField]
    TMP_Text droneText;
    [SerializeField]
    TMP_Text sSpeedText;
    [SerializeField]
    TMP_Text dSpeedText;
    [SerializeField]
    TMP_Text tSpeedText;
    [SerializeField]
    TMP_Text kPerText;
    [SerializeField]
    Button landButton;
    [SerializeField]
    Button soujikiButton;
    [SerializeField]
    Button droneButton;
    [SerializeField]
    Button sSpeedButton;
    [SerializeField]
    Button dSpeedButton;
    [SerializeField]
    Button tSpeedButton;
    [SerializeField]
    Button kPerButton;

    [SerializeField]
    RectTransform optionPanel;

    KyuukonCountPerTime counterPerTime;

    void Awake()
    {
        instance = this;
        // �͂��߂ɉ��������Ă���
        //kyuukonCount = 1;
        //totalKyuukonCount = kyuukonCount;
        TryGetComponent(out audioSource);
    }

    private void Start()
    {
        if (loadSaveData)
        {
            TryLoad();
        }
        saveTimer = new TadaLib.Timer(saveIntervalSec);
        counterPerTime = new KyuukonCountPerTime(10, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 5f;
        }
        else
        {
            Time.timeScale = 1f;
        }
#endif

        // �Z�[�u����
        if (saveTimer.IsTimeout())
        {
            Debug.Log("save");
            Save();
            saveTimer.TimeReset();
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (Input.GetMouseButtonDown(0))
        {

            //���C���J������̃}�E�X�J�[�\���̂���ʒu����Ray���΂�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //���C���[�}�X�N�쐬
            //int layerMask = LayerMaskNo.DEFAULT;

            //Ray�̒���
            float maxDistance = 20;

            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance);
            if (hit)
            {
                // �`���[���b�v�����n����
                if (hit.collider.tag == "Tulip")
                {
                    var tulip = hit.collider.GetComponent<Tulip>();
                    GetTulip(tulip);
                }
            }
            else
            {
                PlantKyuukon(mousePos);
            }
        }

        // �{�^���\��
        if (!landButton.gameObject.activeSelf && kyuukonCount > landPrice)
        {
            landButton.gameObject.SetActive(true);
            landText.text = $"�y�n({landPrice}T)";
        }
        landButton.interactable = kyuukonCount > landPrice;
        if (!soujikiButton.gameObject.activeSelf && kyuukonCount > soujikiPrice)
        {
            soujikiButton.gameObject.SetActive(true);
            soujikiText.text = $"�������n�@({soujikiPrice}T)";
        }
        soujikiButton.interactable = kyuukonCount > soujikiPrice;
        if (!droneButton.gameObject.activeSelf && kyuukonCount > dronePrice)
        {
            droneButton.gameObject.SetActive(true);
            droneText.text = $"������܂��@({dronePrice}T)";
        }
        droneButton.interactable = kyuukonCount > dronePrice;
        if (!sSpeedButton.gameObject.activeSelf && kyuukonCount > sSpeedPrice)
        {
            sSpeedButton.gameObject.SetActive(true);
            sSpeedText.text = $"���n�@�X�s�[�h�A�b�v({sSpeedPrice}T)";
        }
        sSpeedButton.interactable = kyuukonCount > sSpeedPrice;
        if (!dSpeedButton.gameObject.activeSelf && kyuukonCount > dSpeedPrice)
        {
            dSpeedButton.gameObject.SetActive(true);
            dSpeedText.text = $"��܂��@�X�s�[�h�A�b�v({dSpeedPrice}T)";
        }
        dSpeedButton.interactable = kyuukonCount > dSpeedPrice;
        if (!tSpeedButton.gameObject.activeSelf && kyuukonCount > tSpeedPrice)
        {
            tSpeedButton.gameObject.SetActive(true);
            tSpeedText.text = $"�`���[���b�v�X�s�[�h�A�b�v({tSpeedPrice}T)";
        }
        tSpeedButton.interactable = kyuukonCount > tSpeedPrice;
        if (!kPerButton.gameObject.activeSelf && kyuukonCount > kPerPrice)
        {
            kPerButton.gameObject.SetActive(true);
            kPerText.text = $"�����l�����A�b�v({kPerPrice}T)";
        }
        kPerButton.interactable = kyuukonCount > kPerPrice;

        kyuukonCountText.text = $"{kyuukonCount}";

        KoitanDebug.Display($"�����̏����� = {kyuukonCount}\n");
        KoitanDebug.Display($"�������l���� = {totalKyuukonCount}\n");
        KoitanDebug.Display($"���Ԃ����苅���l���� = {counterPerTime.GetCount(totalKyuukonCount):F1}\n");
        //KoitanDebug.Display($"�A�����`���[���b�v�̖{�� = {plantTulipCount}\n");
        //KoitanDebug.Display($"���n�����`���[���b�v�̖{�� = {getTulipCount}\n");
        KoitanDebug.Display($"�^�C�� : {(int)Time.time}s\n");
    }

    public void GetTulip(Tulip tulip)
    {
        if (tulip.state == TulipState.Tulip)
        {
            tulipList.Remove(tulip);
            Destroy(tulip.gameObject);
            getTulipCount++;
            // ��
            audioSource.PlayOneShot(getSe);
            // �擾���������̕\��
            Vector3 targetPos = kyuukonImage.position;
            // �����_���ȕ���
            float randamDeg = Random.Range(0f, 360f);
            for (int i = 0; i < kyuukonPerTulip; i++)
            {
                var kyuukonRect = Instantiate(kyuukonIcon, kyuukonImage.root);
                kyuukonRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, tulip.transform.position + Quaternion.Euler(0, 0, randamDeg + i * 360 / kyuukonPerTulip) * Vector3.right * 0.15f * kyuukonPerTulip);
                kyuukonRect.localScale = Vector3.one * 2 / Camera.main.orthographicSize;
                kyuukonRect.DOMove(targetPos, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    Destroy(kyuukonRect.gameObject);
                    kyuukonCount++;
                    totalKyuukonCount++;
                    /*
                    kyuukonImage.localScale = Vector3.one;
                    kyuukonImage.DOPunchScale(Vector3.one * 0.1f, 0.1f);
                    */
                });
            }
        }
    }

    /// <summary>
    /// �y�n�𔃂�
    /// </summary>
    public void BuyLand()
    {
        if (kyuukonCount > landPrice)
        {
            kyuukonCount -= landPrice;
            Camera.main.orthographicSize += 1;
            walls.localScale = Vector3.one * Camera.main.orthographicSize;
            landPrice = Mathf.FloorToInt(landPrice * landMag);
            landText.text = $"�y�n({landPrice}T)";
            landWidth = landBaseWidth * Camera.main.orthographicSize;
            landHeight = landBaseHeight * Camera.main.orthographicSize;
            audioSource.PlayOneShot(buySe);
        }
    }

    /// <summary>
    /// �������n�@�𔃂�
    /// </summary>
    public void BuySoujiki()
    {
        if (kyuukonCount > soujikiPrice)
        {
            kyuukonCount -= soujikiPrice;
            soujikiPrice = Mathf.FloorToInt(soujikiPrice * soujikiMag);
            soujikiText.text = $"�������n�@({soujikiPrice}T)";
            var soujiki = Instantiate(soujikiPrefab);
            soujikiList.Add(soujiki);
            audioSource.PlayOneShot(buySe);
        }
    }

    public void BuyDrone()
    {
        if (kyuukonCount > dronePrice)
        {
            kyuukonCount -= dronePrice;
            dronePrice = Mathf.FloorToInt(dronePrice * droneMag);
            droneText.text = $"������܂��@({dronePrice}T)";
            var drone = Instantiate(dronePrefab);
            droneList.Add(drone);
            audioSource.PlayOneShot(buySe);
        }
    }

    public void BuySpeedUpSoujiki()
    {
        /*
        foreach (Soujiki soujiki in soujikiList)
        {
            soujiki.speed *= 2;
        }
        */
        if (kyuukonCount > sSpeedPrice)
        {
            kyuukonCount -= sSpeedPrice;
            sSpeedPrice = Mathf.FloorToInt(sSpeedPrice * sSpeedMag);
            sSpeedText.text = $"���n�@�X�s�[�h�A�b�v({sSpeedPrice}T)";
            Soujiki.speed *= Soujiki.speedMag;
            audioSource.PlayOneShot(buySe);
        }
    }

    public void BuySpeedUpDrone()
    {
        if (kyuukonCount > dSpeedPrice)
        {
            kyuukonCount -= dSpeedPrice;
            dSpeedPrice = Mathf.FloorToInt(dSpeedPrice * dSpeedMag);
            dSpeedText.text = $"��܂��@�X�s�[�h�A�b�v({dSpeedPrice}T)";
            Drone.speed *= Drone.speedMag;
            Drone.interval /= Drone.speedMag;
            audioSource.PlayOneShot(buySe);
        }
    }

    public void BuySpeedUpTulip()
    {
        if (kyuukonCount > tSpeedPrice)
        {
            kyuukonCount -= tSpeedPrice;
            tSpeedPrice = Mathf.FloorToInt(tSpeedPrice * tSpeedMag);
            tSpeedText.text = $"�`���[���b�v�X�s�[�h�A�b�v({tSpeedPrice}T)";
            Tulip.tulipTime /= Tulip.tulipTimeMag;
            audioSource.PlayOneShot(buySe);
        }
    }

    public void BuyKPerUp()
    {
        if (kyuukonCount > kPerPrice)
        {
            kyuukonCount -= kPerPrice;
            kPerPrice = Mathf.FloorToInt(kPerPrice * kPerMag);
            kPerText.text = $"�����l�����A�b�v({kPerPrice}T)";
            kyuukonPerTulip += 1;
            audioSource.PlayOneShot(buySe);
        }
    }

    public void OpenOptionPanel()
    {
        optionPanel.gameObject.SetActive(true);
        optionPanel.localScale = Vector3.zero;
        optionPanel.DOScale(Vector3.one, 0.25f);
    }

    public void CloseOptionPanel()
    {
        optionPanel.DOScale(Vector3.zero, 0.25f).OnComplete(() => optionPanel.gameObject.SetActive(false));
    }

    public bool EnablePlant(Vector3 pos)
    {
        if (kyuukonCount > 0)
        {
            // �A�����邩���ׂ�
            RaycastHit2D circleHit = Physics2D.CircleCast((Vector2)pos, 0.6f, Vector2.zero, 10);
            if (!circleHit)
            {
                return true;
            }
        }
        return false;
    }

    void ForcePlantKyuukon(Vector3 pos)
    {
        if (kyuukonCount > 0)
        {
            // �`���[���b�v��A����
            var tulip = Instantiate(tulipPrefab);
            tulip.transform.position = pos;
            plantTulipCount++;
            kyuukonCount--;
        }
    }

    public bool PlantKyuukon(Vector3 pos)
    {
        // �A�����邩���ׂ�
        if (EnablePlant(pos))
        {
            ForcePlantKyuukon(pos);
            return true;
        }
        return false;
    }

    void Save()
    {
        var data = global::Save.SaveData.CreateZeroValue();
        data.KyuukonCount = kyuukonCount;
        data.TotalKyuukonCount = totalKyuukonCount;
        data.LandScale = Camera.main.orthographicSize;
        data.LandPrice = landPrice;
        data.SoujikiCount = soujikiList.Count;
        data.SoujikiPrice = soujikiPrice;
        data.DroneCount = droneList.Count;
        data.DronePrice = dronePrice;
        data.SoujikiSpeed = Soujiki.speed;
        data.SoujikiSpeedPrice = sSpeedPrice;
        data.DroneSpeed = Drone.speed;
        data.DroneSpeedPrice = dSpeedPrice;
        data.TulipSpeed = Tulip.tulipTime;
        data.TulipSpeedPrice = tSpeedPrice;

        data.Save();
    }

    bool TryLoad()
    {
        // �Z�[�u�f�[�^������΃��[�h����
        if (global::Save.SaveData.HasSaveData())
        {
            var data = global::Save.SaveData.Load();

            kyuukonCount = data.KyuukonCount;
            totalKyuukonCount = data.TotalKyuukonCount;
            Camera.main.orthographicSize = data.LandScale;
            walls.localScale = Vector3.one * data.LandScale;
            landWidth = landBaseWidth * Camera.main.orthographicSize;
            landHeight = landBaseHeight * Camera.main.orthographicSize;
            landPrice = data.LandPrice;
            for (int idx = 0; idx < data.SoujikiCount; ++idx)
            {
                var soujiki = Instantiate(soujikiPrefab);
                soujikiList.Add(soujiki);
            }
            soujikiPrice = data.SoujikiPrice;
            for (int idx = 0; idx < data.DroneCount; ++idx)
            {
                var drone = Instantiate(dronePrefab);
                droneList.Add(drone);
            }
            dronePrice = data.DronePrice;
            Soujiki.speed = data.SoujikiSpeed;
            sSpeedPrice = data.SoujikiSpeedPrice;
            Drone.speed = data.DroneSpeed;
            dSpeedPrice = data.DroneSpeedPrice;
            Tulip.tulipTime = data.TulipSpeed;
            tSpeedPrice = data.TulipSpeedPrice;

            return true;
        }

        return false;
    }
}
