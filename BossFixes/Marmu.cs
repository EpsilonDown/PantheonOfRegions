using Vasi;
using HutongGames.PlayMaker.Actions;

namespace PantheonOfRegions.Behaviours
{
    internal class Marmu : MonoBehaviour
    {
        private PlayMakerFSM _control;

        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");

            ReflectionHelper.GetField<EnemyDeathEffects, GameObject>(GetComponent<EnemyDeathEffectsNoEffect>(), "corpse").LocateMyFSM("Control").GetState("End").RemoveAction<CreateObject>();
        }

        private void Start()
        {
            _control.Fsm.GetFsmFloat("Tele X Max").Value = 56f;
            _control.Fsm.GetFsmFloat("Tele X Min").Value = 23f;
            _control.Fsm.GetFsmFloat("Tele Y Max").Value = 37f;
            _control.Fsm.GetFsmFloat("Tele Y Min").Value = 33f;
        }
    }
}
