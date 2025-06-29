using HutongGames.PlayMaker.Actions;
using Osmi.Game;
using Vasi;

namespace PantheonOfRegions.Actions
{
    internal class ReflectBeam : MonoBehaviour
    {
        private Vector3 Reflectpos;
        private Vector3 Reflectdir;
        private Vector3 Reflectangle;
        private PlayMakerFSM _control;
        private RaycastHit2D hit;
        private GameObject? refbeam;
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("Control");
        }
        private void Start()
        {

            _control.InsertCustomAction("Antic", () =>
            {

                Vector3 direction = new Vector3(
                    Mathf.Cos(gameObject.transform.eulerAngles.z * Mathf.Deg2Rad),
                    Mathf.Sin(gameObject.transform.eulerAngles.z * Mathf.Deg2Rad), 0).normalized;

                hit = Physics2D.Raycast(gameObject.transform.position, direction, 1000, LayerMask.GetMask("Enemies"));
                if (hit.collider != null)
                {
                    Reflectpos = hit.point;
                    Reflectdir = Vector3.Reflect(direction, hit.normal).normalized;
                    Reflectangle = new Vector3(0f, 0f, Mathf.Atan2(Reflectdir.x, Reflectdir.y) * Mathf.Rad2Deg + 90f);
                    refbeam = Instantiate(PantheonOfRegions.RadianceObjects["Eye Beam"], Reflectpos, Quaternion.Euler(Reflectangle));
                    refbeam.RemoveComponent<ReflectBeam>();
                    refbeam.RemoveComponent<ReflectedBeam>();
                    refbeam.SetActive(true);
                    refbeam.LocateMyFSM("Control").SendEvent("ANTIC");
                }

            }, 2);


            _control.InsertCustomAction("Fire", () =>
            {
                if (refbeam != null)
                {
                    refbeam.LocateMyFSM("Control").SendEvent("FIRE");
                }
            }, 2);

            _control.AddCustomAction("End", () =>
            {
                Destroy(refbeam);
            });
        }
    }
    internal class ReflectedBeam : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}