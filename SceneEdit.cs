using PantheonOfRegions.Behaviours;
using HutongGames.PlayMaker.Actions;
using Osmi.Game;
using UnityEngine;
namespace PantheonOfRegions;
public sealed partial class PantheonOfRegions
{
    private static bool running = false;
    private static List<GameObject> loadedboss = new List<GameObject>();
    //public GameObject healthsharer = null;
    public static void EditScene(Scene prev, Scene next)
    {
        BossSpawner Spawner = new BossSpawner();
        GameObject SpawnBoss(string Boss, Vector2 spawnPoint)
        {
            GameObject boss = Spawner.SpawnBoss(Boss, spawnPoint);
            loadedboss.Add(boss);
            return boss;
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
                GameObject buz1 = GameObject.Find("Giant Buzzer Col");
                GameObject buz2 = GameObject.Find("Giant Buzzer Col (1)");
                buz1
                    .LocateMyFSM("Big Buzzer")
                    .InsertCustomAction("Check Dir 2", () =>
                    {
                        Gorb!.SetActive(true);
                        SharedHealthManager cliffs = new[] { buz1, buz2, Gorb }
                        .ShareHealth(name: "Howlers");
                        cliffs.HP = BossSceneController.Instance.BossLevel == 0 ? 1200 : 1600;
                        PantheonOfRegions.InstaBoss["cliffs"] = cliffs.gameObject;
                        HeroController.instance.gameObject.AddComponent<TriggerDetect>();
                    }, 0);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Mega_Moss_Charger":
                running = true;
                GameObject Hornet = SpawnBoss("hornetprotector", new Vector2(60.0f, 10.0f));
                GameObject MassiveMossCharger = GameObject.Find("Mega Moss Charger");
                MassiveMossCharger
                    .LocateMyFSM("Mossy Control")
                    .InsertCustomAction("Roar", () =>
                    {
                        Hornet.SetActive(true);
                        new[] { MassiveMossCharger, Hornet }
                        .ShareHealth(name: "Ambushers").HP = 1000;
                        HeroController.instance.gameObject.AddComponent<TriggerDetect>();
                    }, 2);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Failed_Champion":
                //Failed Champion + Mawlek - Minor Fix Needed
                running = true;
                GameObject Mawlek = SpawnBoss("broodingmawlek", new Vector2(60.0f, 50.0f));
                GameObject FailedChampion = GameObject.Find("False Knight Dream");
                FailedChampion.AddComponent<EnemyTracker>();
                FailedChampion.LocateMyFSM("FalseyControl").InsertCustomAction("Start Fall", () =>
                {
                    Mawlek.SetActive(true);
                    SharedHealthManager crossroads = new[] { FailedChampion, Mawlek }.ShareHealth(name: "Crossroads");
                    crossroads.HP = BossSceneController.Instance.BossLevel == 0 ? 1610 : 2010;
                    InstaBoss["crossroads"] = crossroads.gameObject;
                    HeroController.instance.gameObject.AddComponent<TriggerDetect>();
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
                        ElderHu!.SetActive(true);
                        new[] { 1, 2, 3 }
                            .Map(i => "Battle Sub/Mantis Lord S" + i)
                            .Map(path => battle.transform.Find(path)!.gameObject).Append(ElderHu!)
                            .ShareHealth(name: "Alliance Of Battle").HP =
                                BossSceneController.Instance.BossLevel == 0 ? 2000 : 2400;
                    }, 4);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Crystal_Guardian_2":
                running = true;
                GameObject EnragedGuardian = GameObject.Find("Battle Scene/Zombie Beam Miner Rematch");
                InstaBoss["guardian"] = EnragedGuardian;
                EnragedGuardian
                    .LocateMyFSM("Beam Miner")
                    .InsertCustomAction("Battle Init", () =>
                    {
                        GameObject Xero = SpawnBoss("xero", new Vector2(30.0f, 17.0f));
                        Xero!.SetActive(true);
                        
                    }, 2);

                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Soul_Tyrant":
                //Soul Warrior + Knight
                running = true;
                GameObject SoulWarrior = SpawnBoss("soulwarrior", new Vector2(30.0f, 30.0f));
                GameObject SoulTyrant = GameObject.Find("Dream Mage Lord");
                GameObject SoulTyrant2 = GameObject.Find("Dream Mage Lord Phase2");
                SoulTyrant2.AddComponent<SoulTyrant2>();
                SoulTyrant
                    .LocateMyFSM("Mage Lord")
                    .InsertCustomAction("Roar", () =>
                    {
                        SoulWarrior.SetActive(true);
                        new[] { SoulWarrior, SoulTyrant }
                        .ShareHealth(name: "soulmasters").HP = 1200;
                    }, 2);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Traitor_Lord":
                //Traitor Lord + Marmu
                running = true;
                GameObject Marmu = SpawnBoss("marmu", new Vector2(40.0f, 36.0f));
                GameObject TraitorLord = GameObject.Find("Battle Scene/Wave 3/Mantis Traitor Lord");
                TraitorLord.LocateMyFSM("Mantis").RemoveAction("Slam?", 2);

                TraitorLord
                    .LocateMyFSM("Mantis")
                    .InsertCustomAction("Roar", () =>
                    {
                        Marmu.SetActive(true);
                        new[] { TraitorLord, Marmu }
                        .ShareHealth(name: "Queen's Tributes").HP = 1100;
                    }, 0);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Nailmasters":
                running = true;
                GameObject Oro = GameObject.Find("Brothers/Oro");
                Oro.AddComponent<EnemyTracker>();
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////


            case "GG_Uumuu":
                running = true;
                GameObject NoEyes = SpawnBoss("noeyes", new Vector2(55.0f, 120.0f));
                GameObject Uumuu = GameObject.Find("Mega Jellyfish GG");
                
                Uumuu
                    .LocateMyFSM("Mega Jellyfish")
                    .AddCustomAction("Start", () =>
                    {
                        NoEyes!.SetActive(true);
                        SharedHealthManager blinders = new[] { Uumuu, NoEyes }
                        .ShareHealth(name: "blindprotectors");
                        blinders.HP = 800;
                        GameObject.Destroy(blinders.GetComponent<NonBouncer>());
                        GameObject.Destroy(blinders.GetComponent<BoxCollider2D>());
                    }); 
                break;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Nosk":
                running = true;
                GameObject Galien2 = SpawnBoss("galien", new Vector2(110.0f, 10.0f));
                GameObject Nosk2 = GameObject.Find("Mimic Spider");
                Nosk2
                    .LocateMyFSM("Mimic Spider")
                    .InsertCustomAction("GG Activate", () =>
                    {
                        Galien2!.SetActive(true);
                        new[] { Nosk2, Galien2 }
                        .ShareHealth(name: "stalkers2").HP = 1000;
                    }, 0);
                break;
            case "GG_Nosk_V":
                running = true;
                GameObject Galien = SpawnBoss("galien", new Vector2(110.0f, 10.0f));
                GameObject Nosk = GameObject.Find("Mimic Spider");
                Nosk
                    .LocateMyFSM("Mimic Spider")
                    .InsertCustomAction("GG Activate", () =>
                    {
                        Galien!.SetActive(true);
                        new[] { Nosk, Galien }
                        .ShareHealth(name: "stalkers").HP = 1000;
                    }, 0);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_White_Defender":
                //Flukemarm + White Defender (Not the other meaning)
                running = true;
                GameObject Flukemarm = SpawnBoss("flukemarm", new Vector2(75.0f, 20.0f));
                GameObject WhiteDefender = GameObject.Find("White Defender");
                WhiteDefender
                    .LocateMyFSM("Dung Defender")
                    .InsertCustomAction("Erupt Out First 2", () =>
                    {
                        Flukemarm!.SetActive(true);
                        new[] { WhiteDefender, Flukemarm }
                        .ShareHealth(name: "waterways").HP = 2000;
                    }, 0);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_Hornet_2":
                running = true;

                GameObject HiveKnight = SpawnBoss("hiveknight", new Vector2(30.0f, 36.0f));
                GameObject HornetSentinel = GameObject.Find("Boss Holder/Hornet Boss 2");
                HornetSentinel.LocateMyFSM("Control").Fsm.GetFsmBool("Can Barb").Value = true;
                HornetSentinel
                    .LocateMyFSM("Control")
                    .InsertCustomAction("Init", () =>
                    {
                        HiveKnight.SetActive(true);
                        new[] { HornetSentinel, HiveKnight }
                        .ShareHealth(name: "stinger knights").HP = 1600;
                    }, 0);
                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_God_Tamer":
                //God Tamer + Obblelobles - Done??
                running = true;
                GameObject Lobster = GameObject.Find("Lobster");
                GameObject Oblobble = SpawnBoss("oblobble", new Vector2(90.0f, 10.0f));
                GameObject Rageblobble = SpawnBoss("oblobble", new Vector2(100.0f, 10.0f));
                PantheonOfRegions.InstaBoss["oblobble"] = Oblobble;
                PantheonOfRegions.InstaBoss["rageblobble"] = Rageblobble;
                Lobster.LocateMyFSM("Control").AddCustomAction("Init", () =>
                {
                    Oblobble.SetActive(true);
                    SharedHealthManager Champions = new[] { Lobster, Oblobble, Rageblobble }.ShareHealth(name: "colosseum");
                    Champions.HP = 1400;
                    PantheonOfRegions.InstaBoss["colosseum"] = Champions.gameObject;
                    Lobster.AddComponent<EnemyTracker>();
                });

                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_Collector":
                //Collector + Watcher knights
                running = true;
                GameObject Collector = GameObject.Find("Battle Scene/Jar Collector");
                SharedHealthManager citycollector = new[] { Collector }.ShareHealth(name: "citycollector");
                citycollector.HP = 1000;
                PantheonOfRegions.InstaBoss["collector"] = citycollector.gameObject;
                Collector.AddComponent<EnemyTracker>();

                break;

            ////////////////////////////////////////////////////////////////////////////////////////////////////

            case "GG_Gruz_Mother":
                //Gruz + Sly
                running = true;
                GameObject GruzMother = GameObject.Find("_Enemies/Giant Fly");
                GameObject Sly = Spawner.SpawnBoss("greatnailsagesly", new Vector2(98.0f, 15.0f));
                PantheonOfRegions.InstaBoss["sly"] = Sly;
                GruzMother.AddComponent<EnemyTracker>();
                Sly.SetActive(true);
                GruzMother.LocateMyFSM("Big Fly Control").InsertCustomAction("Wake", () =>
                {
                    SharedHealthManager flylords = new[] { GruzMother , Sly }.ShareHealth(name: "fly lords");
                    flylords.HP = 1800;
                    PantheonOfRegions.InstaBoss["flylords"] = flylords.gameObject;
                    
                }, 0);

                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Grimm_Nightmare":
                //NKG + Zote
                running = true;
                GameObject NKG = GameObject.Find("Grimm Control/Nightmare Grimm Boss");
                GameObject Zote = Spawner.SpawnBoss("greyprincezote", new Vector2(90.0f, 10.0f));
                Zote.SetActive(true);
                PantheonOfRegions.InstaBoss["greyprincezote"] = Zote;
                for (int i = 0; i < 3; i++)
                {
                    GameObject balloon = Spawner.SpawnBoss("volatilezoteling", new Vector2(70.0f + 15f * i, 10.0f));
                    balloon.SetActive(true);
                }
                SharedHealthManager nightmares = new[] { NKG, Zote }.ShareHealth(name: "reapers");
                nightmares.HP = 2000;
                PantheonOfRegions.InstaBoss["reapers"] = nightmares.gameObject;
                NKG.AddComponent<EnemyTracker>();



                break;
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            case "GG_Hollow_Knight":
                //PV + Lost Kin

                running = true;
                GameObject LostKin = SpawnBoss("lostkin", new Vector2(35.0f, 20.0f));
                GameObject PureVessel = GameObject.Find("Battle Scene/HK Prime");
                PantheonOfRegions.InstaBoss["lostkin"] = LostKin;
                PantheonOfRegions.InstaBoss["purevessel"] = PureVessel;
                PureVessel.AddComponent<EnemyTracker>();

                PureVessel
                    .LocateMyFSM("Control")
                    .InsertCustomAction("Intro 4", () =>
                    {
                        LostKin!.SetActive(true);
                        new[] { PureVessel, LostKin }
                        .ShareHealth(name: "void vessels").HP = 2000;
                    }, 1);

                break;

            case "GG_Radiance":
                //Absrad + Markoth + Seer
                running = true;
                GameObject Markoth = SpawnBoss("markoth", new Vector2(55.0f, 30.0f));
                GameObject Seer = GameObject.Instantiate(PantheonOfRegions.GameObjects["noeyes"], new Vector3(68f, 30f, 0f), Quaternion.identity);
                Seer.AddComponent<Seer>();
                Seer.SetActive(false);
                PlayMakerFSM BattleScene = GameObject.Find("Boss Control").LocateMyFSM("Control");
                PantheonOfRegions.InstaBoss["markoth"] = Markoth;
                PantheonOfRegions.InstaBoss["seer"] = Seer;

                BattleScene.AddCustomAction("Appear Boom", () =>
                { 
                    Markoth.SetActive(true);
                    Seer.SetActive(true);
                });
                BattleScene.AddCustomAction("Battle Start", () =>
                {
                    GameObject AbsoluteRadiance = GameObject.Find("Boss Control/Absolute Radiance");
                    SharedHealthManager moths = new[] { AbsoluteRadiance, Markoth }.ShareHealth(name: "moths");
                    PantheonOfRegions.InstaBoss["radiance"] = AbsoluteRadiance;
                    PantheonOfRegions.InstaBoss["moths"] = moths.gameObject;
                    moths.HP = 3600;
                    AbsoluteRadiance.AddComponent<AbsoluteRadiance>().enabled = true;
                    Seer.LocateMyFSM("Movement").SetState("Choose Target");

                });
                break;

            case "GG_Workshop":
                /*
                GameObject Seer2 = GameObject.Instantiate(PantheonOfRegions.GameObjects["noeyes"], new Vector3(11.0f, 37.0f, 0f), Quaternion.identity);
                GameObject.DontDestroyOnLoad(Seer2);
                Seer2.AddComponent<Seer>();
                Seer2.SetActive(true);
                PantheonOfRegions.InstaBoss["seer"] = Seer2;
                var playeritem = HeroController.instance.gameObject.GetComponent<BoxCollider2D>();
                playeritem.gameObject.AddComponent<TriggerDetect>();  */
                //GameObject Noskstatue = GameObject.Find("GG_Statue_Nosk");
                PantheonCleanup();

                break;

            default:
                running = false;
                return;
        }
        Inactive:
            running = false;
            return;

    }

    private static void PantheonCleanup()
    {
        if (loadedboss != null)
        {
            foreach (GameObject boss in loadedboss) { GameObject.Destroy(boss); }
        }
    }
}
