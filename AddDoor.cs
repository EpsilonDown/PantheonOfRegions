
using System.Reflection;

namespace PantheonOfRegions
{
    public partial class PantheonOfRegions
    {
		private static IEnumerator SetPantheon()
		{
			yield return new WaitWhile(() => !GameObject.Find("GG_Final_Challenge_Door"));
			GameObject newdoor = GameObject.Instantiate(GameObject.Find("GG_Final_Challenge_Door"));
			BossSequenceDoor sequencedoor = newdoor.GetComponent<BossSequenceDoor>();
			newdoor.transform.position = new Vector3(132f, 20f, 1.4f);
			BossSequence sequence = new BossSequence();
			List<BossScene> bossScenes = new List<BossScene>();

			foreach (string name in gs.PantheonRooms)
			{
				BossScene bossScene = ScriptableObject.CreateInstance<BossScene>();
				bossScene.sceneName = name;
				bossScene.isHidden = false;
				bossScene.requireUnlock = false;
				bossScenes.Add(bossScene);
				Modding.Logger.Log(name);
			}
			ReflectionHelper.SetField<BossSequence, BossScene[]>(sequence, "bossScenes", bossScenes.ToArray());
			sequence.achievementKey = "";
			sequence.customEndScene = "";
			sequence.customEndScenePlayerData = "";
			sequence.useSceneUnlocks = false;
			sequencedoor.bossSequence = sequence;
			sequencedoor.playerDataString = "CustomBossDoor";
			sequencedoor.descriptionKey = "CustomBossDoorDesc";
			sequencedoor.descriptionSheet = "CustomBossDoorDesc";
			sequencedoor.titleMainKey = "CustomBossDoorTitle";
			sequencedoor.titleMainSheet = "CustomBossDoorTitle";
			sequencedoor.titleSuperKey = "CustomBossDoorSuper";
			sequencedoor.titleSuperSheet = "CustomBossDoorSuper";
			sequencedoor.requiredComplete = new BossSequenceDoor[0];
			sequencedoor.completedDisplay.SetActive(true);
			sequencedoor.completedAllDisplay.SetActive(true);
			sequencedoor.lockSet.SetActive(false);
			sequencedoor.unlockedSet.SetActive(true);
			sequencedoor.doLockBreakSequence = false;
			sequencedoor.challengeFSM.FsmVariables.FindFsmString("To Scene").Value = bossScenes[0].sceneName;
			sequencedoor.completedDisplay.GetComponent<SpriteRenderer>().color = Color.black;//void
			sequencedoor.completedAllDisplay.GetComponent<SpriteRenderer>().color = Color.black;
			GameObject camlock = sequencedoor.transform.Find("CameraLockArea").gameObject;
            camlock.transform.position = new Vector3(132f, 40f, 1.4f);
            CameraLockArea cameralock = camlock.GetComponent<CameraLockArea>();
            cameralock.cameraYMax += -22f;
			cameralock.cameraYMin += -22f;//adjust camera position
			MethodInfo camlockstart = typeof(CameraLockArea).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic);
			cameralock.StartCoroutine((IEnumerator)camlockstart.Invoke(cameralock, new object[0]));
			yield break;
		}
	}
}
