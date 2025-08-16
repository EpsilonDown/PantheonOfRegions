using Vasi;
using HutongGames.PlayMaker.Actions;
using Osmi.Game;
using GlobalEnums;

namespace PantheonOfRegions.Behaviours
{
    internal class Lobster: MonoBehaviour
    {

        private GameObject healthsharer;
        private bool end = false;
        
        private void Start()
        {
            healthsharer = PantheonOfRegions.SharedBoss;
        }
        private void Update()
        {
            if (healthsharer.GetComponent<SharedHealthManager>().HP < 600 && end == false)
            {
                PantheonOfRegions.InstaBoss["oblobble"].GetComponent<HealthManager>().StopSharing(10);
                PantheonOfRegions.InstaBoss["oblobble"].GetComponent<HealthManager>().Die(1f, AttackTypes.Generic, true);
                PantheonOfRegions.InstaBoss["rageblobble"].SetActive(true);
                PantheonOfRegions.InstaBoss["rageblobble"].AddComponent<Rageblobble>();
                end = true;
            }

        }

    }
}
