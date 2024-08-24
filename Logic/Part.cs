using System;
using System.Collections.Generic;
using System.Text;

namespace CellphoneRepairTrackerApp.Logic
{
    public class Part
    {
        public string PartName { get; set; }
        public uint Quantity { get; set; }

        public Part(string partName, uint quantity)
        {
            if (string.IsNullOrEmpty(partName)) throw new ArgumentException("Invalid part name!");
            if (quantity == 0) throw new ArgumentOutOfRangeException("Quantity");

            PartName = partName;
            Quantity = quantity;
        }
    }
}
