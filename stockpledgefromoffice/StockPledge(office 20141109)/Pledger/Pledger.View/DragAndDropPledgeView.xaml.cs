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
using Data.DTC;
using Pledger.Data;
using System.Data.OleDb;

namespace Pledger.View
{
    /// <summary>
    /// Interaction logic for DragAndDropPledgeView.xaml
    /// </summary>
    public partial class DragAndDropPledgeView : UserControl
    {
        spAlanGetRealTimePositions_Pledger_ResultDA DDA = new spAlanGetRealTimePositions_Pledger_ResultDA();
        List<spAlanGetRealTimePositions_Pledger_Result> RealTimePos = new List<spAlanGetRealTimePositions_Pledger_Result>();

        public DragAndDropPledgeView()
        {
            InitializeComponent();
            checkBoxCreateApexTrade.IsEnabled = false;
            checkBoxSendToOcc.IsEnabled = false;
        }

        private void buttonPledge_Click(object sender, RoutedEventArgs e)
        {
            PledgeItemsOnGrid();
        }

        

        private void PledgeItemsOnGrid()
        {
            try
            {
                DDA.Load(RealTimePos, "0269");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Pledge aborted: " + ex.InnerException.Message);
                return;
            }


            List<OCCStockPledge> StocksToPledge = new List<OCCStockPledge>();

            List<CSVDragObject> cs = dg1.ItemsSource as List<CSVDragObject>;

            if (DoesGridHaveErrors(cs))
            {
                return;
            }

            List<CSVDragObject> csrejects = new List<CSVDragObject>();

            foreach (CSVDragObject csitem in cs)
            {
                var r = RealTimePos.Find(x => x.cusip.ToUpper() == csitem.CUSIP.ToUpper());
                if (r != null)
                {
                    //testing alandias 20140617
                    if(r.Ticker==null)
                    {
                        csrejects.Add(csitem);
                    }
                    int qty = int.Parse(csitem.Qty);
                    StocksToPledge.Add(new OCCStockPledge(r, qty));
                }
                    
                

                else
                {
                    csrejects.Add(csitem);
                }
            }

            string csrejects_string = string.Join("\n", csrejects.Select(x => x.CUSIP).ToArray());

            if (csrejects.Count > 0)
            {
                MessageBoxResult mbr_csrejects = MessageBox.Show("These items won't be pledged because we can't find the price for this CUSIP.  Pledge everything else? \n " + csrejects_string, "Confirm", MessageBoxButton.OKCancel);

                if (mbr_csrejects == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            //new PledgeProcessor().PledgeListOfStocks(StocksToPledge, checkBoxCreateApexTrade.IsChecked.Value, checkBoxSendToOcc.IsChecked.Value);
            new PledgeProcessor().PledgeListOfStocks(StocksToPledge, true, true);

            MessageBox.Show("Completed");

        }

        private bool DoesGridHaveErrors(List<CSVDragObject> cs)
        {
            if (cs == null)
            {
                MessageBox.Show("Noting on Grid");
                return true;
            }

            if (checkBoxSendToOcc.IsChecked == false && checkBoxCreateApexTrade.IsChecked == false)
            {
                MessageBox.Show("You must check either Send To Occ and/or Create Apex Trade");
                return true;
            }

            if (cs.Count < 1)
            {
                MessageBox.Show("There are no items");
                return true;
            }

            foreach (CSVDragObject item in cs)
            {
                int tmp;
                if (!int.TryParse(item.Qty, out tmp))
                {
                    MessageBox.Show("The qty for cusip " + item.CUSIP + " is not reading in as a number");
                    return true;
                }
            }

            return false;
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
            string[] Files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string ext = System.IO.Path.GetExtension(Files[0]).ToLower();

            switch (ext)
            {
                case ".csv": dataGrid1_Drop_csv(Files[0]);
                    break;
                case ".xls": dataGrid1_Drop_xls(Files[0]);
                    break;
                default: MessageBox.Show("you can only import csv or xls type file");
                    break;
            }

        }

        private void dataGrid1_Drop_csv(string file)
        {
            List<string> FileLines = new List<string>();
            FileLines = File.ReadAllLines(file).ToList();

            List<CSVDragObject> CSVDOList = new List<CSVDragObject>();

            foreach (string FileLine in FileLines)
            {
                string[] ColumnsInLine = FileLine.Split(",".ToCharArray());
                CSVDOList.Add(new CSVDragObject(ColumnsInLine[0], ColumnsInLine[1]));
            }

            dg1.ItemsSource = null;
            dg1.ItemsSource = CSVDOList;
        }


        private void dataGrid1_Drop_xls(string file)
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.jet.OLEDB.4.0;Data Source=" + file + ";Extended Properties=\"Excel 8.0;IMEX=1;HDR=no;\"";

            //pass query
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "select * from [Sheet1$]";
            cmd.Connection = con;

            //Read Data
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds, "demo");

            System.Data.DataTable dt = ds.Tables[0];
            List<CSVDragObject> CSVDOList = new List<CSVDragObject>();

            foreach (System.Data.DataRow r in dt.Rows)
            {
                CSVDOList.Add(new CSVDragObject(r[0].ToString(), r[1].ToString()));
            }

            dg1.ItemsSource = null;
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
