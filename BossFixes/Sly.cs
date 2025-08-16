using HutongGames.PlayMaker.Actions;
using Osmi.Game;
using Satchel;
using Vasi;

namespace PantheonOfRegions.Behaviours
{
    internal class GreatNailsageSly : MonoBehaviour
    {
        private PlayMakerFSM _control;
        private GameObject stunnail;
        private GameObject deathnail;
        private GameObject wallspotl;
        private GameObject wallspotr;
        
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");
            stunnail = Instantiate(PantheonOfRegions.GameObjects["stunnail"], new Vector2(90f, 15f), Quaternion.identity);
            deathnail = Instantiate(PantheonOfRegions.GameObjects["deathnail"], new Vector2(90f, 15f), Quaternion.identity);
            wallspotl = Instantiate(PantheonOfRegions.GameObjects["wallspotl"], new Vector2(86f, 15f), Quaternion.identity);
            wallspotr = Instantiate(PantheonOfRegions.GameObjects["wallspotr"], new Vector2(102f, 15f), Quaternion.identity);
        }

        private void Start()
        {

            _control.RemoveAction("Init", 36);
            _control.RemoveAction("Init", 35);
            _control.RemoveAction("Init", 34);
            _control.RemoveAction("Init", 33);
            _control.RemoveAction("Init", 32);
            _control.RemoveAction("Init", 31);

            _control.Fsm.GetFsmGameObject("Stun Nail").Value = stunnail;
            _control.Fsm.GetFsmGameObject("Death Nail").Value = deathnail;
            _control.Fsm.GetFsmGameObject("Wallspot L").Value = wallspotl;
            _control.Fsm.GetFsmGameObject("Wallspot R").Value = wallspotr;

            _control.AddCustomAction("Stun Wait", () => { _control.SendEvent("READY"); });
            _control.AddCustomAction("Grabbing", () => { _control.SendEvent("GRABBED"); });
            _control.GetAction<FloatCompare>("Cyc Down").float2.Value = 19f;
            _control.GetAction<FloatOperator>("Cyc Jump Launch").float1.Value = 20f;
            _control.GetAction<SetFloatValue>("Jump To L", 0).floatValue.Value = 98f;
            _control.GetAction<SetFloatValue>("Jump To L", 1).floatValue.Value = 86f;
            _control.GetAction<SetFloatValue>("Jump To R", 0).floatValue.Value = 90f;
            _control.GetAction<SetFloatValue>("Jump To R", 1).floatValue.Value = 102f;
            _control.Fsm.GetFsmFloat("Topslash Y").Value = 24f;

            _control.GetAction<SetPosition>("Air Catch", 6).x = 94f;
            _control.GetAction<SetPosition>("Air Catch", 6).y = 20f;
            _control.GetAction<SetPosition>("Bounce L", 5).x = 86.5f;
            _control.GetAction<SetPosition>("Bounce R", 5).x = 101.5f;

            _control.GetAction<SetPosition>("Bounce D", 6).y = 15.5f;
            _control.GetAction<SetPosition>("Bounce U", 5).y = 23.5f;
            

            _control.RemoveAction("Begin Rage", 5);
            _control.RemoveAction("Acended HP", 1);


        }



    }
}
