using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.mariuszgromada.math.mxparser;
namespace DifferentialEvolution
{
    class DiffEv
    {
        private double[][] population;
        private double[][] nextPopulation;
        private double[] vectorR1, vectorR2, vectorR3; //wektory rodzicielskie - selekcja

        public void Populate(double min, double max, int size, int popSize, bool inrange, double[] start, double rad)
        {
            Random random = new Random();
            this.population = new double[popSize][];
            this.nextPopulation = new double[popSize][];
            for (int i = 0;i<popSize;i++)
            {
                double[] vector = new double[size];
                for (int j = 0; j < Globals.Poly_range; j++)
                {
                    if (inrange == true)
                    {
                        vector[j] = Math.Round(random.NextDouble(-rad, rad), 7) + start[j];
                    }
                    else
                    {
                        double dolny = Globals.XA_lower[j];
                        double gorny = Globals.XA_upper[j];
                        vector[j] = Math.Round(random.NextDouble(dolny, gorny), 7);
                    }
                }
                population[i] = vector;
                nextPopulation[i] = vector;
            }
        }
        public double[][] Select() //selekcja trzech wektorów rodzicielskich
        {
            Random random = new Random();
            double[][] results = new double[3][];
         
            int r1 = random.Next(0, population.Length);
            int r2 = random.Next(0, population.Length);
            int r3 = random.Next(0, population.Length);
            while (r2==r1)
            {
                r2 = random.Next(0, population.Length);
            }
            while (r3 == r1||r3==r2)
            {
                r3 = random.Next(0, population.Length);
            }
            results[0] = population[r1];
            results[1] = population[r2];
            results[2] = population[r3];
            return results;
        }

        public double [] Mutate()
        {
            double F = 0.4; //współczynnik skalowania
            double scale = 0.8;
            double[] v = new double[vectorR2.Length];
            double[] u = new double[vectorR2.Length];
            for (int i =0; i <vectorR2.Length;i++)
            {
                v[i] = vectorR2[i] - vectorR3[i];
            }
            for (int i = 0; i < Globals.Poly_range; i++)
            {
                double mutagen = F * v[i];
                if (Globals.XA_lower[i] > 0)
                {
                    if ( mutagen >= 0)
                    {
                        u[i] = vectorR1[i] + F * v[i];
                    } else
                    {
                        u[i] = vectorR1[i] - F * v[i];
                    }
                } else
                {
                    if (mutagen >= 0)
                    {
                        u[i] = vectorR1[i] - F * v[i];
                    }
                    else
                    {
                        u[i] = vectorR1[i] + F * v[i];
                    }
                }
                
               
                while((u[i]<Globals.XA_lower[i]) || (u[i]>Globals.XA_upper[i]))
                {
                    u[i] = u[i]*scale;
                }    
            }
            return u;
        }

        public double[] Crossover(double[] Pti, double[]u )
        {
            double[] y = new double[u.Length];
            double pc = 0.7; //prawdopodobieństwo krzyżowania
            Random random = new Random();
            for (int i =0;i<u.Length;i++)
            {
                double r = random.NextDouble();
                if (r < pc) y[i] = u[i];
                else y[i] = Pti[i];
            }
            return y;
        }


        public void Inherit(double[] Pti, double[] u,int i)
        {
            if (Adaptation(ref Pti) < Adaptation(ref u)) nextPopulation[i]=Pti;
            else nextPopulation[i]=u;
        }

        public double[][] GetPopulation()
        {
            return this.population;
        }
        public void FillVectors(double[][] vect)
        {
            vectorR1 = vect[0];
            vectorR2 = vect[1];
            vectorR3 = vect[2];
        }
        public double[][] GetNextPopulation()
        {
            return this.nextPopulation;
        }

        public void SetPopulation(double [][] newPopulation)
        {
            this.population = newPopulation;
        }
      


        public double Adaptation(ref double[] vector) //obliczanie przystosowania wektora (wartości funkcji)
        {
            Function fxy = new Function("f(x1, x2, x3, x4, x5) = " + Globals.Polynomial);
            Argument arg_x1 = new Argument("x1", vector[0]);
            Argument arg_x2 = new Argument("x2", vector[1]);
            Argument arg_x3 = new Argument("x3", vector[2]);
            Argument arg_x4 = new Argument("x4", vector[3]);
            Argument arg_x5 = new Argument("x5", vector[4]);
            
           
            if (Globals.Polynomial.Contains("x5"))
            {
                Function fxy2 = new Function("f(x1, x2, x3, x4, x5) = " + Globals.Polynomial);
                Expression Equation = new Expression("f(x1, x2, x3, x4, x5)", fxy2, arg_x1, arg_x2, arg_x3, arg_x4, arg_x5);
                vector[5] = (double)Equation.calculate();
                return vector[5];

            }
            else if (Globals.Polynomial.Contains("x4"))
            {
                Function fxy3 = new Function("f(x1, x2, x3, x4) = " + Globals.Polynomial);
                Expression Equation = new Expression("f(x1, x2, x3, x4)", fxy3, arg_x1, arg_x2, arg_x3, arg_x4);
                vector[5] = (double)Equation.calculate();
                return vector[5];

            }

            else if (Globals.Polynomial.Contains("x3"))
            {
                Function fxy4 = new Function("f(x1, x2, x3) = " + Globals.Polynomial);
                Expression Equation = new Expression("f(x1, x2, x3)", fxy4, arg_x1, arg_x2, arg_x3);
                vector[5] = (double)Equation.calculate();
                return vector[5];
            }
            else if (Globals.Polynomial.Contains("x2"))
            {
                Function fxy1 = new Function("f(x1, x2) = " + Globals.Polynomial);
                Expression Equation = new Expression("f(x1, x2)", fxy1, arg_x1, arg_x2);
                vector[5] = (double)Equation.calculate();
                return vector[5];
            }
            else
            { 
                Function fxy1 = new Function("f(x1) = " + Globals.Polynomial);
                Expression Equation = new Expression("f(x1)", fxy1, arg_x1);
                vector[5] = (double)Equation.calculate();
                return vector[5];
            }
        }
    }
}
