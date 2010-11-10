using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

class SLX_Lead : INotifyPropertyChanged
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
        FirstName = string.Empty;
        LastName = string.Empty;
        Company = string.Empty;
        WorkPhone = string.Empty;
        Title = string.Empty;
        Industry = string.Empty;
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

    private string company;
    public string Company
    {
        get { return company; }
        set
        {
            if (value != company)
            {
                company = value;
                // This next line is the only change within this property.
                Notify("Company");
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
}

