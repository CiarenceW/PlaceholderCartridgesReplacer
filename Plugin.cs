using BepInEx;
using Receiver2ModdingKit;

namespace PlaceholderCartridgesReplacer
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("receiver2.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            ModdingKitCorePlugin.AddTaskAtCoreStartup(new ModdingKitCorePlugin.StartupAction(CartridgeModelReplacerManager.Main));
        }
    }
}
