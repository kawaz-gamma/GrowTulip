using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save
{
    // �ۑ������f�[�^
    public struct SaveData
    {
        // ����������
        public int KyuukonCount;
        // �����̑����l����
        public int TotalKyuukonCount;
        // �y�n�̑傫��
        public float LandScale;
        // �y�n�̉��i
        public int LandPrice;
        // ���n�@�̐�
        public int SoujikiCount;
        // ���n�@�̉��i
        public int SoujikiPrice;
        // ������܂��@�̐�
        public int DroneCount;
        // ������܂��@�̉��i
        public int DronePrice;
        // ���n�@�̃X�s�[�h
        public float SoujikiSpeed;
        // ���n�@�X�s�[�h�A�b�v�̉��i
        public int SoujikiSpeedPrice;
        // ��܂��@�̃X�s�[�h
        public float DroneSpeed;
        // ��܂��@�̃X�s�[�h�A�b�v�̉��i
        public int DroneSpeedPrice;
        // �`���[���b�v�̃X�s�[�h
        public float TulipSpeed;
        // �`���[���b�v�̃X�s�[�h�A�b�v���i
        public int TulipSpeedPrice;
        // �`���[���b�v����Ƃ�鋅����
        public int KyuukonPerTulip;
        // �`���[���b�v����Ƃ�鋅�����A�b�v�̉��i
        public int KyuukonPerPrice;
        // �����L���O�ɓo�^����郆�[�U�[��
        public string UserName;
        // �o�ߎ���
        public float ElapsedTime;
        // �����l�����o�̗L��
        public int IsKyuukonUi;

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
                UserName = "Guest",
                ElapsedTime = 0.0f,
                IsKyuukonUi = 1
            };
        }

        public static SaveData Load()
        {
            // �Z�[�u�f�[�^�����ׂĂ��邩�m���߂�
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
                UserName = PlayerPrefs.GetString(nameof(UserName)),
                ElapsedTime = PlayerPrefs.GetFloat(nameof(ElapsedTime)),
                IsKyuukonUi = PlayerPrefs.GetInt(nameof(IsKyuukonUi))
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
            if (!PlayerPrefs.HasKey(nameof(ElapsedTime))) return false;
            if (!PlayerPrefs.HasKey(nameof(IsKyuukonUi))) return false;

            return true;
        }

        public void DeleteSaveData()
        {
            PlayerPrefs.DeleteKey(nameof(KyuukonCount));
            PlayerPrefs.DeleteKey(nameof(TotalKyuukonCount));
            PlayerPrefs.DeleteKey(nameof(LandScale));
            PlayerPrefs.DeleteKey(nameof(LandPrice));
            PlayerPrefs.DeleteKey(nameof(SoujikiCount));
            PlayerPrefs.DeleteKey(nameof(SoujikiPrice));
            PlayerPrefs.DeleteKey(nameof(DroneCount));
            PlayerPrefs.DeleteKey(nameof(DronePrice));
            PlayerPrefs.DeleteKey(nameof(SoujikiSpeed));
            PlayerPrefs.DeleteKey(nameof(SoujikiSpeedPrice));
            PlayerPrefs.DeleteKey(nameof(DroneSpeed));
            PlayerPrefs.DeleteKey(nameof(DroneSpeedPrice));
            PlayerPrefs.DeleteKey(nameof(TulipSpeed));
            PlayerPrefs.DeleteKey(nameof(TulipSpeedPrice));
            PlayerPrefs.DeleteKey(nameof(KyuukonPerTulip));
            PlayerPrefs.DeleteKey(nameof(KyuukonPerPrice));
            PlayerPrefs.DeleteKey(nameof(UserName));
            PlayerPrefs.DeleteKey(nameof(ElapsedTime));
            PlayerPrefs.DeleteKey(nameof(IsKyuukonUi));
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
            Save(ElapsedTime, nameof(ElapsedTime));
            Save(IsKyuukonUi, nameof(IsKyuukonUi));
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