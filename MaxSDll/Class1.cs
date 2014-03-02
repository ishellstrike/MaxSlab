﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxSDll
{
    public class MaxSdResult {
        public double SobstvMax, Sobstv2;
        public double[] SobstvVector;
    }

    public static class MaxSd
    {

        private const double Epsilon = 0.00001;
        private const int TooLong = 99999;

        public static int Method(double[,] matrix, out MaxSdResult outstring)
        {
            outstring = new MaxSdResult();
            var str = matrix.GetLength(0);
            var predY = new double[str];
            for (var i = 0; i < str; i++)
            {
                predY[i] = 1;
            }

            var y = new double[str];
            for (var i = 0; i < str; i++)
            {
                y[i] = predY[i];
            }

            var saveY = new double[str];
            for (var i = 0; i < str; i++)
            {
                saveY[i] = y[i];
            }
            var sobstv1Ready = true;
            var sobstv2Ready = true;
            int Monotonnost; //монотонность lambda2
            bool Change;
            double lambda = 0, Savelambda;
            double lambda2 = 0, Savelambda2;


            var lo = 0;
            while (sobstv1Ready || Ending(saveY, y))
            {
                lo++;
                if (lo > TooLong)
                {
                    break;
                }
                Norm(ref predY);//нормализация
                Savelambda = lambda;
                VectorY(matrix, ref predY, ref y, ref saveY);//находим следующий Y
                lambda = First_1(predY, y); //находим 1ое собств. значение
                var delta = Math.Abs(lambda - Savelambda);//характеризует погрешность
                if (delta < Epsilon && sobstv1Ready)
                {
                    sobstv1Ready = false;
                    outstring.SobstvMax = lambda;
                }
            }
            outstring.SobstvVector = new double[str];
            for (var i = 0; i < str; i++) {
                outstring.SobstvVector[i] = y[i];
            }

            for (var i = 0; i < str; i++)
            {
                predY[i] = 1;
            }
            for (var i = 0; i < str; i++)
                y[i] = predY[i];
            for (var i = 0; i < str; i++)
                saveY[i] = y[i];
            var numb = 0;
            Monotonnost = 1;
            while (sobstv2Ready)
            {
                lo++;
                if (lo > TooLong)
                {
                    break;
                }
                Savelambda2 = lambda2;
                VectorY(matrix, ref predY, ref y, ref saveY);//находим следующий Y
                lambda2 = First_2(predY, y, saveY, lambda);//находим 2ое собств. значение
                Change = SignChange(numb, lambda2, Savelambda2, ref Monotonnost); //проверка монотонности
                if (Change)
                {
                    outstring.Sobstv2 = Savelambda2;
                    sobstv2Ready = false;
                }
                numb++;
            }

            return lo;

        }


        static void Norm(ref double[] y) //нормализация вектора
         {
             var n = Math.Sqrt(y.Sum(t => t * t));
             if (n > 0)
             {
                 for (var i = 0; i < y.Length; i++)
                 {
                     y[i] /= n;
                 }
             }
         }

        static bool SignChange(int numb, double lambda2, double Savelambda2, ref int Mono) //проверка монотонности последовательности находимых lambda2
         {
             var Change = false;
             int? NewMono = null;
             if (numb > 0) //со 2го шага
             { //текущее направление
                 if (lambda2 > Savelambda2)
                     NewMono = 1;
                 if (lambda2 < Savelambda2)
                     NewMono = -1;
                 if (lambda2 == Savelambda2)
                     NewMono = 0;
                 if ((NewMono != Mono) && (NewMono != null)) //сесли последовательность меняет направление возвращаем флаг изменения
                     Change = true;
             }
             else
             { //если шаг первый - определяем начальное направление
                 if (lambda2 > Savelambda2)
                     Mono = 1;
                 if (lambda2 < Savelambda2)
                     Mono = -1;
                 if (lambda2 == Savelambda2)
                     Mono = 0;
             }
             return Change;
         }

        private static bool Ending(double[] saveY, double[] y)
        {
            return Enumerable.Range(0, saveY.Length).Any(i => Math.Abs(saveY[i] - y[i]) > Epsilon);
        }

        /// <summary>
        /// перемножение квадратной матрицы и вектора
        /// </summary>
        /// <param name="a">Квадратная матрица</param>
        /// <param name="b">Вектор</param>
        /// <param name="c">Результат</param>
        static void MatrixMultVector(double[,] a, double[] b, ref double[] c)
        {
            var str = a.GetLength(0);
            for (var i = 0; i < str; i++)
                for (var j = 0; j < str; j++)
                {
                    c[i] = c[i] + a[i, j] * b[j];
                }
        }

        /// <summary>
        /// вычисление вектора Y(k+1)=AY(k)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="predY">пред. вектор (k)</param>
        /// <param name="y">новый (k+1)</param>
        /// <param name="saveY">пред. вектор (k-1)</param>
        static void VectorY(double[,] a, ref double[] predY, ref double[] y, ref double[] saveY)
        {
            var str = a.GetLength(0);
            var c = new double[str];
            for (var i = 0; i < str; i++)
            {
                c[i] = 0;
            }
            MatrixMultVector(a, predY, ref c);
            for (var i = 0; i < str; i++)
            {
                saveY[i] = y[i];
            }
            for (var i = 0; i < str; i++)
            {
                y[i] = predY[i];
            }
            for (var i = 0; i < str; i++)
            {
                predY[i] = c[i];
            }
        }

        /// <summary>
        /// скалярное произведение векторов
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static double ScalarMult(double[] a, double[] b)
        {
            var str = a.GetLength(0);
            double result = 0;
            for (var i = 0; i < str; i++)
                result = result + a[i] * b[i];
            return result;
        }

        /// <summary>
        /// первый случай (максимальное по модулю собственое значение вещественно и единственно), поиск lambda_1
        /// </summary>
        /// <param name="predY"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static double First_1(double[] predY, double[] y)
        {
            double lambda;//будет содержать значение lambda k-го шага
            double x1, x2;
            x1 = ScalarMult(predY, y);
            x2 = ScalarMult(y, y);
            lambda = x1 / x2;
            return lambda;
        }

        /// <summary>
        /// поиск lambda_2
        /// </summary>
        /// <param name="predY"></param>
        /// <param name="y"></param>
        /// <param name="saveY"></param>
        /// <param name="lambda1"></param>
        /// <returns></returns>
        static double First_2(double[] predY, double[] y, double[] saveY, double lambda1)
        {
            var x1 = ScalarMult(predY, y) - lambda1 * ScalarMult(y, y);
            var x2 = ScalarMult(y, y) - lambda1 * ScalarMult(y, saveY);
            return x1 / x2;
        }
    }
}
