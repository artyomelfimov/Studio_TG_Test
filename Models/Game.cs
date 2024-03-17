using System.Linq;
using System.Text.Json.Serialization;

namespace StudioTG_Test.Models
{
    public class Game : NewGameRequest
    {
        [JsonPropertyName("Game_id")]
        public Guid Game_id { get; set; }
        public bool Completed { get; set; }
        public bool Started { get; set; } = true;
        public List<List<string>> Field
        {
            get
            {
                var list = new List<List<string>>();
                for (int i = 0; i < Height; i++)
                {
                    list.Add([]);
                    for (int j = 0; j < Width; j++)
                    {
                        list[i].Add(_field[i][j].GetCellValue(false));
                    }
                }
                return list;
            }
        }
        private List<List<Cell>> _field;

        public Game(NewGameRequest requestedparams)
        {
            Game_id = Guid.NewGuid();
            Height = requestedparams.Height;
            Width = requestedparams.Width;
            MinesCount = requestedparams.MinesCount;
            SetupField();
        }

        private void SetupField()
        {
            _field = [];
            for (int i = 0; i < Height; i++)
            {
                _field.Add([]);
                for (int j = 0; j < Width; j++)
                {
                    _field[i].Add(new Cell() { Value = "0", IsHidden = true, Row = i, Col = j });
                }
            }
            
        }

        private void LandMines(Cell cell)
        {
            var rand = new Random();
            foreach (var item in _field.SelectMany(row => row).Except([cell]).OrderBy(x => rand.Next()).Take(MinesCount))
            {
                item.Value = "X";
            }
        }

        private void MarkField()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (_field[i][j].GetCellValue() != "X")
                    {
                        int count = 0;
                        for (int x = i - 1; x < i + 2; x++)
                        {
                            for (int y = j - 1; y < j + 2; y++)
                            {
                                if (x < 0 || y < 0 || x > Height - 1 || y > Width - 1)
                                    continue;
                                if (_field[x][y].GetCellValue() == "X")
                                    count++;
                            }
                        }
                        _field[i][j].Value = count.ToString();
                    }
                }
            }
        }

        public Game GameTurn(GameTurnRequest request)
        {
            Cell cell = _field[request.Row][request.Col];
            if (Started)
            {
                LandMines(cell);
                MarkField();
                Started = false;
            }
            OpenSomeCell(cell);
            return this;
        }
        public void OpenSomeCell(Cell cell)
        {
            switch (cell.GetCellValue())
            {
                case "0":
                    OpenZeroCells(cell);
                    foreach (Cell relatedcell in GetRelatedCells(cell))
                    {
                        if (relatedcell.GetCellValue() != "X")
                        {
                            OpenSomeCell(relatedcell);
                        }
                    }   
                    break;
                case "X":
                    Completed = true;
                    OpenAllCells();
                    break;
                default:
                    OpenCell(cell);
                    break;
            }
        }
        private bool HasHiddenEmptyCell()
        {
            return _field.Any(row => row.Any(item => item.IsHidden && item.GetCellValue() != "X"));
        }

        private void OpenZeroCells(Cell cell)
        {
                cell.IsHidden = false;
        }

        private void OpenAllCells()
        {
            foreach (var item in _field.SelectMany(row => row))
            {
                item.IsHidden = false;
            }
        }
        public List<Cell> GetRelatedCells(Cell cell)
        {
            List<Cell> relatedCells = new List<Cell>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue; 

                    int newRow = cell.Row + i;
                    int newCol = cell.Col + j;

                    if (newRow >= 0 && newRow < Height && newCol >= 0 && newCol < Width && _field[newRow][newCol].IsHidden == true)
                    {
                        relatedCells.Add(_field[newRow][newCol]);
                    }
                }
            }

            return relatedCells;
        }
        private void OpenCell(Cell cell)
        {
            cell.IsHidden = false;
            if (!HasHiddenEmptyCell())
            {
                Completed = true;
                OpenAllCells();
                foreach (var item in _field.SelectMany(row => row).Where(i => i.GetCellValue() == "X"))
                {
                    item.Value = "M";
                }
            }
        }
    }

    
}
