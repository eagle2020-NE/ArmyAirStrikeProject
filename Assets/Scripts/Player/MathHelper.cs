using System;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathHelper
{
    public static float ClampAngle(float angle, float min, float max)
    {
        // TODO: Should move to Math Helper class
        if (min == max && min == 0.0f)
        {
            return angle;
        }

        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
        {
            inverse = !inverse;
            tmin -= 180;
        }
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        if (max > 180)
        {
            inverse = !inverse;
            tmax -= 180;
        }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
    }

    public static float NormalizeAngle(float angle)
    {
        while (angle <= -180) angle += 360;
        while (angle > 180) angle -= 360;
        return angle;
    }

    public static float Scale(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return ((newMax - newMin) * (value - oldMin) / (oldMax - oldMin)) + newMin;
    }

    /// <summary>
    /// return true if x is anywhere between -bounds and bounds
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bounds"></param>
    /// <returns></returns>
    public static bool InRange(float value, float bounds)
    {
        return Mathf.Abs(value) <= bounds;
    }

    /// <summary>
    /// return true if x is anywhere between -bounds and bounds
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bounds"></param>
    /// <returns></returns>
    public static bool OutOfRange(float value, float bounds)
    {
        return Mathf.Abs(value) > bounds;
    }
}


public class Matrix2D
{
    private float[,] _matrix;
    public int RowCount { get; }
    public int ColCount { get; }
    public string[] ColNames;
    public string[] RowNames;

    public Matrix2D(int i, int j)
    {
        _matrix = new float[i, j];
        RowCount = j;
        ColCount = i;
    }

    public Matrix2D(float[,] matrix) : this(matrix.GetLength(0), matrix.GetLength(1))
    {
        _matrix = matrix.Clone() as float[,];
    }

    public Matrix2D(Matrix2D matrix) : this(matrix.ColCount, matrix.RowCount)
    {
        for (var row = 0; row < RowCount; ++row)
        {
            for (var col = 0; col < ColCount; ++col)
            {
                if (col < ColCount - 1)
                {
                    _matrix[col, row] = matrix._matrix[col, row];
                }
            }
        }
    }

    public float this[int col, int row]
    {
        get { return _matrix[col, row]; }
        set { _matrix[col, row] = value; }
    }

    public void Multiply(float num)
    {
        Multiply(num, ref _matrix);
    }

    public void CopyTo(ref float[,] to)
    {
        CopyTo(_matrix, ref to);
    }

    public void ReplaceFrom(float[,] from)
    {
        _matrix = from;
    }

    public void Fill(float num)
    {
        Fill(num, ref _matrix);
    }

    public void Add(ref float[,] op)
    {
        Add(op, ref _matrix);
    }

    public void Add(ref Matrix2D op)
    {
        Add(op._matrix, ref _matrix);
    }

    public void Normalize()
    {
        Normalize(ref _matrix);
    }

    public void InverseNormalizeMatrix()
    {
        InverseNormalizeMatrix(ref _matrix);
    }

    public float[,] ToArray()
    {
        return _matrix.Clone() as float[,];
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (_matrix.Length <= 0)
        {
            sb.Append("Empty" + Environment.NewLine);
        }

        if (ColNames?.Length > 0)
        {
            foreach (var colName in ColNames)
            {
                sb.Append(colName + ", ");
            }
            sb.AppendLine();
        }

        for (var row = 0; row < RowCount; ++row)
        {
            if (RowNames?.Length > 0)
            {
                sb.Append(RowNames[row] + " ");
            }

            for (var col = 0; col < ColCount; ++col)
            {
                var cellValue = this._matrix[col, row];
                sb.Append(cellValue);
                if (col < ColCount - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }

    public static void Multiply(float num, ref float[,] array)
    {
        if ((array?.Length ?? 0) <= 0) return;

        var colLength = array.GetLength(0);
        var rowLength = array.GetLength(1);

        for (var iCol = 0; iCol < colLength; ++iCol)
        {
            for (var iRow = 0; iRow < rowLength; ++iRow)
            {
                array[iCol, iRow] *= num;
            }
        }
    }

    public static void CopyTo(float[,] from, ref float[,] to)
    {
        if ((from?.Length ?? 0) <= 0 || (to?.Length ?? 0) <= 0) return;

        var fromColLength = from.GetLength(0);
        var fromRowLength = from.GetLength(1);

        var toColLength = to.GetLength(0);
        var toRowLength = to.GetLength(1);

        if (!(fromColLength == toColLength && fromRowLength == toRowLength))
        {
            throw new InvalidOperationException(
                $"Different size of matrices not allow. From: [{fromColLength},{fromRowLength}] To: [{toColLength},{toRowLength}]");
        }

        for (var iCol = 0; iCol < fromColLength; ++iCol)
        {
            for (var iRow = 0; iRow < fromRowLength; ++iRow)
            {
                to[iCol, iRow] = from[iCol, iRow];
            }
        }
    }

    public static void Fill(float num, ref float[,] array)
    {
        if ((array?.Length ?? 0) <= 0) return;

        var colLength = array.GetLength(0);
        var rowLength = array.GetLength(1);

        for (var iCol = 0; iCol < colLength; ++iCol)
        {
            for (var iRow = 0; iRow < rowLength; ++iRow)
            {
                array[iCol, iRow] = num;
            }
        }
    }

    public static void Add(float[,] from, ref float[,] to)
    {
        if ((from?.Length ?? 0) <= 0 || (to?.Length ?? 0) <= 0) return;

        var fromColLength = from.GetLength(0);
        var fromRowLength = from.GetLength(1);

        var toColLength = to.GetLength(0);
        var toRowLength = to.GetLength(1);

        if (!(fromColLength == toColLength && fromRowLength == toRowLength))
        {
            throw new InvalidOperationException(
                $"Different size of matrices not allow. From: [{fromColLength},{fromRowLength}] To: [{toColLength},{toRowLength}]");
        }

        for (var iCol = 0; iCol < fromColLength; ++iCol)
        {
            for (var iRow = 0; iRow < fromRowLength; ++iRow)
            {
                to[iCol, iRow] += from[iCol, iRow];
            }
        }
    }

    public static void Normalize(ref float[,] matrix)
    {
        if ((matrix?.Length ?? 0) <= 0) return;

        var allDistance = matrix.Cast<float>().ToList();
        var min = allDistance.Min();
        var max = allDistance.Max();

        var shooterCount = matrix.GetLength(0);
        var targetCount = matrix.GetLength(1);

        for (var iShooter = 0; iShooter < shooterCount; iShooter++)
        {
            for (var itarget = 0; itarget < targetCount; itarget++)
            {
                var oldValue = matrix[iShooter, itarget];
                var newValue = MathHelper.Scale(oldValue, min, max, 0.0f, 1.0f);
                matrix[iShooter, itarget] = newValue;
            }
        }
    }

    public static void InverseNormalizeMatrix(ref float[,] matrix)
    {
        if ((matrix?.Length ?? 0) <= 0) return;

        var allDistance = matrix.Cast<float>().ToList();
        var min = allDistance.Min();
        var max = allDistance.Max();

        var shooterCount = matrix.GetLength(0);
        var targetCount = matrix.GetLength(1);

        for (var iShooter = 0; iShooter < shooterCount; iShooter++)
        {
            for (var itarget = 0; itarget < targetCount; itarget++)
            {
                matrix[iShooter, itarget] = 1 - matrix[iShooter, itarget];
            }
        }
    }
}