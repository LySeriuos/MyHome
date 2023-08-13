using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    internal class DeviceWarranty
    {
		private int _warrantyPeriod;

		public int WarrantyPeriod
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

		public string ShopName
		{
			get { return _shopName; }
			set { _shopName = value; }
		}

		private string _shopWebAdrress;

		public string ShopWebAdrress
		{
			get { return _shopWebAdrress; }
			set { _shopWebAdrress = value; }
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
