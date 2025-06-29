using HutongGames.PlayMaker.Actions;
using Osmi.Game;

namespace PantheonOfRegions.Behaviours
{
    internal class TheCollector : MonoBehaviour
    {
        private PlayMakerFSM _control;
        private static List<GameObject> summoned = new List<GameObject>();

        private int knightcount = 0;
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");

        }

        private void Start()
        {
            
            _control.GetState("Init").RemoveAction(13);
            _control.GetState("Init").RemoveAction(12);
            _control.GetState("Init").RemoveAction(11);
            _control.GetState("Buzzer").RemoveAction(0);
            _control.GetState("Roller").RemoveAction(0);
            _control.GetState("Spitter").RemoveAction(0);

            foreach (string s in new[] { "Roller", "Spitter", "Buzzer" })
            {
                _control.AddCustomAction(s, () =>
                {
                    if (knightcount < 6)
                    {
                        GameObject storedObject = _control.Fsm.GetFsmGameObject("Spawn Jar").Value;
                        storedObject.AddComponent<SpawnJar>();
                        knightcount++;
                    }
                });
            }



            //On.HealthManager.Die += CollectorDeath;

        }
        /*
        private void CollectorDeath(On.HealthManager.orig_Die orig, HealthManager self, float? attackDirection, AttackTypes attackType, bool ignoreEvasion)
        {
            foreach (GameObject boss in summoned)
            {
                GameObject.Destroy(boss);
            }
        } */
        internal class SpawnJar : MonoBehaviour
        {
            private IEnumerator Start()
            {
                yield return new WaitUntil(() =>
                {
                    return gameObject.transform.GetPositionY() < 98;
                });
                GameObject jarspawn = Instantiate(PantheonOfRegions.GameObjects["watcherknight"], new Vector3(50f, 98f, 0f), Quaternion.identity);
                //jarspawn.tag = "Boss";
                jarspawn.transform.position = transform.position;
                jarspawn.AddComponent<EnemyTracker>();
                jarspawn.GetComponent<HealthManager>().hp = 200;
                jarspawn.AddToShared(PantheonOfRegions.InstaBoss["collector"].GetComponent<SharedHealthManager>());
                jarspawn.GetComponent<HealthManager>().hp = 200;
                jarspawn.SetActive(true);
            }
        }
    }

}
