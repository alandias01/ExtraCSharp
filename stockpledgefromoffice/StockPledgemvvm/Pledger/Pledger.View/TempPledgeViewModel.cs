using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Windows;

namespace Pledger.View
{
    public class TempPledgeViewModel : PledgeViewModel
    {
        public Command btnCommand { get; set; }
        
        public TempPledgeViewModel()
        {
            btnCommand = new Command(s =>
            {
                MessageBox.Show("SUCCESS!!!");
            });


        }
    }
}
