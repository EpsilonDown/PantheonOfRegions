
namespace PantheonOfRegions;
public class BossSpawner : MonoBehaviour
{
    public GameObject SpawnBoss(string Boss, Vector2 spawnPoint)
    {
        GameObject boss = Instantiate(PantheonOfRegions.GameObjects[Boss], spawnPoint, Quaternion.identity);
        boss.AddComponent<EnemyTracker>();
        boss.SetActive(false);
        var hm = boss.GetComponent<HealthManager>();
        hm.SetGeoSmall(0);
        hm.SetGeoMedium(0);
        hm.SetGeoLarge(0);

        return boss;
    }
}
public class Spritebuilder : MonoBehaviour
{
    public void ApplyTextureToTk2dSprite(GameObject target, Texture2D texture)
    {
        tk2dSprite sprite = target.GetComponent<tk2dSprite>();
        sprite!.CurrentSprite.material.mainTexture = texture;
        // Create a new material with the loaded texture
        Material newMaterial = new Material(Shader.Find("tk2d/BlendVertexColor"))
        {
            mainTexture = texture
        };
        // Apply the material to the tk2dSprite
        sprite.GetComponent<Renderer>().material = newMaterial;
        sprite.ForceBuild();
    }
}
