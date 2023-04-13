using System;
using Newtonsoft.Json;
using TMI.Helper;
using UnityEngine;

namespace TMI.ConfigManagement.Unity.Serializer {
    
    [CreateAssetMenu(fileName = "SerializerConfig", menuName = "TMI/Config/Module/Serializer", order = 0)]
    public class SerializerConfig : BaseConfig {

        [SerializeField]
        private bool shouldIntendFormatting = false;

        protected override void OnInitialize_Runtime() {
            SerializationHelper.currentFormatting = shouldIntendFormatting ? Formatting.Indented : Formatting.None;
        }
    }
    
}

