using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class UndoRedo
    {
        public SpreadsheetCell oldCell;

        public string changedPropertyName;

        public UndoRedo(SpreadsheetCell undoRedoCell, string whatChanged)
        {
            this.oldCell = undoRedoCell;
            this.changedPropertyName = whatChanged;
        }

        public void Evaluate(ref SpreadsheetCell senderCell)
        {
            senderCell.Text = this.oldCell.Text;
            senderCell.BGColor = this.oldCell.BGColor;
        }

        public Tuple<int, int> GetCoordinates()
        {
            return new Tuple<int, int>(this.oldCell.RowIndex, this.oldCell.ColumeIndex);
        }
    }
}
