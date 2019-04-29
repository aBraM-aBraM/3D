﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace _3D
{
	public partial class Form1 : Form
	{
		Pen p = new Pen(new SolidBrush(Color.Black));
		Graphics g;


		public Form1()
		{
			InitializeComponent();
			this.Paint += new System.Windows.Forms.PaintEventHandler(Form1_Paint);
			
		}



		private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			g = e.Graphics;
			Engine engine = new Engine(g,this);
		}
	}

}
