using Osmi;

namespace PantheonOfRegions
{
    public sealed partial class PantheonOfRegions: Mod, ITogglableMod
    {
		private List<string> scenes = new List<string> { "GG_Hollow_Knight", "GG_Radiance", "GG_Grimm_Nightmare" };
		public static PoRSequence gs = new PoRSequence();
		public bool isCustom = false;
        public PantheonOfRegions() =>
			OsmiHooks.SceneChangeHook += EditScene;
        public override string GetVersion() => "v0.1";
	
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += AddDoor;
            ModHooks.GetPlayerVariableHook += ChangeCustomDoorVar;
            ModHooks.LanguageGetHook += ChangeText;
            On.BossSequenceController.SetupNewSequence += BossSequenceController_SetupNewSequence;
            On.BossSceneController.Start += CheckHUD;

            Dictionary<string, GameObject> gameObjects = new();
            foreach (KeyValuePair<string, GameObject> pair in GameObjects)
            {
                string goName = pair.Key;
                if (_preloadDictionary.Keys.Contains(goName))
                {
                    (string sceneName, string enemyPath) = _preloadDictionary[goName];
                    GameObject gameObject = preloadedObjects[sceneName][enemyPath];
                    gameObjects.Add(goName, gameObject);
                }
            }

            foreach (KeyValuePair<string, GameObject> pair in gameObjects)
                GameObjects[pair.Key] = pair.Value;
            
        }


        private IEnumerator CheckHUD(On.BossSceneController.orig_Start orig, BossSceneController self)
        {
			//from HUDInChecker
			yield return orig(self);
			if(BossSequenceController.IsInSequence&&!scenes.Contains(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
            {
				yield return new WaitUntil(() => GameManager.instance.gameState == GameState.PLAYING);
				GameCameras.instance.hudCanvas.LocateMyFSM("Slide Out").SendEvent("IN");
			}
        }

        private void BossSequenceController_SetupNewSequence(On.BossSequenceController.orig_SetupNewSequence orig, BossSequence sequence, BossSequenceController.ChallengeBindings bindings, string playerData)
        {
            orig(sequence, bindings, playerData);
			if (sequence.achievementKey == "")
			{
				isCustom = true;
			}
			else
			{
				isCustom = false;
			}
		}


        private object ChangeCustomDoorVar(Type type, string name, object value)
        {
            if(name== "CustomBossDoor")
            {
				return new BossSequenceDoor.Completion
				{
					viewedBossSceneCompletions=gs.PantheonRooms
				};

            }

			return value;
        }
        public string ChangeText(string key, string sheetTitle, string orig) => key switch
        {
            "CustomBossDoorSuper" => "Pantheon of",
            "CustomBossDoorTitle" => "Regions",
            "CustomBossDoorDesc" => "Fight Gods Attuned through the Regions",
	        "VENGEFLY_SUPER" => "Cliff",
            "VENGEFLY_MAIN" => "Howlers",
	        "MEGA_MOSS_SUPER" => "Green",
            "MEGA_MOSS_MAIN" => "Ambushers",
            "FALSE_KNIGHT_DREAM_MAIN" => "Crossroads",
            "FALSE_KNIGHT_DREAM_SUB" => "Dominators",
            "SISTERS_MAIN" => "Alliance",
            "SISTERS_SUB" => "of Battle",
            "ENRAGED_GUARDIAN_SUPER" => "Enlightened",
            "ENRAGED_GUARDIAN_MAIN" => "Fanatics",
            "MAGE_LORD_DREAM_SUPER" => "Soul",
            "MAGE_LORD_DREAM_MAIN" => "Overlords",
            "TRAITOR_LORD_MAIN" => "Queen's",
            "TRAITOR_LORD_SUB" => "Tributes",
            "TEMP_NM_SUPER" => "Family",
            "TEMP_NM_MAIN" => "Nailmasters",
            "MEGA_JELLY_MAIN" => "Blind",
            "MEGA_JELLY_SUB" => "Protectors",
            "MIMIC_SPIDER_MAIN" => "Deepnest",
            "MIMIC_SPIDER_SUB" => "Stalkers",
            "WHITE_DEFENDER_MAIN" => "Waterways",
            "WHITE_DEFENDER_SUB" => "Guardians",
            "HORNET_MAIN" => "Stinger Knights",
            "LOBSTER_LANCER_C_SUPER" => "Champions of",
            "LOBSTER_LANCER_C_MAIN" => "Colosseum",
	        "BIGFLY_SUPER" => "Lord of",
            "BIGFLY_MAIN" => "Flies",
            "BIGFLY_SUB" => "",
            "NIGHTMARE_GRIMM_SUPER" => "Nightmare King",
            "NIGHTMARE_GRIMM_MAIN" => "ZOTE",
            "HK_PRIME_MAIN" => "Void Vessels",
            "ABSOLUTE_RADIANCE_MAIN" => "RADIANCE",
            "ABSOLUTE_RADIANCE_SUPER" => "Mother Of Moths",

            _ => orig

        };


        private void AddDoor(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
			bool flag = arg1.name == "GG_Atrium_Roof";
			if (flag)
			{
				GameManager.instance.StartCoroutine(SetPantheon());
			}
			if(arg0.name== "GG_End_Sequence")
            {
				HeroController.instance.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
			Log("Set Custom Door");
		}
       
    public void Unload()
        {
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= AddDoor;
			ModHooks.GetPlayerVariableHook -= ChangeCustomDoorVar;
            ModHooks.LanguageGetHook -= ChangeText;
            On.BossSequenceController.SetupNewSequence -= BossSequenceController_SetupNewSequence;
			On.BossSceneController.Start -= CheckHUD;
			Log("Unloaded");
        }
	}
}
