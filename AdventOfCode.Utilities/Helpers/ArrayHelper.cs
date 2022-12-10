namespace AdventOfCodeUtilities.Helpers
{
    public static class ArrayHelper
    {
        public static bool IsValidCoordinate<T>(int x, int y, T[][] array)
        {
            return !(x < 0 || y < 0 || x > array.Length - 1 || y > array[0].Length - 1);
        }

        public static void ArrayPrinter<T>(T[][] array)
        {
            foreach (var row in array)
            {
                foreach (var item in row)
                {
                    Console.Write(item);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static void ArrayPrinter<T>(T[,] array)
        {
            var row = array.GetLength(0);
            var col = array.GetLength(1);

            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    Console.Write(array[i, j]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static bool ArraysAreTheSame<T>(T[][] arr1, T[][] arr2) where T : IComparable<T>
        {
            var row = arr1.Length;
            var col = arr1[0].Length;

            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    if (arr1[i][j].CompareTo(arr2[i][j]) != 0)
                        return false;
                }
            }

            return true;
        }
    }
}