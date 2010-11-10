using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using GoogleGeocoder;
using Redemption;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WebChart;

namespace CustomTaskPane
{
    public delegate void UpdateUI(SDataPayload payload);
    public delegate void UpdateOppoUI(string str);
    public delegate void UpdateTicketUI(string str);
    public delegate void UpdateAddressUI(string str);
    public delegate void ClearAll();

    public partial class SLX_UserControl : UserControl
    {
        Microsoft.Office.Interop.Outlook.MailItem curEmail;

        ISDataService mydataService;

        SLX_Contact slxContact;
        SLX_Account slxAccount;
        SLX_Lead slxLead;
        SLX_Address slxAddress;
        
        public SLX_UserControl()
        {
            InitializeComponent();

            
            mydataService = SDataDataService.mydataService();
            slxContact = new SLX_Contact();
            slxAccount = new SLX_Account();
            slxLead = new SLX_Lead();
            slxAddress = new SLX_Address();
            SetDataBindings();

            flowLayoutPanel1.Controls.Clear();
            //flowLayoutPanel1.Controls.Add(panelControls);
            flowLayoutPanel1.Controls.Add(panelAccounts);
            if (SLX_Outlook_AddIn.Properties.Settings.Default.AcctPanelMinimized == true)
            {
                panelAccounts.Height = 30;
                btnAcctMinimize.Text = "+";
                flowLayoutPanel2.AutoScroll = false;
            }
            else
            {
                panelAccounts.Height = 216;
                btnAcctMinimize.Text = "--";
                flowLayoutPanel2.AutoScroll = true;
            } 

            flowLayoutPanel1.Controls.Add(panelContacts);
            if (SLX_Outlook_AddIn.Properties.Settings.Default.ContPanelMinimized == true)
            {
                panelContacts.Height = 30;
                btnContMinimize.Text = "+";
                flowLayoutPanel3.AutoScroll = false;
            }
            else
            {
                panelContacts.Height = 216;
                btnContMinimize.Text = "--";
                flowLayoutPanel3.AutoScroll = true;
            } 

            flowLayoutPanel1.Controls.Add(panelLeads);
            if (SLX_Outlook_AddIn.Properties.Settings.Default.LeadPanelMinimized == true)
            {
                panelLeads.Height = 30;
                btnLeadMinimize.Text = "+";
                flowLayoutPanel4.AutoScroll = false;
            }
            else
            {
                panelLeads.Height = 216;
                btnLeadMinimize.Text = "--";
                flowLayoutPanel4.AutoScroll = true;
            } 

            flowLayoutPanel1.Controls.Add(panelOppoList);
            if (SLX_Outlook_AddIn.Properties.Settings.Default.OppoListPanelMinimized == true)
            {
                panelOppoList.Height = 30;
                btnOppoListMinimize.Text = "+";
                dgvOpportunities.Visible = false;
            }
            else
            {
                panelOppoList.Height = 216;
                btnOppoListMinimize.Text = "--";
                dgvOpportunities.Visible = true;
            } 

            flowLayoutPanel1.Controls.Add(panelOpportunityChart);
            if (SLX_Outlook_AddIn.Properties.Settings.Default.OppoChartPanelMinimized == true)
            {
                panelOpportunityChart.Height = 30;
                btnOppoChartMinimize.Text = "+";
                picBoxOpportunity.Visible = false;
            }
            else
            {
                panelOpportunityChart.Height = 216;
                btnOppoChartMinimize.Text = "--";
                picBoxOpportunity.Visible = true;
            } 

            flowLayoutPanel1.Controls.Add(panelTicketList);
            if (SLX_Outlook_AddIn.Properties.Settings.Default.TicketListPanelMinimized == true)
            {
                panelTicketList.Height = 30;
                btnTicketListMinimize.Text = "+";
                dgvTickets.Visible = false;
            }
            else
            {
                panelTicketList.Height = 216;
                btnTicketListMinimize.Text = "--";
                dgvTickets.Visible = true;
            } 

            flowLayoutPanel1.Controls.Add(panelAddressMap);
            if (SLX_Outlook_AddIn.Properties.Settings.Default.GooglePanelMinimized == true)
            {
                panelAddressMap.Height = 30;
                btnGoogleMapMinimize.Text = "+";
                webBrowserAddress.Visible = false;
            }
            else
            {
                panelAddressMap.Height = 216;
                btnGoogleMapMinimize.Text = "--";
                webBrowserAddress.Visible = true;
            }

            flowLayoutPanel1.Controls.Add(panelERP);
            if (SLX_Outlook_AddIn.Properties.Settings.Default.ERPPanelMinimized == true)
            {
                panelERP.Height = 30;
                btnERPMinimized.Text = "+";
                picBoxERP.Visible = false;
            }
            else
            {
                panelERP.Height = 216;
                btnERPMinimized.Text = "--";
                picBoxERP.Visible = true;
            } 
        }

        public void Reload(Microsoft.Office.Interop.Outlook.MailItem mail)
        {
            curEmail = mail;

            try
            {
                dgvTickets.DataSource = null;
                dgvOpportunities.DataSource = null;
                backgroundWorker1.RunWorkerAsync(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetDataBindings()
        {
            txtContactFirstName.DataBindings.Clear();
            txtContactFirstName.DataBindings.Add(new Binding("Text", slxContact, "FirstName"));
            txtContactLastName.DataBindings.Clear();
            txtContactLastName.DataBindings.Add(new Binding("Text", slxContact, "LastName"));
            txtContactTitle.DataBindings.Clear();
            txtContactTitle.DataBindings.Add(new Binding("Text", slxContact, "Title"));
            txtContactWorkPhone.DataBindings.Clear();
            txtContactWorkPhone.DataBindings.Add(new Binding("Text", slxContact, "WorkPhone"));
            txtContactMobilePhone.DataBindings.Clear();
            txtContactMobilePhone.DataBindings.Add(new Binding("Text", slxContact, "MobilePhone"));
            txtContactType.DataBindings.Clear();
            txtContactType.DataBindings.Add(new Binding("Text", slxContact, "Type"));
            txtContactStatus.DataBindings.Clear();
            txtContactStatus.DataBindings.Add(new Binding("Text", slxContact, "Status"));
            txtContactMgr.DataBindings.Clear();
            txtContactMgr.DataBindings.Add(new Binding("Text", slxContact, "AcctMgr"));

            txtAccountName.DataBindings.Clear();
            txtAccountName.DataBindings.Add(new Binding("Text", slxAccount, "AccountName"));
            txtAccountEmployees.DataBindings.Clear();
            txtAccountEmployees.DataBindings.Add(new Binding("Text", slxAccount, "Employees"));
            txtAccountIndustry.DataBindings.Clear();
            txtAccountIndustry.DataBindings.Add(new Binding("Text", slxAccount, "Industry"));
            txtAccountMainPhone.DataBindings.Clear();
            txtAccountMainPhone.DataBindings.Add(new Binding("Text", slxAccount, "MainPhone"));
            txtAccountRegion.DataBindings.Clear();
            txtAccountRegion.DataBindings.Add(new Binding("Text", slxAccount, "Region"));
            txtAccountRevenue.DataBindings.Clear();
            txtAccountRevenue.DataBindings.Add(new Binding("Text", slxAccount, "Revenue"));
            txtAccountStatus.DataBindings.Clear();
            txtAccountStatus.DataBindings.Add(new Binding("Text", slxAccount, "Status"));
            txtAccountType.DataBindings.Clear();
            txtAccountType.DataBindings.Add(new Binding("Text", slxAccount, "Type"));
            txtAccountSubType.DataBindings.Clear();
            txtAccountSubType.DataBindings.Add(new Binding("Text", slxAccount, "SubType"));

            txtLeadFirstName.DataBindings.Clear();
            txtLeadFirstName.DataBindings.Add(new Binding("Text", slxLead, "FirstName"));
            txtLeadLastName.DataBindings.Clear();
            txtLeadLastName.DataBindings.Add(new Binding("Text", slxLead, "LastName"));
            txtLeadCompany.DataBindings.Clear();
            txtLeadCompany.DataBindings.Add(new Binding("Text", slxLead, "Company"));
            txtLeadWorkPhone.DataBindings.Clear();
            txtLeadWorkPhone.DataBindings.Add(new Binding("Text", slxLead, "WorkPhone"));
            txtLeadTitle.DataBindings.Clear();
            txtLeadTitle.DataBindings.Add(new Binding("Text", slxLead, "Title"));
            txtLeadIndustry.DataBindings.Clear();
            txtLeadIndustry.DataBindings.Add(new Binding("Text", slxLead, "Industry"));
        }

        private string EmailSearch(string address,string entity)
        {
            try
            {
                SDataResourceCollectionRequest mydataCollection = new SDataResourceCollectionRequest(mydataService);
                mydataCollection.ResourceKind = entity;
                mydataCollection.QueryValues.Add("where", "Email eq '" + address + "'");
                AtomFeed contactfeed = mydataCollection.Read();
                string contactId;

                if (contactfeed.Entries.Count() > 0)
                {
                    foreach (AtomEntry entry in contactfeed.Entries)
                    {
                        string tempURI = entry.Id.Uri.AbsoluteUri;
                        contactId = tempURI.Substring(tempURI.IndexOf("'") + 1, tempURI.LastIndexOf("'") - tempURI.IndexOf("'") - 1);
                        return contactId;
                    }
                }

                return null;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        private string CleanPhoneNumber(string number)
        {
            string temp = Regex.Replace(number, "[^0-9]", "");

            temp = Convert.ToInt64(temp).ToString("###-###-####");

            return temp;
        }

        private void GatherSLXInformation()
        {
            //Creating a SafeEmail object so I can always get SMTP address even from Exchange emails
            SafeMailItem safeMail = new SafeMailItem();

            safeMail.Item = curEmail;

            string contactId = EmailSearch(safeMail.Sender.SMTPAddress, "Contacts");

            string leadId = EmailSearch(safeMail.Sender.SMTPAddress, "Leads");

           

            if (!this.InvokeRequired)
            {
                ClearAll();
            }
            else
            {
                this.Invoke(new ClearAll(this.ClearAll), null);
            }

            if (!String.IsNullOrEmpty(contactId))
            {
                SDataSingleResourceRequest tempContact = new SDataSingleResourceRequest(mydataService);

                tempContact.ResourceKind = "Contacts";

                tempContact.Include = "Account,AccountManager/UserInfo,Address";

                tempContact.ResourceSelector = "('" + contactId + "')";

                AtomEntry contactEnty = tempContact.Read();

                SDataPayload contact = (SDataPayload)contactEnty.GetSDataPayload();
                SDataPayload contactAcct = (SDataPayload)contact.Values["Account"];
                SDataPayload contactMgr = (SDataPayload)contact.Values["AccountManager"];
                SDataPayload contactMgrInfo = (SDataPayload)contactMgr.Values["UserInfo"];
                SDataPayload contactAddress = (SDataPayload)contact.Values["Address"];

                if (!this.InvokeRequired)
                {
                    UpdateContactUI(contact);
                    UpdateAccountUI(contactAcct);
                    UpdateManagerUI(contactMgrInfo);
                    UpdateOppoUI(contactId);
                    UpdateTicketUI(contactId);
                    UpdateAddressUI((contactAddress.Values["FullAddress"] != null ? contactAddress.Values["FullAddress"].ToString() : ""));
                }
                else
                {
                    this.BeginInvoke(new UpdateUI(this.UpdateContactUI), contact);
                    this.BeginInvoke(new UpdateUI(this.UpdateAccountUI), contactAcct);
                    this.BeginInvoke(new UpdateUI(this.UpdateManagerUI), contactMgrInfo);
                    this.BeginInvoke(new UpdateOppoUI(this.UpdateOppoUI), contactId);
                    this.BeginInvoke(new UpdateTicketUI(this.UpdateTicketUI), contactId);
                    this.BeginInvoke(new UpdateAddressUI(this.UpdateAddressUI), (contactAddress.Values["FullAddress"] != null ? contactAddress.Values["FullAddress"].ToString() : ""));
                }
            }
            
            if (!String.IsNullOrEmpty(leadId))
            {
                SDataSingleResourceRequest tempLead = new SDataSingleResourceRequest(mydataService);
                tempLead.ResourceKind = "Leads";
                tempLead.Include = "Address";
                tempLead.ResourceSelector = "('" + leadId + "')";
                AtomEntry leadEnty = tempLead.Read();
                SDataPayload lead = (SDataPayload)leadEnty.GetSDataPayload();

                SDataPayload tempLeadAddress = (SDataPayload)lead.Values["Address"];
                string leadAddressId = tempLeadAddress.Key;
                SDataSingleResourceRequest tempLeadAddressRequest = new SDataSingleResourceRequest(mydataService);
                tempLeadAddressRequest.ResourceKind = "LeadAddresses";
                tempLeadAddressRequest.ResourceSelector = "('" + leadAddressId + "')";
                AtomEntry leadAddressEnty = tempLeadAddressRequest.Read();
                SDataPayload leadAddress = (SDataPayload)leadAddressEnty.GetSDataPayload();

                if (!this.InvokeRequired)
                {
                    UpdateLeadUI(lead);
                    UpdateAddressUI((leadAddress.Values["FullAddress"] != null ? leadAddress.Values["FullAddress"].ToString() : ""));
                }
                else
                {
                    this.BeginInvoke(new UpdateUI(this.UpdateLeadUI), lead);
                    this.BeginInvoke(new UpdateAddressUI(this.UpdateAddressUI), (leadAddress.Values["FullAddress"] != null ? leadAddress.Values["FullAddress"].ToString() : ""));
                }
            }
        }

        private void ClearAll()
        {
            slxContact.Clear();
            slxAccount.Clear();
            slxLead.Clear();
            picBoxOpportunity.Visible = false;
        }

        private void UpdateContactUI(SDataPayload contact)
        {
            slxContact.Id = (contact.Key.ToString());
            slxContact.FirstName = (contact.Values["FirstName"] != null ? contact.Values["FirstName"].ToString() : "");
            slxContact.LastName = (contact.Values["LastName"] != null ? contact.Values["LastName"].ToString() : "");
            slxContact.WorkPhone = (contact.Values["WorkPhone"] != null ? CleanPhoneNumber(contact.Values["WorkPhone"].ToString()) : "");
            slxContact.MobilePhone = (contact.Values["Mobile"] != null ? CleanPhoneNumber(contact.Values["Mobile"].ToString()) : "");
            slxContact.Type = (contact.Values["Type"] != null ? contact.Values["Type"].ToString() : "");
            slxContact.Status = (contact.Values["Status"] != null ? contact.Values["Status"].ToString() : "");
            slxContact.Title = (contact.Values["Title"] != null ? contact.Values["Title"].ToString() : "");
        }

        private void UpdateAccountUI(SDataPayload contactAcct)
        {
            slxAccount.AccountName = (contactAcct.Values["AccountName"] != null ? contactAcct.Values["AccountName"].ToString() : "");
            slxAccount.Employees = (contactAcct.Values["Employees"] != null ? contactAcct.Values["Employees"].ToString() : "");
            slxAccount.Industry = (contactAcct.Values["Industry"] != null ? contactAcct.Values["Industry"].ToString() : "");
            slxAccount.MainPhone = (contactAcct.Values["MainPhone"] != null ? CleanPhoneNumber(contactAcct.Values["MainPhone"].ToString()) : "");
            slxAccount.Region = (contactAcct.Values["Region"] != null ? contactAcct.Values["Region"].ToString() : "");
            slxAccount.Revenue = (contactAcct.Values["Revenue"] != null ? String.Format("{0:C}", Convert.ToDecimal(contactAcct.Values["Revenue"])) : "");
            slxAccount.Status = (contactAcct.Values["Status"] != null ? contactAcct.Values["Status"].ToString() : "");
            slxAccount.Type = (contactAcct.Values["Type"] != null ? contactAcct.Values["Type"].ToString() : "");
            slxAccount.SubType = (contactAcct.Values["SubType"] != null ? contactAcct.Values["SubType"].ToString() : "");

        }

        private void UpdateManagerUI(SDataPayload contactMgrInfo)
        {
            slxContact.AcctMgr = (contactMgrInfo.Values["UserName"] != null ? contactMgrInfo.Values["UserName"].ToString() : "");
        }

        private void UpdateLeadUI(SDataPayload lead)
        {
            slxLead.FirstName = (lead.Values["FirstName"] != null ? lead.Values["FirstName"].ToString() : "");
            slxLead.LastName = (lead.Values["LastName"] != null ? lead.Values["LastName"].ToString() : "");
            slxLead.Company = (lead.Values["Company"] != null ? lead.Values["Company"].ToString() : "");
            slxLead.WorkPhone = (lead.Values["WorkPhone"] != null ? CleanPhoneNumber(lead.Values["WorkPhone"].ToString()) : "");
            slxLead.Title = (lead.Values["Title"] != null ? lead.Values["Title"].ToString() : "");
            slxLead.Industry = (lead.Values["Industry"] != null ? lead.Values["Industry"].ToString() : "");
        }

        private void UpdateOppoUI(string id)
        {
            try
            {
                ISDataService service;
                service = SDataDataService.mydataService();

                SDataResourceCollectionRequest oppoContactsCollection = new SDataResourceCollectionRequest(service);

                oppoContactsCollection.ResourceKind = "opportunitycontacts";
                oppoContactsCollection.QueryValues.Add("where", "Contact.Id eq '" + id + "'");

                AtomFeed oppoContactsFeed = oppoContactsCollection.Read();

                picBoxOpportunity.Visible = false;

                if (oppoContactsFeed.Entries.Count() > 0)
                {

                    DataTable table = new DataTable();

                    table.Columns.Add("Id");
                    table.Columns.Add("Description");
                    table.Columns.Add("Probability");
                    table.Columns.Add("Potential");
                    table.Columns.Add("ActualAmount");
                    table.Columns.Add("Status");

                    //--------------------------------------
                    // Create The Chart
                    ChartEngine engine = new ChartEngine();
                    engine.Size = picBoxOpportunity.Size;
                    ChartCollection charts = new ChartCollection(engine);
                    engine.Charts = charts;
                    int pointCount = 0;

                    ChartPointCollection data = new ChartPointCollection();
                    Chart columnChart = new ColumnChart(data, Color.DarkGreen);
                    columnChart.Fill.Color = Color.FromArgb(50, Color.Green);
                    columnChart.ShowLineMarkers = true;
                    columnChart.DataLabels.Visible = true;

                    foreach (AtomEntry entry in oppoContactsFeed.Entries)
                    {
                        SDataPayload oppoContact = entry.GetSDataPayload();

                        SDataPayload tempOppo = (SDataPayload)oppoContact.Values["Opportunity"];

                        SDataSingleResourceRequest oppo = new SDataSingleResourceRequest(service);

                        oppo.ResourceKind = "Opportunities";

                        oppo.ResourceSelector = "('" + tempOppo.Key + "')";

                        AtomEntry oppoEnty = oppo.Read();

                        SDataPayload opportunitiy = (SDataPayload)oppoEnty.GetSDataPayload();

                        DataRow dr = table.NewRow();

                        dr[0] = opportunitiy.Key.ToString();
                        dr[1] = (opportunitiy.Values["Description"] != null ? opportunitiy.Values["Description"].ToString() : "No Description");
                        dr[2] = (opportunitiy.Values["CloseProbability"] != null ? opportunitiy.Values["CloseProbability"].ToString() + "%" : "0%");
                        dr[3] = (opportunitiy.Values["SalesPotential"] != null ? String.Format("{0:C}", Convert.ToDecimal(opportunitiy.Values["SalesPotential"])) : "");
                        dr[4] = (opportunitiy.Values["ActualAmount"] != null ? String.Format("{0:C}", Convert.ToDecimal(opportunitiy.Values["ActualAmount"])) : "");
                        dr[5] = (opportunitiy.Values["Status"] != null ? opportunitiy.Values["Status"].ToString() : "No Status");

                        pointCount = Convert.ToInt32(opportunitiy.Values["SalesPotential"].ToString().Substring(0,opportunitiy.Values["SalesPotential"].ToString().IndexOf('.')));

                        data.Add(new ChartPoint("Some Data", pointCount));

                        table.Rows.Add(dr);
                    }

                    dgvOpportunities.DataSource = table;
                    dgvOpportunities.Columns["Id"].Visible = false;
                    dgvOpportunities.Refresh();

                    charts.Add(columnChart);
                    engine.GridLines = GridLines.Horizontal;
                    Image image = engine.GetBitmap();
                    //--------------------------------------
                    // At this point we have the chart already
                    //--------------------------------------
                    // show the already generated image
                    picBoxOpportunity.Image = image;

                    if (SLX_Outlook_AddIn.Properties.Settings.Default.OppoChartPanelMinimized == true)
                    {
                        picBoxOpportunity.Visible = false;
                    }
                    else
                    {
                        picBoxOpportunity.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateTicketUI(string id)
        {
            try
            {
                ISDataService service;
                service = SDataDataService.mydataService();

                SDataResourceCollectionRequest ticketContactsCollection = new SDataResourceCollectionRequest(service);

                ticketContactsCollection.ResourceKind = "tickets";
                ticketContactsCollection.QueryValues.Add("where", "Contact.Id eq '" + id + "'");

                AtomFeed ticketContactsFeed = ticketContactsCollection.Read();

                if (ticketContactsFeed.Entries.Count() > 0)
                {

                    DataTable table = new DataTable();

                    table.Columns.Add("Id");
                    table.Columns.Add("TicketNumber");
                    table.Columns.Add("CreateDate");
                    table.Columns.Add("Subject");

                    foreach (AtomEntry entry in ticketContactsFeed.Entries)
                    {
                        SDataPayload ticket = (SDataPayload)entry.GetSDataPayload();

                        DataRow dr = table.NewRow();

                        dr[0] = ticket.Key.ToString();
                        dr[1] = ticket.Values["TicketNumber"].ToString();
                        dr[2] = Convert.ToDateTime(ticket.Values["CreateDate"]).ToShortDateString();
                        dr[3] = (ticket.Values["Subject"] != null ? ticket.Values["Subject"].ToString() : "");

                        table.Rows.Add(dr);
                    }

                    dgvTickets.DataSource = table;
                    dgvTickets.Columns["Id"].Visible = false;
                    dgvTickets.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateAddressUI(string address)
        {
            Coordinate coordinate = Geocode.GetCoordinates(address);
            decimal latitude = coordinate.Latitude;
            decimal longitude = coordinate.Longitude;
            string tempAddress = String.Format("http://maps.google.com/maps/api/staticmap?center={0}&zoom=14&size=300x250&maptype=roadmap&markers=color:blue|{1},{2}&sensor=false", address, latitude.ToString(), longitude.ToString());
            Uri addURI = new Uri(tempAddress);
            webBrowserAddress.Url = addURI;
        }

        private void OpenOpportunity(DataGridViewCellEventArgs e)
        {
            try
            {
                Process iexplore = new Process();

                string tempURL = SLX_Outlook_AddIn.Properties.Settings.Default.SDATA;

                tempURL += "/{0}/Opportunity.aspx?entityid={1}";

                iexplore.StartInfo.FileName = "iexplore.exe";
                iexplore.StartInfo.Arguments = String.Format(tempURL, "SlxClient", dgvOpportunities.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                iexplore.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenTicket(DataGridViewCellEventArgs e)
        {
            try
            {
                Process iexplore = new Process();

                string tempURL = SLX_Outlook_AddIn.Properties.Settings.Default.SDATA;

                tempURL += "/{0}/Ticket.aspx?entityid={1}";

                iexplore.StartInfo.FileName = "iexplore.exe";
                iexplore.StartInfo.Arguments = String.Format(tempURL, "SlxClient", dgvTickets.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                iexplore.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvOpportunities_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 0)//Details
            //{
            //    SLX_Outlook_AddIn.OpportunityDetails frmOppoDetails = new SLX_Outlook_AddIn.OpportunityDetails(dgvOpportunities.Rows[e.RowIndex].Cells[2].Value.ToString());
            //    frmOppoDetails.ShowDialog();
            //}

            if (e.ColumnIndex == 0)//Open
            {
                OpenOpportunity(e);
            }
        }

        private void dgvTickets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)//Open
            {
                OpenTicket(e);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SLX_UserControl argTest = e.Argument as SLX_UserControl;
            argTest.GatherSLXInformation();
        }

        private void btnUpdateContact_Click(object sender, EventArgs e)
        {
            try
            {
                var b = new SDataSingleResourceRequest(mydataService);
                b.ResourceKind = "contacts";
                b.ResourceSelector = "'" + slxContact.Id + "'";
                AtomEntry entry = b.Read();
                SDataPayload payload = entry.GetSDataPayload();
                payload.Values["FirstName"] = txtContactFirstName.Text;
                payload.Values["LastName"] = txtContactLastName.Text;
                payload.Values["Title"] = txtContactTitle.Text;

                string wkPhone = txtContactWorkPhone.Text;
                if (!String.IsNullOrEmpty(wkPhone))
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < wkPhone.Length; i++)
                    {
                        if (char.IsLetterOrDigit(wkPhone[i]))
                            sb.Append(wkPhone[i]);
                    }

                    wkPhone = sb.ToString();
                    payload.Values["WorkPhone"] = wkPhone;
                }

                string mPhone = txtContactMobilePhone.Text;
                if (!String.IsNullOrEmpty(mPhone))
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < mPhone.Length; i++)
                    {
                        if (char.IsLetterOrDigit(mPhone[i]))
                            sb.Append(mPhone[i]);
                    }

                    mPhone = sb.ToString();
                    payload.Values["Mobile"] = mPhone;
                }
                
                b.Entry = entry;
                AtomEntry updatedEnty = b.Update();

                MessageBox.Show("Updates Completed");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAcctMinimize_Click(object sender, EventArgs e)
        {
            if (btnAcctMinimize.Text == "--")
            {
                panelAccounts.Height = 30;
                btnAcctMinimize.Text = "+";
                SLX_Outlook_AddIn.Properties.Settings.Default.AcctPanelMinimized = true;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                flowLayoutPanel2.AutoScroll = false;
            }
            else
            {
                panelAccounts.Height = 216;
                btnAcctMinimize.Text = "--";
                SLX_Outlook_AddIn.Properties.Settings.Default.AcctPanelMinimized = false;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                flowLayoutPanel2.AutoScroll = true;
            }
        }

        private void btnContMinimize_Click(object sender, EventArgs e)
        {
            if (btnContMinimize.Text == "--")
            {
                panelContacts.Height = 30;
                btnContMinimize.Text = "+";
                SLX_Outlook_AddIn.Properties.Settings.Default.ContPanelMinimized = true;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                flowLayoutPanel3.AutoScroll = false;
            }
            else
            {
                panelContacts.Height = 216;
                btnContMinimize.Text = "--";
                SLX_Outlook_AddIn.Properties.Settings.Default.ContPanelMinimized = false;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                flowLayoutPanel3.AutoScroll = true;
            }
        }

        private void btnLeadMinimize_Click(object sender, EventArgs e)
        {
            if (btnLeadMinimize.Text == "--")
            {
                panelLeads.Height = 30;
                btnLeadMinimize.Text = "+";
                SLX_Outlook_AddIn.Properties.Settings.Default.LeadPanelMinimized = true;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                flowLayoutPanel4.AutoScroll = false;
            }
            else
            {
                panelLeads.Height = 216;
                btnLeadMinimize.Text = "--";
                SLX_Outlook_AddIn.Properties.Settings.Default.LeadPanelMinimized = false;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                flowLayoutPanel4.AutoScroll = true;
            }
        }

        private void btnGoogleMapMinimize_Click(object sender, EventArgs e)
        {
            if (btnGoogleMapMinimize.Text == "--")
            {
                panelAddressMap.Height = 30;
                btnGoogleMapMinimize.Text = "+";
                SLX_Outlook_AddIn.Properties.Settings.Default.GooglePanelMinimized = true;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                webBrowserAddress.Visible = false;
            }
            else
            {
                panelAddressMap.Height = 216;
                btnGoogleMapMinimize.Text = "--";
                SLX_Outlook_AddIn.Properties.Settings.Default.GooglePanelMinimized = false;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                webBrowserAddress.Visible = true;
            }
        }

        private void btnOppoListMinimize_Click(object sender, EventArgs e)
        {
            if (btnOppoListMinimize.Text == "--")
            {
                panelOppoList.Height = 30;
                btnOppoListMinimize.Text = "+";
                SLX_Outlook_AddIn.Properties.Settings.Default.OppoListPanelMinimized = true;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                dgvOpportunities.Visible = false;
            }
            else
            {
                panelOppoList.Height = 216;
                btnOppoListMinimize.Text = "--";
                SLX_Outlook_AddIn.Properties.Settings.Default.OppoListPanelMinimized = false;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                dgvOpportunities.Visible = true;
            }
        }

        private void btnOppoChartMinimize_Click(object sender, EventArgs e)
        {
            if (btnOppoChartMinimize.Text == "--")
            {
                panelOpportunityChart.Height = 30;
                btnOppoChartMinimize.Text = "+";
                SLX_Outlook_AddIn.Properties.Settings.Default.OppoChartPanelMinimized = true;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                picBoxOpportunity.Visible = false;
            }
            else
            {
                panelOpportunityChart.Height = 216;
                btnOppoChartMinimize.Text = "--";
                SLX_Outlook_AddIn.Properties.Settings.Default.OppoChartPanelMinimized = false;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                picBoxOpportunity.Visible = true;
            }
        }

        private void btnTicketListMinimize_Click(object sender, EventArgs e)
        {
            if (btnTicketListMinimize.Text == "--")
            {
                panelTicketList.Height = 30;
                btnTicketListMinimize.Text = "+";
                SLX_Outlook_AddIn.Properties.Settings.Default.TicketListPanelMinimized = true;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                dgvTickets.Visible = false;
            }
            else
            {
                panelTicketList.Height = 216;
                btnTicketListMinimize.Text = "--";
                SLX_Outlook_AddIn.Properties.Settings.Default.TicketListPanelMinimized = false;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                dgvTickets.Visible = true;
            }
        }

        private void btnERPMinimized_Click(object sender, EventArgs e)
        {
            if (btnERPMinimized.Text == "--")
            {
                panelERP.Height = 30;
                btnERPMinimized.Text = "+";
                SLX_Outlook_AddIn.Properties.Settings.Default.ERPPanelMinimized = true;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                picBoxERP.Visible = false;
            }
            else
            {
                panelERP.Height = 216;
                btnERPMinimized.Text = "--";
                SLX_Outlook_AddIn.Properties.Settings.Default.ERPPanelMinimized = false;
                SLX_Outlook_AddIn.Properties.Settings.Default.Save();
                picBoxERP.Visible = true;
            }
        }

        private void flowLayoutPanel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void flowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            // Just add the control to the new panel.
            // No need to remove from the other panel, this changes the Control.Parent property.
            Point p = flowLayoutPanel1.PointToClient(new Point(e.X, e.Y));
            var item = flowLayoutPanel1.GetChildAtPoint(p);
            int index = flowLayoutPanel1.Controls.GetChildIndex(item, false);
            //flowLayoutPanel1.Controls.SetChildIndex(data, index);
            flowLayoutPanel1.Invalidate();
        }
    }
}
