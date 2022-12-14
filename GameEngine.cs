using System;

namespace LifeSimulation
{
    public class GameEngine
    {
        private readonly int _rows;
        private readonly int _columns;
        private bool[,] field;

        public uint CurrentGeneration { get; private set; }

        public GameEngine(int rows, int columns, int density)
        {
            _rows = rows;
            _columns = columns;
            field = new bool[columns, rows];

            Random random = new Random();

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density) == 0;
                }
            }
        }

        public void NextGeneration()
        {
            var newField = new bool[_columns, _rows];

            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);

                    if (!field[x, y] && neighboursCount == 3)
                        newField[x, y] = true;
                    else if (field[x, y] && (neighboursCount < 2 || neighboursCount > 3))
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];
                }
            }

            field = newField;
            CurrentGeneration++;
        }

        public bool[,] GetCurrentGeneration()
        {
            var result = new bool[_columns, _rows];
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    result[x, y] = field[x, y];
                }
            }
            return result;
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + _columns) % _columns;
                    int row = (y + j + _rows) % _rows;

                    bool isSelfChecking = col == x && row == y;

                    if (field[col, row] && !isSelfChecking)
                        count++;
                }
            }

            return count;
        }

        public void AddLife(int x, int y)
        {
            UpdateLife(x, y, true);
        }

        public void RemoveLife(int x, int y)
        {
            UpdateLife(x, y, false);
        }

        private void UpdateLife(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
                field[x, y] = state;
        }

        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _columns && y < _rows;
        }
    }
}
