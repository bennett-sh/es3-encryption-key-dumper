using BepInEx;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ES3EncryptionKeyDumper
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        void Awake()
        {
            var firstpass = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(dll => dll.GetName().Name == "Assembly-CSharp-firstpass");

            if (firstpass == null)
            {
                Logger.LogFatal("Couldn't detect ES3: Assembly-CSharp-firstpass.dll not found");
                return;
            }

            Type settings = firstpass.GetType("ES3Settings");

            if (settings == null)
            {
                Logger.LogFatal("Couldn't detect ES3: ES3Settings class not found");
                return;
            }

            PropertyInfo currentSettings = settings.GetProperty("defaultSettings");

            if (currentSettings == null)
            {
                Logger.LogFatal("Couldn't detect ES3: defaultSettings property not found");
                return;
            }

            object settingsInstance = currentSettings.GetValue(null, null);
            FieldInfo encryptionKey = settingsInstance.GetType().GetField("encryptionPassword");

            if (encryptionKey == null)
            {
                Logger.LogFatal("Couldn't detect ES3: encryptionPassword field not found");
                return;
            }

            Logger.LogInfo("Found ES3 encryption key: " + encryptionKey.GetValue(settingsInstance));
        }
    }
}
