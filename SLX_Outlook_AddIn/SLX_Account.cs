using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


class SLX_Account : INotifyPropertyChanged
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
        AccountName = string.Empty;
        Employees = string.Empty;
        Industry = string.Empty;
        MainPhone = string.Empty;
        Region = string.Empty;
        Revenue = string.Empty;
        Status = string.Empty;
        Type = string.Empty;
        SubType = string.Empty;
    }

    private string accountName;
    public string AccountName
    {
        get { return accountName; }
        set
        {
            if (value != accountName)
            {
                accountName = value;
                // This next line is the only change within this property.
                Notify("AccountName");
            }
        }
    }

    private string employees;
    public string Employees
    {
        get { return employees; }
        set
        {
            if (value != employees)
            {
                employees = value;
                // This next line is the only change within this property.
                Notify("Employees");
            }
        }
    }

    private string industry;
    public string Industry
    {
        get { return industry; }
        set
        {
            if (value != industry)
            {
                industry = value;
                // This next line is the only change within this property.
                Notify("Industry");
            }
        }
    }

    private string mainPhone;
    public string MainPhone
    {
        get { return mainPhone; }
        set
        {
            if (value != mainPhone)
            {
                mainPhone = value;
                // This next line is the only change within this property.
                Notify("MainPhone");
            }
        }
    }

    private string region;
    public string Region
    {
        get { return region; }
        set
        {
            if (value != region)
            {
                region = value;
                // This next line is the only change within this property.
                Notify("Region");
            }
        }
    }

    private string revenue;
    public string Revenue
    {
        get { return revenue; }
        set
        {
            if (value != revenue)
            {
                revenue = value;
                // This next line is the only change within this property.
                Notify("Revenue");
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

    private string subType;
    public string SubType
    {
        get { return subType; }
        set
        {
            if (value != subType)
            {
                subType = value;
                // This next line is the only change within this property.
                Notify("SubType");
            }
        }
    }
}

