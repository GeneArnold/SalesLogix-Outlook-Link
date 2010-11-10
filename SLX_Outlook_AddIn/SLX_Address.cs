using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


class SLX_Address : INotifyPropertyChanged
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
        Address1 = string.Empty;
        Address2 = string.Empty;
        Address3 = string.Empty;
        Address4 = string.Empty;
        City = string.Empty;
        State = string.Empty;
        PostalCode = string.Empty;
        FullAddress = string.Empty;
    }

    private string address1;
    public string Address1
    {
        get { return fullAddress; }
        set
        {
            if (value != fullAddress)
            {
                fullAddress = value;
                // This next line is the only change within this property.
                Notify("Address1");
            }
        }
    }

    private string address2;
    public string Address2
    {
        get { return fullAddress; }
        set
        {
            if (value != fullAddress)
            {
                fullAddress = value;
                // This next line is the only change within this property.
                Notify("Address2");
            }
        }
    }

    private string address3;
    public string Address3
    {
        get { return fullAddress; }
        set
        {
            if (value != fullAddress)
            {
                fullAddress = value;
                // This next line is the only change within this property.
                Notify("Address3");
            }
        }
    }

    private string address4;
    public string Address4
    {
        get { return fullAddress; }
        set
        {
            if (value != fullAddress)
            {
                fullAddress = value;
                // This next line is the only change within this property.
                Notify("Address4");
            }
        }
    }

    private string city;
    public string City
    {
        get { return fullAddress; }
        set
        {
            if (value != fullAddress)
            {
                fullAddress = value;
                // This next line is the only change within this property.
                Notify("City");
            }
        }
    }

    private string state;
    public string State
    {
        get { return fullAddress; }
        set
        {
            if (value != fullAddress)
            {
                fullAddress = value;
                // This next line is the only change within this property.
                Notify("State");
            }
        }
    }

    private string postalCode;
    public string PostalCode
    {
        get { return fullAddress; }
        set
        {
            if (value != fullAddress)
            {
                fullAddress = value;
                // This next line is the only change within this property.
                Notify("PostalCode");
            }
        }
    }

    private string fullAddress;
    public string FullAddress
    {
        get { return fullAddress; }
        set
        {
            if (value != fullAddress)
            {
                fullAddress = value;
                // This next line is the only change within this property.
                Notify("FullAddress");
            }
        }
    }
}

