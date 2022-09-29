using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nzy3D.Plot3D.Primitives;
using org.mariuszgromada.math.mxparser;

namespace DifferentialEvolution
{
    class MyMapper : nzy3D.Plot3D.Builder.Mapper
    {

        public override double f(double x, double y)
        {

            Function fxy = new Function("f(x1, x2) = " + Globals.Polynomial);
            Argument arg_x = new Argument("x1", x);
            Argument arg_y = new Argument("x2", y);
            Expression Equation = new Expression("f(x1,x2)", fxy, arg_x, arg_y);
            return (double)Equation.calculate();

            //   return 3*(1 - x)*(1-x) * Math.Exp(-(x*x) - (y + 1)*(y+1)) - 10*(x/5 - x*x*x - y*y*y*y*y) * Math.Exp(-x*x - y*y) - (1/ 3) * Math.Exp(-(x + 1)*(x+1) - y*y);
        }
    }

    class MyMapper2 : nzy3D.Plot3D.Builder.Mapper
    {

        public override double f(double x, double y)
        {

            Function fxy = new Function("f(x1, x2) = " + Globals.Polynomial);
            Argument arg_x = new Argument("x1", x);
            Argument arg_y = new Argument("x2", y);
            Expression Equation = new Expression("f(x1,x2)", fxy, arg_x, arg_y);
            double tmp = (double)Equation.calculate();
            return tmp/100;

            //   return 3*(1 - x)*(1-x) * Math.Exp(-(x*x) - (y + 1)*(y+1)) - 10*(x/5 - x*x*x - y*y*y*y*y) * Math.Exp(-x*x - y*y) - (1/ 3) * Math.Exp(-(x + 1)*(x+1) - y*y);
        }
    }
}
