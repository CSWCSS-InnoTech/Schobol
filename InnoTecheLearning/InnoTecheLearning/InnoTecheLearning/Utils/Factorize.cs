using System;
using System.Collections.Generic;
using System.Numerics;

namespace InnoTecheLearning
{
    partial class Utils
    {
        /// <summary>
        /// Factorizes an expression.
        /// </summary>
        /// <param name="A">Multiplied by X².</param>
        /// <param name="B">Multiplied by XY.</param>
        /// <param name="C">Multiplied by Y².</param>
        /// <returns>An IEnumerable which
        /// [0] = First Root
        /// [1] = Second Root
        /// [2] = First factor's coefficient
        /// [3] = First factor's constant term
        /// [4] = Second factor's coefficient
        /// [5] = Second factor's constant term
        /// [6] = The highest common factor among the coefficients.
        /// Factorized Result: [6]([2]X+[3]Y)([4]X+[5]Y)</returns>
        public static void FactorizeNonthrowable(double A, double B, double C
            , out Complex Root1, out Complex Root2)
        {
            C = B * B - 4 * A * C;
            Root1 = (Complex.Sqrt(C) - B) / (2 * A);
            Root2 = (-Complex.Sqrt(C) - B) / (2 * A);
        }
        public static double Factorize(double A, double B, double C
            , out Complex Root1, out Complex Root2,
            out double Factor1Co, out double Factor1CTerm,
            out double Factor2Co, out double Factor2CTerm, double SafeCheck = 100000)
        {
            double M = 0;
            double X, Y;
            C = B * B - 4 * A * C;
            Root1 = (Complex.Sqrt(C) - B) / (2 * A);
            Root2 = (-Complex.Sqrt(C) - B) / (2 * A);
            if (Root1.Imaginary != 0) throw new ArithmeticException("Imaginary root cannot be factorized.");
            if (Root2.Imaginary != 0) throw new ArithmeticException("Imaginary root cannot be factorized.");
            X = Root1.Real;
            Y = Root2.Real;

            Label1: M++;
            if (M > SafeCheck) throw new ArithmeticException("Will take too long to factorize.");
            if (X * M != Math.Round(X * M)) goto Label1;
            A = A / M;
            Factor1Co = M;
            Factor1CTerm = -X * M;
            M -= M;

            X = Y;
            Label2: M++;
            if (X * M != Math.Round(X * M)) goto Label2;
            A = A / M;
            Factor2Co = M;
            Factor2CTerm = -X * M;
            M -= M;
            return A;
        }
        public static string FactorizeSafe(double A, double B, double C,
            out Complex Root1, out Complex Root2, char X = 'X', char Y = 'Y', double SafeCheck = 100000)
        {
            try
            {
                double Factor1Co, Factor1CTerm, Factor2Co, Factor2CTerm;
                double HCF = Factorize(A, B, C, out Root1, out Root2, out Factor1Co, out Factor1CTerm,
                    out Factor2Co, out Factor2CTerm, SafeCheck);
                return Prefix(HCF) + "(" + Prefix(Factor1Co) + X + Postfix(Factor1CTerm) + ")" +
                       "(" + Prefix(Factor2Co) + Y + Postfix(Factor2CTerm) + ")";
            }
            catch (ArithmeticException ex)
            {
                FactorizeNonthrowable(A, B, C, out Root1, out Root2);
                return ex.Message;
            }
        }
        public static string Prefix(double n)
        { return n == 1 ? "" : n == -1 ? "-" : n.ToString(); }
        public static string Postfix(double n)
        { return n == 0 ? "" : (n > 0 ? "+" : "") + n.ToString(); }
    }
} 