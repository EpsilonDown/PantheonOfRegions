using HutongGames.PlayMaker.Actions;
using Vasi;

namespace PantheonOfRegions.Behaviours
{
    internal class Watcherknight : MonoBehaviour
    {
        private PlayMakerFSM _control;
        
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Black Knight");
        }

        private void Start()
        {
            
            _control.ChangeTransition("Init Facing","FINISHED","Bugs In");
            
        }

    }

}
