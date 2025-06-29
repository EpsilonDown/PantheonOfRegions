using Vasi;
using HutongGames.PlayMaker.Actions;


namespace PantheonOfRegions.Behaviours
{
    class Sibling : MonoBehaviour
    {
        private PlayMakerFSM _control;

        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");
        }

        private void Start()
        {
            
            //_control.SetState("Pause");

            //yield return new WaitUntil(() => _control.ActiveStateName == "Idle");


            GetComponent<DamageHero>().damageDealt = 2;
            GetComponent<HealthManager>().hp = 20;

            _control.GetAction<IntCompare>("Friendly?").integer2 = 10;
            transform.Find("Alert Range").gameObject.transform.localScale *= 20;
            _control.RemoveAction("Pause", 0);


            _control.RemoveAction("Init", 7);
            _control.RemoveAction("Idle", 7);
            _control.RemoveAction("Idle", 6);
            _control.RemoveAction("Idle", 5);
            _control.AddCustomAction("Idle", () => { _control.SendEvent("ALERT"); });


            _control.ChangeTransition("Startle", "FRIENDLY", "Chase");
            _control.RemoveAction("Chase", 7);
            _control.RemoveAction("Chase", 6);
            _control.RemoveAction("Chase", 5);
            _control.RemoveAction("Chase", 4);
            _control.RemoveTransition("Chase", "UNALERT");

            
            Modding.Logger.Log("sibling  Edited full");
        }
    }
}