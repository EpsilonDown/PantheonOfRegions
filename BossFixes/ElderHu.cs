using Vasi;
using HutongGames.PlayMaker.Actions;
using Random = UnityEngine.Random;
using System.Security.Policy;

namespace PantheonOfRegions.Behaviours
{
    internal class ElderHu : MonoBehaviour
    {
        private GameObject _ringHolder;

        private PlayMakerFSM _attacking;
        private PlayMakerFSM _movement;

        private void Awake()
        {
            _attacking = gameObject.LocateMyFSM("Attacking");
            _movement = gameObject.LocateMyFSM("Movement");

            _ringHolder = Instantiate(PantheonOfRegions.GameObjects["ringholder"], new Vector2(30.0f, 10.0f), Quaternion.identity);
            _ringHolder.SetActive(true);
            _ringHolder.name = _ringHolder.name.Replace("(Clone)", "");

            var corpse = ReflectionHelper.GetField<EnemyDeathEffects, GameObject>(GetComponent<EnemyDeathEffectsNoEffect>(), "corpse");
            corpse.LocateMyFSM("Control").GetState("End").RemoveAction<CreateObject>();
            corpse.LocateMyFSM("Control").GetState("End").AddMethod(() => Destroy(corpse));

        }
        private void Start()
        {
            
            _attacking.SetState(_attacking.Fsm.StartState);

            _attacking.GetAction<FloatCompare>("Choose Pos", 1).float2 = 30f - 6f;
            _attacking.GetAction<FloatCompare>("Choose Pos", 2).float2 = 30f + 6f;
            _attacking.GetAction<SetPosition>("Set L").x = 17 + 2;
            _attacking.GetAction<SetPosition>("Set L").y = transform.position.y;
            _attacking.GetAction<SetPosition>("Set R").x = 42 - 2;
            _attacking.GetAction<SetPosition>("Set R").y = transform.position.y;

            _attacking.GetAction<SetPosition>("Mega Warp Out").x = HeroController.instance.transform.position.x;


            for (int index = 1; index <= 7; index++)
            {
                _movement.Fsm.GetFsmVector3($"P{index}").Value = RandomVector3();
            }
            
            _movement.GetAction<FloatCompare>("Choose L").float2 = 30 - 10;
            _movement.GetAction<FloatCompare>("Choose R").float2 = 30 + 10;
            _movement.GetAction<FloatCompare>("Set Warp").float2 = 30;
            _movement.GetAction<SetVector3XYZ>("Choose L").x = 30 - 5;
            _movement.GetAction<SetVector3XYZ>("Choose L").y = transform.position.y;
            _movement.GetAction<SetVector3XYZ>("Choose R").x = 30 + 5;
            _movement.GetAction<SetVector3XYZ>("Choose R").y = transform.position.y;
            _movement.GetAction<SetPosition>("Return").x = 30;
            _movement.GetAction<SetPosition>("Return").y = 10;

            foreach (Transform ringTransform in _ringHolder.transform)
            {
                ringTransform.position = new Vector2(ringTransform.position.x, 18f);
                PlayMakerFSM ringCtrl = ringTransform.GetComponent<PlayMakerFSM>();
                ringCtrl.GetAction<FloatCompare>("Down").float2 = 8f;
                FsmState checkPos = ringCtrl.GetState("Check Pos");

                checkPos.GetAction<FloatCompare>(1).lessThan = new FsmEvent("RESET");
                checkPos.GetAction<FloatCompare>(2).greaterThan = new FsmEvent("RESET");
                checkPos.RemoveTransition("CANCEL");
                checkPos.AddTransition("RESET", "Reset");
                ringCtrl.GetState("Reset").RemoveAction<SetPosition>();
                ringCtrl.GetState("Antic").InsertMethod(0, () => ringTransform.position = new Vector2(ringTransform.position.x, 18f));
                ringCtrl.SetState(ringCtrl.Fsm.StartState);
            } 
            
        }

        private Vector3 RandomVector3()
        {
            float x = Random.Range(17, 42);
            float y = Random.Range(12, 18);
            float z = 0.006f;

            return new Vector3(x, y, z);
        }
    }
}
