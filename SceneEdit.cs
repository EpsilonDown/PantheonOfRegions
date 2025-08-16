using PantheonOfRegions.Behaviours;
using HutongGames.PlayMaker.Actions;
using Osmi.Game;
using UnityEngine;
using IL;
using On;
namespace PantheonOfRegions;
public sealed partial class PantheonOfRegions
{
    private static bool running = false;
    public static GameObject SharedBoss;
    //public GameObject healthsharer = null;
    public static void EditScene(Scene prev, Scene next)
    {
        BossSpawner Spawner = new BossSpawner();
        GameObject SpawnBoss(string Boss, Vector2 spawnPoint)
        {
            GameObject boss = Spawner.SpawnBoss(Boss, spawnPoint);
            //boss.AddComponent<SpellNerf>();
            return boss;
        }
        GameObject BossFinder(string Boss)
        {
            GameObject boss = GameObject.Find(Boss);
            //boss.AddComponent<SpellNerf>();
            return boss;
        }
        void HealthSharer(int hp, string sharename, params GameObject[] sharedbosses)
        {
            foreach (GameObject go in sharedbosses){ go.SetActive(true); }
            SharedHealthManager sharer = sharedbosses.ShareHealth(name: sharename);
            sharer.HP = hp;
            SharedBoss = sharer.gameObject;
            sharer.gameObject.RemoveComponent<BoxCollider2D>();
        }

        if (!BossSequenceController.IsInSequence && !GlobalSettings.modifyhall)
        {
            goto Inactive;
        }

        switch (next.name)
        {
            case "GG_Vengefly_V":
                //Vengefly Kings + Gorb -> Minor Fix Needed
                running = true;
                GameObject Gorb = SpawnBoss("gorb", new Vector2(43.0f, 20.0f));
                GameObject buz1 = BossFinder("Giant Buzzer Col");
                GameObject buz2 = BossFinder("Giant Buzzer Col (1)");
                buz1
                    .LocateMyFSM("Big Buzzer")
                    .InsertCustomAction("Check Dir 2", () =>
                    {
                        HealthSharer(1200, "cliffs", new[] { buz1, buz2, Gorb });
                    }, 0);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Mega_Moss_Charger":
                running = true;
                GameObject Hornet = SpawnBoss("hornetprotector", new Vector2(60.0f, 10.0f));
                GameObject MMCharger = BossFinder("Mega Moss Charger");
                MMCharger
                    .LocateMyFSM("Mossy Control")
                    .InsertCustomAction("Roar", () =>
                    {
                        HealthSharer(1000, "ambushers", new[] { MMCharger, Hornet });
                    }, 2);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Failed_Champion":
                //Failed Champion + Mawlek - Minor Fix Needed
                running = true;
                GameObject Mawlek = SpawnBoss("broodingmawlek", new Vector2(60.0f, 50.0f));
                GameObject FailedChampion = BossFinder("False Knight Dream");
                FailedChampion.AddComponent<EnemyTracker>();
                FailedChampion.LocateMyFSM("FalseyControl").InsertCustomAction("Start Fall", () =>
                {
                    HealthSharer(1610, "crossroads", new[] { FailedChampion, Mawlek });
                }, 0);

                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Mantis_Lords_V":
                running = true;
                GameObject ElderHu = SpawnBoss("elderhu", new Vector2(30.0f, 15.0f));
                GameObject battle = next.GetRootGameObjects().First(go => go.name == "Mantis Battle");
                ElderHu.transform.Find("Target").transform.position = new Vector2(30f, 12f);

                battle.transform.Find("Mantis Lord Throne 2").gameObject
                    .LocateMyFSM("Mantis Throne Main")
                    .InsertCustomAction("Roar 2", () =>
                    {
                        HealthSharer(2000, "alliance of battle", new[] { 
                            battle.transform.Find("Battle Sub/Mantis Lord S1").gameObject,
                            battle.transform.Find("Battle Sub/Mantis Lord S2").gameObject,
                            battle.transform.Find("Battle Sub/Mantis Lord S3").gameObject,
                            ElderHu });
                    }, 4);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Crystal_Guardian_2":
                running = true;
                GameObject EnragedGuardian = BossFinder("Battle Scene/Zombie Beam Miner Rematch");
                GameObject Xero = SpawnBoss("xero", new Vector2(30.0f, 17.0f));
                HealthSharer(1200, "crystals", new[] { EnragedGuardian, Xero });
                break;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Soul_Tyrant":
                //Soul Warrior + Knight
                running = true;
                GameObject SoulWarrior = SpawnBoss("soulwarrior", new Vector2(30.0f, 30.0f));
                GameObject SoulTyrant = BossFinder("Dream Mage Lord");
                GameObject SoulTyrant2 = BossFinder("Dream Mage Lord Phase2");
                SoulTyrant2.AddComponent<SoulTyrant2>();
                SoulTyrant
                    .LocateMyFSM("Mage Lord")
                    .InsertCustomAction("Roar", () =>
                    {
                        HealthSharer(1400, "soulmasters", new[] { SoulWarrior, SoulTyrant });
                    }, 2);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Traitor_Lord":
                //Traitor Lord + Marmu
                running = true;
                GameObject Marmu = SpawnBoss("marmu", new Vector2(40.0f, 36.0f));
                GameObject TraitorLord = BossFinder("Battle Scene/Wave 3/Mantis Traitor Lord");
                TraitorLord.LocateMyFSM("Mantis").RemoveAction("Slam?", 2);
                TraitorLord
                    .LocateMyFSM("Mantis")
                    .InsertCustomAction("Roar", () =>
                    {
                        HealthSharer(1200, "queens", new[] { TraitorLord, Marmu });
                    }, 0);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Nailmasters":
                running = true;
                GameObject Oro = BossFinder("Brothers/Oro");
                GameObject Mato = BossFinder("Brothers/Mato");
                GameObject Sheo = SpawnBoss("sheo", new Vector2(45.0f, 6.9f));
                Sheo.SetActive(true);
                PantheonOfRegions.InstaBoss["sheo"] = Sheo;
                Oro.AddComponent<EnemyTracker>();
                Oro.LocateMyFSM("nailmaster")
                    .InsertCustomAction("Reactivate", () =>
                    {
                        HealthSharer(2000, "nailmasters", new[] { Oro, Mato, Sheo });
                    }, 0);

                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////


            case "GG_Uumuu":
                running = true;
                GameObject NoEyes = SpawnBoss("noeyes", new Vector2(55.0f, 120.0f));
                GameObject Uumuu = BossFinder("Mega Jellyfish GG");
                Uumuu
                    .LocateMyFSM("Mega Jellyfish")
                    .AddCustomAction("Start", () =>
                    {
                        HealthSharer(1000, "blinders", new[] { Uumuu, NoEyes });
                        
                    }); 
                break;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Nosk":
                running = true;
                GameObject Galien2 = SpawnBoss("galien", new Vector2(110.0f, 10.0f));
                GameObject Nosk2 = BossFinder("Mimic Spider");
                Nosk2.AddComponent<EnemyTracker>();
                Nosk2.LocateMyFSM("Mimic Spider")
                    .InsertCustomAction("GG Activate", () =>
                    {
                        HealthSharer(1000, "stalkers", new[] { Nosk2, Galien2 });
                    }, 0);

                break;
            case "GG_Nosk_V":
                running = true;
                GameObject Galien = SpawnBoss("galien", new Vector2(110.0f, 10.0f));
                GameObject Nosk = BossFinder("Mimic Spider");
                Nosk.AddComponent<EnemyTracker>();
                Nosk
                    .LocateMyFSM("Mimic Spider")
                    .InsertCustomAction("GG Activate", () =>
                    {
                        HealthSharer(1000, "stalkers", new[] { Nosk, Galien });
                    }, 0);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_White_Defender":
                //Flukemarm + White Defender
                running = true;
                GameObject Flukemarm = SpawnBoss("flukemarm", new Vector2(75.0f, 20.0f));
                GameObject WhiteDefender = BossFinder("White Defender");
                WhiteDefender
                    .LocateMyFSM("Dung Defender")
                    .InsertCustomAction("Erupt Out First 2", () =>
                    {
                        HealthSharer(1800, "waterway defenders", new[] { WhiteDefender, Flukemarm });
                    }, 0);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_Hornet_2":
                running = true;

                GameObject HiveKnight = SpawnBoss("hiveknight", new Vector2(30.0f, 36.0f));
                GameObject HornetSentinel = BossFinder("Boss Holder/Hornet Boss 2");
                HornetSentinel.LocateMyFSM("Control").Fsm.GetFsmBool("Can Barb").Value = true;
                HealthSharer(1600, "stingers", new[] { HornetSentinel, HiveKnight });
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_God_Tamer":
                running = true;
                GameObject Entry = GameObject.Find("Entry Object");
                GameObject Lobster = Entry.transform.Find("Lobster").gameObject;
                GameObject Lancer = Entry.transform.Find("Lancer").gameObject;
                GameObject Oblobble = SpawnBoss("oblobble", new Vector2(90.0f, 10.0f));
                GameObject Rageblobble = SpawnBoss("oblobble", new Vector2(100.0f, 10.0f));
                PantheonOfRegions.InstaBoss["oblobble"] = Oblobble;
                PantheonOfRegions.InstaBoss["rageblobble"] = Rageblobble;
                Lancer
                    .LocateMyFSM("Control")
                    .AddCustomAction("Init", () =>
                    {
                        Lobster.AddComponent<EnemyTracker>();
                        HealthSharer(1400, "colosseum champions", new[] { Lobster, Lancer, Oblobble, Rageblobble });
                        Rageblobble.SetActive(false);
                    });
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_Collector":
                //Collector + Watcher knights
                running = true;
                GameObject Collector = GameObject.Find("Battle Scene/Jar Collector");
                HealthSharer(1000, "citycollector", new[] { Collector });
                PantheonOfRegions.InstaBoss["collector"] = Collector;
                Collector.AddComponent<EnemyTracker>();
                break;

            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_Gruz_Mother":
                //Gruz + Sly
                running = true;
                GameObject GruzMother = BossFinder("_Enemies/Giant Fly");
                GameObject Sly = Spawner.SpawnBoss("greatnailsagesly", new Vector2(98.0f, 15.0f));
                Sly.SetActive(true);
                PantheonOfRegions.InstaBoss["sly"] = Sly;
                GruzMother.AddComponent<EnemyTracker>();
                HealthSharer(1800, "flylords", new[] { GruzMother, Sly });

                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Grimm_Nightmare":
                //NKG + Zote
                running = true;
                GameObject NKG = BossFinder("Grimm Control/Nightmare Grimm Boss");
                GameObject Zote = Spawner.SpawnBoss("greyprincezote", new Vector2(90.0f, 10.0f));
                Zote.SetActive(true);
                PantheonOfRegions.InstaBoss["nkg"] = NKG;
                PantheonOfRegions.InstaBoss["greyprincezote"] = Zote;
                for (int i = 0; i < 3; i++)
                {
                    GameObject balloon = Spawner.SpawnBoss("volatilezoteling", new Vector2(70.0f + 15f * i, 10.0f));
                    balloon.SetActive(true);
                }
                HealthSharer(2200, "reapers", new[] { NKG, Zote });
                NKG.AddComponent<EnemyTracker>();

                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Hollow_Knight":
                //PV + Lost Kin

                running = true;
                GameObject LostKin = SpawnBoss("lostkin", new Vector2(35.0f, 20.0f));
                GameObject PureVessel = BossFinder("Battle Scene/HK Prime");
                PantheonOfRegions.InstaBoss["lostkin"] = LostKin;
                PantheonOfRegions.InstaBoss["purevessel"] = PureVessel;
                PureVessel.AddComponent<EnemyTracker>();
                PureVessel
                    .LocateMyFSM("Control")
                    .InsertCustomAction("Intro 4", () =>
                    {
                        LostKin!.SetActive(true);
                        HealthSharer(2200, "voidvessels", new[] { PureVessel, LostKin });
                    }, 1);

                break;

            case "GG_Radiance":
                //Absrad + Markoth + Seer
                running = true;
                GameObject BattleScene = GameObject.Find("Boss Control");
                GameObject AbsoluteRadiance = BattleScene.transform.Find("Absolute Radiance").gameObject;
                GameObject Markoth = SpawnBoss("markoth", new Vector2(54.0f, 30.0f));
                GameObject Seer = GameObject.Instantiate(PantheonOfRegions.GameObjects["noeyes"], new Vector3(68f, 27f, 0f), Quaternion.identity);
                Seer.AddComponent<Seer>();
                Seer.SetActive(false);
                PantheonOfRegions.InstaBoss["markoth"] = Markoth;
                PantheonOfRegions.InstaBoss["seer"] = Seer;
                PantheonOfRegions.InstaBoss["radiance"] = AbsoluteRadiance;
                BattleScene.LocateMyFSM("Control").AddCustomAction("Appear Boom", () =>
                { 
                    Markoth.SetActive(true);
                    Seer.SetActive(true);
                });
                BattleScene.LocateMyFSM("Control").AddCustomAction("Battle Start", () =>
                {
                    HealthSharer(3600, "moths", new[] { AbsoluteRadiance, Markoth });
                    AbsoluteRadiance.AddComponent<AbsoluteRadiance>().enabled = true;
                    Seer.LocateMyFSM("Movement").SetState("Choose Target");
                });
                break;

            case "GG_Workshop":
                PlayerData.instance.equippedCharm_30 = false;
                break;
            case "GG_Atrium_Roof":
                PlayerData.instance.equippedCharm_30 = false;
                break;
            default:
                running = false;
                return;
        }
        Inactive:
            running = false;
            return;

    }

}
