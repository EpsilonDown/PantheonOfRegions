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
            _spider.GetState("Roof Jump?").RemoveAction(1);

            _spider.GetState("Roof Drop").InsertMethod(0, () => _hm.IsInvincible = true);
            _spider.GetState("Falling").InsertMethod(0, () => _hm.IsInvincible = false);
        }
    }
}