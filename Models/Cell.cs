using System.Text.Json.Serialization;

namespace StudioTG_Test.Models
{
 
    public class Cell
    {
        public string GetCellValue(bool ignoreHidden = true)
        {
            return IsHidden && !ignoreHidden ? " " : Value;
        }
        public string Value { private get; set; }
        public bool IsHidden { get; set; }
        public int Col { get; set; }
        public int Row { get; set; }
    }
}
