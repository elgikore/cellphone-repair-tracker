using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CellphoneRepairTrackerApp.Logic
{
    public class CellphoneRepairDetails
    {
        private List<char> _invalidChars = "!@#$%^&*()_[]{}<>?/\\;:~`".ToCharArray().ToList();
        private const char _comma = ',';
        private const string _commaExceptionMessage = "Please don't type comma as it would interfere the text file when loading.";

        public string CellphoneBrand { get; set; }
        public DateTime RequestRepairDate { get; set; }
        public DateTime ActualRepairDate { get; set; }
        public List<Part> PartsUsed { get; set; }
        public string Repairman { get; set; }
        public List<string> ServicesUsed { get; set; }

        public CellphoneRepairDetails(string cellphoneBrand, string repairman, List<Part> partsUsed, List<string> servicesUsed, DateTime requestRepairDate, DateTime actualRepairDate)
        {
            //null checcking
            if (string.IsNullOrEmpty(cellphoneBrand)) throw new ArgumentNullException("Cellphone");
            if (string.IsNullOrEmpty(repairman)) throw new ArgumentNullException("Repairman");
            if (partsUsed == null) throw new ArgumentNullException("Parts Used");
            if (servicesUsed == null || servicesUsed.Count == 0) throw new ArgumentNullException("Services Used");
            if (requestRepairDate == null || actualRepairDate == null) throw new ArgumentNullException("Date invalid");

            //valid name checking of the repairman
            foreach (char character in _invalidChars) { if (repairman.Contains(character)) throw new ArgumentException("Invalid name of the repairman!"); }
            if (!Regex.IsMatch(repairman, "[a-zA-Z]")) throw new ArgumentException("Repairman's name must have letters!");
            if (Regex.IsMatch(repairman, "[0-9]")) throw new ArgumentException("Repairman's name mustn't have numbers!");

            //no commas so that it would not interfere when loading the text file
            if (cellphoneBrand.Contains(_comma)) throw new ArgumentException(_commaExceptionMessage);
            if (repairman.Contains(_comma)) throw new ArgumentException(_commaExceptionMessage + " Rather, use the \"First Name - Middle Initial - Last Name\" order.");

            //actual repair date should not be less than the request repair date
            if (actualRepairDate < requestRepairDate) throw new ArgumentOutOfRangeException("Actual repair date is less than the requested repair date.");

            CellphoneBrand = cellphoneBrand;
            Repairman = repairman;
            PartsUsed = partsUsed;
            ServicesUsed = servicesUsed;
            RequestRepairDate = requestRepairDate;
            ActualRepairDate = actualRepairDate;
        }

        public override string ToString() //for display in textbox
        {
            const int charSpacing = -40;

            StringBuilder sb = new StringBuilder();
            sb.Append($"{"Cellphone Brand:", charSpacing}\t{CellphoneBrand}\n");
            sb.Append($"{"Repairman:", charSpacing}\t{Repairman}\n");
            sb.Append($"{"Request Repair Date:", charSpacing}\t{RequestRepairDate.ToString("yyyy/MM/dd")}\n");
            sb.Append($"{"Actual Repair Date:", charSpacing}\t{ActualRepairDate.ToString("yyyy/MM/dd")}\n\n");
            sb.Append("---------------------------------------------------------------------------------------\n\n");
            sb.Append("Parts Used in repaing:\n");
            PartsUsed.ForEach(partsUsed => sb.Append($"{partsUsed.Quantity}{"x", -5}{partsUsed.PartName}\n"));
            sb.Append("\nServices Used:\n");
            ServicesUsed.ForEach(serviceUsed => sb.Append($"- {serviceUsed}\n"));
            
            return sb.ToString();
        }
    }
}
