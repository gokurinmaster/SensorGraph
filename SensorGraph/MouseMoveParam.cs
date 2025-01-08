using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorGraph
{
	public class MouseMoveParam
	{
		public bool IsMouseDown { get; set; } = false;
		public Point MouseDownPos { get; set; } = new Point();

		public double MouseDownXMax { get; set; } 
		public double MouseDownXMin { get; set; }
		public double MouseDownYMax { get; set; }
		public double MouseDownYMin { get; set; }

		public double RateX { get; set; } 
		public double RateY { get; set; }
	}
}
