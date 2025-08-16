using Vasi;
using HutongGames.PlayMaker.Actions;
using Random = UnityEngine.Random;

namespace PantheonOfRegions.Behaviours
{
    internal class LostKin : MonoBehaviour
    {
        private PlayMakerFSM _control;
        private PlayMakerFSM _spawn;
        private static readonly Lazy<Texture2D> lostkinTex = new(() => AssemblyUtils.GetTextureFromResources("VoidKin.png"));
        private BossSpawner Spawner = new BossSpawner();
        private Spritebuilder Spritebuilder = new Spritebuilder();
        
        private void Awake()
        {
            _control = gameObject.LocateMyFSM("IK Control");
            _spawn = gameObject.LocateMyFSM("Spawn Balloon");

        }

        private void Start()
        {
            Spritebuilder.ApplyTextureToTk2dSprite(gameObject, lostkinTex.Value);
            for (int x = 16; x >= 6; x-=2)
            {
                _control.InsertCustomAction("Dstab Land", () =>
                { _control.Fsm.GetFsmGameObject("Projectile").Value.AddComponent<Headglob>(); }, x);
            }
            _control.AddState("Final Stun");
            _control.GetState("Final Stun").CopyActionData(_control.GetState("Stun Start"));

            _control.GetAction<SendRandomEvent>("Damage Response").weights = new FsmFloat[] { 0.3f, 0f, 0.3f, 0.2f };

            _control.GetAction<Tk2dPlayAnimationWithEvents>("Intro Land").animationCompleteEvent = null;

            _control.GetState("Intro Land").AddAction(new Wait()
            {
                time = new(1f),
                finishEvent = FsmEvent.GetFsmEvent("FINISHED")
            });

            #region hit effect
            UObject.DestroyImmediate(gameObject.GetComponent<EnemyDeathEffects>());
            UObject.DestroyImmediate(gameObject.GetComponent<InfectedEnemyEffects>());
            ReflectionHelper.SetField(gameObject.GetComponent<HealthManager>(), "preventInvincibleEffect", true);

            EnemyHitEffectsUninfected hitEffect = gameObject.AddComponent<EnemyHitEffectsUninfected>();
            ReflectionHelper.SetField(gameObject.GetComponent<HealthManager>(), "hitEffectReceiver", hitEffect as IHitEffectReciever);

            EnemyHitEffectsUninfected voidEffect = PantheonOfRegions.InstaBoss["purevessel"]!.GetComponent<EnemyHitEffectsUninfected>();
            hitEffect.effectOrigin = voidEffect.effectOrigin;
            hitEffect.audioPlayerPrefab = voidEffect.audioPlayerPrefab;
            hitEffect.enemyDamage = voidEffect.enemyDamage;
            hitEffect.uninfectedHitPt = voidEffect.uninfectedHitPt;
            hitEffect.slashEffectGhost1 = voidEffect.slashEffectGhost1;
            hitEffect.slashEffectGhost2 = voidEffect.slashEffectGhost2;
            hitEffect.uninfectedHitPt = voidEffect.uninfectedHitPt;
            #endregion

            _control.Fsm.GetFsmFloat("Air Dash Height").Value = 6 + 3;
            _control.Fsm.GetFsmFloat("Left X").Value = 29;
            _control.Fsm.GetFsmFloat("Min Dstab Height").Value = 6 + 5;
            _control.Fsm.GetFsmFloat("Right X").Value = 61;
            _control.Fsm.GetFsmFloat("Right X").Value = 61;
            _control.Fsm.GetFsmBool("Rewake Range").Value = true;

            _control.GetAction<RandomFloat>("Aim Jump 2").min = 45 - 1;
            _control.GetAction<RandomFloat>("Aim Jump 2").max = 45 + 1;
            _control.GetAction<SetPosition>("Intro Fall").x = transform.position.x;
            _control.GetAction<SetPosition>("Intro Fall").y = transform.position.y;
            _control.GetAction<SetPosition>("Set X", 0).x = transform.position.x;
            _control.GetAction<SetPosition>("Set X", 2).x = transform.position.x;
            _control.GetAction<SetDamageHeroAmount>("Roar End", 3).damageDealt = 2;

            _control.RemoveAction("Dstab Land", 2);
            _control.RemoveAction("Roar", 9);
            _control.RemoveAction("Roar", 8);
            _control.RemoveAction("Roar", 7);
            _control.RemoveAction("Roar", 6);
            _control.RemoveAction("Roar", 1);

            _spawn.Fsm.GetFsmFloat("Wait Min").Value = 5f;
            _spawn.Fsm.GetFsmFloat("Wait Max").Value = 6f;
            _spawn.RemoveAction("Spawn", 8);
            _spawn.RemoveAction("Spawn", 3);

            _spawn.AddCustomAction("Spawn", () =>
            {
                Vector3 SpawnPos;
                while (true)
                {
                    SpawnPos = new Vector3(Random.Range(30f, 60f), Random.Range(8f, 14f), 0f);
                    if (Vector2.Distance(HeroController.instance.transform.position, SpawnPos) > 10f) break;
                }
                GameObject shade = Spawner.SpawnBoss("sibling", SpawnPos);
                shade.SetActive(true);
                Destroy(shade.transform.GetChild(6).gameObject);
                _spawn.Fsm.GetFsmGameObject("Spawned Enemy").Value = shade;
            });
        }
    }
    internal class Headglob : MonoBehaviour
    {
        private Sprite headglobSprite = null;
        private static readonly Texture2D headglobTex = AssemblyUtils.GetTextureFromResources("void_glob.png");
        private void GlobApplyTexture(GameObject projectile)
        {
            var renderer = projectile.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = headglobSprite;
            Destroy(projectile.transform.Find("Gas Attack").gameObject);
        }
        private void Awake()
        {
            headglobSprite = Sprite.Create(headglobTex, new Rect(0, 0, 109, 110), new Vector2(0.545f, 0.55f), 64);
            GlobApplyTexture(gameObject);
        }
    }
}