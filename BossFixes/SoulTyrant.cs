using HutongGames.PlayMaker.Actions;
using Random = UnityEngine.Random;
namespace PantheonOfRegions.Behaviours
{
    internal class SoulTyrant2 : MonoBehaviour
    {
        private PlayMakerFSM _mage;

        private void Awake()
        {
            _mage = gameObject.LocateMyFSM("Mage Lord 2");
        }

        private void Start()
        {
            _mage.AddCustomAction("Quake Antic", () =>
            {
                Vector3 SpawnPos;
                while (true)
                {
                    SpawnPos = new Vector3(Random.Range(3f, 36f), Random.Range(35f, 40f), 0f);
                    if (Vector2.Distance(HeroController.instance.transform.position, SpawnPos) > 8f) break;
                }
                GameObject folly = Instantiate(PantheonOfRegions.GameObjects["folly"], SpawnPos, Quaternion.identity);
                Destroy(folly.transform.GetChild(5).gameObject);
                Destroy(folly.transform.GetChild(4).gameObject);
                folly.LocateMyFSM("Control").ChangeTransition("Initiate","FINISHED","Startle");
                folly.SetActive(true);
            });
            _mage.AddCustomAction("Orb Summon", () =>
            {
                Vector3 SpawnPos;
                while (true)
                {
                    SpawnPos = new Vector3(Random.Range(3f, 36f), 29f, 0f);
                    if (Vector2.Distance(HeroController.instance.transform.position, SpawnPos) > 6f) break;
                }
                GameObject blob = Instantiate(PantheonOfRegions.GameObjects["blob"], SpawnPos, Quaternion.identity);
                Destroy(blob.transform.GetChild(2).gameObject);
                blob.LocateMyFSM("Blob").ChangeTransition("Init", "SPAWNS", "Activate");
                blob.SetActive(true);
            });
        }
    }
}
