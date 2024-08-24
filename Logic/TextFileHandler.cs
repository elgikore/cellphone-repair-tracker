using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CellphoneRepairTrackerApp.Logic
{
    public class TextFileHandler
    {
        private const string _txtFilePath = @"..\..\..\CellphoneRecords.txt";
        public const int parametersOfOneClassPart = 2;
        private const int _numberOfInfoInOneLineInTxtFile = 6;

        public static List<CellphoneRepairDetails> ReadCellphoneRepairDetailsFromTxtFileDefault()
        {
            return ReadCellphoneRepairDetailsFromSpecifiedTxtFile(_txtFilePath);
        }

        public static List<CellphoneRepairDetails> ReadCellphoneRepairDetailsFromSpecifiedTxtFile(string filePath)
        {
            StreamReader txtReader = new StreamReader(filePath);
            if (txtReader == null) throw new ArgumentNullException("File is empty!");

            List<CellphoneRepairDetails> cellphoneRepairDetails = new List<CellphoneRepairDetails>();
            while (!txtReader.EndOfStream)
            {
                List<Part> partsUsed = new List<Part>();
                List<string> servicesUsed = new List<string>();

                string[] txtReaderEntry = txtReader.ReadLine().Split(',');
                if (txtReaderEntry.Length < _numberOfInfoInOneLineInTxtFile || txtReaderEntry.Length > _numberOfInfoInOneLineInTxtFile)
                    throw new FileFormatException($"Number of information in one line is too little or too many. The number of information in one line of text file is {_numberOfInfoInOneLineInTxtFile}.");

                string cellphoneBrand = txtReaderEntry[0];
                string repairman = txtReaderEntry[1];
                string partsUsedToBeSplit = txtReaderEntry[2];
                string servicesUsedToBeSplit = txtReaderEntry[3];
                DateTime requestRepairDate = DateTime.ParseExact(txtReaderEntry[4], "yyyy/MM/dd", null);
                DateTime actualRepairDate = DateTime.ParseExact(txtReaderEntry[5], "yyyy/MM/dd", null);

                //reconstructing the list of partsUsed and servicesUsed
                string[] parts = partsUsedToBeSplit.Split('|');
                string[] services = servicesUsedToBeSplit.Split('|');
                
                for (int i=0; i < parts.Length; i += parametersOfOneClassPart) { partsUsed.Add(new Part(parts[i], Convert.ToUInt32(parts[i + 1]))); } //for partsUsed

                foreach (var service in services) { servicesUsed.Add(service); } //for servicesUsed

                //finally the actual element in cellphoneRepairDetails list
                cellphoneRepairDetails.Add(new CellphoneRepairDetails(cellphoneBrand, repairman, partsUsed, servicesUsed, requestRepairDate, actualRepairDate));
            }

            txtReader.Close();

            return cellphoneRepairDetails;
        }

        public static void AppendCellphoneRepairDetailsToTxt(CellphoneRepairDetails cellphoneRepairDetails)
        {
            StreamWriter writeToTxt = new StreamWriter(_txtFilePath, true);

            //for partsUsed
            string partsUsedToConcatenate = ConcatenateTextForPartsUsed(cellphoneRepairDetails);

            //for servicesUsed
            string servicesUsedToConcatenate = ConcatenateTextForServicesUsed(cellphoneRepairDetails);

            string requestDate = cellphoneRepairDetails.RequestRepairDate.ToString("yyyy/MM/dd");
            string actualRepairDate = cellphoneRepairDetails.RequestRepairDate.ToString("yyyy/MM/dd");

            writeToTxt.WriteLine($"{cellphoneRepairDetails.CellphoneBrand},{cellphoneRepairDetails.Repairman},{partsUsedToConcatenate},{servicesUsedToConcatenate},{requestDate},{actualRepairDate}");
            writeToTxt.Close();
        }

        private static string ConcatenateTextForPartsUsed(CellphoneRepairDetails cellphoneRepairDetails)
        {
            string partsUsedToConcatenate = null;
            cellphoneRepairDetails.PartsUsed.ForEach(partsUsed => partsUsedToConcatenate += $"{partsUsed.PartName}|{partsUsed.Quantity}|"); 
            return partsUsedToConcatenate = partsUsedToConcatenate.Remove(partsUsedToConcatenate.Length - 1); //to get rid of the extra '|"
        }

        private static string ConcatenateTextForServicesUsed(CellphoneRepairDetails cellphoneRepairDetails)
        {
            string servicesUsedToConcatenate = null;
            cellphoneRepairDetails.ServicesUsed.ForEach(servicesUsed => servicesUsedToConcatenate += $"{servicesUsed}|");
            return servicesUsedToConcatenate = servicesUsedToConcatenate.Remove(servicesUsedToConcatenate.Length - 1); //to get rid of the extra '|"
        }
    }
}
