using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sage.SData.Client.Core;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Extensions;

namespace SLX_Outlook_AddIn
{
    public partial class OpportunityDetails : Form
    {
        string oppoId;
        public OpportunityDetails(string id)
        {
            InitializeComponent();
            oppoId = id;
        }

        private void OpportunityDetails_Load(object sender, EventArgs e)
        {
            DisplayDetails();
        }

        private void DisplayDetails()
        {
            try
            {
                ISDataService service;
                service = SDataDataService.mydataService();

                SDataSingleResourceRequest oppo = new SDataSingleResourceRequest(service);

                oppo.ResourceKind = "Opportunities";

                oppo.ResourceSelector = "('" + oppoId + "')";

                AtomEntry oppoEnty = oppo.Read();

                SDataPayload opportunitiy = (SDataPayload)oppoEnty.GetSDataPayload();

                txtOppoDescription.Text = opportunitiy.Values["Description"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
