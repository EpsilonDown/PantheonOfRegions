using HutongGames.PlayMaker.Actions;
using Osmi.Game;

namespace PantheonOfRegions.Behaviours
{
    internal class TheCollector : MonoBehaviour
    {
        private PlayMakerFSM _control;
        private int knightcount = 0;
        private int actknightcount = 0;
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");

        }

        private void Start()
        {
            
            _control.GetState("Init").RemoveAction(13);
            _control.GetState("Init").RemoveAction(12);
            _control.GetState("Init").RemoveAction(11);
            foreach (string s in new[] { "Roller", "Spitter", "Buzzer" })
            {
                _control.GetState(s).RemoveAction(0);
                _control.AddCustomAction(s, () =>
                {
                    KnightSpawner();
                });
            }

        }
        private void KnightSpawner()
        {
            if (knightcount < 6 && actknightcount < 2)
            {
                GameObject storedObject = _control.Fsm.GetFsmGameObject("Spawn Jar").Value;
                storedObject.AddComponent<SpawnJar>();
                knightcount++;
                actknightcount++;
            }
        }
        
        internal void KnightRemover()
        {
            actknightcount--;
        } 

    }
    internal class SpawnJar : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitUntil(() =>
            {
                return gameObject.transform.GetPositionY() < 97;
            });
            GameObject jarspawn = Instantiate(PantheonOfRegions.GameObjects["watcherknight"], new Vector3(50f, 98f, 0f), Quaternion.identity);
            jarspawn.transform.position = transform.position;
            jarspawn.SetActive(true);
            jarspawn.AddComponent<EnemyTracker>();
            jarspawn.GetComponent<HealthManager>().hp = 200;
            jarspawn.AddToShared(PantheonOfRegions.SharedBoss.GetComponent<SharedHealthManager>());
            jarspawn.GetComponent<HealthManager>().hp = 200;
            
        }
    }

}
