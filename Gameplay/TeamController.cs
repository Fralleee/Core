using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.Core
{
	public class TeamController : MonoBehaviour
	{
		#region STATICS
		static readonly int RendererColor = Shader.PropertyToID("_BaseColor");

		static string DEFAULT = "Default";

		static string TEAM1 = "Team1";
		static string TEAM1SENSOR = "Team1 Sensor";
		static string TEAM1HITBOXES = "Team1 Hitboxes";
		static string TEAM1PROJECTILES = "Team1 Projectiles";

		static string TEAM2 = "Team2";
		static string TEAM2SENSOR = "Team2 Sensor";
		static string TEAM2HITBOXES = "Team2 Hitboxes";
		static string TEAM2PROJECTILES = "Team2 Projectiles";

		static string NPC = "NPC";
		static string NPCSENSOR = "NPC Sensor";
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
		[Readonly] public int AllySensor;
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

			var eyes = transform.FindChildWithTag("Eyes");
			if (eyes)
				eyes.gameObject.layer = AllySensor;
		}

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
			AllySensor = LayerMask.NameToLayer(TEAM1SENSOR);
			Hitbox = LayerMask.NameToLayer(TEAM1HITBOXES);
			AllyProjectiles = LayerMask.NameToLayer(TEAM1PROJECTILES);
			Hostiles = 1 << LayerMask.NameToLayer(TEAM2);
			HostileProjectiles = (1 << LayerMask.NameToLayer(TEAM2PROJECTILES)) | (1 << LayerMask.NameToLayer(NPCPROJECTILES));
			Neutrals = 1 << LayerMask.NameToLayer(NPC);
			Hitboxes = (1 << LayerMask.NameToLayer(TEAM2HITBOXES)) | (1 << LayerMask.NameToLayer(NPCHITBOXES));
			AttackLayerMask = (1 << LayerMask.NameToLayer(DEFAULT)) | (1 << LayerMask.NameToLayer(TEAM2HITBOXES)) | (1 << LayerMask.NameToLayer(NPCHITBOXES));

			propBlock.SetColor(RendererColor, team1Color);
			foreach (var r in renderers)
				r.SetPropertyBlock(propBlock);
		}

		void SetupTeam2()
		{
			SetupRenderer();

			AllyTeam = LayerMask.NameToLayer(TEAM2);
			AllySensor = LayerMask.NameToLayer(TEAM2SENSOR);
			Hitbox = LayerMask.NameToLayer(TEAM2HITBOXES);
			AllyProjectiles = LayerMask.NameToLayer(TEAM2PROJECTILES);
			Hostiles = 1 << LayerMask.NameToLayer(TEAM1);
			HostileProjectiles = (1 << LayerMask.NameToLayer(TEAM1PROJECTILES)) | (1 << LayerMask.NameToLayer(NPCPROJECTILES));
			Neutrals = 1 << LayerMask.NameToLayer(NPC);
			Hitboxes = (1 << LayerMask.NameToLayer(TEAM1HITBOXES)) | (1 << LayerMask.NameToLayer(NPCHITBOXES));
			AttackLayerMask = (1 << LayerMask.NameToLayer(DEFAULT)) | (1 << LayerMask.NameToLayer(TEAM1HITBOXES)) | (1 << LayerMask.NameToLayer(NPCHITBOXES));

			propBlock.SetColor(RendererColor, team2Color);
			foreach (var r in renderers)
				r.SetPropertyBlock(propBlock);
		}
	}
}
