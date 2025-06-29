using Vasi;
using HutongGames.PlayMaker.Actions;
using Random = UnityEngine.Random;
namespace PantheonOfRegions.Behaviours
{
    internal class NoEyes : MonoBehaviour
    {
        private List<GameObject> _heads = new();

        private PlayMakerFSM _movement;
        private PlayMakerFSM _shotSpawn;
        private PlayMakerFSM _damaged;
        private void Awake()
        {
            _movement = gameObject.LocateMyFSM("Movement");
            _damaged = gameObject.LocateMyFSM("Damage Response");
            _shotSpawn = gameObject.LocateMyFSM("Shot Spawn");
            Destroy(gameObject.LocateMyFSM("Attacking"));
            var corpse = ReflectionHelper.GetField<EnemyDeathEffects, GameObject>(GetComponent<EnemyDeathEffectsNoEffect>(), "corpse");
            corpse.LocateMyFSM("Control").GetState("End").RemoveAction<CreateObject>();
        }

        private void Start()
        {
            FsmState broadcastDeathSet = gameObject.LocateMyFSM("Broadcast Ghost Death").GetState("Set");
            broadcastDeathSet.RemoveAction<SendEventByName>();
            broadcastDeathSet.AddMethod(() =>
            {
                foreach (GameObject noEyesHead in _heads)
                {
                    if (noEyesHead == null) continue;
                    FSMUtility.SendEventToGameObject(noEyesHead, "GHOST DEATH");
                }
            });

            for (int index = 1; index <= 8; index++)
            {
                _movement.Fsm.GetFsmVector3($"P{index}").Value = RandomVector3();
            }
            _movement.RemoveAction("Init",0);
            _shotSpawn.GetAction<RandomFloat>("Spawn L", 1).min = 105f;
            _shotSpawn.GetAction<RandomFloat>("Spawn L", 1).max = 135f;
            _shotSpawn.GetAction<SetPosition>("Spawn L", 2).x = 35f;
            _shotSpawn.GetAction<RandomFloat>("Spawn L", 5).min = 120f;
            _shotSpawn.GetAction<RandomFloat>("Spawn L", 5).max = 135f;
            _shotSpawn.GetAction<SetPosition>("Spawn L", 6).x = 70f;

            _shotSpawn.GetAction<RandomFloat>("Spawn R", 1).min = 105f;
            _shotSpawn.GetAction<RandomFloat>("Spawn R", 1).max = 120f;
            _shotSpawn.GetAction<SetPosition>("Spawn R", 2).x = 70f;
            _shotSpawn.GetAction<RandomFloat>("Spawn R", 5).min = 120f;
            _shotSpawn.GetAction<RandomFloat>("Spawn R", 5).max = 135f;
            _shotSpawn.GetAction<SetPosition>("Spawn R", 6).x = 35f;

            _shotSpawn.GetState("Spawn L").InsertMethod(1, () => _heads.Add(_shotSpawn.Fsm.GetFsmGameObject("Shot").Value));
            _shotSpawn.GetState("Spawn L").InsertMethod(6, () => _heads.Add(_shotSpawn.Fsm.GetFsmGameObject("Shot").Value));
            _shotSpawn.GetState("Spawn R").InsertMethod(1, () => _heads.Add(_shotSpawn.Fsm.GetFsmGameObject("Shot").Value));
            _shotSpawn.GetState("Spawn R").InsertMethod(6, () => _heads.Add(_shotSpawn.Fsm.GetFsmGameObject("Shot").Value));
            //_escal.ChangeTransition("Idle","TOOK DAMAGE","Escalate 2");

            _damaged.GetAction<SendRandomEvent>("Decide").weights = new FsmFloat[] { 0.8f, 0.2f };
        }
        
        private Vector3 RandomVector3()
        {
            float x = Random.Range(40f, 65f);
            float y = Random.Range(110f, 130f);
            float z = 0.006f;

            return new Vector3(x, y, z);
        }
    }
}
