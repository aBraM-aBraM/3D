using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace _3D
{
	public struct Vector3
	{
		public readonly float x, y, z;
		public readonly float Length;
		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			Length = (float)Math.Sqrt(x * x + y * y + z * z);
		}
		public static Vector3 zero = new Vector3(0, 0, 0);
		public static Vector3 one = new Vector3(1, 1, 1);
		public static Vector3 forward = new Vector3(1, 0, 0);
		public static Vector3 right = new Vector3(0, 0, 1);
		public static Vector3 up = new Vector3(0, 1, 0);
		public static Vector3 operator *(float amp, Vector3 vector)
		{
			return new Vector3(amp * vector.x, amp * vector.y, amp * vector.z);
		}
		public static Vector3 operator *(Vector3 vector, float amp)
		{
			return new Vector3(amp * vector.x, amp * vector.y, amp * vector.z);
		}
		public static Vector3 operator +(Vector3 firstVector,Vector3 secondVector)
		{
			return new Vector3(firstVector.x + secondVector.x, firstVector.y + secondVector.y, firstVector.z + secondVector.z);
		}

		public Point GetPoint()
		{
			int amp = 300;
			float varX = 0.6f;
			float varY = 0.9f;
			//minus on y Axis used to compensate for console's upside down y.
			return new Point(   (int)Math.Round((x*varX + z*varY)   + amp  )    , (int)Math.Round(  (-y) + -z * varX + x*varY   ) + amp);
		}
	}
}
