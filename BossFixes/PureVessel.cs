using HutongGames.PlayMaker.Actions;
using Vasi;

namespace PantheonOfRegions.Behaviours
{
    internal class PureVessel : MonoBehaviour
    {

        private PlayMakerFSM _control;

        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");
        }

        private void Start()
        {
            _control.ChangeTransition("Phase?", "PHASE1", "Choice P3");
            _control.ChangeTransition("Phase?", "PHASE2", "Choice P3");
            gameObject.transform.Find("Corpse HK Prime(Clone)").gameObject.AddComponent<PVDeath>();
        }

    }
    internal class PVDeath : MonoBehaviour
    {
        private void Start()
        {
            PlayMakerFSM control = PantheonOfRegions.InstaBoss["lostkin"].LocateMyFSM("IK Control");
            control.SendEvent("STUN");
            control.GetState("Stunned").GetAction<Wait>().time = 100f;
            control.GetState("Stunned").RemoveAction<ActivateGameObject>();
            control.GetState("Stunned").RemoveTransition("TOOK DAMAGE");
        }
    }
}