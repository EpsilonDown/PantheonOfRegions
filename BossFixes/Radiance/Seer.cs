using Vasi;
using HutongGames.PlayMaker.Actions;
using PantheonOfRegions.Actions;
using Random = UnityEngine.Random;
using System;

namespace PantheonOfRegions.Behaviours
{
    internal class Seer : MonoBehaviour
    {
        // 11, 37 bench cord
        private PlayMakerFSM _movement;
        private static readonly Lazy<Texture2D> seerTex = new(() => AssemblyUtils.GetTextureFromResources("Seer.png"));
        private static readonly Lazy<Texture2D> seersoulTex = new(() => AssemblyUtils.GetTextureFromResources("Seersoul.png"));
        private static readonly Lazy<Texture2D> seernailTex = new(() => AssemblyUtils.GetTextureFromResources("Seernail.png"));
        private static readonly Lazy<Texture2D> seermaskTex = new(() => AssemblyUtils.GetTextureFromResources("Seermask.png"));
        private Spritebuilder spritebuilder = new Spritebuilder();
        private int buffstate = 0;
        private void Awake()
        {
            _movement = gameObject.LocateMyFSM("Movement");
            gameObject.GetComponent<DamageHero>().damageDealt = 0;
            Destroy(gameObject.GetComponent<HealthManager>());
            Destroy(gameObject.GetComponent<AudioSource>());
            Destroy(gameObject.LocateMyFSM("Shot Spawn"));
            Destroy(gameObject.LocateMyFSM("Escalation"));
            Destroy(gameObject.LocateMyFSM("Damage Response"));
            gameObject.AddComponent<EnemyDreamnailReaction>();
            gameObject.GetComponent<EnemyDreamnailReaction>().SetConvoTitle("");
            On.EnemyDreamnailReaction.RecieveDreamImpact += OnReceiveDreamImpact;
        }

        private void Start()
        {
            for (int index = 1; index <= 8; index++)
            {
                _movement.Fsm.GetFsmVector3($"P{index}").Value = RandomVector3(22f, 30f);
            }
            _movement.AddState("Wait");
            _movement.RemoveAction("Hover", 3);
            _movement.RemoveAction("Hover", 0);
            _movement.RemoveAction("Check Valid", 2);
            _movement.RemoveAction("Check Valid", 1);
            _movement.InsertCustomAction("Choose Target", () =>
            {
                buffstate = Random.Range(1, 4);
                switch (buffstate)
                {
                    case 1:
                        spritebuilder.ApplyTextureToTk2dSprite(gameObject, seernailTex.Value);
                        break;
                    case 2:
                        spritebuilder.ApplyTextureToTk2dSprite(gameObject, seermaskTex.Value);
                        break;
                    case 3:
                        spritebuilder.ApplyTextureToTk2dSprite(gameObject, seersoulTex.Value);
                        break;

                    default:
                        break;
                }
            }, 0);
            FsmState Hover = _movement.GetState("Hover");
            Hover.GetAction<WaitRandom>().timeMin = 5f;
            Hover.GetAction<WaitRandom>().timeMax = 6f;

            _movement.SetState("Wait");
            gameObject.transform.position = new Vector3(68f, 30f, 0f);
            spritebuilder.ApplyTextureToTk2dSprite(gameObject, seerTex.Value);

        }

        internal void Phase2()
        {
            PlayerData.instance.equippedCharm_30 = true;
            _movement.Fsm.GetFsmVector3("P1").Value = new Vector3(60f, 45f, 0f);
            _movement.Fsm.GetFsmVector3("P2").Value = new Vector3(58f, 35f, 0f);
            _movement.Fsm.GetFsmVector3("P3").Value = new Vector3(51f, 37f, 0f);
            _movement.Fsm.GetFsmVector3("P4").Value = new Vector3(67f, 39f, 0f);
            _movement.Fsm.GetFsmVector3("P5").Value = new Vector3(69f, 40f, 0f);
            _movement.Fsm.GetFsmVector3("P6").Value = new Vector3(48f, 45f, 0f);
            _movement.Fsm.GetFsmVector3("P7").Value = new Vector3(42f, 36f, 0f);
            _movement.Fsm.GetFsmVector3("P8").Value = new Vector3(62f, 35f, 0f);
        }

        private void OnReceiveDreamImpact(On.EnemyDreamnailReaction.orig_RecieveDreamImpact orig, EnemyDreamnailReaction self)
        {
            GameObject BigOrb = PantheonOfRegions.InstaBoss["bigorb"];
            orig(self);
            if (BigOrb != null)
            {
                GameObject nail = PantheonOfRegions.InstaBoss["markoth"].GetComponent<Markoth>().GetNail();
                float dx = BigOrb.transform.position.x - gameObject.transform.position.x;
                float dy = BigOrb.transform.position.y - gameObject.transform.position.y;
                GameObject seernail = Instantiate(nail, gameObject.transform.position,
                    Quaternion.Euler(new Vector3(0f, 0f, Mathf.Atan2(dy, dx) * Mathf.Rad2Deg)));
                seernail.AddComponent<SeerNail>();
                seernail.SetActive(true);
                seernail.GetComponent<SpriteRenderer>().color = Color.black;

            }

        }
        private Vector3 RandomVector3(float ymin, float ymax)
        {
            float x = Random.Range(50f, 72f);
            float y = Random.Range(ymin, ymax);
            float z = 0.006f;

            return new Vector3(x, y, z);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.gameObject.name == "HeroBox")
            {
                switch (buffstate)
                {
                    case 0:
                        break;
                    case 1:
                        HeroController.instance.gameObject.AddComponent<NailBuff>();
                        break;
                    case 2:
                        HeroController.instance.AddHealth(2);
                        break;
                    case 3:
                        HeroController.instance.AddMPCharge(66);
                        break;

                    default:
                        break;
                }
                buffstate = 0;
                spritebuilder.ApplyTextureToTk2dSprite(gameObject, seerTex.Value);
            }
        }

    }
    internal class SeerNail : MonoBehaviour
    {
        private IEnumerator Start()
        {
            Destroy(gameObject.GetComponent<PolygonCollider2D>());
            PlayMakerFSM nailCtrl = gameObject.LocateMyFSM("Control");
            GameObject BigOrb = PantheonOfRegions.InstaBoss["bigorb"];
            nailCtrl.ChangeTransition("Set Pos", "FINISHED", "Antic");
            nailCtrl.GetState("Recycle").InsertMethod(0, () => Destroy(gameObject));
            nailCtrl.Fsm.GetFsmFloat("Distance").Value = Vector3.Distance(BigOrb.transform.position, gameObject.transform.position);
            nailCtrl.SetState("Init");
            nailCtrl.enabled = true;
            yield return new WaitForSeconds(nailCtrl.Fsm.GetFsmFloat("Distance").Value / 30 + 0.4f);
            BigOrb.GetComponent<BigOrb>().Explode();
        }
    }
    internal class NailBuff : MonoBehaviour
    {
        private void ModifySlashColors(bool modify)
        {

            Color color = modify ? new Color(1.0f, 0.5f, 0.0f) : Color.white;
            foreach (GameObject slash in new GameObject[]
            {
                HeroController.instance.slashPrefab,
                HeroController.instance.slashAltPrefab,
                HeroController.instance.downSlashPrefab,
                HeroController.instance.upSlashPrefab,
                HeroController.instance.wallSlashPrefab
            })
            {
                slash.GetComponent<tk2dSprite>().color = color;
            }
        }
        private IEnumerator Start()
        {
            PlayerData.instance.nailDamage += 5;
            ModifySlashColors(true);
            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            Modding.Logger.Log("Buffed Nail");

            yield return new WaitForSeconds(5f);

            PlayerData.instance.nailDamage -= 5;
            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            ModifySlashColors(false);

        }
    }
}
