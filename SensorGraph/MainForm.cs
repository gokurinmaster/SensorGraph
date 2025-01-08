using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace SensorGraph
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			SetupChart();

			if (!bgwReadFile.IsBusy)
			{
				bgwReadFile.RunWorkerAsync();
			}
		}

		private int AxisUnitX = 6;

		private string _logText = string.Empty;

		private void SetupChart()
		{
			ControlDoubleBuffered(chartSensor, true);

			var today = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd 00:00:00"));
			foreach (var item in chartSensor.ChartAreas)
			{
				item.AxisX.LabelStyle.Format = "MM/dd HH:mm";
				item.AxisX.IsMarginVisible = false;

				item.InnerPlotPosition.Auto = false;
				item.InnerPlotPosition.X = 8.0f;
				item.InnerPlotPosition.Y = 4.0f;
				item.InnerPlotPosition.Width = 88.0f;
				item.InnerPlotPosition.Height = 80.0f;

				item.AxisX.Minimum = today.ToOADate();
				item.AxisX.Maximum = today.AddHours(24).ToOADate();
			}
			/*
			//chartSensor.ChartAreas[0].AxisY.Minimum = 10;
			//chartSensor.ChartAreas[0].AxisY.Maximum = 40;
			//chartSensor.ChartAreas[0].AxisY.Interval = 5;
			//chartSensor.ChartAreas[1].AxisY.Minimum = 10;
			//chartSensor.ChartAreas[1].AxisY.Maximum = 80;
			//chartSensor.ChartAreas[1].AxisY.Interval = 10;
			//chartSensor.ChartAreas[2].AxisY.Minimum = 990;
			//chartSensor.ChartAreas[2].AxisY.Maximum = 1030;
			//chartSensor.ChartAreas[2].AxisY.Interval = 10;
			*/

			foreach (var item in chartSensor.Series)
			{
				item.ChartType = SeriesChartType.Line;
				item.XValueType = ChartValueType.DateTime;
				item.BorderWidth = 2;
			}

			chartSensor.MouseWheel += new MouseEventHandler(this.ChartSensor_MouseWheel);
		}

		private void ChartSensor_MouseWheel(object sender, MouseEventArgs e)
		{
			if (IsMouseInChartArea())
			{
				Point mouse = chartSensor.PointToClient(System.Windows.Forms.Cursor.Position);

				AxisUnitX = e.Delta > 0 ? AxisUnitX - 1 : AxisUnitX + 1;
				if (AxisUnitX <= 0)
				{
					AxisUnitX = 1;
				}

				foreach (var item in chartSensor.ChartAreas)
				{
					item.AxisX.Maximum = item.AxisX.Minimum + AxisUnitX / 6.0;
				}
			}
		}

		private void UpdateYRange()
		{
			foreach (var item in chartSensor.Series)
			{
				var xMin = chartSensor.ChartAreas[item.ChartArea].AxisX.Minimum;
				var xMax = chartSensor.ChartAreas[item.ChartArea].AxisX.Maximum;
				var data = item.Points/*.Where(o => xMin <= o.XValue && o.XValue <= xMax)*/;
				var min = data.Select(o => o.YValues[0]).Min();
				var max = data.Select(o => o.YValues[0]).Max();

				int yMin = -1;
				int yMax = -1;
				for (int i = 0; i < 1500; i+=2)
				{
					if (yMin == -1 && min < i)
					{
						yMin = i - 2;
					}
					if (yMax == -1 && max < i)
					{
						yMax = i;
						break;
					}
				}

				chartSensor.ChartAreas[item.ChartArea].AxisY.Minimum = yMin;
				chartSensor.ChartAreas[item.ChartArea].AxisY.Maximum = yMax;

				bool search = true;
				while (search)
				{
					for (int i = 8; i > 3; i--)
					{
						if ((yMax - yMin) % i == 0)
						{
							chartSensor.ChartAreas[item.ChartArea].AxisY.Interval = (yMax - yMin) / i;
							chartSensor.ChartAreas[item.ChartArea].AxisY.Maximum = yMax;
							search = false;
							break;
						}
					}
					yMax++;
				}
			}
		}

		private void UpdateChart()
		{
			// ファイル読み込み
			var lines = _logText.Split('\n');
			var data = lines.Where(o => o.Split(',').Length == 4);

			if (!data.Any())
			{
				return;
			}

			List<(DateTime time, double temp)> dicTemp = new List<(DateTime time, double temp)>();
			List<(DateTime time, double humid)> dicHumid = new List<(DateTime time, double humid)>();
			List<(DateTime time, double press)> dicPress = new List<(DateTime time, double press)>();

			dicTemp.AddRange(data.Select(o => (DateTime.Parse(o.Split(',')[0]), double.Parse(o.Split(',')[1]))));
			dicHumid.AddRange(data.Select(o => (DateTime.Parse(o.Split(',')[0]), double.Parse(o.Split(',')[2]))));
			dicPress.AddRange(data.Select(o => (DateTime.Parse(o.Split(',')[0]), double.Parse(o.Split(',')[3]))));

			foreach (var item in chartSensor.Series)
			{
				item.Points.Clear();
			}

			foreach (var item in dicTemp)
			{
				chartSensor.Series[0].Points.AddXY(item.time.ToOADate(), item.temp);
			}
			foreach (var item in dicHumid)
			{
				chartSensor.Series[1].Points.AddXY(item.time.ToOADate(), item.humid);
			}
			foreach (var item in dicPress)
			{
				chartSensor.Series[2].Points.AddXY(item.time.ToOADate(), item.press);
			}

			UpdateYRange();
		}

		private void timerUpdate_Tick(object sender, EventArgs e)
		{
			if (!bgwReadFile.IsBusy)
			{
				bgwReadFile.RunWorkerAsync();
			}
		}

		MouseMoveParam mouseMove = new MouseMoveParam();

		private void chartSensor_MouseDown(object sender, MouseEventArgs e)
		{
			mouseMove.IsMouseDown = false;

			Axis AxisX = chartSensor.ChartAreas[0].AxisX;

			//chartエリア内の場合でマウスの左ボタンの場合のみ有効
			if (!IsMouseInChartArea() || e.Button != MouseButtons.Left) return;

			//マウスボタンが押された時のマウスカーソル位置を覚えておく
			mouseMove.MouseDownPos = System.Windows.Forms.Cursor.Position;

			//マウスボタンが押された時の軸の最小値、最大値を覚えておく
			mouseMove.MouseDownXMin = AxisX.Minimum;
			mouseMove.MouseDownXMax = AxisX.Maximum;

			//作動中
			mouseMove.IsMouseDown = true;
		}

		private void chartSensor_MouseUp(object sender, MouseEventArgs e)
		{
			mouseMove.IsMouseDown = false;
		}

		private void chartSensor_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseMove.IsMouseDown)
			{
				foreach (var item in chartSensor.ChartAreas)
				{
					Axis AxisX = item.AxisX;

					//現在のマウスカーソル位置
					Point mouse = System.Windows.Forms.Cursor.Position;

					if (Math.Abs(mouse.X - mouseMove.MouseDownPos.X) >= 2)
					{
						//マウスカーソルの移動量に比率をかけて値の変化量を算出
						double delta = -mouseMove.RateX * (mouse.X - mouseMove.MouseDownPos.X);

						//グラフビューの変更
						AxisX.Minimum = mouseMove.MouseDownXMin + delta;
						AxisX.Maximum = mouseMove.MouseDownXMax + delta;
					}
				}
			}
		}

		private bool IsMouseInChartArea()
		{
			try
			{
				Axis AxisX = chartSensor.ChartAreas[0].AxisX;

				//Chartのクライアント座標でのマウス位置 
				Point mouse = chartSensor.PointToClient(System.Windows.Forms.Cursor.Position);

				//グラフ表示エリアの座標
				double pos_xmin = AxisX.ValueToPixelPosition(AxisX.ScaleView.ViewMinimum);//X軸左端
				double pos_xmax = AxisX.ValueToPixelPosition(AxisX.ScaleView.ViewMaximum);//X軸右端

				return (pos_xmin <= mouse.X && mouse.X <= pos_xmax);
			}
			catch (Exception)
			{
				return false;
			}
		}

		private double calcRate(Axis axis)
		{
			//値
			double value_min = axis.Minimum;
			double value_max = axis.Maximum;

			//座標
			double pixel_min = axis.ValueToPixelPosition(value_min);
			double pixel_max = axis.ValueToPixelPosition(value_max);

			return (value_max - value_min) / (pixel_max - pixel_min);
		}

		bool atBoot = true;//起動時かどうか
		private void chartSensor_Paint(object sender, PaintEventArgs e)
		{
			if (atBoot)
			{
				//atBoot = false;
				mouseMove.RateX = calcRate(chartSensor.ChartAreas[0].AxisX);
				mouseMove.RateY = calcRate(chartSensor.ChartAreas[0].AxisY);
			}
		}

		private void bgwReadFile_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				Invoke((MethodInvoker)(() => { picLoading.Visible = true; }));
				_logText = File.ReadAllText(FilePath);
			}
			catch
			{
			}

		}

		private void bgwReadFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			picLoading.Visible = false;
			UpdateChart();
		}

		void ControlDoubleBuffered(Control control, bool IsDoubleBuffered)
		{
			control.GetType().InvokeMember(
			"DoubleBuffered", 
		
			BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
					null,
					control,
					new object[] { IsDoubleBuffered });
		}

		private string FilePath => @"\\192.168.3.14\Program\BME280\log.csv";
	}
}
