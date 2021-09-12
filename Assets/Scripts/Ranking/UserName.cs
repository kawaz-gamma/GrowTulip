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
            if(inputField.text.Length > 6)
            {
                inputField.text = inputField.text.Substring(0, 6);
            }
            Name = inputField.text;
            if(Name == "")
            {
                Name = "Guest";
            }
        }
    }
}