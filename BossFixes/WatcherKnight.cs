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

            _control.ChangeTransition("Init Facing", "FINISHED", "Bugs In");
            _control.RemoveAction("Title?",3);
            gameObject.transform.Find("Corpse Black Knight 1(Clone)").gameObject.AddComponent<WatcherDeath>();
        }

    } 
    internal class WatcherDeath : MonoBehaviour
    {
        private void Awake()
        {
            PantheonOfRegions.InstaBoss["collector"].GetComponent<TheCollector>().KnightRemover();
        }
    } 
}