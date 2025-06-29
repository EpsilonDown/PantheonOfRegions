using static Modding.IMenuMod;

namespace PantheonOfRegions
{
    public sealed partial class PantheonOfRegions : IMenuMod
    {
        bool IMenuMod.ToggleButtonInsideMenu => true;

        List<MenuEntry> IMenuMod.GetMenuData(MenuEntry? toggleButtonEntry) => new() {
        toggleButtonEntry!.Value,
        new(
            "Modify Hall of Gods",
            new string[] {
                Lang.Get("MOH_OFF", "MainMenu"),
                Lang.Get("MOH_ON", "MainMenu")
            },
            "",
            i => GlobalSettings.modifyhall = i != 0,
            () => GlobalSettings.modifyhall ? 1 : 0
        ),
        new(
            "Coordinated Mode (WIP)",
            new string[] {
                Lang.Get("MOH_OFF", "MainMenu"),
                Lang.Get("MOH_ON", "MainMenu")
            },
            "",
            i => GlobalSettings.cordmode = i != 0,
            () => GlobalSettings.cordmode ? 1 : 0
        )
    };
    }
    public sealed partial class PantheonOfRegions : IGlobalSettings<GlobalSettings>
    {
        public static GlobalSettings GlobalSettings { get; private set; } = new();
        public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;
        public GlobalSettings OnSaveGlobal() => GlobalSettings;
    }

    public sealed class GlobalSettings
    {
        public bool modifyhall = true;
        public bool cordmode = false;
    }
}