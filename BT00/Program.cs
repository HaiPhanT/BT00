using System;
using System.IO;
using System.Collections.Generic;

namespace BT00
{
    class Program
    {
        struct AdjacencyMatrix
        {
            public Int32 numberOfVertexs;
            public Int32[,] adjacencyMatrix;
        }

        public static string GetFilePath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] splitPath = currentDirectory.Split('\\');
            List<string> splitPathToList = new List<string>(currentDirectory.Split('\\'));

            // cut the path to bin directory
            for (int i = 1; i < 4; ++i)
            {
                splitPathToList.RemoveAt(splitPath.Length - i);
            }
            string filePath = Path.Combine(String.Join("\\", splitPathToList.ToArray()), "adjacency-matrix.txt");
            return filePath;
        }

        public static Int32[,] ReadAdjacencyMatrix(StreamReader streamReader, Int32 numberOfVertexs)
        {
            string buffer;
            Int32[,]  adjacencyMatrix = new Int32[numberOfVertexs, numberOfVertexs];
            Int32 rowIndex = 0;
            while ((buffer = streamReader.ReadLine()) != null)
            {
                string[] line = buffer.Split(" ");
                for (int i = 0; i < line.Length; ++i)
                {
                    adjacencyMatrix[rowIndex, i] = Int32.Parse(line[i]);
                }
                ++rowIndex;
            }

            return adjacencyMatrix;
        }

        public static Boolean IsSymmetricMatrix(Int32[,] adjacencyMatrix)
        {
            if (adjacencyMatrix.GetLength(0) != adjacencyMatrix.GetLength(1))
            {
                // Ma tran doi xung phai la ma tran vuong
                return false;
            }

            // Ma tran doi xung la ma tran co phan tu tai a[i,j] = a[j,i] voi moi i,j la so dong, so cot cua ma tran
            for (int i = 0; i < adjacencyMatrix.GetLength(0) - 1; ++i)
            {
                for (int j = 0; j < adjacencyMatrix.GetLength(0) - 1; ++j)
                {
                    if (adjacencyMatrix[i,j] != adjacencyMatrix[j, i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            string filePath = GetFilePath();
            StreamReader streamReader = new StreamReader(filePath);

            string numberOfVertexs = streamReader.ReadLine();
            AdjacencyMatrix am = new AdjacencyMatrix
            {
                numberOfVertexs = Int32.Parse(numberOfVertexs)
            };

            am.adjacencyMatrix = ReadAdjacencyMatrix(streamReader, am.numberOfVertexs);

            streamReader.Close();


            // Cau 1:
            // a. Print so dinh cua do thi
            Console.WriteLine(am.numberOfVertexs);
            // b. Print ma tran ke bieu dien do thi
            Print2DArray(am.adjacencyMatrix);
            // c. Kiem tra ma tran doi xung => in ra "Ma tran doi xung" hoac "Ma tran khong doi xung"
            Boolean isSymmetricMatrix = IsSymmetricMatrix(am.adjacencyMatrix);
            string matrixType = isSymmetricMatrix ? "Ma tran doi xung" : "Ma tran khong doi xung";
            Console.WriteLine(matrixType);
        }
    }
}
