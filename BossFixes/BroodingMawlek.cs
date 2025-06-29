using Vasi;
using HutongGames.PlayMaker.Actions;

namespace PantheonOfRegions.Behaviours
{
    internal class BroodingMawlek : MonoBehaviour
    {
        private PlayMakerFSM _control;
        
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Mawlek Control");
        }

        private void Start()
        {
            _control.Fsm.GetFsmBool("Skip Title").Value = true;
        }

    }
}
