using HutongGames.PlayMaker.Actions;
using Vasi;
using Random = UnityEngine.Random;
namespace PantheonOfRegions.Behaviours
{
    internal class Markoth : MonoBehaviour
    {
        private List<GameObject> _nails = new();

        private PlayMakerFSM _movement;
        private PlayMakerFSM _shield;
        private PlayMakerFSM _nail;
        private PlayMakerFSM _attack;
        private GameObject markothNail;
        private int phase = 0;
        private void Awake()
        {
            _movement = gameObject.LocateMyFSM("Movement");
            _shield = gameObject.LocateMyFSM("Shield Attack");
            _attack = gameObject.LocateMyFSM("Attacking");

            var corpse = ReflectionHelper.GetField<EnemyDeathEffects, GameObject>(GetComponent<EnemyDeathEffectsNoEffect>(), "corpse");
            corpse.LocateMyFSM("Control").GetState("End").RemoveAction<CreateObject>();
        }

        private void Start()
        {
            //remove shield on death
            FsmState broadcastDeathSet = gameObject.LocateMyFSM("Broadcast Ghost Death").GetState("Set");
            broadcastDeathSet.RemoveAction<SendEventByName>();
            broadcastDeathSet.AddMethod(() => {
                foreach (GameObject markothNail in _nails)
                {
                    FSMUtility.SendEventToGameObject(markothNail, "GHOST DEATH");
                }

                GameObject markothShield = GameObject.Find("Markoth Shield(Clone)");
                FSMUtility.SendEventToGameObject(markothShield.transform.Find("Shield").gameObject, "GHOST DEATH");
                FSMUtility.SendEventToGameObject(markothShield.transform.Find("Shield 2").gameObject, "GHOST DEATH");
            });

            for (int index = 1; index <= 8; index++)
            {
                _movement.Fsm.GetFsmVector3($"P{index}").Value = RandomVector3(22f, 30f);
            }

            var personalObjectPool = GetComponent<PersonalObjectPool>();
            personalObjectPool.DestroyPooled();
            Destroy(personalObjectPool);

            markothNail = personalObjectPool.startupPool[0].prefab;

            markothNail.SetActive(false);

            FsmState nailState = _attack.GetState("Nail");
            nailState.RemoveAction<SpawnObjectFromGlobalPool>();
            _shield.RemoveAction("Init", 2);
            _shield.RemoveAction("Init", 1);

            _shield.SetState("Ready");
            _shield.GetState("Send Attack").RemoveAction(0);
            _shield.GetState("Idle").RemoveAction(1);
            _shield.GetState("Idle").RemoveTransition("FINISHED");
            FsmState nailFan = _attack.AddState("Nail Fan");
            FsmState reflector = _attack.AddState("Reflector");
            FsmState stun = _attack.AddState("Stun");

            reflector.AddTransition("ATTACK END", "Wait");
            nailFan.AddTransition("SHIELD END", "Wait");
            stun.AddTransition("STUN END", "Wait");
            _attack.Fsm.GetFsmBool("Rage").Value = true;

            nailFan.AddCustomAction(() => {
                for (int i = 0; i < 12; i++)
                {
                    int angle = 30 * i; //+ offset;
                    var nail = Instantiate(markothNail,
                        gameObject.transform.position + new Vector3(3 * Mathf.Cos(angle * Mathf.Deg2Rad), 3 * Mathf.Sin(angle * Mathf.Deg2Rad), 0f),
                        Quaternion.Euler(new Vector3(0, 0, angle)));
                    DontDestroyOnLoad(nail);
                    nail.AddComponent<NoAimMarkothNail>();
                    nail.SetActive(true);

                    _nails.Add(nail);
                }
            });
        }
        internal void Phase2()
        {
            phase++;
            for (int index = 1; index <= 8; index++)
            {
                _movement.Fsm.GetFsmVector3($"P{index}").Value = RandomVector3(35f, 55f);
            }
        }
        internal void Phase3()
        {
            phase++;
            for (int index = 1; index <= 8; index++)
            {
                _movement.Fsm.GetFsmVector3($"P{index}").Value = new Vector3(60f, 160f, 0f);
                _movement.RemoveAction("Hover", 0);
            }
        }
        internal void NailCombTop()
        {
            for (int index = 0; index < 32; index += 4)
            {
                GameObject cnail = Instantiate(markothNail, new Vector3(48f + Random.Range(index, index + 3f), 32f + phase * 25f, 0f), Quaternion.Euler(new Vector3(0, 0, -90f)));
                cnail.SetActive(true);
                cnail.AddComponent<MarkothNailcomb>();
            }
        }
        internal void NailComb()
        {
            int type = Random.Range(0, 1);

            for (int index = 0; index < 12; index += 4)
            {
                GameObject cnail = Instantiate(markothNail, new Vector3(48f, 20f + Random.Range(index, index + 3f), 0f), Quaternion.Euler(new Vector3(0, 0, 180 * type)));
                cnail.SetActive(true);
                cnail.AddComponent<MarkothNailcomb>();
            }
        }

        internal GameObject GetNail()
        {
            return markothNail;
        }
        internal void StartNailSpam()
        {
            StartCoroutine(NailSpam());
        }
        internal void StopNailSpam()
        {
            StopAllCoroutines();
        }
        private IEnumerator NailSpam()
        {
            while (true)
            {
                GameObject nail = Instantiate(markothNail, new Vector3(50f, 30f, 0f), Quaternion.identity);
                nail.AddComponent<MarkothNail>();
                nail.SetActive(true);
                yield return new WaitForSeconds(0.3f);
            }
        }
        private Vector3 RandomVector3(float ymin, float ymax)
        {
            float x = Random.Range(50f, 72f);
            float y = Random.Range(ymin, ymax);
            float z = 0.006f;

            return new Vector3(x, y, z);
        }
    }


    internal class MarkothNail : MonoBehaviour
    {
        private void Start()
        {
            gameObject.GetComponent<DamageHero>().damageDealt = 2;
            PlayMakerFSM nailCtrl = gameObject.LocateMyFSM("Control");

            nailCtrl.ChangeTransition("Init","FINISHED","Check Distance");
            FsmState checkDistance = nailCtrl.GetState("Check Distance");
            checkDistance.InsertCustomAction(() => {
                float heroy = nailCtrl.Fsm.GetFsmFloat("Hero Y").Value;
                nailCtrl.Fsm.GetFsmVector3("Tele Pos").Value
                = new Vector3(Random.Range(50f, 72f), Random.Range(heroy+3f, heroy + 8f), 0f);
            }, 0);
            checkDistance.GetAction<FloatCompare>(2).float2 = 4f;
            checkDistance.RemoveAction(3);
            
            nailCtrl.GetState("Recycle").InsertMethod(0, () => Destroy(gameObject));
            nailCtrl.SetState(nailCtrl.Fsm.StartState);
            nailCtrl.enabled = true;
        }
    }
    internal class NoAimMarkothNail : MonoBehaviour
    {
        private void Start()
        {
            PlayMakerFSM nailCtrl = gameObject.LocateMyFSM("Control");
            nailCtrl.ChangeTransition("Init","FINISHED","Antic");
            nailCtrl.GetState("Antic").GetAction<Wait>(1).time = 1.5f;
            nailCtrl.GetState("Recycle").InsertMethod(0, () => Destroy(gameObject));
            nailCtrl.SetState(nailCtrl.Fsm.StartState);
            nailCtrl.enabled = true;
        }
    }
    internal class MarkothNailcomb : MonoBehaviour
    {

        private void Start()
        {
            PlayMakerFSM nailCtrl = gameObject.LocateMyFSM("Control");
            nailCtrl.ChangeTransition("Init", "FINISHED", "Antic");
            nailCtrl.GetState("Antic").GetAction<Wait>(1).time = 0.5f;
            nailCtrl.GetState("Fire").GetAction<SetVelocityAsAngle>().speed = 20f;
            nailCtrl.GetState("Recycle").InsertMethod(0, () => Destroy(gameObject));
            nailCtrl.SetState(nailCtrl.Fsm.StartState);
            nailCtrl.enabled = true;
        }
    }
}
