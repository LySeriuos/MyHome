using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHome.Models
{
    public class DeviceWarranty
    {
        private TimeSpan _warrantyPeriod;

        public TimeSpan WarrantyPeriod
        {
            get { return _warrantyPeriod; }
            set { _warrantyPeriod = value; }
        }

        private int _years;

        public int Years
        {
            get { return _years; }
            set { _years = value; }
        }

        public DateTime WarrantyEnd
        {
            get
            {
                return _purchaseDate + _warrantyPeriod + _extendedWarranty;
            }
        }

        private string _receiptLink;

        public string ReceiptLink
        {
            get { return _receiptLink; }
            set { _receiptLink = value; }
        }

        private Shop _shop;

        public Shop Shop
        {
            get { return _shop; }
            set { _shop = value; }
        }


        [Required]  

        private DateTime _purchaseDate = DateTime.Now;

        public DateTime PurchaseDate
        {
            get { return _purchaseDate; }
            set { _purchaseDate = value; }
        }

        private string _extraDeviceInsuranceLink;

        public string ExtraInsuranceWarrantyLink
        {
            get { return _extraDeviceInsuranceLink; }
            set { _extraDeviceInsuranceLink = value; }
        }

        private TimeSpan _extendedWarranty;

        public TimeSpan ExtraInsuranceWarrantyLenght
        {
            get { return _extendedWarranty; }
            set { _extendedWarranty = value; }
        }

        private int _extendedWarrantyinYears;

        public int ExtendedWarrantyinYears
        {
            get { return _extendedWarrantyinYears; }
            set { _extendedWarrantyinYears = value; }
        }


        public override string ToString()
        {
            return _warrantyPeriod.ToString("%d") + " " + _purchaseDate.ToShortDateString() + " " + " " + _extendedWarranty.ToString("%d");
        }



    }
}
