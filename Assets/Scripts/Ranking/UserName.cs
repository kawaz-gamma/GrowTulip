using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ranking
{
    public class UserName : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_InputField inputField;

        public string Name { private set; get; } = "Guest";

        private void Update()
        {
            Name = inputField.text;
            if(Name == "")
            {
                Name = "Guest";
            }
        }
    }
}