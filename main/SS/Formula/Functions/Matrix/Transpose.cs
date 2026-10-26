using MathNet.Numerics.LinearAlgebra;
using NPOI.SS.Formula.Eval;
using static NPOI.SS.Formula.Functions.Matrix.MatrixFunction;

namespace NPOI.SS.Formula.Functions.Matrix;

public class Transpose : OneArrayArg
{
    private MutableValueCollector instance = new MutableValueCollector(false, true);

    protected override double[] CollectValues(ValueEval arg)
    {
        return instance.collectValues(arg);
    }

    protected override double[,] Evaluate(double[,] d1)
    {
        var temp = Matrix<double>.Build.DenseOfArray(d1);
        return temp.Transpose().ToArray();
    }
}
