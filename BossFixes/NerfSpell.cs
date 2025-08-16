/* broken, not used
namespace PantheonOfRegions.Behaviours
{
    internal class SpellNerf : MonoBehaviour
    {

        private void Start()
        {
            On.HealthManager.TakeDamage += SoulNerf;
        }
        private void SoulNerf(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            orig(self, hitInstance);
            switch (hitInstance.AttackType)
            {
                case AttackTypes.Nail:
                case AttackTypes.NailBeam:
                    if (hitInstance.AttackType == AttackTypes.Nail)
                    {
                        HeroController.instance.playerData.TakeMP(2);
                        break;
                    }
                    break;
            }
        }
    }
} */
