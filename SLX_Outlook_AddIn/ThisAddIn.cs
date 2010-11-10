using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using Redemption;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;


namespace SLX_Outlook_AddIn
{
    public partial class ThisAddIn
    {
        Office.CommandBarButton cmdButton;
        Office.CommandBarButton cmdGotoContact;
        Office.CommandBarButton cmdOpportunities;
        Office.CommandBarButton cmdSettings;
        Office.CommandBarButton cmdCreateAccountContact;
        Office.CommandBarButton cmdCreateLead;
        Office.CommandBarButton cmdGotoLead;

        CustomTaskPane.SLX_UserControl slxUserControl;
        Microsoft.Office.Tools.CustomTaskPane slxTaskPane;

        Office.CommandBar newToolBar;
        Office.CommandBarButton firstButton;
        Outlook.Explorers selectExplorers;
        Outlook.Explorer activeExplorer;
        
        ISDataService mydataService;
        SDataResourceCollectionRequest mydataCollection;
        //SDataSingleResourceRequest mydataSingleRequest;
        string contactId;
        string leadId;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            // Custom context menu item event managed
            this.Application.ItemContextMenuDisplay += new Microsoft.Office.Interop.Outlook.ApplicationEvents_11_ItemContextMenuDisplayEventHandler(MenuItem_ItemContextMenuDisplay);

            slxUserControl = new CustomTaskPane.SLX_UserControl();
            slxTaskPane = this.CustomTaskPanes.Add(slxUserControl, "SalesLogix");
            slxTaskPane.Visible = true;
            slxTaskPane.Width = 290;

            activeExplorer = this.Application.ActiveExplorer();

            activeExplorer.SelectionChange += new Outlook.ExplorerEvents_10_SelectionChangeEventHandler(activeExplorer_SelectionChange);

            selectExplorers = this.Application.Explorers;

            selectExplorers.NewExplorer += new Outlook
                .ExplorersEvents_NewExplorerEventHandler(newExplorer_Event);

            AddToolbar();
        }

        private void newExplorer_Event(Outlook.Explorer new_Explorer)
        {
            ((Outlook._Explorer)new_Explorer).Activate();
            newToolBar = null;
            AddToolbar();
        }

        private void AddToolbar()
        {

            if (newToolBar == null)
            {
                Office.CommandBars cmdBars =
                    this.Application.ActiveExplorer().CommandBars;
                newToolBar = cmdBars.Add("NewToolBar",
                    Office.MsoBarPosition.msoBarTop, false, true);
            }
            try
            {
                Office.CommandBarButton button_1 =
                    (Office.CommandBarButton)newToolBar.Controls
                    .Add(1, missing, missing, missing, missing);
                button_1.Style = Office
                    .MsoButtonStyle.msoButtonIconAndCaption;
                button_1.Caption = "Close SalesLogix Task Pane";
                button_1.Tag = "show";
                button_1.FaceId = 0005;
                if (this.firstButton == null)
                {
                    this.firstButton = button_1;
                    firstButton.Click += new Office.
                        _CommandBarButtonEvents_ClickEventHandler
                        (ButtonClick);
                }

                newToolBar.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonClick(Office.CommandBarButton ctrl, ref bool cancel)
        {
            if (slxTaskPane.Visible)
            {
                slxTaskPane.Visible = false;
                //this.firstButton.FaceId = 0220;
                this.firstButton.Caption = "Open SLX SalesLogix Pane";
            }
            else
            {
                slxTaskPane.Visible = true;
                //this.firstButton.FaceId = 0112;
                this.firstButton.Caption = "Close SalesLogix Task Pane";
            }
        }

        void activeExplorer_SelectionChange()
        {
            Outlook.MailItem mail = Application.ActiveExplorer().Selection[1] as Outlook.MailItem;

            if (mail != null)
            {
                slxUserControl.Reload(mail);
            }

        }

        public void MenuItem_ItemContextMenuDisplay(Microsoft.Office.Core.CommandBar CommandBar, Microsoft.Office.Interop.Outlook.Selection Selection)
        {
            try
            {
                
                // Commadbarpopup control to context menu item
                Office.CommandBarPopup CustomItem = (Office.CommandBarPopup)CommandBar.Controls.Add(Office.MsoControlType.msoControlPopup, Type.Missing, "Custom Menu Item", CommandBar.Controls.Count + 1, Type.Missing);
                // Added to separate group in context menu
                CustomItem.BeginGroup = true;
                // Set the tag value for the menu
                CustomItem.Tag = "CustomMenuItem";
                // Caption for the context menu item
                CustomItem.Caption = "SalesLogix Actions";
                // Set it to visible
                CustomItem.Visible = true;

                //Website with all the faceid's http://www.kebabshopblues.co.uk/2007/01/04/visual-studio-2005-tools-for-office-commandbarbutton-faceid-property/
                cmdCreateLead = (Office.CommandBarButton)CustomItem.Controls.Add(1, missing, missing, missing, true);
                cmdCreateLead.Caption = "Create Lead";
                cmdCreateLead.Click += new Office._CommandBarButtonEvents_ClickEventHandler(cmdCreateLead_Click);
                cmdCreateLead.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                cmdCreateLead.FaceId = 0682;

                //Website with all the faceid's http://www.kebabshopblues.co.uk/2007/01/04/visual-studio-2005-tools-for-office-commandbarbutton-faceid-property/
                cmdGotoLead = (Office.CommandBarButton)CustomItem.Controls.Add(1, missing, missing, missing, true);
                cmdGotoLead.Caption = "Goto Lead";
                cmdGotoLead.Click += new Office._CommandBarButtonEvents_ClickEventHandler(cmdGotoLead_Click);
                cmdGotoLead.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                cmdGotoLead.FaceId = 2103;

                //Website with all the faceid's http://www.kebabshopblues.co.uk/2007/01/04/visual-studio-2005-tools-for-office-commandbarbutton-faceid-property/
                cmdCreateAccountContact = (Office.CommandBarButton)CustomItem.Controls.Add(1, missing, missing, missing, true);
                cmdCreateAccountContact.Caption = "Create Account/Contact";
                cmdCreateAccountContact.Click += new Office._CommandBarButtonEvents_ClickEventHandler(cmdCreateAccountContact_Click);
                cmdCreateAccountContact.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                cmdCreateAccountContact.FaceId = 0682;

                //Website with all the faceid's http://www.kebabshopblues.co.uk/2007/01/04/visual-studio-2005-tools-for-office-commandbarbutton-faceid-property/
                cmdGotoContact = (Office.CommandBarButton)CustomItem.Controls.Add(1, missing, missing, missing, true);
                cmdGotoContact.Caption = "Goto Contact";
                cmdGotoContact.Click += new Office._CommandBarButtonEvents_ClickEventHandler(cmdGotoContact_Click);
                cmdGotoContact.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                cmdGotoContact.FaceId = 2103;

                //Website with all the faceid's http://www.kebabshopblues.co.uk/2007/01/04/visual-studio-2005-tools-for-office-commandbarbutton-faceid-property/
                cmdButton = (Office.CommandBarButton)CustomItem.Controls.Add(1, missing, missing, missing, true);
                cmdButton.Caption = "Create Opportunity / Ticket / Activity";
                cmdButton.Click += new Office._CommandBarButtonEvents_ClickEventHandler(cmdButton_Click);
                cmdButton.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                cmdButton.FaceId = 0577;

                //Website with all the faceid's http://www.kebabshopblues.co.uk/2007/01/04/visual-studio-2005-tools-for-office-commandbarbutton-faceid-property/
                //cmdOpportunities = (Office.CommandBarButton)CustomItem.Controls.Add(1, missing, missing, missing, true);
                //cmdOpportunities.Caption = "Opportunity List";
                //cmdOpportunities.Click += new Office._CommandBarButtonEvents_ClickEventHandler(cmdOpportunities_Click);
                //cmdOpportunities.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                //cmdOpportunities.FaceId = 0008;

                cmdSettings = (Office.CommandBarButton)CustomItem.Controls.Add(1, missing, missing, missing, true);
                cmdSettings.Caption = "Settings";
                cmdSettings.Click += new Office._CommandBarButtonEvents_ClickEventHandler(cmdSettings_Click);
                cmdSettings.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                cmdSettings.FaceId = 0212;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void cmdCreateLead_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (Application.ActiveExplorer().Selection.Count > 1)
                {
                    MessageBox.Show("Please only select a single email.");
                    return;
                }

                Outlook.MailItem mail = Application.ActiveExplorer().Selection[1] as Outlook.MailItem;

                SafeMailItem safeMail = new SafeMailItem();

                safeMail.Item = mail;

                string add = safeMail.Sender.SMTPAddress;

                if (!String.IsNullOrEmpty(add))
                {
                    Regex regex = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);

                    if (regex.IsMatch(add))
                    {
                        Form frmCreateLead = new CreateLead(mail);
                        //frmCreateLead.FormClosed += new FormClosedEventHandler(frmCreate_FormClosed);
                        frmCreateLead.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Not a valid email address");
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void cmdCreateAccountContact_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (Application.ActiveExplorer().Selection.Count > 1)
                {
                    MessageBox.Show("Please only select a single email.");
                    return;
                }

                Outlook.MailItem mail = Application.ActiveExplorer().Selection[1] as Outlook.MailItem;

                SafeMailItem safeMail = new SafeMailItem();

                safeMail.Item = mail;

                string add = safeMail.Sender.SMTPAddress;

                if (!String.IsNullOrEmpty(add))
                {
                    Regex regex = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);

                    if (regex.IsMatch(add))
                    {
                        contactId = EmailSearch(add,"Contacts");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (contactId == null)
                {
                    Form frmCreate = new CreateAccountContact(mail);
                    frmCreate.FormClosed += new FormClosedEventHandler(frmCreate_FormClosed);
                    frmCreate.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Contact with this email address already exists");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void cmdGotoContact_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (Application.ActiveExplorer().Selection.Count > 1)
                {
                    MessageBox.Show("Please only select a single email.");
                    return;
                }

                Outlook.MailItem mail = Application.ActiveExplorer().Selection[1] as Outlook.MailItem;

                SafeMailItem safeMail = new SafeMailItem();

                safeMail.Item = mail;

                string add = safeMail.Sender.SMTPAddress;

                if (!String.IsNullOrEmpty(add))
                {
                    Regex regex = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);

                    if (regex.IsMatch(add))
                    {
                        contactId = EmailSearch(add,"Contacts");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (contactId == null)
                {
                    MessageBox.Show("Could not find contact");
                }
                else
                {
                    Process iexplore = new Process();

                    string tempURL = Properties.Settings.Default.SDATA;

                    tempURL += "/{0}/Contact.aspx?entityid={1}";

                    iexplore.StartInfo.FileName = "iexplore.exe";
                    iexplore.StartInfo.Arguments = String.Format(tempURL, "SlxClient", contactId);
                    iexplore.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        void cmdGotoLead_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (Application.ActiveExplorer().Selection.Count > 1)
                {
                    MessageBox.Show("Please only select a single email.");
                    return;
                }

                Outlook.MailItem mail = Application.ActiveExplorer().Selection[1] as Outlook.MailItem;

                SafeMailItem safeMail = new SafeMailItem();

                safeMail.Item = mail;

                string add = safeMail.Sender.SMTPAddress;

                if (!String.IsNullOrEmpty(add))
                {
                    Regex regex = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);

                    if (regex.IsMatch(add))
                    {
                        leadId = EmailSearch(add,"Leads");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (leadId == null)
                {
                    MessageBox.Show("Could not find lead");
                }
                else
                {
                    Process iexplore = new Process();

                    string tempURL = Properties.Settings.Default.SDATA;

                    tempURL += "/{0}/Lead.aspx?entityid={1}";

                    iexplore.StartInfo.FileName = "iexplore.exe";
                    iexplore.StartInfo.Arguments = String.Format(tempURL, "SlxClient", leadId);
                    iexplore.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void cmdOpportunities_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (Application.ActiveExplorer().Selection.Count > 1)
                {
                    MessageBox.Show("Please only select a single email.");
                    return;
                }

                Outlook.MailItem mail = Application.ActiveExplorer().Selection[1] as Outlook.MailItem;

                SafeMailItem safeMail = new SafeMailItem();

                safeMail.Item = mail;

                string add = safeMail.Sender.SMTPAddress;

                if (!String.IsNullOrEmpty(add))
                {
                    Regex regex = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);

                    if (regex.IsMatch(add))
                    {
                        contactId = EmailSearch(add,"Contacts");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (contactId == null)
                {
                    MessageBox.Show("Could not find contact");
                }
                else
                {
                    Form frmOpportunities = new Opportunities(contactId);
                    frmOpportunities.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        void cmdButton_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (Application.ActiveExplorer().Selection.Count > 1)
                {
                    MessageBox.Show("Please only select a single email.");
                    return;
                }
                
                Outlook.MailItem mail = Application.ActiveExplorer().Selection[1] as Outlook.MailItem;

                SafeMailItem safeMail = new SafeMailItem();

                safeMail.Item = mail;

                string add = safeMail.Sender.SMTPAddress;

                if (!String.IsNullOrEmpty(add))
                {
                    Regex regex = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);

                    if (regex.IsMatch(add))
                    {
                        contactId = EmailSearch(add,"Contacts");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (contactId == null)
                {
                    Form frmCreate = new CreateAccountContact(mail);
                    frmCreate.FormClosed += new FormClosedEventHandler(frmCreate_FormClosed);
                    frmCreate.ShowDialog();
                }
                else
                {
                    Form frmContactFound = new ContactFound(contactId, safeMail);
                    frmContactFound.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void frmCreate_FormClosed(object sender, FormClosedEventArgs e)
        {
            //CreateAccountContact frm = (CreateAccountContact)sender;

            //Outlook.MailItem mail = Application.ActiveExplorer().Selection[1] as Outlook.MailItem;

            //SafeMailItem safeMail = new SafeMailItem();

            //safeMail.Item = mail;

            //string add = safeMail.Sender.SMTPAddress;

            //contactId = EmailSearch(add);

            //if ((frm.DialogResult == DialogResult.Yes) & (!String.IsNullOrEmpty(contactId)))
            //{
            //    Form frmContactFound = new ContactFound(contactId, safeMail);
            //    frmContactFound.ShowDialog();
            //}
        }

        void cmdSettings_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            Form frmSettings = new Settings();
            frmSettings.ShowDialog();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private string EmailSearch(string address, string entity)
        {
            try
            {
                mydataService = SDataDataService.mydataService();

                mydataCollection = new SDataResourceCollectionRequest(mydataService);
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

        private bool testSettings()
        {
            try
            { 

                string userName = Properties.Settings.Default.UserName;
                string password = Properties.Settings.Default.Password;
                string url = Properties.Settings.Default.SDATA;

                if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(url))
                {
                    return false;
                }

                string temp = url.Substring(url.Length - 1, 1);

                if (temp == "/")
                {
                    url += "sdata/slx/dynamic/-/";
                }
                else
                {
                    url += "/sdata/slx/dynamic/-/";
                }

                ISDataService service;
                service = new SDataService(url, userName, password);

                SDataResourceCollectionRequest sdataCollection = new SDataResourceCollectionRequest(service);

                sdataCollection.ResourceKind = "Accounts";

                AtomFeed accountFeed = sdataCollection.Read();

                if (accountFeed.Entries.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SDataClientException ex)
            {
                return false;
            }
            }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
