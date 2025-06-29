using HutongGames.PlayMaker.Actions;
using Vasi;

namespace PantheonOfRegions.Behaviours
{
    internal class Nosk : MonoBehaviour
    {
        private HealthManager _hm;
        private PlayMakerFSM _constrainX;
        private PlayMakerFSM _spider;

        private void Awake()
        {
            _hm = GetComponent<HealthManager>();
            _constrainX = gameObject.LocateMyFSM("constrain_x");
            _spider = gameObject.LocateMyFSM("Mimic Spider");
        }

        private void Start()
        {
            _constrainX.Fsm.GetFsmFloat("Edge L").Value = 35;
            _constrainX.Fsm.GetFsmFloat("Edge R").Value = 70;

            _spider.Fsm.GetFsmFloat("Jump Max X").Value = 70 - 6;
            _spider.Fsm.GetFsmFloat("Jump Min X").Value = 35 + 6;
            _spider.Fsm.GetFsmFloat("Roof Y").Value = 20 - 2;

            _spider.GetState("Trans 1").RemoveAction<ApplyMusicCue>();
            _spider.GetState("Trans 1").RemoveAction<CreateObject>();
            _spider.GetState("Trans 1").RemoveAction<SetFsmGameObject>();
            _spider.GetState("Trans 1").RemoveAction(6);
            _spider.GetState("Roof Jump?").RemoveAction(1);

            _spider.GetState("Roof Drop").InsertMethod(0, () => _hm.IsInvincible = true);
            _spider.GetState("Falling").InsertMethod(0, () => _hm.IsInvincible = false);



        }
    }
}