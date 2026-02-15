using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtualDeviceLib;

namespace V_LinuxTestTool
{
    public class Program
    {
        static VirtualDevice virtualDevice = new VirtualDevice();
        static void Main(string[] args)
        {
            Console.WriteLine("V-LinuxTestTool Console Application");

            while (true)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Get Config");
                Console.WriteLine("2. Get Status");
                Console.WriteLine("3. Open Fiscal Day");
                Console.WriteLine("4. Close Fiscal Day");
                Console.WriteLine("5. Submit Receipt");
                Console.WriteLine("6. Get License");
                Console.WriteLine("0. Exit");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine(virtualDevice.GetConfig());
                        break;
                    case "2":
                        Console.WriteLine(virtualDevice.GetStatus());
                        break;
                    case "3":
                        Console.WriteLine(virtualDevice.OpenFiscalDay());
                        break;
                    case "4":
                        Console.WriteLine(virtualDevice.CloseFiscalDay());
                        break;
                    case "5":
                        SubmitReceipt();
                        break;
                    case "6":
                        Console.WriteLine(virtualDevice.GetLicense());
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Try again.");
                        break;
                }
            }

            Console.ReadKey();
        }

        private static void SubmitReceipt()
        {
            Console.WriteLine("Enter Receipt Type (FiscalInvoice, DebitNote, or CreditNote):");
            string receiptType = Console.ReadLine();

            Console.WriteLine("Enter Currency:");
            string currency = Console.ReadLine();

            DateTime now = DateTime.Now;
            string today = now.ToString("yyyyMMddTHHmmss");

            var myReceiptLines = new List<receiptLine>();

            if (receiptType.ToLower().Contains("credit"))
            {
                receiptType = "CreditNote";
                Console.WriteLine("Enter Original Invoice Number:");
                string originalInvoiceNumber = Console.ReadLine();

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
            }
            else
            {
                if (receiptType.ToLower().Contains("invoice"))
                {
                    receiptType = "FiscalInvoice";
                }
                else 
                { 
                    receiptType = "DebitNote";
                }
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
            }

            string receiptLines = JsonConvert.SerializeObject(myReceiptLines);

            var submitReceipt = virtualDevice.SubmitReceipt(
                receiptType,
                currency,
                $"TEST{today}",
                "originalInvoiceNumber",
                receiptType == "CreditNote" ? -1.00M : 1.00M,
                receiptType == "CreditNote" ? 0.00M : 0.00M,
                "Test Invoice",
                receiptLines,
                true,
                "CASH",
                "InvoiceA4",
                "Axis Solutions Pvt Ltd",
                "Axis Solution",
                "220192567",
                "2000152399",
                "0778612578",
                "developers@axissol.com",
                "Harare",
                "Bargate Road Vainona",
                "60",
                "Harare");

            Console.WriteLine(submitReceipt);
        }
    }

    public class receiptLine
    {
        public string receiptLineName { get; set; }
        public int receiptLineNo { get; set; }
        public decimal receiptLineQuantity { get; set; }
        public string receiptLineType { get; set; }
        public decimal receiptLineTotal { get; set; }
        public string receiptLineHSCode { get; set; }
        public decimal receiptLinePrice { get; set; }
        public string taxCode { get; set; }
        public decimal taxPercent { get; set; }
    }
}
