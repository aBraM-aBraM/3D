using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;


namespace _3D
{
	class Engine
	{
		Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));

		System.Threading.Thread t;

		Form1 f;

		Graphics graphics;

		Shape shape;

		public Engine(Graphics gfx,Form1 f)
		{
			this.f = f;
			graphics = gfx;
			pen.Width = 2f;
			Start();
			t = new System.Threading.Thread(Update);
			t.Start();
		}

		private void Start()
		{
			InitCube();
			DrawBackground();
			UpdateScreen();
		}



		public void Update()
		{
				{
					MethodInvoker mi = delegate () { f.Text = DateTime.Now.ToString(); };
					f.Invoke(mi);
				}
		}

		private void InitCube()
		{
			int amp = 20;
			Vector3[] vertices = new Vector3[] { new Vector3(3, 3, 3) * amp, new Vector3(6, 3, 3)*amp, new Vector3(3, 6, 3) * amp, new Vector3(6, 6, 3) * amp ,Vector3.zero};
			Corner[] corners = new Corner[5];
			corners[0] = new Corner(vertices[0], vertices[1], vertices[2]);
			corners[1] = new Corner(vertices[1], vertices[0], vertices[3]);
			corners[2] = new Corner(vertices[2], vertices[0], vertices[3]);
			corners[3] = new Corner(vertices[3], vertices[1], vertices[2]);
			corners[4] = new Corner(vertices[4], vertices[0], vertices[3]);
			shape = new Shape(corners);
		}

		private void DrawCube()
		{
			pen.Color = Color.Green;
			for (int i = 0; i < shape.vertices.Length; i++)
			{
				for (int j = 0; j < shape.vertices[i].connections.Length; j++)
				{
					
					graphics.DrawLine(pen, shape.vertices[i].center.GetPoint(), shape.vertices[i].connections[j].GetPoint());
				}
			}
		}

		
		private void DrawBackground()
		{
			Form1.ActiveForm.BackColor = Color.DarkGray;
			float amp = 50;
			pen.Color = Color.LightGray;
			for (int i = 0; i < 5; i++)
			{
				float length = 200f;
				float spaceAmp = 30f;
				graphics.DrawLine(pen, (Vector3.forward * i * spaceAmp).GetPoint(), (Vector3.forward * i * spaceAmp + Vector3.right * length).GetPoint());
				graphics.DrawLine(pen, (Vector3.right * i * spaceAmp).GetPoint(), (Vector3.right * i * spaceAmp + Vector3.forward * length).GetPoint());
			}
			pen.Color = Color.Red;
			graphics.DrawLine(pen, (Vector3.zero * amp).GetPoint(), (Vector3.up * amp).GetPoint());
			pen.Color = Color.Blue;
			graphics.DrawLine(pen, (Vector3.zero * amp).GetPoint(), (Vector3.right * amp).GetPoint());
			pen.Color = Color.Yellow;
			graphics.DrawLine(pen, (Vector3.zero * amp).GetPoint(), (Vector3.forward * amp).GetPoint());
		}
		private void UpdateScreen()
		{
			graphics.Clear(f.BackColor);
			DrawBackground();
			DrawCube();
			//Brush b = new SolidBrush(Color.Red);
		}
	}
}
