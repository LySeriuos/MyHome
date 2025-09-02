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
        public int Id { get; set; }
        public DateTime now = DateTime.Now;

        private TimeSpan _warrantyPeriod;
        public TimeSpan WarrantyPeriod
        {           
            get
            {
                return now.AddYears(_years) - now;
            }
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
                return _purchaseDate.AddYears(_years + _extendedWarrantyinYears);
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
        private DateTime _purchaseDate = new DateTime();

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

        private TimeSpan _extraInsuranceWarrantyLenght;

        public TimeSpan ExtraInsuranceWarrantyLenght
        {
            get { return now.AddYears(_extendedWarrantyinYears) - now; }
            set { _extraInsuranceWarrantyLenght = value; }
        }

        private int _extendedWarrantyinYears;

        public int ExtendedWarrantyinYears
        {
            get { return _extendedWarrantyinYears; }
            set { _extendedWarrantyinYears = value; }
        }


        public override string ToString()
        {
            return _warrantyPeriod.ToString("%d") + " " + _purchaseDate.ToShortDateString() + " " + " " + _extraInsuranceWarrantyLenght.ToString("%d");
        }



    }
}
