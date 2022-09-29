using nzy3D.Chart;
using nzy3D.Colors;
using nzy3D.Colors.ColorMaps;
using nzy3D.Maths;
using nzy3D.Plot3D.Builder;
using nzy3D.Plot3D.Builder.Concrete;
using nzy3D.Plot3D.Builder.Delaunay;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Primitives.Axes.Layout;
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using org.mariuszgromada.math.mxparser;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace DifferentialEvolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private nzy3D.Chart.Controllers.Thread.Camera.CameraThreadController t;
        private IAxeLayout axeLayout;
        private int oldGenerationsNumber;
        public MainWindow()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string fnc1 = "sin(x)+y";
            string fnc2 = "x+y";
            string fnc3 = "(y - x^2)^2 + (1-x)^2";
            Globals.All_points[0] = new List<nzy3D.Plot3D.Primitives.Point>();
            this.DataContext = this;
            // Create the interop host control.
            System.Windows.Forms.Integration.WindowsFormsHost host =
                new System.Windows.Forms.Integration.WindowsFormsHost();
           // Zakres_x_gorny.Text = "2";
           // Zakres_x_dolny.Text = "-2";
           // Zakres_Y_gorny.Text = "2";
           // Zakres_Y_dolny.Text = "-2";
            Wynik_output1.Visibility = Visibility.Visible;
            x1x2_text.Visibility = Visibility.Visible;
            MainGrid.Visibility = Visibility.Visible;
            host.Child = Globals.renderer;
            Globals.Wykres = new Chart(Globals.renderer, Quality.Nicest);
            this.MainGrid.Children.Add(host);
            Globals.All_points = new List<nzy3D.Plot3D.Primitives.Point>[Globals.Generation];
            oldGenerationsNumber = Globals.Generation;
            show_map.Visibility = Visibility.Hidden;
            // host.Visibility.Equals(false);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisposeBackgroundThread();
        }

        private void DisposeBackgroundThread()
        {
            if ((t != null))
            {
                t.Dispose();
            }
        }


        private bool _DisplayTickLines;
        public bool DisplayTickLines
        {
            get
            {
                return _DisplayTickLines;
            }
            set
            {
                _DisplayTickLines = value;
                OnPropertyChanged("DisplayTickLines");
                if (axeLayout != null)
                {
                    axeLayout.TickLineDisplayed = value;
                }
            }
        }

        private bool _DisplayXTicks;
        public bool DisplayXTicks
        {
            get
            {
                return _DisplayXTicks;
            }
            set
            {
                _DisplayXTicks = value;
                OnPropertyChanged("DisplayXTicks");
                if (axeLayout != null)
                {
                    axeLayout.XTickLabelDisplayed = value;
                }

            }
        }

        private bool _DisplayYTicks;
        public bool DisplayYTicks
        {
            get
            {
                return _DisplayYTicks;
            }
            set
            {
                _DisplayYTicks = value;
                OnPropertyChanged("DisplayYTicks");
                if (axeLayout != null)
                {
                    axeLayout.YTickLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayZTicks;
        public bool DisplayZTicks
        {
            get
            {
                return _DisplayZTicks;
            }
            set
            {
                _DisplayZTicks = value;
                OnPropertyChanged("DisplayZTicks");
                if (axeLayout != null)
                {
                    axeLayout.ZTickLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayXAxisLabel;
        public bool DisplayXAxisLabel
        {
            get
            {
                return _DisplayXAxisLabel;
            }
            set
            {
                _DisplayXAxisLabel = value;
                OnPropertyChanged("DisplayXAxisLabel");
                if (axeLayout != null)
                {
                    axeLayout.XAxeLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayYAxisLabel;
        public bool DisplayYAxisLabel
        {
            get
            {
                return _DisplayYAxisLabel;
            }
            set
            {
                _DisplayYAxisLabel = value;
                OnPropertyChanged("DisplayYAxisLabel");
                if (axeLayout != null)
                {
                    axeLayout.YAxeLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayZAxisLabel;
        public bool DisplayZAxisLabel
        {
            get
            {
                return _DisplayZAxisLabel;
            }
            set
            {
                _DisplayZAxisLabel = value;
                OnPropertyChanged("DisplayZAxisLabel");
                if (axeLayout != null)
                {
                    axeLayout.ZAxeLabelDisplayed = value;
                }
            }
        }




        private void DoNothing_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }
        void View_all(Chart Workplace)
        {
            //if (!Globals.All_points.Contains(null))
            //  { 
            for (int i = 0; i < oldGenerationsNumber; i++)
            {
                List<nzy3D.Plot3D.Primitives.Point> points = Globals.All_points[i];
                if (!Globals.All_points.Contains(null))
                {
                    foreach (nzy3D.Plot3D.Primitives.Point each_point in points)
                    {
                        Workplace.Scene.Graph.Add(each_point);
                    }
                }
            }
            //}
        }
        void View_one(Chart Workplace, int index_gen)
        {
            RemovePoints(Workplace);
            List<nzy3D.Plot3D.Primitives.Point> points = Globals.All_points[index_gen];
            foreach (nzy3D.Plot3D.Primitives.Point each_point in points)
            {
                Workplace.Scene.Graph.Add(each_point);
            }
        }






        void CreatePoints(Chart Workplace, double[][] Pop, int array_index, double[] mini)
        {
            double Xp, Yp, Zp;
            List<nzy3D.Plot3D.Primitives.Point> points = new List<nzy3D.Plot3D.Primitives.Point>();
            for (int i = 0; i < Globals.Population; i++)
            {
                Xp = Pop[i][0];
                Yp = Pop[i][1];
                Zp = Pop[i][Globals.SIZE - 1];
                if (Xp == mini[0] && Yp == mini[1] && Zp == mini[Globals.SIZE - 1])
                {
                    Coord3d point = new Coord3d(Xp, Yp, Zp);
                    points.Add(new nzy3D.Plot3D.Primitives.Point() { xyz = point, rgb = Color.MAGENTA, Width = 20 });
                    //points.Color = new Color(1, 0, 0, 0.9)
                }
                else
                {
                    Coord3d point = new Coord3d(Xp, Yp, Zp);
                    points.Add(new nzy3D.Plot3D.Primitives.Point() { xyz = point, rgb = Color.RED, Width = 10 });
                    //points.Color = new Color(1, 0, 0, 0.9)
                }
            }
            Globals.All_points[array_index] = points;

        }

        void CreatePoint(Chart Workplace, double[] opt)
        {
    
            Coord3d point = new Coord3d(opt[0], opt[1], opt[5]/100);
            Globals.opt_point = new nzy3D.Plot3D.Primitives.Point(point, Color.MAGENTA, (float)10.0);
            Workplace.Scene.Graph.Add(Globals.opt_point);
        }
        void RemovePoint(Chart Workplace)
        {
            Workplace.Scene.Graph.Remove(Globals.opt_point);
        }
        void RemovePoints(Chart Workplace)
        {
            for (int i = 0; i < oldGenerationsNumber; i++)
            {
                List<nzy3D.Plot3D.Primitives.Point> points = Globals.All_points[i];
                if (points == null)
                { }
                else
                {
                    foreach (nzy3D.Plot3D.Primitives.Point each_point in points)
                    {
                        Workplace.Scene.Graph.Remove(each_point);
                    }
                }
            }
        }
        void poly_range()
        {
            if (Globals.Polynomial.Contains("x5"))
            {
                Globals.Poly_range = 5;

            }
            else if (Globals.Polynomial.Contains("x4"))
            {
                Globals.Poly_range = 4;
            }

            else if (Globals.Polynomial.Contains("x3"))
            {
                Globals.Poly_range = 3;
            }
            else if (Globals.Polynomial.Contains("x2"))
            {
                Globals.Poly_range = 2;
            }
            else
            {
                Globals.Poly_range = 1;

            }
        }
        private void Print_graph_button(object sender, RoutedEventArgs e)
        {
            RemovePoints(Globals.Wykres);
            poly_range();
            Globals.Wykres.Scene.Graph.Remove(Globals.surface);
            //System.Globalization.CultureInfo.InvariantCulture
            Globals.XA_lower[0] = (Convert.ToDouble(Zakres_x_dolny.Text));
            Globals.XA_upper[0] = Convert.ToDouble(Zakres_x_gorny.Text);
            Globals.XA_lower[1] = (Convert.ToDouble(Zakres_Y_dolny.Text));
            Globals.XA_upper[1] = Convert.ToDouble(Zakres_Y_gorny.Text);
            // Create a range for the graph generation
            Range rangeX = new Range(Globals.XA_lower[0], Globals.XA_upper[0]);
            Range rangeY = new Range(Globals.XA_lower[1], Globals.XA_upper[1]);
            Range rangeZ = new Range(Globals.XA_lower[0], Globals.XA_upper[0]);
            int steps = Globals.steps;

            // Build a nice surface to display with cool alpha colors 
            // (alpha 0.8 for surface color and 0.5 for wireframe)

            Globals.surface = Builder.buildOrthonomal(new OrthonormalGrid(rangeX, steps, rangeY, steps), new MyMapper());

            // MultiColorScatter scatter = new MultiColorScatter(points, new ColorMapper(new ColorMapRainbow(), -0.5f, 0.5f));

            Globals.surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), Globals.surface.Bounds.zmin, Globals.surface.Bounds.zmax, new Color(1, 1, 1, 0.8));
            Globals.surface.FaceDisplayed = true;
            Globals.surface.WireframeDisplayed = true;

            Globals.surface.WireframeColor = Color.BLACK;
            Globals.surface.WireframeColor.mul(new Color(1, 1, 1, 0.5));

            // Create the chart and embed the surface within

            Globals.Wykres.AxeLayout.XAxeLabel = "X1";
            Globals.Wykres.AxeLayout.YAxeLabel = "X2";
            DisplayXTicks = true;
            DisplayXAxisLabel = true;
            DisplayYTicks = true;
            DisplayYAxisLabel = true;
            DisplayZTicks = true;
            DisplayZAxisLabel = true;
            DisplayTickLines = true;
            Show_all.IsEnabled = true;
            
            //View_all(Globals.Wykres);
            try
            {
                Globals.Wykres.Scene.Graph.Add(Globals.surface);
                

                nzy3D.Chart.Controllers.Mouse.Camera.CameraMouseController mouse = new nzy3D.Chart.Controllers.Mouse.Camera.CameraMouseController();
                mouse.addControllerEventListener(Globals.renderer);
                Globals.Wykres.addController(mouse);

                DisposeBackgroundThread();

                Globals.renderer.setView(Globals.Wykres.View);
                Show_all.IsEnabled = true;
                Slider_generacji.IsEnabled = true;
                if (Globals.reserve == true) {
                    if (show_map.IsVisible == true)
                    {
                        show_map.Visibility = Visibility.Hidden;
                        Globals.reserve = false;
                    }
                    else
                    {
                        show_map.Visibility = Visibility.Visible;
                    }
                } 
                
            }
            catch (Exception ex)
            {
                string message = "Błędna zawartość funkcji. Proszę wpisać jeszcze raz.";
                string caption = "Wykryto błąd!";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, caption, buttons);
            }

        }
        private void Last_pop_button(object sender, RoutedEventArgs e)
        {
            //global chart
        }

        private void Show_all_button(object sender, RoutedEventArgs e)
        {
            View_all(Globals.Wykres);
            Removepoints_butt.IsEnabled = true;
        }

        private void Next_pop_button(object sender, RoutedEventArgs e)
        {

        }

        double[] BestSolution(double[][] solutions) //obliczanie optimum;
        {
            DiffEv DE = new DiffEv();
            double adaptVal = DE.Adaptation(ref solutions[0]);
            double[] minimum = solutions[0];
            for (int i = 1; i < solutions.Length; i++) //selekcja wektorów rodzicielskich
            {
                double newAdapt = DE.Adaptation(ref solutions[i]);
                if (newAdapt < adaptVal)
                {
                    adaptVal = newAdapt;
                    minimum = solutions[i];
                }
            }
            return minimum;
        }


        private void Oblicz_Click(object sender, RoutedEventArgs e)
        {
            RemovePoints(Globals.Wykres);
            poly_range();
            oldGenerationsNumber = Globals.Generation;
            Globals.XA_lower[0] = (Convert.ToDouble(Zakres_x_dolny.Text));
            Globals.XA_upper[0] = Convert.ToDouble(Zakres_x_gorny.Text);
            Globals.XA_lower[1] = (Convert.ToDouble(Zakres_Y_dolny.Text));
            Globals.XA_upper[1] = Convert.ToDouble(Zakres_Y_gorny.Text);

            int L = Globals.Generation;
            int reserve = 0;
            DiffEv DE = new DiffEv();
            DE.Populate(Globals.MIN, Globals.MAX, Globals.SIZE, Globals.Population, Globals.inrange, Globals.radius, Globals.promien);
            double[][] solutions = DE.GetPopulation();
            //foreach (double[] vector in solutions)
            //{
            //    Console.WriteLine(vector[0] +" "+ vector[1] + " " + vector[2]);
            //}
            //Console.WriteLine("End");
            
            int t = 0;
            while (t < L)
            {

                for (int i = 0; i < solutions.Length; i++) //selekcja wektorów rodzicielskich
                {
                    double[][] parents = DE.Select();
                    DE.FillVectors(parents);
                    double[] u = DE.Mutate();
                    double[] y = DE.Crossover(solutions[i], u);
                    DE.Inherit(solutions[i], y, i);
                    //Console.WriteLine(string.Join(" ; ",parents[0]) + " " + string.Join(" ; ", parents[1]) + " " + string.Join(" ; ", parents[2]));
                }

                double[] minimum = BestSolution(solutions);
                //wyświetlenie punktu danej iteracji w oknie
                Globals.gen_best[t] = minimum;
                CreatePoints(Globals.Wykres, DE.GetPopulation(), t, minimum);
                DE.SetPopulation(DE.GetNextPopulation());
                solutions = DE.GetPopulation();
                t++;
            }
            //wybór najlepszego rozwiązania
            double[] optimum = BestSolution(solutions);
            Wynik_output.Text = Convert.ToString(optimum[5]);
            Globals.optimum = optimum;
            Globals.reserve = true;
            Wynik_output1.Text = Convert.ToString(optimum[0]) + " ; " + Convert.ToString(optimum[1]);
            //wyświetlenie rozwiązania optymalnego w oknie
        }

        private void Current_Function_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Click(object sender, EventArgs e)
        {
         

        }

        private void Radio4_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)Radio1.IsChecked)
            {
                Globals.Polynomial = "x1^2-x2^1/3";
            }
            if ((bool)Radio2.IsChecked)
            {
                Globals.Polynomial = "sin(x1)+x2^2";
            }
            if ((bool)Radio3.IsChecked)
            {
                Globals.Polynomial = " 3 * (1 - x1) ^ 2 * e ^ (-(x1 ^ 2) - (x2 + 1) ^ 2) - 10 * (x1 / 5 - x1 ^ 3 - x2^ 5) * e ^ (-x1 ^ 2 - x2 ^ 2) - (1 / 3) * e ^ (-(x1 + 1) ^ 2 - x2 ^ 2)";
            }
            if ((bool)Radio4.IsChecked)
            {
                Globals.Polynomial = " 100*(x2-x1^2)^2+(1-x1)^2";
            }
            Given_function.Text = Globals.Polynomial;
        }

        private void Steps_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            Globals.steps = Convert.ToInt32(steps_input.Text);
        }


        private void Generacja_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isInteger = int.TryParse(Generacja_input.Text, out int n);
            if (Generacja_input.Text != "")
            {
                if (isInteger == false)
                {
                    string message = "Błędna liczba generacji. Należy podać liczbę całkowitą.";
                    string caption = "Wykryto błąd!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, caption, buttons);
                }
                else if (Convert.ToInt32(Generacja_input.Text) < 0)
                {
                    string message = "Liczba generacji nie może być ujemna";
                    string caption = "Wykryto błąd!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, caption, buttons);
                }
                else
                {
                    Globals.Generation = Convert.ToInt32(Generacja_input.Text);
                    Globals.gen_best = new double[Globals.Generation][];
                    Globals.All_points = new List<nzy3D.Plot3D.Primitives.Point>[Globals.Generation];
                }

            }

        }
        private void Given_function_TextChanged(object sender, TextChangedEventArgs e)
        {
            Globals.Polynomial = Given_function.Text;
            try
            {
                if (Given_function.Text.Contains("x3") || Given_function.Text.Contains("x4") || Given_function.Text.Contains("x5"))
                {

                    this.Print_graph.Visibility = Visibility.Hidden;
                    Wynik_output1.Visibility = Visibility.Hidden;
                    x1x2_text.Visibility = Visibility.Hidden;
                    MainGrid.Visibility = Visibility.Hidden;
                    wyswietl_gen_text.Visibility = Visibility.Hidden;
                    Show_Gx.Visibility = Visibility.Hidden;
                    Slider_generacji.Visibility = Visibility.Hidden;
                }
                else
                {
                    Print_graph.Visibility = Visibility.Visible;
                    Wynik_output1.Visibility = Visibility.Visible;
                    x1x2_text.Visibility = Visibility.Visible;
                    MainGrid.Visibility = Visibility.Visible;
                    wyswietl_gen_text.Visibility = Visibility.Visible;
                    Show_Gx.Visibility = Visibility.Visible;
                    Slider_generacji.Visibility = Visibility.Visible;
                    inrange_check.Visibility = Visibility.Visible;
                    Slider_wyniku.Visibility = Visibility.Visible;
                    Show_Gx1.Visibility = Visibility.Visible;
                    wys_wynik_txt.Visibility = Visibility.Visible;

                }
                if (!Given_function.Text.Contains("x3"))
                {
                    x3_wynik.Visibility = Visibility.Hidden;
                } else
                {
                    x3_wynik.Visibility = Visibility.Visible;
                }
                if (!Given_function.Text.Contains("x4"))
                {
                    x4_wynik.Visibility = Visibility.Hidden;
                } else
                {
                    x4_wynik.Visibility = Visibility.Visible;
                }

                if (!Given_function.Text.Contains("x5"))
                {
                    x5_wynik.Visibility = Visibility.Hidden;
                }
                else
                {
                    x5_wynik.Visibility = Visibility.Visible;
                }
            }      
            catch (Exception ex)
            {

            }
        }
        public bool CheckInput(string text)
        {
            var isDouble = double.TryParse(text, out double n);
            if (isDouble == false)
            {
                string message = "Błędna liczba. ";
                string caption = "Wykryto błąd!";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, caption, buttons);
                return false;
            }      
            else
            {
                return true;
            }

        }
 
        private void Populacja_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isInteger = int.TryParse(Populacja_input.Text, out int n);
            if (Populacja_input.Text != "")
            {
                if (isInteger == false)
                {
                    string message = "Błędna liczba populacji. Należy podać liczbę całkowitą.";
                    string caption = "Wykryto błąd!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, caption, buttons);
                }
                else if (Convert.ToInt32(Populacja_input.Text) < 0)
                {
                    string message = "Liczba populacji nie może być ujemna";
                    string caption = "Wykryto błąd!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, caption, buttons);

                }
                else
                {
                    Globals.Population = Convert.ToInt32(Populacja_input.Text);
                }
            }
        }
        private void Removepoints_butt_Click(object sender, RoutedEventArgs e)
        {
            RemovePoints(Globals.Wykres);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider_generacji.Maximum = oldGenerationsNumber - 1;
            Show_Gx.Text = Convert.ToString(Slider_generacji.Value + 1);
            View_one(Globals.Wykres, (int)Slider_generacji.Value);
        }

        private void PC_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.PC_TextBox.Text.Equals(""))
            {
                Globals.PC = Globals.PC_def;
            }
            else
            {
                Globals.PC = Convert.ToDouble(PC_TextBox.Text.Replace('.', ','));
            }

        }

        private void F_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.F_Textbox.Text.Equals(""))
            {
                Globals.F = Globals.F_def;
            }
            else
            {
                Globals.F = Convert.ToDouble(F_Textbox.Text.Replace('.', ','));
            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Globals.inrange = true;
        }
        private void Inrange_check_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.inrange = false;
        }
        private void X1_r_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x1_r.Text))
            {
                Globals.radius[0] = Convert.ToDouble(x1_r.Text);
            }

        }

        private void X2_r_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x2_r.Text))
            {
                Globals.radius[1] = Convert.ToDouble(x2_r.Text);
            }
        }

        private void X3_r_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x3_r.Text))
            {
                Globals.radius[2] = Convert.ToDouble(x3_r.Text);
            }
        }

        private void X4_r_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x4_r.Text))
            {
                Globals.radius[3] = Convert.ToDouble(x4_r.Text);
            }
        }

        private void X5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x5_r.Text))
            {
                Globals.radius[4] = Convert.ToDouble(x5_r.Text);
            }
        }

        private void Promien_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(promien.Text))
            {
                Globals.promien = Convert.ToDouble(promien.Text);
            }
        }

        private void Slider_wyniku_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider_wyniku.Maximum = Globals.Generation - 1;
            Show_Gx1.Text = Convert.ToString(Slider_wyniku.Value + 1);

            x1_wynik.Text = Convert.ToString(Globals.gen_best[Convert.ToInt32(Slider_wyniku.Value)][0]);
            x2_wynik.Text = Convert.ToString(Globals.gen_best[Convert.ToInt32(Slider_wyniku.Value)][1]);
            x3_wynik.Text = Convert.ToString(Globals.gen_best[Convert.ToInt32(Slider_wyniku.Value)][2]);
            x4_wynik.Text = Convert.ToString(Globals.gen_best[Convert.ToInt32(Slider_wyniku.Value)][3]);
            x5_wynik.Text = Convert.ToString(Globals.gen_best[Convert.ToInt32(Slider_wyniku.Value)][4]);

        }

        private void X1_max_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x1_max.Text))
            {
                Globals.XA_upper[0] = Convert.ToDouble(x1_max.Text);
            }
        }

        private void X1_min_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x1_min.Text))
            {
                Globals.XA_lower[0] = Convert.ToDouble(x1_min.Text);
            }
        
    }

        private void X2_max_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x2_max.Text))
            {
                Globals.XA_upper[1] = Convert.ToDouble(x2_max.Text);
            }
        }

        private void X2_min_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x2_min.Text))
            {
                Globals.XA_lower[1] = Convert.ToDouble(x2_min.Text);
            }
        }

        private void X3_max_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x3_max.Text))
            {
                Globals.XA_upper[2] = Convert.ToDouble(x3_max.Text);
            }
        }

        private void X3_min_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x3_min.Text))
            {
                Globals.XA_lower[2] = Convert.ToDouble(x3_min.Text);
            }
        }

        private void X4_max_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x4_max.Text))
            {
                Globals.XA_upper[3] = Convert.ToDouble(x4_max.Text);
            }
        }

        private void X4_min_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x4_min.Text))
            {
                Globals.XA_lower[3] = Convert.ToDouble(x4_min.Text);
            }
        }

        private void X5_max_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x5_max.Text))
            {
                Globals.XA_upper[4] = Convert.ToDouble(x5_max.Text);
            }
        }

        private void X5_min_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckInput(x5_min.Text))
            {
                Globals.XA_lower[4] = Convert.ToDouble(x5_min.Text);
            }
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            Print_graph.IsEnabled = false;
            Oblicz_button.IsEnabled = false;
            Show_all.IsEnabled = false;
            Removepoints_butt.IsEnabled = false;
            Range rangeX = new Range(Globals.XA_lower[0], Globals.XA_upper[0]);
            Range rangeY = new Range(Globals.XA_lower[1], Globals.XA_upper[1]);

            int steps = Globals.steps;
            Globals.heatmap = Builder.buildOrthonomal(new OrthonormalGrid(rangeX, steps, rangeY, steps), new MyMapper2());

            Globals.heatmap.ColorMapper = new ColorMapper(new ColorMapRainbow(), Globals.surface.Bounds.zmin / 100, Globals.surface.Bounds.zmax / 100, new Color(1, 1, 1, 0.8));
            Globals.heatmap.FaceDisplayed = true;
            Globals.heatmap.WireframeDisplayed = false;

            Globals.heatmap.WireframeColor = Color.BLACK;
            Globals.heatmap.WireframeColor.mul(new Color(1, 1, 1, 0.5));
            Globals.Wykres.Scene.Graph.Add(Globals.heatmap);
            Globals.Wykres.Scene.Graph.Remove(Globals.surface);
            CreatePoint(Globals.Wykres, Globals.optimum);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.Wykres.Scene.Graph.Remove(Globals.heatmap);
            Globals.Wykres.Scene.Graph.Add(Globals.surface);
            RemovePoint(Globals.Wykres);
            Print_graph.IsEnabled = true;
            Oblicz_button.IsEnabled = true;
            Show_all.IsEnabled = true;
            Removepoints_butt.IsEnabled = true;
        }

        private void Zakres_x_dolny_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
