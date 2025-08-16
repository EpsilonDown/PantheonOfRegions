namespace PantheonOfRegions.Behaviours
{
    public class Oro : MonoBehaviour
    {
        private GameObject _sheo;

        private PlayMakerFSM _oroControl;
        private void Awake()
        {
            _oroControl = gameObject.LocateMyFSM("nailmaster");
        }

        private IEnumerator Start()
        {
            yield return null;
            while (HeroController.instance == null) yield return null;

            _sheo = PantheonOfRegions.InstaBoss["sheo"];

            _oroControl.Fsm.GetFsmInt("P2 HP").Value = 99999;
            _oroControl.InsertCustomAction("Look 2", () => { _sheo.LocateMyFSM("nailmaster_sheo").SetState("Look"); }, 0);

            Vector3 paintingPos = new Vector3(47.8f, 6.4f, 2f);
            GameObject painting = Instantiate(PantheonOfRegions.GameObjects["painting"], paintingPos, Quaternion.identity);
            painting.SetActive(true);
            
        }


    }
}
