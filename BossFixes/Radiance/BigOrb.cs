using HutongGames.PlayMaker.Actions;
using PantheonOfRegions.Behaviours;
using Vasi;

namespace PantheonOfRegions.Actions
{
    internal class BigOrb : MonoBehaviour
    {
        private float _growRate = 1f;
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
            Destroy(gameObject.GetComponentInChildren<CircleCollider2D>());

            PantheonOfRegions.AudioClips["Scream"].PlayOneShot(transform.position);


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

            yield return new WaitForSeconds(0.1f);

            Explode();
        }
        internal void Explode()
        {
            gameObject.LocateMyFSM("Orb Control").SetState("Dissipate");
            PantheonOfRegions.InstaBoss["seer"].LocateMyFSM("Movement").SetState("Choose Target");
            PantheonOfRegions.AudioClips["Explode"].PlayOneShot(transform.position);
            Destroy(gameObject);
            if (killer != null)
            {
                Destroy(killer);
            }
        }

    }
    internal static class Extensions
    {
        public static void PlayOneShot(this AudioClip clip, Vector3 location)
        {
            IEnumerator PlayAndRecycle()
            {
                GameObject audioPlayer = PantheonOfRegions.RadianceObjects["Audio Player"].Spawn(location);
                var audioSource = audioPlayer.GetComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(clip.length);
                audioSource.clip = null;
            }
            GameManager.instance.StartCoroutine(PlayAndRecycle());
        }
    }
}