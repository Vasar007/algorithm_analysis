using System;
using AlgorithmAnalysis.Math.Functions;
using MathNet.Numerics;

namespace AlgorithmAnalysis.Math
{
    internal static class FitDecorator
    {
        /// <summary>
        /// Least-Squares fitting the points (x, y) to an exponential
        /// y : x -> a * exp(r * x).
        /// </summary>
        public static IModelledFunction ExponentialFunc(double[] xArray, double[] yArray)
        {
            Tuple<double, double> parameters = Fit.Exponential(xArray, yArray);
            double a = parameters.Item1;
            double r = parameters.Item2;

            Func<double, double> function = t => a * System.Math.Exp(r * t);

            return new ExponentialFunction(function, parameters);
        }

        /// <summary>
        /// Least-Squares fitting the points (x, y) to a line
        /// y : x -> a + b * x.
        /// </summary>
        public static IModelledFunction LineFunc(double[] xArray, double[] yArray)
        {
            Tuple<double, double> parameters = Fit.Line(xArray, yArray);
            double intercept = parameters.Item1;
            double slope = parameters.Item2;

            Func<double, double> function = t => intercept + slope * t;

            return new LineFunction(function, parameters);
        }

        /// <summary>
        /// Least-Squares fitting the points (x, y) to a logarithm
        /// y : x -> a + b * ln(x).
        /// </summary>
        public static IModelledFunction LogarithmFunc(double[] xArray, double[] yArray)
        {
            Tuple<double, double> parameters = Fit.Logarithm(xArray, yArray);
            double a = parameters.Item1;
            double b = parameters.Item2;

            Func<double, double> function = t => a + b * System.Math.Log(t);

            return new LogarithmFunction(function, parameters);
        }

        /// <summary>
        /// Least-Squares fitting the points (x, y) to a k-order polynomial
        /// y : x -> p0 + p1 * x + p2 * x^2 + ... + pk * x^k.
        /// </summary>
        public static IModelledFunction PolynomialFunc(double[] xArray, double[] yArray, int order)
        {
            double[] parameters = Fit.Polynomial(xArray, yArray, order);

            Func<double, double> function = t => Polynomial.Evaluate(t, parameters);

            return new PolynomialFunction(function, parameters);
        }

        /// <summary>
        /// Least-Squares fitting the points (x, y) to a power
        /// y : x -> a * x^b.
        /// </summary>
        public static IModelledFunction PowerFunc(double[] xArray, double[] yArray)
        {
            Tuple<double, double> parameters = Fit.Power(xArray, yArray);
            double a = parameters.Item1;
            double b = parameters.Item2;

            Func<double, double> function = t => a * System.Math.Pow(t, b);

            return new PowerFunction(function, parameters);
        }
    }
}
