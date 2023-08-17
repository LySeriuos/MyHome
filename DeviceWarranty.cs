using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    internal class DeviceWarranty
    {
		private TimeSpan _warrantyPeriod;

		public TimeSpan WarrantyPeriod
		{
			get { return _warrantyPeriod; }
			set { _warrantyPeriod = value; }
		}

		private string _receiptLink;

		public string ReceiptLink
		{
			get { return _receiptLink; }
			set { _receiptLink = value; }
		}

		private string _shopName;

		public string ShopName									//maybe store / shop class
		{														//maybe store / shop class
			get { return _shopName; }							//maybe store / shop class
			set { _shopName = value; }							//maybe store / shop class
		}														//maybe store / shop class
																//maybe store / shop class
		private string _shopWebAdrress;							//maybe store / shop class
																//maybe store / shop class
		public string ShopWebAdrress							//maybe store / shop class
		{														//maybe store / shop class
			get { return _shopWebAdrress; }						//maybe store / shop class
			set { _shopWebAdrress = value; }					//maybe store / shop class
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
