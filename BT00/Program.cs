using System;
using System.IO;
using System.Collections.Generic;

namespace BT00
{
    class Program
    {
        struct AdjacencyMatrix
        {
            public int numberOfVertexs;
            public int[,] adjacencyMatrix;
        }

        struct AdjacencyList
        {
            public int numberOfVertexs;
            public int[,] adjacencyList;
        }

        public static string GetFilePath(string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] splitPath = currentDirectory.Split('\\');
            List<string> splitPathToList = new List<string>(currentDirectory.Split('\\'));

            // cut the path to bin directory
            for (int i = 1; i < 4; ++i)
            {
                splitPathToList.RemoveAt(splitPath.Length - i);
            }
            string filePath = Path.Combine(String.Join("\\", splitPathToList.ToArray()), fileName);
            return filePath;
        }

        public static int[,] ReadMatrix(StreamReader streamReader, int numberOfVertexs)
        {
            string buffer;
            int[,] adjacencyMatrix = new int[numberOfVertexs, numberOfVertexs];
            int rowIndex = 0;
            while ((buffer = streamReader.ReadLine()) != null)
            {
                string[] line = buffer.Split(" ");
                for (int i = 0; i < line.Length; ++i)
                {
                    adjacencyMatrix[rowIndex, i] = int.Parse(line[i]);
                }
                ++rowIndex;
            }

            return adjacencyMatrix;
        }

        public static Boolean IsSymmetricMatrix(int[,] adjacencyMatrix)
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
                    if (adjacencyMatrix[i, j] != adjacencyMatrix[j, i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Boolean IsBipartiteGraph(int[,] adjacencyList)
        {
            if (adjacencyList.Length == 0)
            {
                return false;
            }

            List<List<int>> edgeVertexs = new List<List<int>>();

            // luu danh sach 2 chieu, trong do tai vi tri dinh thu i(index cua mang) se chua tap dinh co canh noi voi no
            for (int i = 0; i < adjacencyList.GetLength(0); ++i)
            {
                // chi xet nhung dinh co canh ke
                if (adjacencyList[0, 0] == 0)
                {
                    continue;
                }

                List<int> currentEdgeVertexs = new List<int>();

                for (int j = 1; j < adjacencyList.GetLength(1) - 1; ++j)
                {
                    currentEdgeVertexs.Add(adjacencyList[i, j]);
                }

                edgeVertexs.Add(currentEdgeVertexs);
            }

            // kiem tra xem voi moi dinh thu j trong tap dinh cua dinh i, co ton tai dinh thu i trong tap dinh cua dinh j hay khong
            for (int row = 0; row < edgeVertexs.Count - 1; ++row)
            {
                for (int col = 0; col < edgeVertexs[row].Count - 1; ++col)
                {
                    // neu co mot cap dinh khong thoa dieu kien thi ket luan khong phai do thi hai chieu
                    int currentEdgeIndex = edgeVertexs[row][col];
                    if (!edgeVertexs[currentEdgeIndex].Contains(row))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static int[,] ConvertAMToAL(int[,] adjacencyMatrix)
        {
            int length = adjacencyMatrix.GetLength(0);
            int[,] adjacencyList = new int[length, length];
            for (int i = 0; i < length; ++i)
            {
                int numberOfEdges = 0;
                int startIndexPos = 1;
                for (int j = 0; j < length; ++j)
                {
                    if (adjacencyMatrix[i, j] != 0)
                    {
                        adjacencyList[i, startIndexPos] = j;
                        ++numberOfEdges;
                        ++startIndexPos;
                    }
                }
                adjacencyList[i, 0] = numberOfEdges;
            }
            return adjacencyList;
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
            string filePath = GetFilePath("adjacency-matrix.txt");
            StreamReader streamReader = new StreamReader(filePath);

            string numberOfVertexs = streamReader.ReadLine();
            AdjacencyMatrix AM = new AdjacencyMatrix
            {
                numberOfVertexs = int.Parse(numberOfVertexs)
            };

            AM.adjacencyMatrix = ReadMatrix(streamReader, AM.numberOfVertexs);
            streamReader.Close();

            // Cau 1:
            // a. Print so dinh cua do thi
            Console.WriteLine(AM.numberOfVertexs);
            // b. Print ma tran ke bieu dien do thi
            Print2DArray(AM.adjacencyMatrix);
            // c. Kiem tra ma tran doi xung => in ra "Ma tran doi xung" hoac "Ma tran khong doi xung"
            Boolean isSymmetricMatrix = IsSymmetricMatrix(AM.adjacencyMatrix);
            string matrixType = isSymmetricMatrix ? "Ma tran doi xung" : "Ma tran khong doi xung";
            Console.WriteLine(matrixType);

            // ----------------------------------------------------------------------

            filePath = GetFilePath("adjacency-list.txt");
            streamReader = new StreamReader(filePath);

            AdjacencyList AL = new AdjacencyList
            {
                numberOfVertexs = int.Parse(streamReader.ReadLine())
            };

            AL.adjacencyList = ReadMatrix(streamReader, AL.numberOfVertexs);
            streamReader.Close();

            // Cau 2:
            // a. Print so dinh cua do thi
            Console.WriteLine(AL.numberOfVertexs);
            // b. Print danh sach ke bieu dien do thi
            Print2DArray(AL.adjacencyList);
            // c. Kiem tra moi cap dinh {A,B}, neu A -> B va B -> A, print "Danh sach ke bieu dien do thi hai chieu", nguoc lai in "Danh sach ke bieu dien do thi mot chieu"
            Boolean isBipartiteGraph = IsBipartiteGraph(AL.adjacencyList);
            matrixType = isBipartiteGraph ? "Danh sach ke bieu dien do thi hai chieu" : "Danh sach ke bieu dien do thi mot chieu";
            Console.WriteLine(matrixType);

            // Cau 3:
            // 
            Console.WriteLine("-----------------------");
            int[,] result = ConvertAMToAL(AM.adjacencyMatrix);
            Print2DArray(result);

            // Cau 4:
        }
    }
}
