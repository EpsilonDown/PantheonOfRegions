using Vasi;
using HutongGames.PlayMaker.Actions;
using Random = UnityEngine.Random;

namespace PantheonOfRegions.Behaviours
{
    internal class Galien : MonoBehaviour
    {
        private GameObject _hammer;
        private PlayMakerFSM _movement;
        private void Awake()
        {
            _movement = gameObject.LocateMyFSM("Movement");
            _hammer = Instantiate(PantheonOfRegions.GameObjects["hammer"]);
            _hammer.transform.SetPosition2D(transform.position);
            _hammer.LocateMyFSM("Attack").Fsm.GetFsmGameObject("Ghost Warrior Galien").Value = gameObject;
            _hammer.AddComponent<GalienHammer>();
            _hammer.SetActive(true);

            var corpse = ReflectionHelper.GetField<EnemyDeathEffects, GameObject>(GetComponent<EnemyDeathEffectsNoEffect>(), "corpse");
            corpse.LocateMyFSM("Control").GetState("End").RemoveAction<CreateObject>();

            GetComponent<HealthManager>().OnDeath += OnDeath;
        }

        private void Start()
        {
            for (int index = 1; index <= 7; index++)
            {
                _movement.Fsm.GetFsmVector3($"P{index}").Value = RandomVector3();
            }

            FsmState broadcastDeathSet = gameObject.LocateMyFSM("Broadcast Ghost Death").GetState("Set");
            broadcastDeathSet.RemoveAction<SendEventByName>();
            broadcastDeathSet.AddMethod(() => {
                foreach (GameObject miniHammer in FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains("Galien Mini Hammer")))
                {
                    FSMUtility.SendEventToGameObject(miniHammer, "GHOST DEATH");
                }
                FSMUtility.SendEventToGameObject(_hammer, "GHOST DEATH");
            });
        }
        private Vector3 RandomVector3()
        {
            float x = Random.Range(90f, 120f);
            float y = Random.Range(10f, 15f);
            float z = 0.006f;

            return new Vector3(x, y, z);
        }
        private void OnDeath()
        {
            Destroy(_hammer);
        }
    }

    internal class GalienHammer : MonoBehaviour
    {
        private PlayMakerFSM _attack;
        private PlayMakerFSM _control;

        private void Awake()
        {
            _attack = gameObject.LocateMyFSM("Attack");
            _control = gameObject.LocateMyFSM("Control");
        }

        private IEnumerator Start()
        {
            _attack.Fsm.GetFsmFloat("Floor Y").Value = 5f;
            _attack.Fsm.GetFsmFloat("Slam Y").Value = 5.4f;
            _attack.Fsm.GetFsmFloat("Wall L X").Value = 71f;
            _attack.Fsm.GetFsmFloat("Wall R X").Value = 117f;

            _control.GetAction<SetVector3XYZ>("Init").x = transform.position.x;
            _attack.GetAction<ChaseObjectV2>("Chase").accelerationForce = 10f;

            _attack.SetState(_attack.Fsm.StartState);
            _control.SetState(_control.Fsm.StartState);
            _control.SendEvent("READY");

            yield return new WaitUntil(() => _control.ActiveStateName == "Emerge");

            _control.SendEvent("READY");
        }
    }
}
