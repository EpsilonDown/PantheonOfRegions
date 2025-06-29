using HutongGames.PlayMaker.Actions;
using Vasi;

namespace PantheonOfRegions.Actions
{
    internal class SweepBeam : MonoBehaviour
    {
        private PlayMakerFSM _control;
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");
        }
        private void Start()
        {
            _control.GetState("Beam Sweep L").RemoveAction<SetPosition>();
            _control.GetState("Beam Sweep R").RemoveAction<SetPosition>();
            _control.GetState("Beam Sweep L").RemoveAction<BoolTest>();
            _control.GetState("Beam Sweep R").RemoveAction<BoolTest>();
            _control.AddCustomAction("Beam Sweep L", () =>
            {
                gameObject.transform.position = new Vector3(GetMarkothPos(), 16f, 0f);
            });
            _control.AddCustomAction("Beam Sweep R", () =>
            {
                gameObject.transform.position = new Vector3(GetMarkothPos(), 16f, 0f);
            });

        }
        private float GetMarkothPos()
        {
            return PantheonOfRegions.InstaBoss["markoth"].transform.GetPositionX();
        }
    }
}