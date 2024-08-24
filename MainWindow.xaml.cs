using CellphoneRepairTrackerApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CellphoneRepairTrackerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CellphoneRepairDetails> _loadCellphoneRepairHistory = TextFileHandler.ReadCellphoneRepairDetailsFromTxtFileDefault();
        
        private List<CellphoneRepairDetails> RepairHistoryByActualRepairDate()
        {
            return _loadCellphoneRepairHistory.OrderByDescending(recentDate => recentDate.ActualRepairDate).ToList();
        }

        public MainWindow()
        {
            InitializeComponent(); 
            RepairHistoryByActualRepairDate().ForEach(repairHistory => HistoryList.Items.Add(repairHistory));
            EntryCount.Text = Convert.ToString(HistoryList.Items.Count);
        }

        private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try { DisplayDetails.Text = HistoryList.SelectedItem.ToString(); }
            catch (NullReferenceException) { MessageBox.Show("Nothing happened!", "Refresh Items", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void AddCellphoneRepairDetails_Click(object sender, RoutedEventArgs e)
        {
            AddCellphoneRepairDetailsWindow addCellphoneRepairDetailsWindow = new AddCellphoneRepairDetailsWindow();
            addCellphoneRepairDetailsWindow.Show();
        }

        private void RefreshListBox_Click(object sender, RoutedEventArgs e)
        {
            HistoryList.Items.Clear();
            _loadCellphoneRepairHistory = TextFileHandler.ReadCellphoneRepairDetailsFromTxtFileDefault();
            RepairHistoryByActualRepairDate().ForEach(repairHistory => HistoryList.Items.Add(repairHistory));
            EntryCount.Text = Convert.ToString(HistoryList.Items.Count);
        }
    }
}
