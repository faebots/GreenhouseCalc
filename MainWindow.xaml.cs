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

namespace GreenhouseCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ComboBox[] _seedComboBoxList = new ComboBox[5];
        ComboBox[] _seedCountComboBoxList = new ComboBox[5];
        SeedCalc calculator = new SeedCalc();

        public MainWindow()
        {
            InitializeComponent();
            var seedList = calculator.SeedList;
            _seedComboBoxList = new[] {CbSeed1, CbSeed2, CbSeed3, CbSeed4, CbSeed5};
            _seedCountComboBoxList = new[] {CbSeedCount1, CbSeedCount2, CbSeedCount3, CbSeedCount4, CbSeedCount5};
            foreach (var box in _seedComboBoxList)
                box.ItemsSource = seedList;
            ValidateSeedCount();
        }

        private bool ValidateSeedCount()
        {
            int count = 0;
            for (int i = 0; i < 5; i++)
            {
                if (_seedComboBoxList[i] != null && _seedComboBoxList[i].SelectedItem != null)
                {
                    ComboBoxItem item = (ComboBoxItem)_seedCountComboBoxList[i].SelectedItem;
                    int x;
                    if (int.TryParse(item.Content.ToString(), out x))
                        count += x;
                    else
                        return false;
                }
                    
            }
            if (count > 5)
                return false;
            else
                return true;
        }

        private void CbSeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox x = (ComboBox)sender;
            int i;
            if (int.TryParse(x.Name.Substring(x.Name.Length - 1), out i))
            {
                if (_seedComboBoxList[i] != null)
                {
                    for (int j = 0; i < 5; i++)
                    {
                        if (j == i)
                            continue;

                    }
                }
            }
            if (!ValidateSeedCount())
            {
                x.SelectedIndex = -1;
                MessageBox.Show("Error: total seeds cannot exceed 5.", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            
        }

        private void CbSeedCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ValidateSeedCount())
            {
                ComboBox x = (ComboBox)sender;
                x.SelectedIndex = 0;
                MessageBox.Show("Error: total seeds cannot exceed 5.", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

    }
}
