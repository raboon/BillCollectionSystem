using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillCollectionUploadManager
{
    public class TransactionStatement
    {
        private string _traceno;
       
        public string traceno
        {
            get { return _traceno; }
            set { _traceno = value; }
        }

        private string _daily_trn_no;
       
        public string daily_trn_no
        {
            get { return _daily_trn_no; }
            set { _daily_trn_no = value; }
        }

        private string _accountno;
       
        public string accountno
        {
            get { return _accountno; }
            set { _accountno = value; }
        }

        private string _tr_Branch_Code;
       
        public string Trn_Branch_Code
        {
            get { return _tr_Branch_Code; }
            set { _tr_Branch_Code = value; }
        }

        private string _date;
       
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private string _vdate;
       
        public string vdate
        {
            get { return _vdate; }
            set { _vdate = value; }
        }

        private string _referance;
       
        public string Reference
        {
            get { return _referance; }
            set { _referance = value; }
        }

        private decimal _debit;
       
        public decimal Debit
        {
            get { return _debit; }
            set { _debit = value; }
        }

        private decimal _credit;
       
        public decimal Credit
        {
            get { return _credit; }
            set { _credit = value; }
        }

        private decimal _balance;
       
        public decimal Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        private string _baltype;
       
        public string Baltype
        {
            get { return _baltype; }
            set { _baltype = value; }
        }

        private int _userCode;
       
        public int User_code
        {
            get { return _userCode; }
            set { _userCode = value; }
        }

        private string _time;
       
        public string time
        {
            get { return _time; }
            set { _time = value; }
        }

        private string _remarks;
       
        public string Remark
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        private string _isPostedYN;
       
        public string postedYN
        {
            get { return _isPostedYN; }
            set { _isPostedYN = value; }
        }

        private string _islamicyn;
       
        public string islamicyn
        {
            get { return _islamicyn; }
            set { _islamicyn = value; }
        }
    }
}
