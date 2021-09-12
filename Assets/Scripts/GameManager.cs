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
    private const float SAVE_INTERVAL_SEC = 10.0f;
    private TadaLib.Timer saveTimer;

    // 音
    AudioSource audioSource;
    [SerializeField]
    AudioClip getSe;
    [SerializeField]
    AudioClip buySe;
    [SerializeField]
    AudioClip plantSe;
    public static GameManager instance;

    private const float LAND_BASE_WIDTH = 1.25f;
    private const float LAND_BASE_HEIGHT = 1f;
    public const float INIT_LAND_WIDTH = 2.5f;
    public const float INIT_LAND_HEIGHT = 2f;
    public static float landWidth = INIT_LAND_WIDTH;
    public static float landHeight = INIT_LAND_HEIGHT;

    [SerializeField]
    Tulip tulipPrefab;
    [SerializeField]
    Soujiki soujikiPrefab;
    [SerializeField]
    Drone dronePrefab;
    [SerializeField]
    RectTransform kyuukonImage;

    [SerializeField]
    TMP_Text kyuukonCountText;
    int plantTulipCount = 0;
    int getTulipCount = 0;
    public int kyuukonCount = 0;

    private const int INIT_TOTAL_KYUUKON_COUNT = 1;
    public static int totalKyuukonCount { get; private set; } = INIT_TOTAL_KYUUKON_COUNT;
    public static float KyuukonPerTime => instance.CounterPerTime.GetCount(totalKyuukonCount);

    int kyuukonPerTulip = 4;
    public TulipList tulipList = new TulipList();
    public List<Soujiki> soujikiList = new List<Soujiki>();
    public List<Drone> droneList = new List<Drone>();
    int landPrice = 5;
    private const float LAND_MAG = 1.5f;
    int soujikiPrice = 10;
    private const float SOUJIKI_MAG = 1.5f;
    int dronePrice = 20;
    private const float DRONE_MAG = 1.5f;
    int sSpeedPrice = 30;
    private const float SOUJIKI_SPEED_MAG = 1.5f;
    int dSpeedPrice = 40;
    private const float DRONE_SPEED_MAG = 1.5f;
    int tSpeedPrice = 50;
    private const float TULIP_SPEED_MAG = 1.5f;
    int kPerPrice = 100;
    private const float KYUUKON_PER_COUNT = 1.5f;
    bool landKnown;
    bool soujikiKnown;
    bool droneKnown;
    bool sSpeedKnown;
    bool dSpeedKnown;
    bool tSpeedKnown;
    bool kPerKnown;
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
    UpgradeButton[] upgrades;

    [SerializeField]
    Animator kyuuKonIconAnim;

    [SerializeField]
    RectTransform optionPanel;

    [SerializeField]
    private Ranking.UserName userName;

    public KyuukonCountPerTime CounterPerTime { private set; get; }

    [SerializeField]
    private KyuukonIconPool kyuukonIconPool;

    void Awake()
    {
        instance = this;
        // はじめに何個か持っている
        //kyuukonCount = 1;
        //totalKyuukonCount = kyuukonCount;
        TryGetComponent(out audioSource);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Start()
    {
        if (loadSaveData)
        {
            TryLoad();
        }
        saveTimer = new TadaLib.Timer(SAVE_INTERVAL_SEC);
        CounterPerTime = new KyuukonCountPerTime(10, 0.1f);
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

        // セーブする
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

            //メインカメラ上のマウスカーソルのある位置からRayを飛ばす
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //レイヤーマスク作成
            //int layerMask = LayerMaskNo.DEFAULT;

            //Rayの長さ
            float maxDistance = 20;

            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance);
            if (hit)
            {
                // チューリップを収穫する
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

        // ボタン表示
        /*
        for (int i = 0; i < upgrades.Length; i++)
        {

        }
        */
        // 土地
        if (!landButton.gameObject.activeSelf)
        {
            landButton.gameObject.SetActive(true);
            landText.text = $"???({landPrice}T)";
        }
        if (!landKnown && kyuukonCount > landPrice)
        {
            landText.text = $"土地({landPrice}T)";
            landKnown = true;
        }
        landButton.interactable = kyuukonCount > landPrice;
        // 自動収穫機
        if (landKnown && !soujikiButton.gameObject.activeSelf)
        {
            soujikiButton.gameObject.SetActive(true);
            soujikiText.text = $"???({soujikiPrice}T)";
        }
        if (!soujikiKnown && kyuukonCount > soujikiPrice)
        {
            soujikiText.text = $"自動収穫機({soujikiPrice}T)";
            soujikiKnown = true;
        }
        soujikiButton.interactable = kyuukonCount > soujikiPrice;
        //自動種まき機
        if (soujikiKnown && !droneButton.gameObject.activeSelf)
        {
            droneButton.gameObject.SetActive(true);
            droneText.text = $"???({dronePrice}T)";
        }
        if (!droneKnown && kyuukonCount > dronePrice)
        {
            droneText.text = $"自動種まき機({dronePrice}T)";
            droneKnown = true;
        }
        droneButton.interactable = kyuukonCount > dronePrice;
        //収穫機スピード
        if (droneKnown && !sSpeedButton.gameObject.activeSelf)
        {
            sSpeedButton.gameObject.SetActive(true);
            sSpeedText.text = $"???({sSpeedPrice}T)";
        }
        if (!sSpeedKnown && kyuukonCount > sSpeedPrice)
        {
            sSpeedText.text = $"自動収穫機スピードアップ({sSpeedPrice}T)";
            sSpeedKnown = true;
        }
        sSpeedButton.interactable = kyuukonCount > sSpeedPrice;
        //種まき機スピード
        if (sSpeedKnown && !dSpeedButton.gameObject.activeSelf)
        {
            dSpeedButton.gameObject.SetActive(true);
            dSpeedText.text = $"???({dSpeedPrice}T)";
        }
        if (!dSpeedKnown && kyuukonCount > dSpeedPrice)
        {
            dSpeedText.text = $"自動種まき機スピードアップ({dSpeedPrice}T)";
            dSpeedKnown = true;
        }
        dSpeedButton.interactable = kyuukonCount > dSpeedPrice;
        //チューリップ
        if (dSpeedKnown && !tSpeedButton.gameObject.activeSelf)
        {
            tSpeedButton.gameObject.SetActive(true);
            tSpeedText.text = $"???({tSpeedPrice}T)";
        }
        if (!tSpeedKnown && kyuukonCount > tSpeedPrice)
        {
            tSpeedText.text = $"チューリップスピードアップ({tSpeedPrice}T)";
            tSpeedKnown = true;
        }
        tSpeedButton.interactable = kyuukonCount > tSpeedPrice;
        //球根獲得数
        if (tSpeedKnown && !kPerButton.gameObject.activeSelf)
        {
            kPerButton.gameObject.SetActive(true);
            kPerText.text = $"???({kPerPrice}T)";
        }
        if (!kPerKnown && kyuukonCount > kPerPrice)
        {
            kPerText.text = $"球根獲得数アップ({kPerPrice}T)";
            kPerKnown = true;
        }
        kPerButton.interactable = kyuukonCount > kPerPrice;

        kyuukonCountText.text = $"{kyuukonCount}";

        KoitanDebug.Display($"球根の所持数 = {kyuukonCount}\n");
        KoitanDebug.Display($"球根総獲得数 = {totalKyuukonCount}\n");
        KoitanDebug.Display($"時間あたり球根獲得数 = {CounterPerTime.GetCount(totalKyuukonCount):F1}\n");
        //KoitanDebug.Display($"植えたチューリップの本数 = {plantTulipCount}\n");
        //KoitanDebug.Display($"収穫したチューリップの本数 = {getTulipCount}\n");
        KoitanDebug.Display($"タイム : {(int)Time.time}s\n");
    }

    public void GetTulip(Tulip tulip)
    {
        if (tulip.state == TulipState.Tulip)
        {
            tulipList.Remove(tulip);
            Destroy(tulip.gameObject);
            getTulipCount++;
            // 音
            audioSource.PlayOneShot(getSe);
            // 取得した球根の表示
            Vector3 targetPos = kyuukonImage.position;
            Vector3 centerPos = RectTransformUtility.WorldToScreenPoint(Camera.main, tulip.transform.position);
            // ランダムな方向
            float randamDeg = Random.Range(0f, 360f);
            // 増やす数
            int baseKyuukonCount = kyuukonPerTulip / 2;
            int amari = kyuukonPerTulip % 2;
            int initKyuukonCount = baseKyuukonCount;
            if (getTulipCount % 2 < amari)
            {
                initKyuukonCount += 1;
            }
            for (int i = 0; i < initKyuukonCount; i++)
            {
                var kyuukonRect = kyuukonIconPool.InstantiateObject(kyuukonImage.parent);
                kyuukonRect.localScale = Vector3.one * 2 / Camera.main.orthographicSize;
                kyuukonRect.position = centerPos;
                var seq = DOTween.Sequence();
                seq.Append(kyuukonRect.DOMove(RectTransformUtility.WorldToScreenPoint(Camera.main, tulip.transform.position + Quaternion.Euler(0, 0, randamDeg + i * 360 / initKyuukonCount) * Vector3.right * 0.15f * initKyuukonCount), 0.25f).SetEase(Ease.OutCubic))
                   .AppendInterval(i * 0.2f)
                   .Append(kyuukonRect.DOMove(targetPos, 0.5f).SetEase(Ease.InBack))
                   .OnComplete(() =>
                   {
                       kyuukonIconPool.DestroyObject(kyuukonRect.gameObject);
                       kyuukonCount++;
                       totalKyuukonCount++;
                       /*
                       kyuukonImage.localScale = Vector3.one;
                       kyuukonImage.DOPunchScale(Vector3.one * 0.1f, 0.1f);
                       */
                   });
                /*
                kyuukonRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, tulip.transform.position + Quaternion.Euler(0, 0, randamDeg + i * 360 / kyuukonPerTulip) * Vector3.right * 0.15f * kyuukonPerTulip);
                kyuukonRect.DOMove(targetPos, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                  {
                      Destroy(kyuukonRect.gameObject);
                      kyuukonCount++;
                      totalKyuukonCount++;
                      *//*
                      kyuukonImage.localScale = Vector3.one;
                      kyuukonImage.DOPunchScale(Vector3.one * 0.1f, 0.1f);
                      *//*
                  });*/

            }
        }
    }

    /// <summary>
    /// 土地を買う
    /// </summary>
    public void BuyLand()
    {
        if (kyuukonCount > landPrice)
        {
            kyuukonCount -= landPrice;
            Camera.main.orthographicSize += 1;
            walls.localScale = Vector3.one * Camera.main.orthographicSize;
            landPrice = Mathf.FloorToInt(landPrice * LAND_MAG);
            landText.text = $"土地({landPrice}T)";
            landWidth = LAND_BASE_WIDTH * Camera.main.orthographicSize;
            landHeight = LAND_BASE_HEIGHT * Camera.main.orthographicSize;
            audioSource.PlayOneShot(buySe);
        }
    }

    /// <summary>
    /// 自動収穫機を買う
    /// </summary>
    public void BuySoujiki()
    {
        if (kyuukonCount > soujikiPrice)
        {
            kyuukonCount -= soujikiPrice;
            soujikiPrice = Mathf.FloorToInt(soujikiPrice * SOUJIKI_MAG);
            soujikiText.text = $"自動収穫機({soujikiPrice}T)";
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
            dronePrice = Mathf.FloorToInt(dronePrice * DRONE_MAG);
            droneText.text = $"自動種まき機({dronePrice}T)";
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
            sSpeedPrice = Mathf.FloorToInt(sSpeedPrice * SOUJIKI_SPEED_MAG);
            sSpeedText.text = $"収穫機スピードアップ({sSpeedPrice}T)";
            Soujiki.speed *= Soujiki.speedMag;
            audioSource.PlayOneShot(buySe);
        }
    }

    public void BuySpeedUpDrone()
    {
        if (kyuukonCount > dSpeedPrice)
        {
            kyuukonCount -= dSpeedPrice;
            dSpeedPrice = Mathf.FloorToInt(dSpeedPrice * DRONE_SPEED_MAG);
            dSpeedText.text = $"種まき機スピードアップ({dSpeedPrice}T)";
            Drone.speed *= Drone.SPEED_MAG;
            Drone.interval /= Drone.SPEED_MAG;
            audioSource.PlayOneShot(buySe);
        }
    }

    public void BuySpeedUpTulip()
    {
        if (kyuukonCount > tSpeedPrice)
        {
            kyuukonCount -= tSpeedPrice;
            tSpeedPrice = Mathf.FloorToInt(tSpeedPrice * TULIP_SPEED_MAG);
            tSpeedText.text = $"チューリップスピードアップ({tSpeedPrice}T)";
            Tulip.tulipTime /= Tulip.TULIP_TIME_MAG;
            audioSource.PlayOneShot(buySe);
        }
    }

    public void BuyKPerUp()
    {
        if (kyuukonCount > kPerPrice)
        {
            kyuukonCount -= kPerPrice;
            kPerPrice = Mathf.FloorToInt(kPerPrice * KYUUKON_PER_COUNT);
            kPerText.text = $"球根獲得数アップ({kPerPrice}T)";
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
            // 植えられるか調べる
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
            // チューリップを植える
            var tulip = Instantiate(tulipPrefab);
            tulip.transform.position = pos;
            plantTulipCount++;
            kyuukonCount--;
        }
    }

    public bool PlantKyuukon(Vector3 pos)
    {
        // 植えられるか調べる
        if (EnablePlant(pos))
        {
            ForcePlantKyuukon(pos);
            // 音
            audioSource.PlayOneShot(plantSe);
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
        data.KyuukonPerTulip = kyuukonPerTulip;
        data.KyuukonPerPrice = kPerPrice;
        data.UserName = userName.Name;

        data.Save();
    }

    bool TryLoad()
    {
        // セーブデータがあればロードする
        if (global::Save.SaveData.HasSaveData())
        {
            var data = global::Save.SaveData.Load();

            kyuukonCount = Mathf.Max(1, data.KyuukonCount);
            totalKyuukonCount = data.TotalKyuukonCount;
            Camera.main.orthographicSize = data.LandScale;
            walls.localScale = Vector3.one * data.LandScale;
            landWidth = LAND_BASE_WIDTH * Camera.main.orthographicSize;
            landHeight = LAND_BASE_HEIGHT * Camera.main.orthographicSize;
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
            kyuukonPerTulip = data.KyuukonPerTulip;
            kPerPrice = data.KyuukonPerPrice;
            userName.SetUserName(data.UserName);

            return true;
        }

        return false;
    }

    public void InitializeGameManager()
    {
        // GameManager分
        totalKyuukonCount = INIT_TOTAL_KYUUKON_COUNT;

        landWidth = INIT_LAND_WIDTH;
        landHeight = INIT_LAND_HEIGHT;

        Soujiki.speed = Soujiki.INIT_SPEED;
        Drone.speed = Drone.INIT_SPEED;
        Tulip.tulipTime = Tulip.INIT_TULIP_TIME;
    }
}
