using Vasi;
using HutongGames.PlayMaker.Actions;
using Satchel;

namespace PantheonOfRegions.Behaviours
{
    internal class Flukemarm : MonoBehaviour
    {
        private PlayMakerFSM _mother;
        
        private void Awake()
        {
            _mother = gameObject.LocateMyFSM("Fluke Mother");
        }

        private void Start()
        {

            GameObject hatcherCage = Instantiate(PantheonOfRegions.GameObjects["hatchercage"], transform.position + new Vector3(0f,10f,0f), Quaternion.identity);
            hatcherCage.SetActive(true);
            foreach (var collider in hatcherCage.GetComponents<BoxCollider2D>())
            {
                Destroy(collider);
            }
            _mother.Fsm.GetFsmGameObject("Cage").Value = hatcherCage;
            //GameObject.Destroy(GameObject.Find("Fluke Fly"));
            _mother.RemoveAction("Roar Start", 11);
            _mother.RemoveAction("Roar Start", 10);
            _mother.RemoveAction("Roar Start", 9);
            _mother.RemoveAction("Roar Start", 8);
        }
    }
}
