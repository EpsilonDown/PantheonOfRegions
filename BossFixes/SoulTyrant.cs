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
                GameObject folly = Instantiate(PantheonOfRegions.GameObjects["folly"],
                    new Vector3(Random.Range(3f, 36f), Random.Range(35f, 40f), 0f), Quaternion.identity);
                Destroy(folly.transform.GetChild(5).gameObject);
                Destroy(folly.transform.GetChild(4).gameObject);
                folly.LocateMyFSM("Control").ChangeTransition("Initiate","FINISHED","Startle");
                folly.SetActive(true);
            });
            _mage.AddCustomAction("Orb Summon", () =>
            {
                GameObject blob = Instantiate(PantheonOfRegions.GameObjects["blob"],
                    new Vector3(Random.Range(3f, 36f), 29f, 0f), Quaternion.identity);
                Destroy(blob.transform.GetChild(2).gameObject);
                blob.LocateMyFSM("Blob").ChangeTransition("Init", "SPAWNS", "Activate");
                blob.SetActive(true);
            });
        }
    }
}
