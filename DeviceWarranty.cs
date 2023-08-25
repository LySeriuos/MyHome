using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    public class DeviceWarranty
    {
		private TimeSpan _warrantyPeriod;

		public TimeSpan WarrantyPeriod
		{
			get { return _warrantyPeriod; }
			set { _warrantyPeriod = value; }
		}


		public DateTime WarrantyEnd
		{
			get 
			{
				return _purchaseDate + _warrantyPeriod;

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

		private DateTime _purchaseDate;

		public DateTime PurchaseDate
		{
			get { return _purchaseDate; }
			set { _purchaseDate = value; }
		}

		private string _extraDeviceInsuranceLink;

		public string ExtraDeviceInsuranceLink
        {
			get { return _extraDeviceInsuranceLink; }
			set { _extraDeviceInsuranceLink = value; }
		}

		private int _extraDeviceInsuranceLength;

		public int ExtraDeviceInsuranceLength
        {
			get { return _extraDeviceInsuranceLength; }
			set { _extraDeviceInsuranceLength = value; }
		}





	}
}
