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

		Corner chosenCorner;
		int index;

		bool isChoosing = true;

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
			index = 0;
			DrawBackground();
			InitShape();
			UpdateScreen();
		}

		public void HandleInput(object sender,KeyEventArgs e)
		{
			
			if(chosenCorner == null)
			{
				chosenCorner= shape.vertices[index];
			}
			switch (e.KeyCode)
			{
				case Keys.Right:
					if (isChoosing)
					{
						if (index != shape.vertices.Length - 1)
						{
							chosenCorner = shape.vertices[index + 1];
						}
						else
						{
							index = 0;
							chosenCorner = shape.vertices[index];
						}
					}
					if (!isChoosing)
					{
						chosenCorner.center += Vector3.forward;
					}
					break;
				case Keys.Left:
					if (isChoosing)
					{
						if (index != 0)
						{
							chosenCorner = shape.vertices[index - 1];
						}
						else
						{
							index = shape.vertices.Length - 1;
							chosenCorner = shape.vertices[index];
						}
					}
					if (!isChoosing)
					{
						chosenCorner.center -= Vector3.forward;
					}
					break;
				case Keys.Up:
					if (!isChoosing)
					{
						chosenCorner.center += Vector3.right;
					}
					break;
				case Keys.Down:
					if (!isChoosing)
					{
						chosenCorner.center -= Vector3.right;
					}
					break;
				case Keys.W:
					if (!isChoosing)
					{
						chosenCorner.center += Vector3.up;
					}
					break;
				case Keys.S:
					if (!isChoosing)
					{
						chosenCorner.center -= Vector3.up;
					}
					break;
				case Keys.Enter:
					chosenCorner = null;
					isChoosing = !isChoosing;
					break;
			}
			//UpdateScreen();
			pen.Color = Color.Orange;
			graphics.DrawEllipse(pen,chosenCorner.center.GetPoint().X,chosenCorner.center.GetPoint().Y, 2, 2);
		}

		public void Update()
		{
				{
					MethodInvoker mi = delegate () { f.Text = DateTime.Now.ToString(); };
					f.Invoke(mi);
				}
		}

		private void InitShape()
		{
			//shape = CreateBox(new Vector3(50, 50, 50), 50);
			shape = CreatePolygon(new Vector3(40, 40, 40), 3,50, 50);
		}
		
		private Shape CreateBox(Vector3 startPoint,float edgeSize)
		{
			Corner[] corners = new Corner[8];
			Vector3[] vertices = new Vector3[8];
			for (int i = 0; i < 4; i++)
			{
				switch (i)
				{
					case 0:
						vertices[i] = startPoint;
						vertices[vertices.Length - i - 1] = startPoint + new Vector3(0, edgeSize, 0);
						break;
					case 1:
						vertices[i] = startPoint + new Vector3(edgeSize, 0, 0);
						vertices[vertices.Length - i - 1] = vertices[i] + new Vector3(0, edgeSize, 0);
						break;
					case 2:
						vertices[i] = startPoint + new Vector3(0, 0, edgeSize);
						vertices[vertices.Length - i - 1] = vertices[i] + new Vector3(0, edgeSize, 0);
						break;
					case 3:
						vertices[i] = startPoint + new Vector3(edgeSize, 0, edgeSize);
						vertices[vertices.Length - i - 1] = vertices[i] + new Vector3(0, edgeSize, 0);
						break;
				}
			}
			for (int i = 0; i < 8; i++)
			{
				corners[i] = new Corner(vertices[i]);
				for (int j = 0; j < vertices.Length; j++)
				{
					if(vertices[j].Distance(corners[i].center) == edgeSize)
					{
						corners[i].AddConnection(vertices[j]);
					}
				}
			}
			return new Shape(corners);
		}
		private Shape CreatePolygon(Vector3 startPoint,int numOfVertices, float radius, float height, float offsetDegrees = 0)
		{
			Vector3[] vertices = new Vector3[numOfVertices * 2];
			Corner[] corners = new Corner[numOfVertices * 2];
			for (int i = 0; i < numOfVertices; i++)
			{
				vertices[i] = startPoint + new Vector3((float)Math.Cos(Math.PI * 2 / (float)numOfVertices * i + offsetDegrees*Math.PI/180) * radius, 0, (float)Math.Sin(Math.PI * 2 / (float)numOfVertices * i + offsetDegrees*Math.PI/180) * radius);
			}
			for (int i = 0; i < numOfVertices; i++)
			{
				vertices[i + numOfVertices] = vertices[i] + new Vector3(0, height, 0);
			}
			for (int i = 0; i < corners.Length; i++)
			{
				corners[i] = new Corner(vertices[i]);
				for (int j = 0; j < vertices.Length; j++)
				{
					float edge = (float)Math.Sqrt(2 * radius * radius - 2 * radius * radius * Math.Cos(2 * Math.PI / numOfVertices));
					if(vertices[j].Distance(corners[i].center) == radius || vertices[j].Distance(corners[i].center) == height || vertices[j].Distance(corners[i].center) == edge)
					{
						corners[i].AddConnection(vertices[j]);
					}
				}
			}
			return new Shape(corners);
		}
		private Shape RotateShape(Shape rotateShape,float degrees)
		{
			Vector3 center = Vector3.zero;
			int index = 0;
			for (index = 0; index < rotateShape.vertices.Length; index++)
			{
				center += new Vector3(rotateShape.vertices[index].center.x, rotateShape.vertices[index].center.y, rotateShape.vertices[index].center.z);
			}
			center /= index + 1;
			pen.Color = Color.Purple;
			graphics.DrawEllipse(pen, center.GetPoint().X, center.GetPoint().Y, 1, 1);
			for (int i = 0; i < rotateShape.vertices.Length; i++)
			{
				float relativeX = rotateShape.vertices[i].center.x - center.x; // cosine
				float relativeZ = rotateShape.vertices[i].center.z - center.z; // sine
				float radius = (float)Math.Sqrt(relativeX * relativeX + relativeZ * relativeZ);
				float currentDegree = (float)Math.Atan(relativeZ / relativeX);

				float wantedDegree = currentDegree + (float)(Math.PI / 180) * degrees;
				float wantedX = (float)Math.Cos(wantedDegree) * radius + center.x;
				float wantedZ = (float)Math.Sin(wantedDegree) * radius + center.z;
				rotateShape.vertices[i].center = new Vector3(wantedX, rotateShape.vertices[i].center.y, wantedZ);
			}
			return shape;
		}
		private void DrawShape(Shape drawShape)
		{
			//edges
			pen.Color = Color.Green;
			for (int i = 0; i < drawShape.vertices.Length; i++)
			{
				for (int j = 0; j < drawShape.vertices[i].connections.Length; j++)
				{				
					graphics.DrawLine(pen, drawShape.vertices[i].center.GetPoint(), drawShape.vertices[i].connections[j].GetPoint());
				}
			}

			//vertices
			pen.Color = Color.Red;
			int circleSize = 2;
			for (int i = 0; i < drawShape.vertices.Length; i++)
			{
				graphics.DrawEllipse(pen, drawShape.vertices[i].center.GetPoint().X, drawShape.vertices[i].center.GetPoint().Y, circleSize, circleSize);
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
			DrawShape(shape);
		}
	}
}
