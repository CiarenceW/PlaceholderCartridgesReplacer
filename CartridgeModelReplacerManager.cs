using Receiver2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlaceholderCartridgesReplacer
{
    internal class CartridgeModelReplacerManager
    {
        private static CartridgeModelReplacer modelReplacer;

        public static void Main()
        {
            LoadCartridgeModels();
            ReplaceCartridgeModels();
        }

        public static void LoadCartridgeModels()
        {
            string current_directory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            Debug.Log(current_directory);
            if (Directory.Exists(current_directory)) //don't need to check but whatevs
            {
                var files_in_directory = Directory.GetFiles(current_directory);
                for (int i = 0; i < files_in_directory.Length; i++)
                {
                    string fileName = files_in_directory[i];
                    if (fileName.Contains(SystemInfo.operatingSystemFamily.ToString().ToLower()))
                    {
                        AssetBundle assetBundle = AssetBundle.GetAllLoadedAssetBundles().FirstOrDefault((AssetBundle bundle) => bundle.name.Contains(Path.GetFileNameWithoutExtension(fileName)));
                        if (assetBundle == null)
                        {
                            assetBundle = AssetBundle.LoadFromFile(fileName);
                            if (assetBundle == null)
                            {
                                Debug.LogWarning("Failed to load AssetBundle " + fileName);
                                return;
                            }
                        }
                        foreach (string asset_name in assetBundle.GetAllAssetNames())
                        {
                            GameObject gameObject = assetBundle.LoadAsset<GameObject>(asset_name);
                            if (gameObject != null)
                            {
                                if (gameObject.TryGetComponent<CartridgeModelReplacer>(out modelReplacer))
                                {
                                    Debug.Log("Found cartridge replacer bundle");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (modelReplacer == null)
                {
                    Debug.LogError("mdoel replacer is null!!!! SHITTTTTT!!!!!!!!!!!");
                }
            }
            else Debug.LogError("direrctort DOESNT FUCKING EDXIST");
        }

        private static void ReplaceCartridgeModels()
        {


            var glint_material = ReceiverCoreScript.Instance().GetMagazinePrefab(Constants.Gun.Glock17, MagazineClass.StandardCapacity).glint_renderer.material;
            for (int i = 0; i < modelReplacer.cartridgeReplacements.Length; i++)
            {
                var cartridge = modelReplacer.cartridgeReplacements[i];
                var round = (from e in ReceiverCoreScript.Instance().generic_prefabs where e.TryGetComponent<ShellCasingScript>(out var shellCasingScript) && shellCasingScript.cartridge_type == cartridge.cartridge_type select e.GetComponent<ShellCasingScript>()).FirstOrDefault();
                cartridge.case_model.transform.parent = (cartridge.case_empty_model.transform.parent = (cartridge.glint_model.transform.parent = round.transform));
                round.go_casing.gameObject.SetActive(false);
                round.go_casing = cartridge.case_empty_model.GetComponent<MeshRenderer>();
                round.go_round.gameObject.SetActive(false);
                round.go_round = cartridge.case_model.GetComponent<MeshRenderer>();
                round.glint_renderer = cartridge.glint_model.GetComponent<MeshRenderer>();
                round.glint_renderer.material = glint_material;

                Debug.LogFormat("Replaced {0} models", round.InternalName);
            }
        }
    }
}
