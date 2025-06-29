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
        private int sharedhp;
        private int ragecount = 0;
        
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");
        }

        private IEnumerator Start()
        {
            yield return null;

            _Zote = PantheonOfRegions.InstaBoss["greyprincezote"];
            zote_control = _Zote.LocateMyFSM("Control");

            _control.InsertCustomAction("Tele Out",() => {
                _Zote.GetComponent<GreyPrinceZote>().LeapEnabler();
            }, 0);
            _control.InsertCustomAction("Spike Return", () => {
                _Zote.GetComponent<GreyPrinceZote>().LeapBlocker();
            }, 0);
            _control.InsertCustomAction("Slash Pos", () => {
                _Zote.GetComponent<GreyPrinceZote>().LeapBlocker();
            }, 0);

            _control.GetState("Explode").AddMethod(() => _Zote.LocateMyFSM("Control").SendEvent("STUN"));
            _control.GetState("Set Balloon HP").RemoveAction(0);
            _control.GetState("Balloon?").RemoveAction(0);
            _control.GetState("Adjust HP").RemoveAction(4);

            _control.InsertCustomAction("Balloon?", () => {
                sharedhp = PantheonOfRegions.InstaBoss["reapers"].GetComponent<SharedHealthManager>().HP;
                if (sharedhp < 1600 && ragecount == 0)
                {
                    zote_control.SetState("Longfall");
                    _control.SendEvent("BALLOON 1");
                }
                else if (sharedhp < 1000 && ragecount == 1)
                {
                    zote_control.SetState("Longfall");
                    _control.SendEvent("BALLOON 1");
                }
                else if (sharedhp < 500 && ragecount == 2)
                {
                    zote_control.SetState("Longfall");
                    _control.SendEvent("BALLOON 1");
                }
                else
                {
                    _control.SetState("Move Choice");
                }
            }, 0);

            _control.InsertCustomAction("Deflate", () => {
                zote_control.SetState("FT Fall");
                ragecount++;
            }, 0);
            /* 
            _control.InsertCustomAction("Adjust HP", () => {
                PantheonOfRegions.InstaBoss["reapers"].GetComponent<SharedHealthManager>().HP -= _control.Fsm.GetFsmInt("Bat Damage").Value;
            }, 0); */
        }

        
    }

}