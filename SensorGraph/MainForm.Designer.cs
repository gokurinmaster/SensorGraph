namespace SensorGraph
{
	partial class MainForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea11 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea12 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.chartSensor = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.timerUpdate = new System.Windows.Forms.Timer(this.components);
			this.bgwReadFile = new System.ComponentModel.BackgroundWorker();
			this.picLoading = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.chartSensor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picLoading)).BeginInit();
			this.SuspendLayout();
			// 
			// chartSensor
			// 
			this.chartSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			chartArea10.Name = "ChartAreaTemp";
			chartArea11.Name = "ChartAreaHumid";
			chartArea12.Name = "ChartAreaPress";
			this.chartSensor.ChartAreas.Add(chartArea10);
			this.chartSensor.ChartAreas.Add(chartArea11);
			this.chartSensor.ChartAreas.Add(chartArea12);
			legend4.Name = "Legend1";
			this.chartSensor.Legends.Add(legend4);
			this.chartSensor.Location = new System.Drawing.Point(12, 12);
			this.chartSensor.Name = "chartSensor";
			series10.ChartArea = "ChartAreaTemp";
			series10.Legend = "Legend1";
			series10.Name = "Temp";
			series11.ChartArea = "ChartAreaHumid";
			series11.Legend = "Legend1";
			series11.Name = "Humid";
			series12.ChartArea = "ChartAreaPress";
			series12.Legend = "Legend1";
			series12.Name = "Press";
			this.chartSensor.Series.Add(series10);
			this.chartSensor.Series.Add(series11);
			this.chartSensor.Series.Add(series12);
			this.chartSensor.Size = new System.Drawing.Size(776, 551);
			this.chartSensor.TabIndex = 0;
			this.chartSensor.Text = "chart1";
			this.chartSensor.Paint += new System.Windows.Forms.PaintEventHandler(this.chartSensor_Paint);
			this.chartSensor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartSensor_MouseDown);
			this.chartSensor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartSensor_MouseMove);
			this.chartSensor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chartSensor_MouseUp);
			// 
			// timerUpdate
			// 
			this.timerUpdate.Enabled = true;
			this.timerUpdate.Interval = 30000;
			this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
			// 
			// bgwReadFile
			// 
			this.bgwReadFile.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwReadFile_DoWork);
			this.bgwReadFile.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwReadFile_RunWorkerCompleted);
			// 
			// picLoading
			// 
			this.picLoading.BackColor = System.Drawing.SystemColors.Control;
			this.picLoading.Image = global::SensorGraph.Properties.Resources.loading;
			this.picLoading.InitialImage = global::SensorGraph.Properties.Resources.loading;
			this.picLoading.Location = new System.Drawing.Point(398, 277);
			this.picLoading.Name = "picLoading";
			this.picLoading.Size = new System.Drawing.Size(16, 16);
			this.picLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picLoading.TabIndex = 1;
			this.picLoading.TabStop = false;
			this.picLoading.Visible = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 575);
			this.Controls.Add(this.picLoading);
			this.Controls.Add(this.chartSensor);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SensorChart";
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			((System.ComponentModel.ISupportInitialize)(this.chartSensor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picLoading)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataVisualization.Charting.Chart chartSensor;
		private System.Windows.Forms.Timer timerUpdate;
		private System.ComponentModel.BackgroundWorker bgwReadFile;
		private System.Windows.Forms.PictureBox picLoading;
	}
}

