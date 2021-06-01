using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core
{
	[ExecuteInEditMode]
	public class AISensor : MonoBehaviour
	{
		[Header("Sensor size")]
		public float Distance = 5f;
		[Range(0, 180)] public float Angle = 45f;
		public float Height = 2f;

		[Header("Sensor configuration")]
		public int scanFrequency = 10;
		public LayerMask layers;
		public LayerMask occlusionLayers;

		public List<GameObject> Objects
		{
			get
			{
				objects.RemoveAll(obj => !obj);
				return objects;
			}
		}

		[Header("Debug")]
		public Color sensorColor = Color.yellow;
		public Color inSightColor = Color.green;
		public bool DebugSensor;
		public bool DebugTargets;

		List<GameObject> objects = new List<GameObject>();
		Collider[] colliders = new Collider[50];
		Mesh mesh;
		int count;
		float scanInterval;
		float scanTimer;

		void Start()
		{
			scanInterval = 1 / scanFrequency;
		}

		void Update()
		{
			scanTimer -= Time.deltaTime;
			if (scanTimer < 0)
			{
				scanTimer += scanInterval;
				Scan();
			}
		}

		void Scan()
		{
			count = Physics.OverlapSphereNonAlloc(transform.position, Distance, colliders, layers, QueryTriggerInteraction.Collide);

			objects.Clear();
			for (int i = 0; i < count; i++)
			{
				GameObject obj = colliders[i].gameObject;
				if (IsInSight(obj))
					objects.Add(obj);
			}
		}

		public bool IsInSight(GameObject obj)
		{
			Vector3 origin = transform.position;
			Vector3 destination = obj.transform.position;
			Vector3 direction = destination - origin;

			if (direction.y < 0 || direction.y > Height)
				return false;

			direction.y = 0;
			float deltaAngle = Vector3.Angle(direction, transform.forward);
			if (deltaAngle > Angle)
				return false;

			origin.y += Height / 2;
			destination.y = origin.y;
			if (Physics.Linecast(origin, destination, occlusionLayers))
				return false;

			return true;
		}

		Mesh CreateWedgeMesh()
		{
			Mesh mesh = new Mesh();

			int segments = 10;
			int numTriangles = (segments * 4) + 2 + 2;
			int numVertices = numTriangles * 3;

			Vector3[] vertices = new Vector3[numVertices];
			int[] triangles = new int[numVertices];

			Vector3 bottomCenter = Vector3.zero;
			Vector3 bottomLeft = Quaternion.Euler(0, -Angle, 0) * Vector3.forward * Distance;
			Vector3 bottomRight = Quaternion.Euler(0, Angle, 0) * Vector3.forward * Distance;

			Vector3 topCenter = bottomCenter + Vector3.up * Height;
			Vector3 topLeft = bottomLeft + Vector3.up * Height;
			Vector3 topRight = bottomRight + Vector3.up * Height;

			int vert = 0;

			// left
			vertices[vert++] = bottomCenter;
			vertices[vert++] = bottomLeft;
			vertices[vert++] = topLeft;

			vertices[vert++] = topLeft;
			vertices[vert++] = topCenter;
			vertices[vert++] = bottomCenter;

			// right
			vertices[vert++] = bottomCenter;
			vertices[vert++] = topCenter;
			vertices[vert++] = topRight;

			vertices[vert++] = topRight;
			vertices[vert++] = bottomRight;
			vertices[vert++] = bottomCenter;

			float currentAngle = -Angle;
			float deltaAngle = (Angle * 2) / segments;
			for (int i = 0; i < segments; i++)
			{
				bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * Distance;
				bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * Distance;
				topLeft = bottomLeft + Vector3.up * Height;
				topRight = bottomRight + Vector3.up * Height;

				// far side
				vertices[vert++] = bottomLeft;
				vertices[vert++] = bottomRight;
				vertices[vert++] = topRight;

				vertices[vert++] = topRight;
				vertices[vert++] = topLeft;
				vertices[vert++] = bottomLeft;

				// top
				vertices[vert++] = topCenter;
				vertices[vert++] = topLeft;
				vertices[vert++] = topRight;

				// bottom
				vertices[vert++] = bottomCenter;
				vertices[vert++] = bottomRight;
				vertices[vert++] = bottomLeft;

				currentAngle += deltaAngle;
			}

			for (int i = 0; i < numVertices; i++)
			{
				triangles[i] = i;
			}

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.RecalculateNormals();

			return mesh;
		}

		void OnValidate()
		{
			scanInterval = 1 / scanFrequency;
			mesh = CreateWedgeMesh();
		}

		void OnDrawGizmos()
		{
			if (DebugSensor)
			{
				Gizmos.color = sensorColor;
				Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
				Gizmos.DrawWireSphere(transform.position, Distance);
			}

			if (DebugTargets)
			{
				Gizmos.color = inSightColor;
				foreach (var obj in Objects)
					Gizmos.DrawSphere(obj.transform.position, 0.5f);
			}
		}
	}
}
