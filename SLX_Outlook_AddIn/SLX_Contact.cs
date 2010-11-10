using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

public class SLX_Contact : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void Notify(string propertyName)
    {
        if (this.PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    public void Clear()
    {
        Id = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Title = string.Empty;
        WorkPhone = string.Empty;
        MobilePhone = string.Empty;
        Status = string.Empty;
        AcctMgr = string.Empty;
        Type = string.Empty;
    }

    private string id;
    public string Id
    {
        get { return id; }
        set
        {
            if (value != id)
            {
                id = value;
                // This next line is the only change within this property.
                Notify("Id");
            }
        }
    }

    private string firstName;
    public string FirstName
    {
        get { return firstName; }
        set
        {
            if (value != firstName)
            {
                firstName = value;
                // This next line is the only change within this property.
                Notify("FirstName");
            }
        }
    }

    private string lastName;
    public string LastName
    {
        get { return lastName; }
        set
        {
            if (value != lastName)
            {
                lastName = value;
                // This next line is the only change within this property.
                Notify("LastName");
            }
        }
    }

    private string title;
    public string Title
    {
        get { return title; }
        set
        {
            if (value != title)
            {
                title = value;
                // This next line is the only change within this property.
                Notify("Title");
            }
        }
    }

    private string workPhone;
    public string WorkPhone
    {
        get { return workPhone; }
        set
        {
            if (value != workPhone)
            {
                workPhone = value;
                // This next line is the only change within this property.
                Notify("WorkPhone");
            }
        }
    }

    private string mobilePhone;
    public string MobilePhone
    {
        get { return mobilePhone; }
        set
        {
            if (value != mobilePhone)
            {
                mobilePhone = value;
                // This next line is the only change within this property.
                Notify("MobilePhone");
            }
        }
    }

    private string status;
    public string Status
    {
        get { return status; }
        set
        {
            if (value != status)
            {
                status = value;
                // This next line is the only change within this property.
                Notify("Status");
            }
        }
    }

    private string acctMgr;
    public string AcctMgr
    {
        get { return acctMgr; }
        set
        {
            if (value != acctMgr)
            {
                acctMgr = value;
                // This next line is the only change within this property.
                Notify("AcctMgr");
            }
        }
    }

    private string type;
    public string Type
    {
        get { return type; }
        set
        {
            if (value != type)
            {
                type = value;
                // This next line is the only change within this property.
                Notify("Type");
            }
        }
    }
}

