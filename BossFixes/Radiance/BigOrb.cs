using HutongGames.PlayMaker.Actions;
using PantheonOfRegions.Behaviours;
using Vasi;

namespace PantheonOfRegions.Actions
{
    internal class BigOrb : MonoBehaviour
    {
        private float _growRate = 0.6f;
        private float _killScale = 10f;
        private GameObject killer;

        private IEnumerator Start()
        {
            PantheonOfRegions.InstaBoss["bigorb"] = gameObject;
            PantheonOfRegions.RadianceObjects["Radiance"].GetComponent<AbsoluteRadiance>().OrbRespawn();
            PantheonOfRegions.RadianceObjects["Shot Charge"].transform.position = transform.position;
            PantheonOfRegions.RadianceObjects["Shot Charge"].GetComponent<ParticleSystem>().Play();
            PantheonOfRegions.InstaBoss["seer"].LocateMyFSM("Movement").SetState("Wait");
            Destroy(gameObject.transform.Find("Hero Hurter").gameObject);

            //PantheonOfRegions.AudioClips["Ghost"].PlayOneShot(transform.position);

            Destroy(gameObject.GetComponentInChildren<CircleCollider2D>());

            

            yield return new WaitUntil(() =>
            {
                gameObject.transform.localScale += Vector3.one * _growRate * Time.deltaTime;
                return gameObject.transform.localScale.x >= _killScale;
            });

            yield return new WaitForSeconds(0.1f);

            killer = new GameObject("Killer");
            killer.layer = (int)PhysLayers.ENEMIES;
            var damager = killer.AddComponent<DamageHero>();
            damager.damageDealt = 4;
            damager.hazardType = (int)HazardType.SPIKES;
            var collider = killer.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 100;
            killer.transform.SetPosition2D(HeroController.instance.transform.position);
            Modding.Logger.Log("Orb Exploded!");

            yield return new WaitForSeconds(0.1f);

            Explode();
        }
        internal void Explode()
        {
            gameObject.LocateMyFSM("Orb Control").SetState("Dissipate");
            PantheonOfRegions.InstaBoss["seer"].LocateMyFSM("Movement").SetState("Choose Target");
            Destroy(gameObject);
            if (killer != null)
            {
                Destroy(killer);
            }
        }
        
        
    }
}