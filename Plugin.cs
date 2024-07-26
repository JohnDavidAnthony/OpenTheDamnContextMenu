using BepInEx;
using BepInEx.Logging;

namespace OpenTheDamnContextMenu
{
    //first string below is your plugin's GUID, it MUST be unique to any other mod. Read more about it in BepInEx docs. Be sure to update it if you copy this code.
    [BepInPlugin("JD.OpenTheDamnContextMenu", "Open The Damn Context Menu", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;

        private void Awake()
        {
            LogSource = Logger;
            LogSource.LogInfo("OpenTheDamnContextMenu plugin loaded!");

            new OpenTheDamnContextMenuPatch().Enable();
        }
    }
}
