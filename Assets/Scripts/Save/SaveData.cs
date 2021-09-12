using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save
{
    // 保存されるデータ
    public struct SaveData
    {
        // 所持球根数
        public int KyuukonCount;
        // 球根の総合獲得数
        public int TotalKyuukonCount;
        // 土地の大きさ
        public float LandScale;
        // 土地の価格
        public int LandPrice;
        // 収穫機の数
        public int SoujikiCount;
        // 収穫機の価格
        public int SoujikiPrice;
        // 自動種まき機の数
        public int DroneCount;
        // 自動種まき機の価格
        public int DronePrice;
        // 収穫機のスピード
        public float SoujikiSpeed;
        // 収穫機スピードアップの価格
        public int SoujikiSpeedPrice;
        // 種まき機のスピード
        public float DroneSpeed;
        // 種まき機のスピードアップの価格
        public int DroneSpeedPrice;
        // チューリップのスピード
        public float TulipSpeed;
        // チューリップのスピードアップ価格
        public int TulipSpeedPrice;
        // チューリップからとれる球根数
        public int KyuukonPerTulip;
        // チューリップからとれる球根数アップの価格
        public int KyuukonPerPrice;
        // ランキングに登録されるユーザー名
        public string UserName;

        public static SaveData CreateZeroValue()
        {
            return new SaveData()
            {
                KyuukonCount = 0,
                TotalKyuukonCount = 0,
                LandScale = 0.0f,
                LandPrice = 0,
                SoujikiCount = 0,
                SoujikiPrice = 0,
                DroneCount = 0,
                DronePrice = 0,
                SoujikiSpeed = 0.0f,
                SoujikiSpeedPrice = 0,
                DroneSpeed = 0.0f,
                DroneSpeedPrice = 0,
                TulipSpeed = 0.0f,
                TulipSpeedPrice = 0,
                KyuukonPerTulip = 0,
                KyuukonPerPrice = 0,
                UserName = "Guest"
            };
        }

        public static SaveData Load()
        {
            // セーブデータがすべてあるか確かめる
            if (!HasSaveData())
            {
                Debug.Log("load failed");
                return CreateZeroValue();
            }

            Debug.Log("load successed");
            return new SaveData()
            {
                KyuukonCount = PlayerPrefs.GetInt(nameof(KyuukonCount)),
                TotalKyuukonCount = PlayerPrefs.GetInt(nameof(TotalKyuukonCount)),
                SoujikiCount = PlayerPrefs.GetInt(nameof(SoujikiCount)),
                LandScale = PlayerPrefs.GetFloat(nameof(LandScale)),
                LandPrice = PlayerPrefs.GetInt(nameof(LandPrice)),
                SoujikiPrice = PlayerPrefs.GetInt(nameof(SoujikiPrice)),
                DroneCount = PlayerPrefs.GetInt(nameof(DroneCount)),
                DronePrice = PlayerPrefs.GetInt(nameof(DronePrice)),
                SoujikiSpeed = PlayerPrefs.GetFloat(nameof(SoujikiSpeed)),
                SoujikiSpeedPrice = PlayerPrefs.GetInt(nameof(SoujikiSpeedPrice)),
                DroneSpeed = PlayerPrefs.GetFloat(nameof(DroneSpeed)),
                DroneSpeedPrice = PlayerPrefs.GetInt(nameof(DroneSpeedPrice)),
                TulipSpeed = PlayerPrefs.GetFloat(nameof(TulipSpeed)),
                TulipSpeedPrice = PlayerPrefs.GetInt(nameof(TulipSpeedPrice)),
                KyuukonPerTulip = PlayerPrefs.GetInt(nameof(KyuukonPerTulip)),
                KyuukonPerPrice = PlayerPrefs.GetInt(nameof(KyuukonPerPrice)),
                UserName = PlayerPrefs.GetString(nameof(UserName))
            };
        }

        public static bool HasSaveData()
        {
            if (!PlayerPrefs.HasKey(nameof(KyuukonCount))) return false;
            if (!PlayerPrefs.HasKey(nameof(TotalKyuukonCount))) return false;
            if (!PlayerPrefs.HasKey(nameof(LandScale))) return false;
            if (!PlayerPrefs.HasKey(nameof(LandPrice))) return false;
            if (!PlayerPrefs.HasKey(nameof(SoujikiCount))) return false;
            if (!PlayerPrefs.HasKey(nameof(SoujikiPrice))) return false;
            if (!PlayerPrefs.HasKey(nameof(DroneCount))) return false;
            if (!PlayerPrefs.HasKey(nameof(DronePrice))) return false;
            if (!PlayerPrefs.HasKey(nameof(SoujikiSpeed))) return false;
            if (!PlayerPrefs.HasKey(nameof(SoujikiSpeedPrice))) return false;
            if (!PlayerPrefs.HasKey(nameof(DroneSpeed))) return false;
            if (!PlayerPrefs.HasKey(nameof(DroneSpeedPrice))) return false;
            if (!PlayerPrefs.HasKey(nameof(TulipSpeed))) return false;
            if (!PlayerPrefs.HasKey(nameof(TulipSpeedPrice))) return false;
            if (!PlayerPrefs.HasKey(nameof(KyuukonPerTulip))) return false;
            if (!PlayerPrefs.HasKey(nameof(KyuukonPerPrice))) return false;
            if (!PlayerPrefs.HasKey(nameof(UserName))) return false;

            return true;
        }

        public void DeleteSaveData()
        {
            PlayerPrefs.DeleteAll();
        }

        public void Save()
        {
            Save(KyuukonCount, nameof(KyuukonCount));
            Save(TotalKyuukonCount, nameof(TotalKyuukonCount));
            Save(LandScale, nameof(LandScale));
            Save(LandPrice, nameof(LandPrice));
            Save(SoujikiCount, nameof(SoujikiCount));
            Save(SoujikiPrice, nameof(SoujikiPrice));
            Save(DroneCount, nameof(DroneCount));
            Save(DronePrice, nameof(DronePrice));
            Save(SoujikiSpeed, nameof(SoujikiSpeed));
            Save(SoujikiSpeedPrice, nameof(SoujikiSpeedPrice));
            Save(DroneSpeed, nameof(DroneSpeed));
            Save(DroneSpeedPrice, nameof(DroneSpeedPrice));
            Save(TulipSpeed, nameof(TulipSpeed));
            Save(TulipSpeedPrice, nameof(TulipSpeedPrice));
            Save(KyuukonPerTulip, nameof(KyuukonPerTulip));
            Save(KyuukonPerPrice, nameof(KyuukonPerPrice));
            Save(UserName, nameof(UserName));
        }

        private void Save(int value, string key)
        {
            PlayerPrefs.SetInt(key, value);
        }
        private void Save(float value, string key)
        {
            PlayerPrefs.SetFloat(key, value);
        }
        private void Save(string value, string key)
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}