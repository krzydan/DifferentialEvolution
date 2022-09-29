using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using nzy3D.Chart;
using nzy3D.Maths;
    namespace DifferentialEvolution
    {
    public class wtf
    {
       public static Range rangeXX = new Range(-1, 1);
        public static Range rangeYY = new Range(-1, 1);
    }
    public static class Globals
    {
        public static double MIN = 1;
        public static double MAX = 2;
        public static int SIZE = 6;
        public static int Poly_range = 5;
        public static String Polynomial = "x^2*y^2"; // edytowalne
        public static readonly String Function_name = "f(x,y)"; // nie edytowalne

        public static int Population = 50;
        public static int Generation = 10;

        public static int steps = 10;

        public static double[] XA_upper = { MAX, MAX, MAX, MAX, MAX, MAX };
        public static double[] XA_lower = { MIN, MIN, MIN, MIN, MIN, MIN };
        public static double[] radius = { 0, 0, 0, 0, 0, 0};
        public static double promien = 1;
        public static bool inrange = false;
        public static double F = 0.4;
        public static double PC = 0.7;
        public static double F_def = 0.4;
        public static double PC_def = 0.7;
        public static double[] optimum = { 0, 0, 0 };
        public static double[][] gen_best = new double[10][];
        public static nzy3D.Chart.Chart Wykres;
        public static List<nzy3D.Plot3D.Primitives.Point>[] All_points;
        public static nzy3D.Plot3D.Rendering.View.Renderer3D renderer = new nzy3D.Plot3D.Rendering.View.Renderer3D();
        public static bool reserve = false;
        public static nzy3D.Plot3D.Primitives.Shape surface = nzy3D.Plot3D.Builder.Builder.buildOrthonomal(new nzy3D.Plot3D.Builder.Concrete.OrthonormalGrid(wtf.rangeXX, 10, wtf.rangeYY, 10), new MyMapper());
        public static nzy3D.Plot3D.Primitives.Shape heatmap = nzy3D.Plot3D.Builder.Builder.buildOrthonomal(new nzy3D.Plot3D.Builder.Concrete.OrthonormalGrid(wtf.rangeXX, 10, wtf.rangeYY, 10), new MyMapper2());
        public static nzy3D.Plot3D.Primitives.Point opt_point = new nzy3D.Plot3D.Primitives.Point();
    }


}

 
