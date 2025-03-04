﻿//using FacilityMeltdown.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BrutalCompanyMinus.Modules
{
    public class MeltdownMoonMapper : MonoBehaviour
    {
        public static MeltdownMoonMapper Instance { get; private set; }

        [field: SerializeField]
        [Tooltip("Lights to flash red during the meltdown sequence. Doesn't include inside lights.")]
        public List<Light> outsideEmergencyLights = new List<Light>();

        [field: SerializeField]
        [Tooltip("Colour to flash lights with, should probably just leave this at red. Only applies to outside lights.")]
        public Color outsideEmergencyLightColour { get; private set; } = Color.red;

        [field: SerializeField]
        public GameObject shockwavePrefab { get; private set; }
        [field: SerializeField]
        public GameObject explosionPrefab { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            if (Instance != null)
                Instance = null;
        }

#if DEBUG
     /*   private void Update()
        {
            if (Keyboard.current.rightBracketKey.wasPressedThisFrame)
            {
                MeltdownAPI.StartMeltdown("DEBUG KEY PRESSED");
            }
        }*/
#endif

        internal static void EnsureMeltdownMoonMapper()
        {
            if (GameObject.FindObjectOfType<MeltdownMoonMapper>() != null) return; // skipping as the moon has its own override

            Instance = new GameObject("DefaultMeltdownMappings").AddComponent<MeltdownMoonMapper>();
            Instance.outsideEmergencyLights = GameObject.Find("Environment").GetComponentsInChildren<Light>().Where((light) =>
            {
                return CheckParentForDisallowed(light.transform);
            }).ToList();
        }

        static bool CheckParentForDisallowed(Transform child)
        {
            if (child.gameObject.name == "Sun" || child.gameObject.name == "ItemShipAnimContainer" || child.gameObject.name == "MapPropsContainer") return false;
            if (child.parent == null) return true;
            return CheckParentForDisallowed(child.parent);
        }
    }
}