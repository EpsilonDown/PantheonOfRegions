using HutongGames.PlayMaker.Actions;
using Osmi.Game;
using Vasi;
using PantheonOfRegions.Actions;
using Random = UnityEngine.Random;
using Satchel;

namespace PantheonOfRegions.Behaviours
{
    internal class AbsoluteRadiance : MonoBehaviour
    {
        private PlayMakerFSM? _commands;
        private PlayMakerFSM? _choices;
        private PlayMakerFSM? _control;
        private PlayMakerFSM? _phase;
        private PlayMakerFSM? ascender;
        private PlayMakerFSM? mk_attack;
        private tk2dSprite? radsprite;
        private Color invisible = new Color(0f,0f,0f,0f);
        private Markoth mk_control;
        private PlayMakerFSM? mk_shield;
        private GameObject? markoth;
        private GameObject? seer;
        private GameObject? healthsharer;
        private GameObject? shield;
        private List<GameObject> shields = new List<GameObject>();
        private List<GameObject> uppershields = new List<GameObject>();
        private List<GameObject> lowershields = new List<GameObject>();
        private List<GameObject> radclones = new List<GameObject>();
        private int phase = 1;
        private void Awake()
        {
            
            _commands = gameObject.LocateMyFSM("Attack Commands");
            _choices = gameObject.LocateMyFSM("Attack Choices");
            _control = gameObject.LocateMyFSM("Control");
            _phase = gameObject.LocateMyFSM("Phase Control");
            radsprite = gameObject.GetComponent<tk2dSprite>();
            GetChildren();
        }

        private void Start()
        {

            //for debug

            for (int i = 1; i <= 3; i++)
            {
                foreach (Transform beam in gameObject.transform.Find("Eye Beam Glow/Burst " + i))
                {
                    beam.gameObject.AddComponent<ReflectBeam>();
                }
            }
            gameObject.transform.Find("Eye Beam Glow/Ascend Beam").gameObject.AddComponent<ReflectBeam>();
            PantheonOfRegions.RadianceObjects["Sweep Beam"].AddComponent<SweepBeam>();
            PantheonOfRegions.RadianceObjects["Sweep Beam 2"].AddComponent<SweepBeam>();

            #region Phase Controller

            _phase.Fsm.GetFsmInt("HP").Value = healthsharer.GetComponent<SharedHealthManager>().HP;
            _phase.Fsm.GetFsmInt("P2 Spike Waves").Value = 3100;
            _phase.Fsm.GetFsmInt("P3 A1 Rage").Value = 2600;
            _phase.Fsm.GetFsmInt("P4 Stun1").Value = 2200;
            _phase.Fsm.GetFsmInt("P5 Ascend").Value = 1000;
            _control.Fsm.GetFsmInt("Death HP").Value = 500;

            for (int i=1; i<=4; i++)
            {
                _phase.GetState("Check " + i).RemoveAction<GetHP>();
                _phase.InsertCustomAction("Check " + i, () =>
                {
                    _phase.Fsm.GetFsmInt("HP").Value = healthsharer.GetComponent<SharedHealthManager>().HP;
                }, 0);
            }
            _control.RemoveAction("Final Idle", 2);
            _control.InsertCustomAction("Final Idle", () =>
            {
                _control.Fsm.GetFsmInt("HP").Value = healthsharer.GetComponent<SharedHealthManager>().HP;
            }, 0);
            #endregion

            StartPhase1();

        }
        private void StartPhase1()
        {

            //Tele in
            _control.AddCustomAction("First Tele", () =>
            {
                
                mk_shield.SendEvent("SHIELD END");
            });

            //nail fan
            _commands.AddCustomAction("Nail Fan", () =>
            {
                mk_shield.SetState("Ready");
                mk_attack.SetState("Nail Fan");
            });

            //reflectors
            _commands.AddCustomAction("NF Glow", () =>
            {
                reflectorshield();
                mk_shield.SetState("Ready");
                mk_attack.SetState("Reflector");
            });
            _commands.AddCustomAction("End", () =>
            {
                mk_shield.SendEvent("SHIELD END");
                mk_attack.SendEvent("SHIELD END");
            });

            _commands.AddCustomAction("EB End", () =>
            {
                clearshield();
                mk_shield.SendEvent("SHIELD END");
                mk_attack.SendEvent("ATTACK END");
            });

            //Orb Control
            _commands.GetState("Orb Antic").GetAction<RandomInt>().min = 4;
            _commands.GetState("Orb Antic").GetAction<RandomInt>().max = 6;
            _commands.GetState("Orb Summon").GetAction<Wait>().time = 0.4f;

            //nail comb

            PantheonOfRegions.RadianceObjects["Nail Comb"].LocateMyFSM("Control").Fsm.GetFsmFloat("Nail Speed").Value = 25f;
            _commands.AddCustomAction("Comb Top 2", () =>{ mk_control.NailComb(); });
            _commands.GetState("Comb Top 2").GetAction<SetFsmInt>().setValue = 3;
            _choices.GetState("Nail Top Sweep").GetAction<SendEventByName>(1).sendEvent = "COMB TOP2";
            _choices.GetState("Nail Top Sweep").GetAction<SendEventByName>(3).sendEvent = "COMB TOP2";
            _commands.AddCustomAction("Comb L", () => { mk_control.NailCombTop(); });
            _commands.AddCustomAction("Comb R", () => { mk_control.NailCombTop(); });
            _commands.AddCustomAction("Comb L 2", () => { mk_control.NailCombTop(); });
            _commands.AddCustomAction("Comb R 2", () => { mk_control.NailCombTop(); });


            foreach (string s in new[] { "Beam Sweep L", "Beam Sweep L 2", "Beam Sweep R", "Beam Sweep R 2" })
            {
                _choices.GetState(s).RemoveAction<SendEventByName>();
                _choices.AddCustomAction(s, () =>
                {
                    PantheonOfRegions.RadianceObjects["Sweep Beam"].LocateMyFSM("Control").SendEvent("BEAM SWEEP L");
                    PantheonOfRegions.RadianceObjects["Sweep Beam 2"].LocateMyFSM("Control").SendEvent("BEAM SWEEP R");
                });
            }

            //Rage
            _control.GetState("Rage Comb").RemoveAction<SpawnObjectFromGlobalPool>();
            _control.GetState("Rage Comb").RemoveAction<SetFsmInt>();
            _control.AddCustomAction("Rage1 Start", () =>
            {
                markoth.GetComponent<Markoth>().StartNailSpam();
                
            });
            //stun control
            _control.AddCustomAction("Stun1 Start", () =>
            {
                mk_shield.SetState("Ready");
                mk_attack.SetState("Stun");
                markoth.GetComponent<Markoth>().StopNailSpam();
                seer.LocateMyFSM("Movement").SetState("Wait");
                markoth.GetComponent<HealthManager>().IsInvincible = true;
            });
            
            _control.RemoveAction("Tendrils1", 1);
            _control.InsertCustomAction("Stun1 Out", () =>
            {
                markoth.SetActive(false);
                seer.SetActive(false);
            },12);
            
            _control.AddCustomAction("Arena 2 Start", () =>
            {
                markoth.SetActive(true);
                seer.SetActive(true);
                markoth.transform.position = new Vector3(60f,50f,0f);
                markoth.GetComponent<HealthManager>().IsInvincible = false;
                markoth.GetComponent<Markoth>().Phase2();
                seer.GetComponent<Seer>().Phase2();
                seer.LocateMyFSM("Movement").SetState("Choose Target");
                mk_shield.SendEvent("SHIELD END");
                mk_attack.SendEvent("STUN END");
                
                phase++;
                StartCoroutine(OrbCooldown(true));
                StartPhase2();
            });
            Modding.Logger.Log("P1 Edit Completed!!");
        }


        private void StartPhase2()
        {
            for (int x = 42; x <= 81; x += 3)
            {
                GameObject uppershield = Instantiate(shield,
                new Vector3(x, 62f, 0), Quaternion.Euler(0, 0, +90f));
                GameObject lowershield = Instantiate(shield,
                new Vector3(x, 27f, 0), Quaternion.Euler(0, 0, -90f));
                uppershield.SetActive(true);
                lowershield.SetActive(true);
                uppershields.Add(uppershield);
                lowershields.Add(lowershield);
            }

            _control.InsertCustomAction("Ascend Tele", () =>
            {
                mk_shield.SetState("Ready");
                mk_attack.SetState("Stun");
                markoth.SetActive(false);
                if (uppershields != null) { foreach (GameObject s in uppershields) { Destroy(s); }; }
            },0);

            _control.AddCustomAction("Ascend Cast", () =>
            {
                markoth.transform.position = new Vector3(60f, 125f, 0);
                markoth.GetComponent<HealthManager>().IsInvincible = true;
                markoth.GetComponent<Markoth>().Phase3();
                markoth.SetActive(true);
                StartCoroutine(AscendShield());
                phase++;
            });

            _control.GetState("Scream").RemoveAction<SetHP>();
            
            _control.AddCustomAction("Scream", () =>
            {
                StopCoroutine(AscendShield());
                if (lowershields != null) { foreach (GameObject s in lowershields) { Destroy(s); }; }
                radsprite.color = invisible;
                gameObject.transform.Find("Legs").gameObject.GetComponent<tk2dSprite>().color = invisible;
                gameObject.transform.Find("Halo").gameObject.GetComponent<SpriteRenderer>().color = invisible;
                gameObject.transform.Find("Pt Tele Out").gameObject.SetActive(false);
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                Destroy(seer);
                StartCoroutine(StartPhase3());
            });
            _commands.GetState("Set Final Orbs").GetAction<Wait>().time = 2f;

            

        }
        private IEnumerator StartPhase3()
        {
            PlayerData.instance.equippedCharm_30 = false;
            for (int y = 0; y <= 1; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    yield return null;
                    GameObject Radclone = Instantiate(PantheonOfRegions.GameObjects["noeyes"], new Vector3(50f + 6f * x, 156.0f + 6f*y, 0f), Quaternion.identity);
                    Radclone.SetActive(true);
                    Radclone.AddComponent<RadClone>();
                    radclones.Add(Radclone);
                }
            }
            _control.AddCustomAction("Death Ready", () =>
            {
                radsprite.color = Color.white;
                gameObject.transform.Find("Legs").gameObject.GetComponent<tk2dSprite>().color = Color.white;
                gameObject.transform.Find("Halo").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                if (radclones != null) { foreach (GameObject s in radclones) { Destroy(s); }; }
                if (markoth != null) { Destroy(markoth); };
            });
            
            //_control.ChangeTransition("Tendrils 2", "FINISHED", "Statue Death 1");
            yield return new WaitForSeconds(1f);
            Shuffling();
        }

        private void GetChildren()
        {
            healthsharer = PantheonOfRegions.SharedBoss;
            markoth = PantheonOfRegions.InstaBoss["markoth"];
            seer = PantheonOfRegions.InstaBoss["seer"];
            mk_control = PantheonOfRegions.InstaBoss["markoth"].GetComponent<Markoth>();
            mk_shield = markoth.LocateMyFSM("Shield Attack");
            mk_attack = markoth.LocateMyFSM("Attacking");

            shield = GameObject.Find("Markoth Shield(Clone)");
            Destroy(shield.LocateMyFSM("Control"));
            shield!.SetActive(false);
            GameObject _bossCtrl = transform.parent.gameObject;
            PantheonOfRegions.RadianceObjects["Abyss Pit"] = _bossCtrl.transform.Find("Abyss Pit").gameObject;
            PantheonOfRegions.RadianceObjects["Radiance"] = gameObject;
            PantheonOfRegions.RadianceObjects["Glow"] = gameObject.transform.Find("Eye Beam Glow").gameObject;
            PantheonOfRegions.RadianceObjects["Beam Burst"] = gameObject.transform.Find("Eye Beam Glow/Burst 1").gameObject;
            PantheonOfRegions.RadianceObjects["Eye Beam"] = gameObject.transform.Find("Eye Beam Glow/Burst 1/Radiant Beam").gameObject;
            PantheonOfRegions.RadianceObjects["Shot Charge"] = gameObject.transform.Find("Shot Charge").gameObject;
            PantheonOfRegions.RadianceObjects["Orb"] = _commands.GetAction<SpawnObjectFromGlobalPool>("Spawn Fireball").gameObject.Value;
            PantheonOfRegions.RadianceObjects["Eye Beam"].CreatePool(30);
            PantheonOfRegions.RadianceObjects["Nail Comb"] = _commands.GetAction<SpawnObjectFromGlobalPool>("Comb Top").gameObject.Value;
            PantheonOfRegions.RadianceObjects["Sweep Beam"] = _bossCtrl.gameObject.transform.Find("Beam Sweeper").gameObject;
            PantheonOfRegions.RadianceObjects["Sweep Beam 2"] = Instantiate(PantheonOfRegions.RadianceObjects["Sweep Beam"]);
            PantheonOfRegions.RadianceObjects["Audio Player"] = gameObject.LocateMyFSM("Teleport").GetAction<AudioPlayerOneShotSingle>("Antic").audioPlayer.Value;
            PantheonOfRegions.AudioClips["Ghost"] = (AudioClip)_commands.GetAction<AudioPlayerOneShotSingle>("Orb Summon").audioClip.Value;
            PantheonOfRegions.AudioClips["Explode"] = (AudioClip)_control.GetAction<AudioPlayerOneShotSingle>("Knight Break").audioClip.Value;
            PantheonOfRegions.AudioClips["Final Hit 2"] = (AudioClip)_control.GetAction<AudioPlayerOneShotSingle>("Statue Death 1").audioClip.Value;
            PantheonOfRegions.AudioClips["Scream"] = (AudioClip)_control.GetAction<AudioPlayerOneShotSingle>("Scream").audioClip.Value;
        }
        private void reflectorshield()
        {
            if (phase == 1)
            {
                int randomangle = Random.Range(0, 1);
                for (int x = -15; x <= 15; x += 3)
                {
                    Vector3 shieldpos = new Vector3(gameObject.transform.GetPositionX() + x, 38f - 0.02f * x * x, 0);
                    GameObject mshield = Instantiate(shield, shieldpos, Quaternion.Euler(0, 0, 90 - 3 * x -2f * randomangle));
                    mshield.SetActive(true);
                    shields.Add(mshield);

                }
            }
        }
        private IEnumerator AscendShield()
        {
            while (true)
            {
                if (lowershields != null)
                {
                    for (int x = -3; x <= 3; x++)
                    {
                        GameObject s = lowershields[x+3];
                        s.transform.position = HeroController.instance.transform.position + new Vector3(3f*x,-12f + 0.2f*x*x,0f);
                        s.transform.rotation = Quaternion.Euler(0f, 0f, -90f + 10f*x);
                    };
                }
                yield return new WaitForSeconds(1f);
            }
        }
        private void clearshield()
        {
            if (shields != null)
            {
                foreach (GameObject s in shields) { Destroy(s); }
                
            }
        }
        internal void OrbRespawn()
        {
            StartCoroutine(OrbCooldown(false));
        }
        private IEnumerator OrbCooldown(bool initial)
        {
            yield return new WaitForSeconds(initial ? 2f : Random.Range(18f, 20f));
            if (phase == 2)
            {
                GameObject MegaOrb = Instantiate(PantheonOfRegions.RadianceObjects["Orb"], new Vector3(60f, 45f, 0f), Quaternion.identity);
                MegaOrb.AddComponent<BigOrb>();
                MegaOrb.SetActive(true);
            }
            if (initial == true)
            {
                PlayMakerFSM convo = PlayMakerFSM.FindFsmOnGameObject(HutongGames.PlayMaker.FsmVariables.GlobalVariables.GetFsmGameObject("Enemy Dream Msg").Value, "Display");
                convo.FsmVariables.GetFsmInt("Convo Amount").Value = 1;
                convo.FsmVariables.GetFsmString("Convo Title").Value = "SEER_BATTLE";
                convo.SendEvent("DISPLAY ENEMY DREAM");
                yield return new WaitForSeconds(3f);
                convo.SendEvent("CANCEL ENEMY DREAM");
            }
        }
        internal void Shuffling()
        {
            StopAllCoroutines();
            StartCoroutine(ShuffleRad());
        }
        internal IEnumerator ShuffleRad()
        {
            int tpcount = Random.Range(1,4);
            for (int y = 0; y < tpcount; y++)
            {
                foreach (GameObject rad in radclones)
                {
                    rad.GetComponent<RadClone>().FakeRad();
                }
                yield return new WaitForSeconds(0.3f);
                int truerad = Random.Range(0, radclones.Count - 1);
                radclones[truerad].GetComponent<RadClone>().TrueRad();
                yield return new WaitForSeconds(0.3f);
            }

        }

    }

}