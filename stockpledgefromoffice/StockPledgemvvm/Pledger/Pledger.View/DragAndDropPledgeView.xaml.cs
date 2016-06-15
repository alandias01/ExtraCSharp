using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Pledger.View
{
    /// <summary>
    /// Interaction logic for DragAndDropPledgeView.xaml
    /// </summary>
    public partial class DragAndDropPledgeView : UserControl
    {
        public DragAndDropPledgeView()
        {
            InitializeComponent();
        }

        private void dataGrid1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void dataGrid1_Drop(object sender, DragEventArgs e)
        {
            //we only care about the first file
            string[] Files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string ext = System.IO.Path.GetExtension(Files[0]).ToLower();

            if (ext != ".csv")
            {
                MessageBox.Show("proceed", "confirm", MessageBoxButton.OKCancel);
                return;
            }

            List<string> FileLines = new List<string>();
            FileLines = File.ReadAllLines(Files[0]).ToList();

            List<CSVDragObject> CSVDOList = new List<CSVDragObject>();

            foreach (string FileLine in FileLines)
            {
                string[] ColumnsInLine = FileLine.Split(",".ToCharArray());
                CSVDOList.Add(new CSVDragObject(ColumnsInLine[0], ColumnsInLine[1]));
            }

            dg1.ItemsSource = CSVDOList;

        }
    }

    public class CSVDragObject
    {
        public string CUSIP { get; set; }
        public string Qty { get; set; }

        public CSVDragObject(string cusip, string qty)
        {
            CUSIP = cusip;
            Qty = qty;
        }
    }
}
