using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ЗЛП
{
    public partial class Form1 : Form
    {
        static double[,] matrix;
        static string output = "";
        static Form1 form = new Form1();

        public Form1()
        {
            InitializeComponent();
            results.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            load();
        }

        private void load()
        {
            string[] mass = File.ReadAllLines("1.txt");
            int m = Convert.ToInt32(mass[0]);
            int n = Convert.ToInt32(mass[1]);

            matrix = new double[n, m];

            for (int p = 0; p < n; p++)
            {
                double [] newMass = mass[p + 2].Split(' ').Select(Double.Parse).ToArray();

                for (int i = 0; i < newMass.Length; i++)
                {
                    matrix[p, i] = newMass[i];
                }
            }
        }

        private void symplexMethod_Click(object sender, EventArgs e)
        {
            string[] mass = File.ReadAllLines("2.txt");
            int m = Convert.ToInt32(mass[0]);
            int n = Convert.ToInt32(mass[1]);

            matrix = new double[n, m];

            for (int p = 0; p < n; p++)
            {
                double[] newMass = mass[p + 2].Split(' ').Select(Double.Parse).ToArray();

                for (int i = 0; i < newMass.Length; i++)
                {
                    matrix[p, i] = newMass[i];
                }
            }
            doSymplexMethod();
        }

        private void doSymplexMethod()
        {
            double[] result = new double[2];
            double[,] table_result;
            Simplex S = new Simplex(matrix);
            table_result = S.Calculate(result);

            results.Text += output;

            int i = 1;
            foreach (double el in result)
            {
                results.Text += "x" + i + " = " + el + "\n";
                i++;
            }
        }

        private void modSymplexMethod_Click(object sender, EventArgs e)
        {
            doModSymplexMethod();
        }

        private void doModSymplexMethod()
        {
            ModifiedSimplex modifiedSimplex = new ModifiedSimplex(matrix);
            double[][]basis = modifiedSimplex.CalculateMod();

            results.Text = output;
            for (int i = 0; i < basis.Length; i++)
            {
                double index = basis[i][0] + 1;
                results.Text += "x" + index + " = " + basis[i][1] + "\n";
            }
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        public class Simplex
        {
            double[,] table;

            int m, n;

            List<int> basis;

            public Simplex(double[,] source)
            {
                m = source.GetLength(0);
                n = source.GetLength(1);
                table = new double[m, n + m - 1];
                basis = new List<int>();

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        if (j < n)
                            table[i, j] = source[i, j];
                        else
                            table[i, j] = 0;
                    }

                    if ((n + i) < table.GetLength(1))
                    {
                        table[i, n + i] = 1;
                        basis.Add(n + i);
                    }
                }

                n = table.GetLength(1);
            }

            public double[,] Calculate(double[] result)
            {
                int mainCol, mainRow;

                output += "z" + "     ";
                for (int i = 1; i < table.GetLength(1); i++)
                {
                    output += "x" + i + "     ";
                }
                output += "\n";

                for (int k = 0; k < m; k++)
                {
                    for (int k1 = 0; k1 < n; k1++)
                    {
                        output += Math.Ceiling(table[k, k1]).ToString();
                        output += "      ";
                    }
                    output += "\n";
                }

                output += "\n\n";

                while (!IsItEnd())
                {
                    mainCol = findMainCol();
                    mainRow = findMainRow(mainCol);
                    basis[mainRow] = mainCol;

                    double[,] new_table = new double[m, n];

                    for (int j = 0; j < n; j++)
                        new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                    for (int i = 0; i < m; i++)
                    {
                        if (i == mainRow)
                            continue;

                        for (int j = 0; j < n; j++)
                            new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                    }
                    table = new_table;

                    output += "z" + "     ";
                    for (int i = 1; i < table.GetLength(1); i++)
                    {
                        output += "x" + i + "     ";
                    }
                    output += "\n";

                    for (int k = 0; k < m; k++)
                    {
                        for (int k1 = 0; k1 < n; k1++)
                        {
                            output += Math.Ceiling(table[k, k1]).ToString();
                            if (Math.Ceiling(table[k, k1]).ToString().Length == 1)
                                output += "      ";
                            else output += "     ";
                        }
                        output += "\n";
                    }

                    output += "\n\n";
                }

                for (int i = 0; i < result.Length; i++)
                {
                    int k = basis.IndexOf(i + 1);
                    if (k != -1)
                        result[i] = table[k, 0];
                    else
                        result[i] = 0;
                }

                return table;
            }

            private bool IsItEnd()
            {
                bool flag = true;

                for (int j = 1; j < n; j++)
                {
                    if (table[m - 1, j] < 0)
                    {
                        flag = false;
                        break;
                    }
                }

                return flag;
            }

            private int findMainCol()
            {
                int mainCol = 1;

                for (int j = 2; j < n; j++)
                    if (table[m - 1, j] < table[m - 1, mainCol])
                        mainCol = j;

                return mainCol;
            }

            private int findMainRow(int mainCol)
            {
                int mainRow = 0;

                for (int i = 0; i < m - 1; i++)
                    if (table[i, mainCol] > 0)
                    {
                        mainRow = i;
                        break;
                    }

                for (int i = mainRow + 1; i < m - 1; i++)
                    if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                        mainRow = i;

                return mainRow;
            }

        }

        public class ModifiedSimplex
        {
            int n, m;

            double[][] table;


            double[][] basis;

            public ModifiedSimplex(double[,]source)
            {
                table = new double[source.GetLength(0)][];
                for (int i = 0; i < source.GetLength(0); i++)
                {
                    table[i] = new double[source.GetLength(1)];
                }

                for (int i = 0; i < source.GetLength(0); i++)
                    for (int j = 0; j < source.GetLength(1); j++)
                        table[i][j] = source[i, j];

                n = source.GetLength(0);
                m = source.GetLength(1);

                basis = new double[n - 1][];
                for (int i = 0; i < n - 1; i++)
                    basis[i] = new double[2];
            }

            public double[][] CalculateMod()
            {
                double[][] podTable = new double[n - 1][];
                for (int i = 0; i < n - 1; i++)
                    podTable[i] = new double[n - 1];

                double[][] inv = new double[n - 1][];
                for (int i = 0; i < n - 1; i++)
                    inv[i] = new double[n - 1];

                double[][] koefCBaz = new double[1][];
                koefCBaz[0] = new double[n - 1];

                double[][] numb = new double[n - 1][];
                for (int i = 0; i < n - 1; i++)
                    numb[i] = new double[1];

                for (int i = 0; i < n - 1; i++)
                    numb[i][0] = table[i][0];

                double[][] b = new double[n][];
                for (int i = 0; i < n; i++)
                    b[i] = new double[1];

                for (int jSet = 0; jSet < m; jSet++)
                {
                    for (int i = 0; i < n - 1; i++)
                        for (int j = 0; j < n - 1; j++)
                        {
                            podTable[i][j] = table[i][j + 1 + jSet];
                            koefCBaz[0][j] = 0 - table[n - 1][jSet + 1 + j];
                            basis[i][0] = jSet + i;
                            //MessageBox.Show(podTable[i][j].ToString() + " " + koefCBaz[j][0].ToString());
                        }

                    inv = MatrixInverse(podTable);

                    string line1 = "";
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = 0; j < n - 1; j++)
                            line1 += Math.Round(inv[i][j], 2) + " ";
                        line1 += "\n";
                    }

                    output += "B^(-1): \n" + line1 + "\n"; 

                    b = Multiplication(inv, numb);

                    for (int i = 0; i < n - 1; i++)
                    {
                        //MessageBox.Show(b[i][0].ToString());
                        basis[i][1] = b[i][0];
                    }

                    if (isPositive(b))
                        break;
                }

                double[][] u = Multiplication(koefCBaz, inv);

                string line = "";
                for (int i = 0; i < u.Length; i++)
                {
                    for (int j = 0; j < u[0].Length; j++)
                        line += Math.Round(u[i][j], 2) + " ";
                    line += "\n";
                }

                output += "u(0) = " + line + "\n";

                double[] delta = new double[m - 1];

                double skalar = 0;
                for (int i = 0; i < m - 1; i++)
                {
                    skalar = 0;
                    for (int j = 0; j < u[0].Length; j++)
                    {
                        skalar += u[0][j] * table[j][i + 1];
                        //MessageBox.Show(u[0][j].ToString() + " " + table[j][i + 1].ToString() + " " + skalar);
                    }
                    //MessageBox.Show((0 - table[n - 1][i + 1]).ToString() + " " + skalar.ToString());
                    delta[i] = (0 - table[n - 1][i + 1]) - skalar;
                    if (delta[i] > 0 && delta[i] - 0.00000001 < 0 || delta[i] < 0 && delta[i] + 0.0000001 > 0)
                        delta[i] = 0;

                    //MessageBox.Show(delta[i].ToString());
                }

                line = "";
                for (int i = 0; i < delta.Length; i++)
                {
                    line +=  Math.Round(delta[i], 2) + " ";
                }
                output += "дельта = " + line + "\n";

                if (!IsEnd(delta))
                {
                    int In = FindIn(delta);
                    int Out = FindOut(inv, In, table, n, b);

                    int index = 0;
                    for (int i = 0; i < n - 1; i++)
                    {
                        if (basis[i][0] == Out)
                            index = i;
                    }
                    basis[index][0] = In;

                    /*string line2 = "";
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = 0; j < 2; j++)
                            line2 += basis[i][j] + " ";
                        line2 += "\n";
                    }*/

                    //MessageBox.Show(line2);

                    koefCBaz[0][index] = table[n - 1][1 + Out];

                    //MessageBox.Show(koefCBaz[0][index].ToString());

                    for (int i = 0; i < n - 1; i++)
                        podTable[i][index] = table[i][1 + In];

                    /*string line2 = "";
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = 0; j < n - 1; j++)
                            line2 += podTable[i][j] + " ";
                        line2 += "\n";
                    }
                    
                    MessageBox.Show(line2);*/

                    inv = MatrixInverse(podTable);

                    string line1 = "";
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = 0; j < n - 1; j++)
                            line1 += Math.Round(inv[i][j], 2) + " ";
                        line1 += "\n";
                    }

                    output += "B^(-1) = \n" + line1 + "\n";

                    b = Multiplication(inv, numb);

                    for (int i = 0; i < n - 1; i++)
                    {
                        basis[i][1] = b[i][0];
                    }

                    u = Multiplication(koefCBaz, inv);

                    line = "";
                    for (int i = 0; i < u.Length; i++)
                    {
                        for (int j = 0; j < u[0].Length; j++)
                            line += Math.Round(u[i][j], 2) + " ";
                        line += "\n";
                    }

                    output += "u = " + line + "\n";

                    for (int i = 0; i < m - 1; i++)
                    {
                        skalar = 0;
                        for (int j = 0; j < u[0].Length; j++)
                        {
                            skalar += u[0][j] * table[j][i + 1];
                            //MessageBox.Show(u[0][j].ToString() + " " + table[j][i + 1].ToString() + " " + skalar);
                        }
                        //MessageBox.Show((0 - table[n - 1][i + 1]).ToString() + " " + skalar.ToString());
                        delta[i] = (0 - table[n - 1][i + 1]) - skalar;
                        if (delta[i] > 0 && delta[i] - 0.00000001 < 0 || delta[i] < 0 && delta[i] + 0.0000001 > 0)
                            delta[i] = 0;

                        //MessageBox.Show(delta[i].ToString());
                    }
                    line = "";
                    for (int i = 0; i < delta.Length; i++)
                    {
                        line += Math.Round(delta[i], 2) + " ";
                    }
                    output += "дельта = " + line + "\n";
                }

                return basis;

            }

            static bool IsEnd(double[] delta)
            {
                for (int i = 0; i < delta.Length; i++)
                {
                    if (delta[i] < 0)
                        return false;
                }
                return true;
            }

            static int FindIn(double[] delta)
            {
                double min = 0;
                int index = 0;

                for (int i = 0; i < delta.Length; i++)
                    if (delta[i] < min)
                    {
                        min = delta[i];
                        index = i;
                    }

                return index;
            }

            static int FindOut(double[][]inv, int In, double[][]table, int n, double[][] b)
            {
                double[][] ved = new double[n - 1][];
                for (int i = 0; i < n - 1; i++)
                    ved[i] = new double[1];
                for (int j = 0; j < n - 1; j++)
                {
                    ved[j][0] = table[j][In + 1];
                    //MessageBox.Show(table[j][In + 1].ToString());
                }

                double[][] a = Multiplication(inv, ved);

                double min = 100000;
                int index = 0;
                for (int i = 0; i < n - 1; i++)
                {
                    if (a[i][0] > 0)
                    {
                        if (b[i][0] / a[i][0] < min)
                        {
                            min = b[i][0] / a[i][0];
                            index = i;
                        }
                    }
                }
                return index;
            }

            static bool isPositive(double[][]matrix)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix[0].Length; j++)
                        if (matrix[i][j] < 0)
                            return false;
                return true;
            }

            static double[][] Multiplication(double[][] a, double[][] b)
            {
                if (a[0].Length != b.Length) throw new Exception("Матрицы нельзя перемножить");
                double[][] r = new double[a.GetLength(0)][];
                for (int i = 0; i < a.GetLength(0); i++)
                    r[i] = new double[b[0].Length];

                for (int i = 0; i < a.Length; i++)
                {
                    for (int j = 0; j < b[0].Length; j++)
                    {
                        for (int k = 0; k < b.Length; k++)
                        {
                            r[i][j] += a[i][k] * b[k][j];
                            //MessageBox.Show(r[i][j].ToString() + " = " + a[i][k].ToString() + " * " + b[k][j].ToString());
                        }
                    }
                }
                return r;
            }

            static double[][] MatrixCreate(int rows, int cols)
            {
                double[][] result = new double[rows][];
                for (int i = 0; i < rows; ++i)
                    result[i] = new double[cols];
                return result;
            }

            static double[][] MatrixIdentity(int n)
            {
                // return an n x n Identity matrix
                double[][] result = MatrixCreate(n, n);
                for (int i = 0; i < n; ++i)
                    result[i][i] = 1.0;

                return result;
            }

            static double[][] MatrixProduct(double[][] matrixA, double[][] matrixB)
            {
                int aRows = matrixA.Length; int aCols = matrixA[0].Length;
                int bRows = matrixB.Length; int bCols = matrixB[0].Length;
                if (aCols != bRows)
                    throw new Exception("Non-conformable matrices in MatrixProduct");

                double[][] result = MatrixCreate(aRows, bCols);

                for (int i = 0; i < aRows; ++i) // each row of A
                    for (int j = 0; j < bCols; ++j) // each col of B
                        for (int k = 0; k < aCols; ++k) // could use k less-than bRows
                            result[i][j] += matrixA[i][k] * matrixB[k][j];

                return result;
            }

            static double[][] MatrixInverse(double[][] matrix)
            {
                int n = matrix.Length;
                double[][] result = MatrixDuplicate(matrix);

                int[] perm;
                int toggle;
                double[][] lum = MatrixDecompose(matrix, out perm,
                  out toggle);
                if (lum == null)
                    throw new Exception("Unable to compute inverse");

                double[] b = new double[n];
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        if (i == perm[j])
                            b[j] = 1.0;
                        else
                            b[j] = 0.0;
                    }

                    double[] x = HelperSolve(lum, b);

                    for (int j = 0; j < n; ++j)
                        result[j][i] = x[j];
                }
                return result;
            }

            static double[][] MatrixDuplicate(double[][] matrix)
            {
                // allocates/creates a duplicate of a matrix.
                double[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
                for (int i = 0; i < matrix.Length; ++i) // copy the values
                    for (int j = 0; j < matrix[i].Length; ++j)
                        result[i][j] = matrix[i][j];
                return result;
            }

            static double[] HelperSolve(double[][] luMatrix, double[] b)
            {
                // before calling this helper, permute b using the perm array
                // from MatrixDecompose that generated luMatrix
                int n = luMatrix.Length;
                double[] x = new double[n];
                b.CopyTo(x, 0);

                for (int i = 1; i < n; ++i)
                {
                    double sum = x[i];
                    for (int j = 0; j < i; ++j)
                        sum -= luMatrix[i][j] * x[j];
                    x[i] = sum;
                }

                x[n - 1] /= luMatrix[n - 1][n - 1];
                for (int i = n - 2; i >= 0; --i)
                {
                    double sum = x[i];
                    for (int j = i + 1; j < n; ++j)
                        sum -= luMatrix[i][j] * x[j];
                    x[i] = sum / luMatrix[i][i];
                }

                return x;
            }

            static double[][] MatrixDecompose(double[][] matrix, out int[] perm, out int toggle)
            {
                // Doolittle LUP decomposition with partial pivoting.
                // rerturns: result is L (with 1s on diagonal) and U;
                // perm holds row permutations; toggle is +1 or -1 (even or odd)
                int rows = matrix.Length;
                int cols = matrix[0].Length; // assume square
                if (rows != cols)
                    throw new Exception("Attempt to decompose a non-square m");

                int n = rows; // convenience

                double[][] result = MatrixDuplicate(matrix);

                perm = new int[n]; // set up row permutation result
                for (int i = 0; i < n; ++i) { perm[i] = i; }

                toggle = 1; // toggle tracks row swaps.
                            // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

                for (int j = 0; j < n - 1; ++j) // each column
                {
                    double colMax = Math.Abs(result[j][j]); // find largest val in col
                    int pRow = j;
                    //for (int i = j + 1; i less-than n; ++i)
                    //{
                    //  if (result[i][j] greater-than colMax)
                    //  {
                    //    colMax = result[i][j];
                    //    pRow = i;
                    //  }
                    //}

                    // reader Matt V needed this:
                    for (int i = j + 1; i < n; ++i)
                    {
                        if (Math.Abs(result[i][j]) > colMax)
                        {
                            colMax = Math.Abs(result[i][j]);
                            pRow = i;
                        }
                    }
                    // Not sure if this approach is needed always, or not.

                    if (pRow != j) // if largest value not on pivot, swap rows
                    {
                        double[] rowPtr = result[pRow];
                        result[pRow] = result[j];
                        result[j] = rowPtr;

                        int tmp = perm[pRow]; // and swap perm info
                        perm[pRow] = perm[j];
                        perm[j] = tmp;

                        toggle = -toggle; // adjust the row-swap toggle
                    }

                    // --------------------------------------------------
                    // This part added later (not in original)
                    // and replaces the 'return null' below.
                    // if there is a 0 on the diagonal, find a good row
                    // from i = j+1 down that doesn't have
                    // a 0 in column j, and swap that good row with row j
                    // --------------------------------------------------

                    if (result[j][j] == 0.0)
                    {
                        // find a good row to swap
                        int goodRow = -1;
                        for (int row = j + 1; row < n; ++row)
                        {
                            if (result[row][j] != 0.0)
                                goodRow = row;
                        }

                        if (goodRow == -1)
                            throw new Exception("Cannot use Doolittle's method");

                        // swap rows so 0.0 no longer on diagonal
                        double[] rowPtr = result[goodRow];
                        result[goodRow] = result[j];
                        result[j] = rowPtr;

                        int tmp = perm[goodRow]; // and swap perm info
                        perm[goodRow] = perm[j];
                        perm[j] = tmp;

                        toggle = -toggle; // adjust the row-swap toggle
                    }
                    // --------------------------------------------------
                    // if diagonal after swap is zero . .
                    //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
                    //  return null; // consider a throw

                    for (int i = j + 1; i < n; ++i)
                    {
                        result[i][j] /= result[j][j];
                        for (int k = j + 1; k < n; ++k)
                        {
                            result[i][k] -= result[i][j] * result[j][k];
                        }
                    }


                } // main j column loop

                return result;
            }
        }

        private void modSymplexMethod_Click_1(object sender, EventArgs e)
        {
            string[] mass = File.ReadAllLines("1.txt");
            int m = Convert.ToInt32(mass[0]);
            int n = Convert.ToInt32(mass[1]);

            matrix = new double[n, m];

            for (int p = 0; p < n; p++)
            {
                double[] newMass = mass[p + 2].Split(' ').Select(Double.Parse).ToArray();

                for (int i = 0; i < newMass.Length; i++)
                {
                    matrix[p, i] = newMass[i];
                }
            }
            doModSymplexMethod();
        }
    }
}
