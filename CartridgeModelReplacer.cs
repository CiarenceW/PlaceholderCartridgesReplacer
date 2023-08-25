using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Receiver2;
using Receiver2ModdingKit;
using UnityEngine;

namespace PlaceholderCartridgesReplacer
{
    public class CartridgeModelReplacer : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private CartridgeSpec.Preset[] cartridge_type_serialized;

        [SerializeField, HideInInspector]
        private GameObject[] glint_model_serialized;

        [SerializeField, HideInInspector]
        private GameObject[] case_model_serialized;

        [SerializeField, HideInInspector]
        private GameObject[] case_empty_model_serialized;

        public CartridgeReplacement[] cartridgeReplacements;

        [SerializeField]
        private int cartridgesToReplace = 0;

        public bool stopBeingABitchFuckYou = false;

        public void OnBeforeSerialize()
        {
            cartridgesToReplace = cartridgeReplacements.Length;

            cartridge_type_serialized = new CartridgeSpec.Preset[cartridgesToReplace];
            glint_model_serialized = new GameObject[cartridgesToReplace];
            case_model_serialized = new GameObject[cartridgesToReplace];
            case_empty_model_serialized = new GameObject[cartridgesToReplace];

            for (int i = 0; i < cartridgesToReplace; i++)
            {
                if (cartridgeReplacements[i].cartridge_type != default) cartridge_type_serialized[i] = cartridgeReplacements[i].cartridge_type;

                if (cartridgeReplacements[i].glint_model != null) glint_model_serialized[i] = cartridgeReplacements[i].glint_model;

                if (cartridgeReplacements[i].case_model != null) case_model_serialized[i] = cartridgeReplacements[i].case_model;

                if (cartridgeReplacements[i].case_empty_model != null) case_empty_model_serialized[i] = cartridgeReplacements[i].case_empty_model;
            }
        }

        public void OnAfterDeserialize()
        {
            if (stopBeingABitchFuckYou)
            {
                cartridgesToReplace = cartridge_type_serialized.Length;

                if (cartridgesToReplace > 0)
                {
                    cartridgeReplacements = new CartridgeReplacement[cartridgesToReplace];
                    for (int i = 0; i < cartridgesToReplace; i++)
                    {
                        if (cartridge_type_serialized[i] != default) cartridgeReplacements[i].cartridge_type = cartridge_type_serialized[i];

                        if (glint_model_serialized[i] != null) cartridgeReplacements[i].glint_model = glint_model_serialized[i];

                        if (case_model_serialized[i] != null) cartridgeReplacements[i].case_model = case_model_serialized[i];

                        if (case_empty_model_serialized[i] != null) cartridgeReplacements[i].case_empty_model = case_empty_model_serialized[i];
                    }
                }
            }
        }
    }

    [Serializable]
    public struct CartridgeReplacement
    {
        public CartridgeSpec.Preset cartridge_type;
        public GameObject glint_model;
        public GameObject case_model;
        public GameObject case_empty_model;
    }
}
