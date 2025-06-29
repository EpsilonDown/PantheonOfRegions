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

            _oroControl.Fsm.GetFsmInt("P2 HP").Value = 99999;

            Vector3 sheoPos = new Vector3(45.0f, 6.9f, 2f);
            Quaternion rotation = Quaternion.identity;
            _sheo = Instantiate(PantheonOfRegions.GameObjects["sheo"], sheoPos, rotation);
            _sheo.AddComponent<Sheo>();
            _sheo.SetActive(true);
            _oroControl.InsertCustomAction("Look 2", () => { _sheo.LocateMyFSM("nailmaster_sheo").SetState("Look"); }, 0);
            _oroControl
                    .InsertCustomAction("Reactivate", () =>
                    {
                        _sheo!.SetActive(true);
                        new[] { "Brothers/Oro", "Brothers/Mato" }
                        .Map(s => GameObject.Find(s)).Append(_sheo)
                        .ShareHealth(name: "nailmasters").HP = 2000;
                    }, 0);

            Vector3 paintingPos = new Vector3(47.8f, 6.4f, 2f);
            GameObject painting = Instantiate(PantheonOfRegions.GameObjects["painting"], paintingPos, rotation);
            painting.SetActive(true);
            
        }


    }
}
