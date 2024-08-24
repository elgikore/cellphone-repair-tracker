using CellphoneRepairTrackerApp.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CellphoneRepairTrackerApp
{
    /// <summary>
    /// Interaction logic for AddCellphoneRepairDetailsWindow.xaml
    /// </summary>
    public partial class AddCellphoneRepairDetailsWindow : Window
    {
        public AddCellphoneRepairDetailsWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("If you close the Add Cellphone Repair Details window, your changes would not be saved.", "Cancel",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) { Close(); }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CellphoneBrandInput.Text) || string.IsNullOrWhiteSpace(RepairmanInput.Text) || string.IsNullOrWhiteSpace(PartUsed.Text) ||
                string.IsNullOrWhiteSpace(QuantityOfEach.Text) || string.IsNullOrWhiteSpace(ServicesUsedInput.Text) || RequestRepairDateInput.SelectedDate == null || 
                ActualRepairDateInput.SelectedDate == null)
            {
                MessageBox.Show("One or more fields are not filled up yet!", "Empty field/s error", MessageBoxButton.OK, MessageBoxImage.Error);
            }    
            else if (QuantityOfEach.Text.Contains('-')) MessageBox.Show("Invalid quantity value", "Negative Quantity", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                string cellphoneBrand = CellphoneBrandInput.Text.Trim();
                string repairman = RepairmanInput.Text.Trim();
                DateTime requestRepairDate = (DateTime)RequestRepairDateInput.SelectedDate;
                DateTime actualRepairDate = (DateTime)ActualRepairDateInput.SelectedDate;

                //for reconstructing partsUsed
                List<Part> partsUsed = new List<Part>();

                string[] partsUsedTemp = PartUsed.Text.Trim().Split(',');
                string[] quantityOfEachTemp = QuantityOfEach.Text.Trim().Split(',');
                if (partsUsedTemp.Length != quantityOfEachTemp.Length) MessageBox.Show("The number of parts used and its corresponding quantities doesn't match.",
                    "Doesn't match", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    for (int i = 0; i < partsUsedTemp.Length; i++) partsUsed.Add(new Part(partsUsedTemp[i].Trim(), Convert.ToUInt32(quantityOfEachTemp[i].Trim())));

                    //for reconstructing servicesUsed
                    string[] servicesUsedTemp = ServicesUsedInput.Text.Trim().Split(',');
                    List<string> servicesUsed = new List<string>();
                    foreach (var service in servicesUsedTemp) { servicesUsed.Add(service.Trim()); }

                    //finally saving the entered data
                    try
                    {
                        CellphoneRepairDetails cellphoneRepairDetails = new CellphoneRepairDetails(cellphoneBrand, repairman, partsUsed, servicesUsed, requestRepairDate, actualRepairDate);
                        TextFileHandler.AppendCellphoneRepairDetailsToTxt(cellphoneRepairDetails);

                        if (MessageBox.Show("Details are saved.\nDo you want to input another record?", "Success!", MessageBoxButton.YesNo,
                        MessageBoxImage.Information) == MessageBoxResult.No) { Close(); }
                        else
                        {
                            CellphoneBrandInput.Clear();
                            RepairmanInput.Clear();
                            PartUsed.Clear();
                            QuantityOfEach.Clear();
                            ServicesUsedInput.Clear();
                            RequestRepairDateInput.SelectedDate = null;
                            ActualRepairDateInput.SelectedDate = null;
                        }
                    }
                    catch (ArgumentOutOfRangeException exception) { MessageBox.Show(exception.Message, "Date error", MessageBoxButton.OK, MessageBoxImage.Error); }
                    catch (ArgumentException exception) { MessageBox.Show(exception.Message, "Formatting or input error", MessageBoxButton.OK, MessageBoxImage.Error); }
                }  
            }
        }
    }
}
