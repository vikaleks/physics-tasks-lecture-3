using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Timers;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private PlotView graph;
        private LineSeries lines;
        private System.Timers.Timer timer;
        
        private float radius;  
        private float speed;
        private float time;      
        private float timeMax = 40;  
        private float timeStep = 0.1f;
        private float omegaW;
        
        private TextBox radiusBox;
        private TextBox speedBox;
        private Button startButton;
        
        private Label textForTB1;
        private Label textForTB2;
        private Label textForTB3;

        public Form1()
        {
            Width = 1400;
            Height = 900;
    
            textForTB1 = new Label { Text = "Радиус", Location = new Point(10, 10) };
            Controls.Add(textForTB1);

            textForTB2 = new Label { Text = "Скорость", Location = new Point(120, 10) };
            Controls.Add(textForTB2);
            
            radiusBox = new TextBox { PlaceholderText = "Введите радиус", Location = new Point(10, 40), Width = 100 };
            speedBox = new TextBox { PlaceholderText = "Введите скорость", Location = new Point(120, 40), Width = 100 };
            startButton = new Button { Text = "Запустить", Location = new Point(230, 10),Size = new Size(140, 60) };
            
            startButton.Click += StartButton_Click;
                

            Controls.Add(radiusBox);
            Controls.Add(speedBox);
            Controls.Add(startButton);
            
            graph = new PlotView
            {
                Dock = DockStyle.Bottom,
                Height = 700
            };
            Controls.Add(graph);
        }
        
        private PlotModel CreatePlotModel()
        {
            var plotModel = new PlotModel { Title = "Динамическая траектория точки на ободе колеса" };
            
            lines = new LineSeries { Title = "Циклоида", Color = OxyColors.Red };
            
            plotModel.Series.Add(lines);

            return plotModel;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(radiusBox.Text, out radius))
            {
                this.radius = radius;
            }
            if (float.TryParse(speedBox.Text, out speed))
            {
                this.speed = speed;
            }
            
            time = 0;
            omegaW = speed / radius;
         
            var plotModel = CreatePlotModel();
            graph.Model = plotModel;
            
            timer = new System.Timers.Timer(30);
            timer.Elapsed += UpdatePlot;
            timer.Start();
        }

        private void UpdatePlot(object sender, ElapsedEventArgs e)
        {
            time += timeStep; 
            if (time > timeMax)
            {
                timer.Stop();
            }

            float x = radius * time * omegaW- radius * (float)Math.Sin(omegaW*time);
            float y = radius-radius*(float)Math.Cos(omegaW*time);

            lines.Points.Add(new DataPoint(x, y));
            graph.InvalidatePlot(true);
        }
    }
}
