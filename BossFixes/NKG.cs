using HutongGames.PlayMaker.Actions;
using Satchel;
using Osmi.Game;
using Vasi;

namespace PantheonOfRegions.Behaviours
{
    internal class NightmareKingGrimm : MonoBehaviour
    {
        private PlayMakerFSM _control;
        private PlayMakerFSM zote_control;
        private GameObject _Zote;
        private int ragecount = 0;
        
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");
        }

        private void Start()
        {

            _Zote = PantheonOfRegions.InstaBoss["greyprincezote"];
            zote_control = _Zote.LocateMyFSM("Control");


            _control.InsertCustomAction("Tele Out",() => {
                _Zote.GetComponent<GreyPrinceZote>().LeapEnabler();
            }, 0);
            _control.InsertCustomAction("Spike Return", () => {
                _Zote.GetComponent<GreyPrinceZote>().LeapBlocker();
            }, 0);

            _control.GetState("Explode").AddMethod(() => zote_control.SendEvent("STUN"));
            _control.GetState("Set Balloon HP").RemoveAction(0);
            _control.GetState("Balloon?").RemoveAction(0);
            _control.GetState("Adjust HP").RemoveAction(4);

            FsmState wait = _control.AddState("Move Wait");
            _control.ChangeTransition("Balloon?","FINISHED","Move Wait");
            wait.AddTransition("MOVE", "Move Choice");

            #region balloon
            _control.InsertCustomAction("Set Balloon HP", () => {
                _control.Fsm.GetFsmInt("HP").Value = PantheonOfRegions.SharedBoss.GetComponent<SharedHealthManager>().HP;
            }, 0);
            _control.InsertCustomAction("Balloon?", () => {
                _control.Fsm.GetFsmInt("HP").Value = PantheonOfRegions.SharedBoss.GetComponent<SharedHealthManager>().HP;
            }, 0);

            _control.InsertCustomAction("Balloon Pos", () => {
                zote_control.SetState("Longfall");
            }, 0);

            _control.InsertCustomAction("Deflate", () => {
                zote_control.SetState("FT Fall");
            }, 0);
            #endregion

            
            _control.AddCustomAction("Adjust HP", () => {
                PantheonOfRegions.SharedBoss.GetComponent<SharedHealthManager>().HP -= _control.Fsm.GetFsmInt("Bat Damage").Value;
            });

            _control.SetState("Init");
        }

        
    }

}