using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.Core
{
  public class TeamController : MonoBehaviour
  {
    #region STATICS
    static readonly int RendererColor = Shader.PropertyToID("_BaseColor");

    static string DEFAULT = "Default";
    static string CORPSE = "Corpse";

    static string TEAM1 = "Team1";
    static string TEAM1HITBOXES = "Team1 Hitboxes";
    static string TEAM1PROJECTILES = "Team1 Projectiles";

    static string TEAM2 = "Team2";
    static string TEAM2HITBOXES = "Team2 Hitboxes";
    static string TEAM2PROJECTILES = "Team2 Projectiles";

    static string NPC = "NPC";
    static string NPCHITBOXES = "NPC Hitboxes";
    static string NPCPROJECTILES = "NPC Projectiles";
    #endregion

    [Header("Setup")]
    public Team team;
    [SerializeField] Collider targetCollider;
    [SerializeField] Transform hitboxParent;
    [SerializeField] Color team1Color = new Color(1, 0.5f, 0.5f);
    [SerializeField] Color team2Color = new Color(0.5f, 0.5f, 1);

    [Header("Layer map")]
    [Readonly] public int AllyTeam;
    [Readonly] public int Hitbox;
    [Readonly] public int AllyProjectiles;
    public LayerMask Hostiles;
    public LayerMask Neutrals;
    public LayerMask HostileProjectiles;
    public LayerMask Hitboxes;
    public LayerMask AttackLayerMask;

    SkinnedMeshRenderer[] renderers;
    MaterialPropertyBlock propBlock;

    [ContextMenu("Setup")]
    public void Setup()
    {
      if (team == Team.Team1)
        SetupTeam1();
      else if (team == Team.Team2)
        SetupTeam2();

      foreach (var col in hitboxParent.GetComponentsInChildren<Collider>())
        col.gameObject.layer = Hitbox;

      targetCollider.gameObject.layer = AllyTeam;
    }

    public bool CheckIfHostile(GameObject target) => Hostiles.IsInLayerMask(target.layer);

    void SetupRenderer()
    {
      propBlock = new MaterialPropertyBlock();
      renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
      foreach (var r in renderers)
        r.GetPropertyBlock(propBlock);
    }

    void SetupTeam1()
    {
      SetupRenderer();

      AllyTeam = LayerMask.NameToLayer(TEAM1);
      Hitbox = LayerMask.NameToLayer(TEAM1HITBOXES);
      AllyProjectiles = LayerMask.NameToLayer(TEAM1PROJECTILES);

      Hostiles = LayerMask.GetMask(TEAM2);
      HostileProjectiles = LayerMask.GetMask(TEAM2PROJECTILES, NPCPROJECTILES);
      Neutrals = LayerMask.GetMask(NPC);
      Hitboxes = LayerMask.GetMask(TEAM2HITBOXES, NPCHITBOXES);
      AttackLayerMask = LayerMask.GetMask(DEFAULT, TEAM2HITBOXES, NPCHITBOXES, CORPSE);

      propBlock.SetColor(RendererColor, team1Color);
      foreach (var r in renderers)
        r.SetPropertyBlock(propBlock);
    }

    void SetupTeam2()
    {
      SetupRenderer();

      AllyTeam = LayerMask.NameToLayer(TEAM2);
      Hitbox = LayerMask.NameToLayer(TEAM2HITBOXES);
      AllyProjectiles = LayerMask.NameToLayer(TEAM2PROJECTILES);

      Hostiles = LayerMask.GetMask(TEAM1);
      HostileProjectiles = LayerMask.GetMask(TEAM1PROJECTILES, NPCPROJECTILES);
      Neutrals = LayerMask.GetMask(NPC);
      Hitboxes = LayerMask.GetMask(TEAM1HITBOXES, NPCHITBOXES);
      AttackLayerMask = LayerMask.GetMask(DEFAULT, TEAM1HITBOXES, NPCHITBOXES, CORPSE);

      propBlock.SetColor(RendererColor, team2Color);
      foreach (var r in renderers)
        r.SetPropertyBlock(propBlock);
    }
  }
}
