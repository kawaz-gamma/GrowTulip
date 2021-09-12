using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save
{
    public class SaveDeleter : MonoBehaviour
    {
        public void Delete()
        {
            var saveData = SaveData.CreateZeroValue();
            saveData.DeleteSaveData();
            GameManager.instance.InitializeGameManager();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}