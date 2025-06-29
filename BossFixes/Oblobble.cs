using Vasi;
using HutongGames.PlayMaker.Actions;
using Osmi.Game;
using Satchel;

namespace PantheonOfRegions.Behaviours
{
    internal class Rageblobble: MonoBehaviour
    {
        private PlayMakerFSM _control;
        private PlayMakerFSM _rager;
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("fat fly bounce");
            _rager = gameObject.LocateMyFSM("Set Rage");
        }
        private void Start()
        {
            _rager.RemoveAction("Set", 6);
            _rager.RemoveAction("Set", 5);
            _control.AddCustomAction("Aim", () =>
            {
                _rager.SendEvent("OBLOBBLE RAGE");
            });
        }

    }
}
