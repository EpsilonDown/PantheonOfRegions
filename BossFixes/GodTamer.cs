using Vasi;
using HutongGames.PlayMaker.Actions;
using Osmi.Game;

namespace PantheonOfRegions.Behaviours
{
    internal class Lobster: MonoBehaviour
    {

        private GameObject healthsharer;
        //private int sharedhp;
        private bool end = false;
        
        private void Start()
        {
            healthsharer = PantheonOfRegions.InstaBoss["colosseum"];
        }
        private void Update()
        {
            if (healthsharer.GetComponent<SharedHealthManager>().HP < 600 && end == false)
            {
                Destroy(PantheonOfRegions.InstaBoss["oblobble"]);
                PantheonOfRegions.InstaBoss["rageblobble"].SetActive(true);
                PantheonOfRegions.InstaBoss["rageblobble"].AddComponent<Rageblobble>();
                end = true;
            }

        }

    }
}
