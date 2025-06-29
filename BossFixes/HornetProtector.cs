using Vasi;
using HutongGames.PlayMaker.Actions;
namespace PantheonOfRegions.Behaviours
{
    internal class HornetProtector : MonoBehaviour
    {
        private PlayMakerFSM _control;

        private void Awake()
        {
            GetComponent<BoxCollider2D>().enabled = true;
            _control = gameObject.LocateMyFSM("Control");

            ReflectionHelper.GetField<EnemyDeathEffects, GameObject>(GetComponent<EnemyDeathEffectsUninfected>(), "corpse").AddComponent<HornetCorpse>();
        }

        private void Start()
        {
            _control.SetState("Pause");

            _control.GetState("Music").RemoveAction<TransitionToAudioSnapshot>();
            _control.GetState("Music").RemoveAction<ApplyMusicCue>();

            _control.Fsm.GetFsmFloat("Air Dash Height").Value = 7 + 4;
            _control.Fsm.GetFsmFloat("Floor Y").Value = 7;
            _control.Fsm.GetFsmFloat("Left X").Value = 28;
            _control.Fsm.GetFsmFloat("Min Dstab Height").Value = 7 + 6;
            _control.Fsm.GetFsmFloat("Right X").Value = 69;
            _control.Fsm.GetFsmFloat("Roof Y").Value = 22;
            _control.Fsm.GetFsmFloat("Sphere Y").Value = 7 + 6;
            _control.Fsm.GetFsmFloat("Throw X L").Value = 28 + 6.5f;
            _control.Fsm.GetFsmFloat("Throw X R").Value = 69 - 6.5f;
            _control.Fsm.GetFsmFloat("Wall X Left").Value = 28 + 1;
            _control.Fsm.GetFsmFloat("Wall X Right").Value = 69 - 1;

            var constrainPos = gameObject.GetComponent<ConstrainPosition>();
            constrainPos.constrainX = constrainPos.constrainY = false;
        }
    }

    internal class HornetCorpse : MonoBehaviour
    {
        private void Start()
        {
            gameObject.LocateMyFSM("Control").GetState("Land").AddCoroutine(TweenOut);
        }

        private IEnumerator TweenOut()
        {
            yield return new WaitForSeconds(3);

            GetComponent<BoxCollider2D>().isTrigger = true;
            
            yield return new WaitUntil(() =>
            {
                transform.Translate(Vector3.down * 25 * Time.deltaTime);
                return transform.position.y <= 7 - 10;
            });

            Destroy(gameObject);
        }
    }
}
