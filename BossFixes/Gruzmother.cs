using HutongGames.PlayMaker.Actions;
using Osmi.Game;

namespace PantheonOfRegions.Behaviours
{
    internal class Gruzmother : MonoBehaviour
    {
        private PlayMakerFSM _control;
        private PlayMakerFSM _bounce;
        private GameObject healthsharer;
        //private int sharedhp;
        private bool end = false;
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Big Fly Control");
            _bounce = gameObject.LocateMyFSM("bouncer_control");
            
        }

        private void Start()
        {
            healthsharer = PantheonOfRegions.SharedBoss;

            _control.GetAction<Wait>("GG Extra Pause", 0).time = 5f;
            _control.AddState("Pause");
            _control.AddTransition("Pause","FINISHED","Super Choose");
            _control.AddCustomAction("Pause",() => {_bounce.SendEvent("STOP");});
            _control.AddAction("Pause", new Wait()
            {
                time = new(4f),
                finishEvent = FsmEvent.GetFsmEvent("FINISHED")
            });
            _control.AddState("Stun");

        }
        private void Update()
        {
            if (healthsharer.GetComponent<SharedHealthManager>().HP < 400 && end == false)
            {
                _control.SetState("Stun");
                PantheonOfRegions.InstaBoss["sly"].LocateMyFSM("Control").SendEvent("ZERO HP");
                Destroy(gameObject);
            }

        }
    }
}
