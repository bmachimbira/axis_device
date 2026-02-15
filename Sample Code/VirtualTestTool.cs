using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualDeviceLib;

namespace VirtualTestTool
{
    public partial class Form1 : Form
    {
        VirtualDevice virtualDevice = new VirtualDevice();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }


        private void btnGetConfig_Click(object sender, EventArgs e)
        {
            MessageBox.Show(virtualDevice.GetConfig());
        }

        private void btnGetStatus_Click(object sender, EventArgs e)
        {
            MessageBox.Show(virtualDevice.GetStatus());
        }

        private void btnOpenFiscalDay_Click(object sender, EventArgs e)
        {
            MessageBox.Show(virtualDevice.OpenFiscalDay());
        }

        private void btnCloseFiscalDay_Click(object sender, EventArgs e)
        {
            MessageBox.Show(virtualDevice.CloseFiscalDay());
        }

        private void btnSubmitReceipt_Click(object sender, EventArgs e)
        {
            var myReceiptLines = new List<receiptLine>();
            DateTime now = DateTime.Now;
            string today = now.ToString("yyyyMMddTHHmmss");

            if (txtReceiptType.Text == "CreditNote") // Process Credit Note
            {
                myReceiptLines.Add(new receiptLine()
                {
                    receiptLineType = "", //Optional
                    receiptLineNo = 1, //Optional
                    receiptLineHSCode = "99001000", 
                    receiptLineName = "Sale Item 1",
                    receiptLinePrice = -1.00M,
                    receiptLineQuantity = 1,
                    receiptLineTotal = -1.00M,
                    taxCode = "A", //NonVatable item [B] Vatable Item [A]
                    taxPercent = 15M, //NonVatable item [0] Vatable Item [15]
                });
                myReceiptLines.Add(new receiptLine()
                {
                    receiptLineType = "Sale", //Optional
                    receiptLineNo = 2, //Optional
                    receiptLineHSCode = "99002000", 
                    receiptLineName = "Sale Item 2",
                    receiptLinePrice = -1.00M,
                    receiptLineQuantity = 1,
                    receiptLineTotal = -1.00M,
                    taxCode = "B", //NonVatable item [B] Vatable Item [A]
                    taxPercent = 0.00M, //NonVatable item [0] Vatable Item [15]
                });
                myReceiptLines.Add(new receiptLine()
                {
                    receiptLineType = "Discount", //Optional
                    receiptLineNo = 3, //Optional
                    receiptLineHSCode = "99001000", 
                    receiptLineName = "Discount Item",
                    receiptLinePrice = 1.00M,
                    receiptLineQuantity = 1,
                    receiptLineTotal = 1.00M,
                    taxCode = "A", //NonVatable item [B] Vatable Item [A]
                    taxPercent = 15.00M, //NonVatable item [0] Vatable Item [15]
                });

                string receiptLines = JsonConvert.SerializeObject(myReceiptLines);

                var submitReceipt = virtualDevice.SubmitReceipt($"{txtReceiptType.Text.Trim()}", txtCurrency.Text,
                $"TEST{today}", $"{txtOriginalInvoiceNumber.Text.Trim()}",
                -1.00M, 0.00M, "Test Invoice", receiptLines, true, "CASH",
                "InvoiceA4", "Axis Solutions Pvt Ltd", "Axis Solution", "220192567", "2000152399",
                "0778612578", "developers@axissol.com", "Harare", "Bargate Road Vainona",
                "60", "Harare");

                MessageBox.Show(submitReceipt);
            }
            else //Process Invoice or Debit Note
            {
                myReceiptLines.Add(new receiptLine()
                {
                    receiptLineType = "", //Optional
                    receiptLineNo = 1, //Optional
                    receiptLineHSCode = "99001000", 
                    receiptLineName = "Sale Item 1",
                    receiptLinePrice = 1.00M,
                    receiptLineQuantity = 1,
                    receiptLineTotal = 1.00M,
                    taxCode = "A", //NonVatable item [B] Vatable Item [A]
                    taxPercent = 15M, //NonVatable item [0] Vatable Item [15]
                });
                myReceiptLines.Add(new receiptLine()
                {
                    receiptLineType = "Sale", //Optional
                    receiptLineNo = 2, //Optional
                    receiptLineHSCode = "99002000", 
                    receiptLineName = "Sale Item 2",
                    receiptLinePrice = 1.00M,
                    receiptLineQuantity = 1,
                    receiptLineTotal = 1.00M,
                    taxCode = "B", //NonVatable item [B] Vatable Item [A]
                    taxPercent = 0.00M, //NonVatable item [0] Vatable Item [15]
                });
                myReceiptLines.Add(new receiptLine()
                {
                    receiptLineType = "Discount", //Optional
                    receiptLineNo = 3, //Optional
                    receiptLineHSCode = "99001000", 
                    receiptLineName = "Discount Item 1",
                    receiptLinePrice = -1.00M,
                    receiptLineQuantity = 1,
                    receiptLineTotal = -1.00M,
                    taxCode = "A", //NonVatable item [B] Vatable Item [A]
                    taxPercent = 15.00M, //NonVatable item [0] Vatable Item [15]
                });

                string receiptLines = JsonConvert.SerializeObject(myReceiptLines);

                var submitReceipt = virtualDevice.SubmitReceipt($"{txtReceiptType.Text.Trim()}", txtCurrency.Text,
                    $"TEST{today}", $"{txtOriginalInvoiceNumber.Text.Trim()}",
                    1.00M, 0.00M, "Test Invoice", receiptLines, true, "CASH",
                     "InvoiceA4", "Axis Solutions Pvt Ltd", "Axis Solution", "220192567", "2000152399",
                     "0778612578", "developers@axissol.com", "Harare", "Bargate Road Vainona",
                     "60", "Harare");

                MessageBox.Show(submitReceipt);
            }

            
        }

        private void btnGetLicense_Click(object sender, EventArgs e)
        {
            MessageBox.Show(virtualDevice.GetLicense());
        }
    }
    public class receiptLine
    {
        public string receiptLineName { get; set; }
        public int receiptLineNo { get; set; }
        public decimal receiptLineQuantity { get; set; }
        public string receiptLineType { get; set; }
        public decimal receiptLineTotal { get; set; }
        public int taxID { get; set; }
        public string receiptLineHSCode { get; set; }
        public decimal receiptLinePrice { get; set; }
        public string taxCode { get; set; }
        public decimal taxPercent { get; set; }
    }
}
