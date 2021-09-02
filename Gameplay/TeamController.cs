using Sirenix.OdinInspector;
using UnityEngine;

namespace Fralle.Core
{
  public class TeamController : MonoBehaviour
  {
    #region STATICS
    static readonly int RendererColor = Shader.PropertyToID("_BaseColor");

    static string Default = "Default";
    static string Corpse = "Corpse";

    static string Team1 = "Team1";
    static string Team1Hitboxes = "Team1 Hitboxes";
    static string Team1Projectiles = "Team1 Projectiles";

    static string Team2 = "Team2";
    static string Team2Hitboxes = "Team2 Hitboxes";
    static string Team2Projectiles = "Team2 Projectiles";

    static string Npc = "NPC";
    static string Npchitboxes = "NPC Hitboxes";
    static string Npcprojectiles = "NPC Projectiles";
    #endregion

    [Header("Setup")]
    public Team team;
    [SerializeField] Collider targetCollider;
    [SerializeField] Transform hitboxParent;
    [SerializeField] Color team1Color = new Color(1, 0.5f, 0.5f);
    [SerializeField] Color team2Color = new Color(0.5f, 0.5f, 1);

    [Header("Layer map")]
    [ReadOnly] public int allyTeam;
    [ReadOnly] public int hitbox;
    [ReadOnly] public int allyProjectiles;
    public LayerMask hostiles;
    public LayerMask neutrals;
    public LayerMask hostileProjectiles;
    public LayerMask hitboxes;
    public LayerMask attackLayerMask;

    SkinnedMeshRenderer[] renderers;
    MaterialPropertyBlock propBlock;

    [ContextMenu("Setup")]
    public void Setup()
    {
      switch (team)
      {
        case Team.Team1:
          SetupTeam1();
          break;
        case Team.Team2:
          SetupTeam2();
          break;
      }

      foreach (Collider col in hitboxParent.GetComponentsInChildren<Collider>())
        col.gameObject.layer = hitbox;

      targetCollider.gameObject.layer = allyTeam;
    }

    public bool CheckIfHostile(GameObject target) => hostiles.IsInLayerMask(target.layer);

    void SetupRenderer()
    {
      propBlock = new MaterialPropertyBlock();
      renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
      foreach (SkinnedMeshRenderer r in renderers)
        r.GetPropertyBlock(propBlock);
    }

    void SetupTeam1()
    {
      SetupRenderer();

      allyTeam = LayerMask.NameToLayer(Team1);
      hitbox = LayerMask.NameToLayer(Team1Hitboxes);
      allyProjectiles = LayerMask.NameToLayer(Team1Projectiles);

      hostiles = LayerMask.GetMask(Team2);
      hostileProjectiles = LayerMask.GetMask(Team2Projectiles, Npcprojectiles);
      neutrals = LayerMask.GetMask(Npc);
      hitboxes = LayerMask.GetMask(Team2Hitboxes, Npchitboxes);
      attackLayerMask = LayerMask.GetMask(Default, Team2Hitboxes, Npchitboxes, Corpse);

      propBlock.SetColor(RendererColor, team1Color);
      foreach (SkinnedMeshRenderer r in renderers)
        r.SetPropertyBlock(propBlock);
    }

    void SetupTeam2()
    {
      SetupRenderer();

      allyTeam = LayerMask.NameToLayer(Team2);
      hitbox = LayerMask.NameToLayer(Team2Hitboxes);
      allyProjectiles = LayerMask.NameToLayer(Team2Projectiles);

      hostiles = LayerMask.GetMask(Team1);
      hostileProjectiles = LayerMask.GetMask(Team1Projectiles, Npcprojectiles);
      neutrals = LayerMask.GetMask(Npc);
      hitboxes = LayerMask.GetMask(Team1Hitboxes, Npchitboxes);
      attackLayerMask = LayerMask.GetMask(Default, Team1Hitboxes, Npchitboxes, Corpse);

      propBlock.SetColor(RendererColor, team2Color);
      foreach (SkinnedMeshRenderer r in renderers)
        r.SetPropertyBlock(propBlock);
    }
  }
}
