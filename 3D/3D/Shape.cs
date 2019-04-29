using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace _3D
{
	class Shape
	{
		public readonly Corner[] vertices;

		public Shape(params Corner[] vertices)
		{
			this.vertices = vertices;
		}
	}
	class Corner
	{
		public Vector3 center;
		public Vector3[] connections;

		public Corner(Vector3 point)
		{
			this.center = point;
		}
		public Corner(float x, float y, float z)
		{
			center = new Vector3(x, y, z);
		}

		public Corner(Vector3 point, params Vector3[] connections)
		{
			this.center = point;
			this.connections = connections;
		}
		public Corner(float x, float y, float z, params Vector3[] connections)
		{
			center = new Vector3(x, y, z);
			this.connections = connections;
		}

		public void Connect(params Vector3[] connections)
		{
			this.connections = connections;
		}

		public void AddConnection(Vector3 vertex)
		{
			if(connections == null)
			{
				connections = new Vector3[1];
				connections[0] = vertex;
			}
			else
			{
				Vector3[] tmp = new Vector3[connections.Length + 1];
				for (int i = 0; i < connections.Length; i++)
				{
					tmp[i] = connections[i];
				}
				tmp[tmp.Length - 1] = vertex;
				connections = tmp;
			}
		}


		public void IncVector(Vector3 vector)
		{
			center = center + vector;
		}
		

	}
}
