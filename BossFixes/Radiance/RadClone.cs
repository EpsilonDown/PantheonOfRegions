using HutongGames.PlayMaker.Actions;
using Osmi.Game;
using PantheonOfRegions.Behaviours;
using Vasi;

namespace PantheonOfRegions.Actions
{
    internal class RadClone : MonoBehaviour
    {

        // 11, 37 bench cord
        private bool truerad = false;
        private tk2dSprite? seerSprite = null;
        private PlayMakerFSM damaged;
        private PlayMakerFSM radattack;
        private tk2dSprite? clonesprite = null;
        private static readonly Lazy<Texture2D> seerTex = new(() => AssemblyUtils.GetTextureFromResources("Seer.png"));
        private Spritebuilder spritebuilder = new Spritebuilder();
        private void Awake()
        {
            Destroy(gameObject.LocateMyFSM("Movement"));
            gameObject.GetComponent<DamageHero>().damageDealt = 0;
            Destroy(gameObject.GetComponent<AudioSource>());
            Destroy(gameObject.GetComponent<Recoil>());
            Destroy(gameObject.LocateMyFSM("Shot Spawn"));
            Destroy(gameObject.LocateMyFSM("Escalation"));
            damaged = gameObject.LocateMyFSM("Damage Response");
            clonesprite = gameObject.GetComponent<tk2dSprite>();
        }
        private void Start()
        {
            damaged.GetAction<SendRandomEvent>("Decide").weights = new FsmFloat[] { 0f, 1f };
            damaged.InsertCustomAction("Decide", () =>
            {
                if (truerad == false)
                {
                    //GameObject beam = Instantiate(PantheonOfRegions.RadianceObjects["Beam Burst"], gameObject.transform);
                    //beam.AddComponent<CloneBeam>();
                    //beam.SetActive(true);
                    StartCoroutine(NailSpammer());
                }
            },0);
            damaged.RemoveAction("Send",0);
            damaged.AddCustomAction("Send", () =>
            {
                PantheonOfRegions.RadianceObjects["Radiance"].GetComponent<AbsoluteRadiance>().Shuffling();
            });
            spritebuilder.ApplyTextureToTk2dSprite(gameObject, seerTex.Value);

        }
        internal void TrueRad()
        {
            truerad = true;
            On.HealthManager.TakeDamage += HealthManagerTakeDamage;
            StartCoroutine(TrueRadglow());
        }
        internal IEnumerator TrueRadglow()
        {
            clonesprite.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
            clonesprite.color = Color.white;
        }

        internal void FakeRad()
        {
            truerad = false;
            On.HealthManager.TakeDamage -= HealthManagerTakeDamage;
        }

        private void HealthManagerTakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hit)
        {
            PantheonOfRegions.InstaBoss["moths"].GetComponent<SharedHealthManager>().HP -= hit.DamageDealt;
            
            orig(self, hit);
        }
        internal IEnumerator NailSpammer()
        {
            GameObject markoth = PantheonOfRegions.InstaBoss["markoth"];
            markoth.GetComponent<Markoth>().StartNailSpam();
            yield return new WaitForSeconds(1f);
            markoth.GetComponent<Markoth>().StopNailSpam();
        }
    }

    /*
    internal class CloneBeam : MonoBehaviour
    {
        private IEnumerator Start()
        {
            foreach (Transform beam in gameObject.transform)
            {
                if (beam.gameObject.GetComponent<ReflectBeam>() != null) { Destroy(beam.gameObject.GetComponent<ReflectBeam>()); };
                beam.gameObject.LocateMyFSM("Control").SendEvent("ANTIC");
            }
            yield return new WaitForSeconds(0.425f);
            foreach (Transform beam in gameObject.transform)
            {
                beam.gameObject.LocateMyFSM("Control").SendEvent("FIRE");
            }
            yield return new WaitForSeconds(0.3f);
            foreach (Transform beam in gameObject.transform)
            {
                beam.gameObject.LocateMyFSM("Control").SendEvent("END");
            }
            yield return new WaitForSeconds(0.3f);
            Destroy(gameObject);
        }
    } */
}