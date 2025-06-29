using HutongGames.PlayMaker.Actions;
using Satchel;

namespace PantheonOfRegions.Behaviours
{
    internal class HiveKnight : MonoBehaviour
    {
        private PlayMakerFSM _control;
        private GameObject _dropper;
        private GameObject _glob;
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");

            _dropper = Instantiate(PantheonOfRegions.GameObjects["beedropper"]);
            _dropper.SetActive(true);
            for (int i = 0; i < 7; i++)
            {
                _dropper.transform.transform.GetChild(i).gameObject.LocateMyFSM("Control").Fsm.GetFsmFloat("X Left").Value = 15f;
                _dropper.transform.transform.GetChild(i).gameObject.LocateMyFSM("Control").Fsm.GetFsmFloat("X Right").Value = 35f;
            }


            _glob = Instantiate(PantheonOfRegions.GameObjects["glob"]);
            _glob.SetActive(true);
            _glob.transform.SetPosition2D(transform.position);
            _glob.LocateMyFSM("Control");
            _glob.transform.transform.GetChild(0).gameObject.LocateMyFSM("Control").Fsm.GetFsmVector3("Tween Vector").Value = new Vector2(0f, 3f);
            _glob.transform.transform.GetChild(1).gameObject.LocateMyFSM("Control").Fsm.GetFsmVector3("Tween Vector").Value = new Vector2(0f, 5f);
            _glob.transform.transform.GetChild(2).gameObject.LocateMyFSM("Control").Fsm.GetFsmVector3("Tween Vector").Value = new Vector2(0f, 7f);
            
        }

        private void Start()
        {
            _control.Fsm.GetFsmGameObject("Droppers").Value = _dropper;
            _control.Fsm.GetFsmGameObject("Globs Container").Value = _glob;



            _control.Fsm.GetFsmFloat("Left X").Value = 15f;
            _control.Fsm.GetFsmFloat("Right X").Value = 35f;
            _control.Fsm.GetFsmFloat("Ground Y").Value = 29f;

            _control.ChangeTransition("Phase Check", "P1", "Phase 3");
            _control.ChangeTransition("Phase Check", "P2", "Phase 3");

        }
    }
}
