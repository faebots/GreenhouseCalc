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
        ComboBox[] _seedComboBoxList = new ComboBox[5];
        ComboBox[] _seedCountComboBoxList = new ComboBox[5];
        SeedCalc calculator = new SeedCalc();
        RadioButton[] _cultivationTiers = new RadioButton[6];

        public MainWindow()
        {
            InitializeComponent();
            var seedList = calculator.SeedList.Select(x => x.Name);
            UpdateLists();
            foreach (var box in _seedComboBoxList)
                box.ItemsSource = seedList;
            ValidateSeedCount();
        }

        private void UpdateLists()
        {
            _seedComboBoxList = new[] { CbSeed1, CbSeed2, CbSeed3, CbSeed4, CbSeed5 };
            _seedCountComboBoxList = new[] { CbSeedCount1, CbSeedCount2, CbSeedCount3, CbSeedCount4, CbSeedCount5 };
            _cultivationTiers = new[] { tier1, tier2, tier3, tier4, tier5, tier6 };
        }

        private int GetSeedTotal()
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
                        return -1;
                }
            }
            return count;
        }
        private bool ValidateSeedCount()
        {
            if (GetSeedTotal() > 5)
                return false;
            else
                return true;
        }

        private bool SeedsAreSelected()
        {
            if (GetSeedTotal() < 1)
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
            if (SubmitBtn != null)
                SubmitBtn.IsEnabled = SeedsAreSelected();
            UpdateLists();
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
            if (SubmitBtn != null)
                SubmitBtn.IsEnabled = SeedsAreSelected();
            UpdateLists();
        }

        int GetCultivationTier()
        {
            if (_cultivationTiers.SingleOrDefault(x => x.IsChecked == true) != null)
                return Array.IndexOf(_cultivationTiers, _cultivationTiers.Single(x => x.IsChecked == true)) + 1;
            else
                return -1;
        }

        
        private List<string> GetSelectedSeeds()
        {
            var seeds = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                var seedCount = _seedCountComboBoxList[i].SelectedIndex;
                var selected = _seedComboBoxList[i].SelectedItem;
                if (selected != null &&
                    seedCount > 0)
                {
                    for (int j = 0; j < seedCount; j++)
                        seeds.Add((string)selected);
                }
            }
            return seeds;
        }
        
        private void CalculateYield()
        {
            var seeds = GetSelectedSeeds();
            var cultivationTier = GetCultivationTier();

            var results = calculator.GetHarvest(seeds, cultivationTier);

            var result = results[0];
            resultHeader.Content = $"{result.Seed} - probability: {result.Probability}";
            var itemRow = new Label[10] { resultItem1, resultItem2, resultItem3, resultItem4, resultItem5, resultItem6, resultItem7, resultItem8, resultItem9, resultItem10 };
            var percRow = new Label[10] { resultPerc1, resultPerc2, resultPerc3, resultPerc4, resultPerc5, resultPerc6, resultPerc7, resultPerc8, resultPerc9, resultPerc10 };
            var items = result.Items;
            for (int i = 0; i < items.Count; i++)
            {
                itemRow[i].Content = items[i].Name;
                percRow[i].Content = items[i].Probability;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateLists();
            if (ValidateSeedCount())
            {
                CalculateYield();
            }
        }
    }
}
