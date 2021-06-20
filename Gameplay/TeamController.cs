using Fralle.Core.Attributes;
using UnityEngine;

namespace Fralle.Core
{
	public class TeamController : MonoBehaviour
	{
		#region STATICS
		static string DEFAULT = "DEFAULT";

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

		[Header("Layer map")]
		[Readonly] public int Self;
		[Readonly] public int Hitbox;
		public LayerMask Hostiles;
		public LayerMask Neutrals;
		public LayerMask AllyProjectiles;
		public LayerMask HostileProjectiles;
		public LayerMask Hitboxes;
		public LayerMask AttackLayerMask;

		[ContextMenu("Setup")]
		public void Setup()
		{
			if (team == Team.Team1)
				SetupTeam1();
			else if (team == Team.Team2)
				SetupTeam2();

			foreach (var col in hitboxParent.GetComponentsInChildren<Collider>())
				col.gameObject.layer = Hitbox;

			targetCollider.gameObject.layer = Self;
		}

		void SetupTeam1()
		{
			Self = LayerMask.NameToLayer(TEAM1);
			Hitbox = LayerMask.NameToLayer(TEAM1HITBOXES);
			AllyProjectiles = 1 << LayerMask.NameToLayer(TEAM1PROJECTILES);
			Hostiles = 1 << LayerMask.NameToLayer(TEAM2);
			HostileProjectiles = (1 << LayerMask.NameToLayer(TEAM2PROJECTILES)) | (1 << LayerMask.NameToLayer(NPCPROJECTILES));
			Neutrals = 1 << LayerMask.NameToLayer(NPC);
			Hitboxes = (1 << LayerMask.NameToLayer(TEAM2HITBOXES)) | (1 << LayerMask.NameToLayer(NPCHITBOXES));
			AttackLayerMask = (1 << LayerMask.NameToLayer(DEFAULT)) | (1 << LayerMask.NameToLayer(TEAM2HITBOXES)) | (1 << LayerMask.NameToLayer(NPCHITBOXES));
		}

		void SetupTeam2()
		{
			Self = LayerMask.NameToLayer(TEAM2);
			Hitbox = LayerMask.NameToLayer(TEAM2HITBOXES);
			AllyProjectiles = 1 << LayerMask.NameToLayer(TEAM2PROJECTILES);
			Hostiles = 1 << LayerMask.NameToLayer(TEAM1);
			HostileProjectiles = (1 << LayerMask.NameToLayer(TEAM1PROJECTILES)) | (1 << LayerMask.NameToLayer(NPCPROJECTILES));
			Neutrals = 1 << LayerMask.NameToLayer(NPC);
			Hitboxes = (1 << LayerMask.NameToLayer(TEAM1HITBOXES)) | (1 << LayerMask.NameToLayer(NPCHITBOXES));
			AttackLayerMask = (1 << LayerMask.NameToLayer(DEFAULT)) | (1 << LayerMask.NameToLayer(TEAM1HITBOXES)) | (1 << LayerMask.NameToLayer(NPCHITBOXES));
		}
	}
}
