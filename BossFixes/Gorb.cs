using Vasi;
using HutongGames.PlayMaker.Actions;
using Osmi.Game;

namespace PantheonOfRegions.Behaviours
{
    internal class Gorb : MonoBehaviour
    {
        private PlayMakerFSM _movement;
        private PlayMakerFSM _attack;
        private void Awake()
        {
            _movement = gameObject.LocateMyFSM("Movement");
            _attack = gameObject.LocateMyFSM("Attacking");

            Destroy(gameObject.LocateMyFSM("FSM"));
            Destroy(gameObject.LocateMyFSM("Distance Attack"));
            var corpse = ReflectionHelper.GetField<EnemyDeathEffects, GameObject>(GetComponent<EnemyDeathEffectsNoEffect>(), "corpse");
            corpse.LocateMyFSM("Control").GetState("End").RemoveAction<CreateObject>();
        }

        private void Start()
        {
            _attack.RemoveAction("Init 2", 1);
            _attack.Fsm.GetFsmInt("HP").Value = 1200;
            _attack.RemoveAction("Double?", 0);
            _attack.InsertCustomAction("Double?", () => { 
               _attack.Fsm.GetFsmInt("HP").Value = PantheonOfRegions.SharedBoss.GetComponent<SharedHealthManager>().HP;
            }, 0);
            _attack.RemoveAction("Triple?", 0);
            _attack.InsertCustomAction("Triple?", () => {
                _attack.Fsm.GetFsmInt("HP").Value = PantheonOfRegions.SharedBoss.GetComponent<SharedHealthManager>().HP;
            }, 0);
            _attack.ChangeTransition("Wait", "FINISHED", "Antic");
            Destroy(gameObject.LocateMyFSM("Broadcast Ghost Death"));
            _movement.SetState(_movement.Fsm.StartState);


            _movement.Fsm.GetFsmVector3("P1").Value = new Vector3(32f, 18f, 0f);
            _movement.Fsm.GetFsmVector3("P2").Value = new Vector3(38f, 20f, 0f);
            _movement.Fsm.GetFsmVector3("P3").Value = new Vector3(44f, 22f, 0f);
            _movement.Fsm.GetFsmVector3("P4").Value = new Vector3(48f, 24f, 0f);
            _movement.Fsm.GetFsmVector3("P5").Value = new Vector3(52f, 22f, 0f);
            _movement.Fsm.GetFsmVector3("P6").Value = new Vector3(56f, 20f, 0f);
            _movement.Fsm.GetFsmVector3("P7").Value = new Vector3(62f, 18f, 0f);

            _movement.GetAction<FloatCompare>("Hover", 4).float2 = 30f;
            _movement.GetAction<FloatCompare>("Hover", 5).float2 = 65f;
            _movement.GetAction<FloatCompare>("Hover", 6).float2 = 15f;
            _movement.GetAction<FaceObject>("Hover").objectB = HeroController.instance.gameObject;


            _movement.GetAction<FloatTestToBool>("Set Warp", 2).float2 = 48f;
            _movement.GetAction<FloatTestToBool>("Set Warp", 3).float2 = 48f;

            _movement.GetAction<SetPosition>("Return").x = 48f;
            _movement.GetAction<SetPosition>("Return").y = 18f;
        }

    }
}
