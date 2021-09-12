using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save
{
    public class SaveDeleter : MonoBehaviour
    {
        public void Delete()
        {
            SaveData.DeleteSaveData();
            GameManager.instance.InitializeGameManager();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}