using HutongGames.PlayMaker.Actions;
using Osmi.Game;
using Vasi;

namespace PantheonOfRegions.Behaviours
{
    internal class FailedChampion : MonoBehaviour
    {
        private PlayMakerFSM _control;
        private PlayMakerFSM _hpcheck;
        private GameObject Mawlek;
        private BossSpawner Spawner = new BossSpawner();
        private bool end = false;
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("FalseyControl");
            _hpcheck = gameObject.LocateMyFSM("Check Health");
            
        }

        private void Start()
        {
            _control.GetState("Check GG").ChangeTransition("FINISHED","Recover");
            

            _hpcheck.GetState("Check").RemoveAction(1);
            _hpcheck.GetState("Check").RemoveAction(0);
            
            _control.GetState("Rubble End").AddAction(_control.GetAction("State 1",2));
            _control.GetState("Rubble End").AddAction(new Wait()
            {
                time = new(2.5f),
                finishEvent = FsmEvent.GetFsmEvent("FINISHED")
            });

        }
        private void Update()
        {
            int sharedhp = PantheonOfRegions.SharedBoss.GetComponent<SharedHealthManager>().HP;
            if (sharedhp < 1110 && _hpcheck.Fsm.GetFsmBool("Stun 1").Value == false)
            {
                _hpcheck.Fsm.GetFsmBool("Stun 1").Value = true;
                _hpcheck.SendEvent("STUN");

            }
            else if (sharedhp < 510 && _hpcheck.Fsm.GetFsmBool("Stun 2").Value == false)
            {
                _hpcheck.Fsm.GetFsmBool("Stun 2").Value = true;
                _hpcheck.SendEvent("STUN");

            }
            else if (sharedhp < 10 && end == false)
            {
                _hpcheck.SendEvent("STUN");
                gameObject.GetComponent<HealthManager>().StopSharing(10);

                end = true;
            }
        }
    }
}