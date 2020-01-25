using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class SalesBillController : Controller
    {
        private DatabaseContext _context;

        public SalesBillController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            var list = _context.SalesBill;

            return View(list);
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public IActionResult Print(int id, string PrintOptions)
        {
            bool dispatch = false;

            if (PrintOptions.Contains(",Dispatch"))
            {
                dispatch = true;
                PrintOptions = PrintOptions.Replace(",Dispatch", "");
            }
            else if (PrintOptions.Contains("Dispatch"))
            {
                dispatch = true;
                PrintOptions = PrintOptions.Replace("Dispatch", "");
            }

            List<string> printOptions = null;

            if (!string.IsNullOrWhiteSpace(PrintOptions))
            {
                printOptions = PrintOptions.Split(',').ToList();
            }

            var salesBillViewModel = from salesBill in _context.SalesBill
                                     join ledgerMaster in _context.LedgerMaster on salesBill.ConsignorId equals ledgerMaster.LedgerMasterId
                                     join ledgerMaster1 in _context.LedgerMaster on salesBill.ConsigneeId equals ledgerMaster1.LedgerMasterId
                                     where salesBill.SalesBillId == id
                                     select new SalesBillViewModel
                                     {
                                         SalesBillId = salesBill.SalesBillId,
                                         InvoiceNo = salesBill.InvoiceNo,
                                         SalesBillDate = salesBill.SalesBillDate,
                                         ConsignorName = ledgerMaster.LedgerMasterName,
                                         ConsignorGst = ledgerMaster.GSTNumber,
                                         ConsignorAddress = ledgerMaster.Address,
                                         ConsignorPhone = ledgerMaster.MobileNumber,
                                         ConsigneeName = ledgerMaster1.LedgerMasterName,
                                         ConsigneeGst = ledgerMaster1.GSTNumber,
                                         ConsigneeAddress = ledgerMaster1.Address,
                                         ConsigneePhone = ledgerMaster1.MobileNumber,
                                         Value = salesBill.Value,
                                         ModeOfPay = salesBill.ModeOfPay,
                                         LRNo = salesBill.LRNo,
                                         EWayBillNo = salesBill.EWayBillNo,
                                         LorryNo = salesBill.LorryNo,
                                         DispatchThrough = salesBill.DispatchThrough,
                                         PaymentMode = salesBill.PaymentMode,
                                         BillType = salesBill.BillType,
                                         DeliveryAt = salesBill.DeliveryAt,
                                         TotalAmount = salesBill.TotalAmount,
                                         DoorDeliveryCharge = salesBill.DoorDeliveryCharge,
                                         AssableValue = salesBill.AssableValue,
                                         GstPercentage = salesBill.GstPercentage,
                                         GstAmount = salesBill.GstAmount,
                                         LoadingExpenses = salesBill.LoadingExpenses,
                                         RoundOff = salesBill.RoundOff,
                                         GrandTotal = salesBill.GrandTotal,
                                         PreviousBalance = salesBill.PreviousBalance,
                                         CurrentBalance = salesBill.CurrentBalance,
                                         AdvanceAmount = salesBill.AdvanceAmount,
                                         BalanceAmount = salesBill.BalanceAmount,
                                         ValueInText = NumberToWords(Convert.ToInt32(salesBill.Value))
                                     };

            var salesBillItems = from salesBillItem in _context.SalesBillItems
                                 join itemMaster in _context.ItemMaster on salesBillItem.ItemMasterId equals itemMaster.ItemMasterId
                                 where salesBillItem.SalesBillId == id
                                 select new SalesBillItemsViewModel
                                 {
                                     SalesBillItemsId = salesBillItem.SalesBillItemsId,
                                     ItemMasterName = itemMaster.ItemMasterName,
                                     Qty = salesBillItem.Qty,
                                     Description = salesBillItem.Description,
                                     Weight = salesBillItem.Weight,
                                     Amount = salesBillItem.Amount
                                 };

            var SalesBillPrintViewModel = new SalesBillPrintViewModel
            {
                SalesBillViewModel = salesBillViewModel.FirstOrDefault(),
                SalesItemBillViewModel = salesBillItems.ToList(),
                PrintOptions = printOptions,
                DispatchPrint = dispatch
            };

            return View(SalesBillPrintViewModel);
        }

        public IActionResult PrintPdf(int id, string PrintOptions)
        {
            bool dispatch = false;

            if (PrintOptions.Contains(",Dispatch"))
            {
                dispatch = true;
                PrintOptions = PrintOptions.Replace(",Dispatch", "");
            }
            else if (PrintOptions.Contains("Dispatch"))
            {
                dispatch = true;
                PrintOptions = PrintOptions.Replace("Dispatch", "");
            }

            List<string> printOptions = null;

            if (!string.IsNullOrWhiteSpace(PrintOptions))
            {
                printOptions = PrintOptions.Split(',').ToList();
            }

            var salesBillViewModel = from salesBill in _context.SalesBill
                                     join ledgerMaster in _context.LedgerMaster on salesBill.ConsignorId equals ledgerMaster.LedgerMasterId
                                     join ledgerMaster1 in _context.LedgerMaster on salesBill.ConsigneeId equals ledgerMaster1.LedgerMasterId
                                     where salesBill.SalesBillId == id
                                     select new SalesBillViewModel
                                     {
                                         SalesBillId = salesBill.SalesBillId,
                                         InvoiceNo = salesBill.InvoiceNo,
                                         SalesBillDate = salesBill.SalesBillDate,
                                         ConsignorName = ledgerMaster.LedgerMasterName,
                                         ConsignorGst = ledgerMaster.GSTNumber,
                                         ConsignorAddress = ledgerMaster.Address,
                                         ConsignorPhone = ledgerMaster.MobileNumber,
                                         ConsigneeName = ledgerMaster1.LedgerMasterName,
                                         ConsigneeGst = ledgerMaster1.GSTNumber,
                                         ConsigneeAddress = ledgerMaster1.Address,
                                         ConsigneePhone = ledgerMaster1.MobileNumber,
                                         Value = salesBill.Value,
                                         ModeOfPay = salesBill.ModeOfPay,
                                         LRNo = salesBill.LRNo,
                                         EWayBillNo = salesBill.EWayBillNo,
                                         LorryNo = salesBill.LorryNo,
                                         DispatchThrough = salesBill.DispatchThrough,
                                         PaymentMode = salesBill.PaymentMode,
                                         BillType = salesBill.BillType,
                                         DeliveryAt = salesBill.DeliveryAt,
                                         TotalAmount = salesBill.TotalAmount,
                                         DoorDeliveryCharge = salesBill.DoorDeliveryCharge,
                                         AssableValue = salesBill.AssableValue,
                                         GstPercentage = salesBill.GstPercentage,
                                         GstAmount = salesBill.GstAmount,
                                         LoadingExpenses = salesBill.LoadingExpenses,
                                         RoundOff = salesBill.RoundOff,
                                         GrandTotal = salesBill.GrandTotal,
                                         PreviousBalance = salesBill.PreviousBalance,
                                         CurrentBalance = salesBill.CurrentBalance,
                                         AdvanceAmount = salesBill.AdvanceAmount,
                                         BalanceAmount = salesBill.BalanceAmount,
                                         ValueInText = NumberToWords(Convert.ToInt32(salesBill.Value))
                                     };

            var salesBillItems = from salesBillItem in _context.SalesBillItems
                                 join itemMaster in _context.ItemMaster on salesBillItem.ItemMasterId equals itemMaster.ItemMasterId
                                 where salesBillItem.SalesBillId == id
                                 select new SalesBillItemsViewModel
                                 {
                                     SalesBillItemsId = salesBillItem.SalesBillItemsId,
                                     ItemMasterName = itemMaster.ItemMasterName,
                                     Qty = salesBillItem.Qty,
                                     Description = salesBillItem.Description,
                                     Weight = salesBillItem.Weight,
                                     Amount = salesBillItem.Amount
                                 };

            var SalesBillPrintViewModel = new SalesBillPrintViewModel
            {
                SalesBillViewModel = salesBillViewModel.FirstOrDefault(),
                SalesItemBillViewModel = salesBillItems.ToList(),
                PrintOptions = printOptions,
                DispatchPrint = dispatch
            };

            return new ViewAsPdf("Print", SalesBillPrintViewModel)
            {
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 0, Right = 0 }, // it's in millimeters
            };
        }

        [HttpGet]
        public IActionResult Create()
        {
            var salesBillId = _context.SalesBill.Count() == 0 ? 1 : _context.SalesBill.Max(x => x.SalesBillId) + 1;

            ViewBag.Customers = _context.LedgerMaster.Where(x => x.LedgerMasterType == 2);
            ViewBag.ItemMaster = _context.ItemMaster;

            var salesBill = new SalesBill
            {
                SalesBillId = salesBillId
            };

            return View(salesBill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SalesBill SalesBill)
        {
            if (ModelState.IsValid)
            {
                _context.SalesBill.Add(SalesBill);
                _context.SaveChanges();

                ModelState.Clear();
                return RedirectToAction("List");
            }

            ViewBag.Customers = _context.LedgerMaster.Where(x => x.LedgerMasterType == 2);
            ViewBag.ItemMaster = _context.ItemMaster;

            return View(SalesBill);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Customers = _context.LedgerMaster.Where(x => x.LedgerMasterType == 2);
            ViewBag.ItemMaster = _context.ItemMaster;

            var list = _context.SalesBill.Find(id);

            list.SalesBillDate.Value.ToString("dd/MM/yyyy");

            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SalesBill salesBill)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(salesBill).Property(x => x.InvoiceNo).IsModified = true;
                _context.Entry(salesBill).Property(x => x.SalesBillDate).IsModified = true;
                _context.Entry(salesBill).Property(x => x.ConsignorId).IsModified = true;
                _context.Entry(salesBill).Property(x => x.ConsigneeId).IsModified = true;
                _context.Entry(salesBill).Property(x => x.Value).IsModified = true;
                _context.Entry(salesBill).Property(x => x.ModeOfPay).IsModified = true;
                _context.Entry(salesBill).Property(x => x.LRNo).IsModified = true;
                _context.Entry(salesBill).Property(x => x.EWayBillNo).IsModified = true;
                _context.Entry(salesBill).Property(x => x.LorryNo).IsModified = true;
                _context.Entry(salesBill).Property(x => x.DispatchThrough).IsModified = true;
                _context.Entry(salesBill).Property(x => x.PaymentMode).IsModified = true;
                _context.Entry(salesBill).Property(x => x.BillType).IsModified = true;
                _context.Entry(salesBill).Property(x => x.DeliveryAt).IsModified = true;
                _context.Entry(salesBill).Property(x => x.TotalAmount).IsModified = true;
                _context.Entry(salesBill).Property(x => x.TotalWeight).IsModified = true;
                _context.Entry(salesBill).Property(x => x.DoorDeliveryCharge).IsModified = true;
                _context.Entry(salesBill).Property(x => x.AssableValue).IsModified = true;
                _context.Entry(salesBill).Property(x => x.GstPercentage).IsModified = true;
                _context.Entry(salesBill).Property(x => x.GstAmount).IsModified = true;
                _context.Entry(salesBill).Property(x => x.LoadingExpenses).IsModified = true;
                _context.Entry(salesBill).Property(x => x.RoundOff).IsModified = true;
                _context.Entry(salesBill).Property(x => x.GrandTotal).IsModified = true;
                _context.Entry(salesBill).Property(x => x.PreviousBalance).IsModified = true;
                _context.Entry(salesBill).Property(x => x.CurrentBalance).IsModified = true;
                _context.Entry(salesBill).Property(x => x.AdvanceAmount).IsModified = true;
                _context.Entry(salesBill).Property(x => x.BalanceAmount).IsModified = true;
                _context.Entry(salesBill).Property(x => x.CreatedBy).IsModified = true;
                _context.Entry(salesBill).Property(x => x.CreateDate).IsModified = true;

                _context.SaveChanges();

                ModelState.Clear();
                return RedirectToAction("List");
            }

            ViewBag.Customers = _context.LedgerMaster.Where(x => x.LedgerMasterType == 2);
            ViewBag.ItemMaster = _context.ItemMaster;

            return View(salesBill);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _context.SalesBillItems.RemoveRange(_context.SalesBillItems.Where(x => x.SalesBillId == id));
            _context.SalesBill.Remove(_context.SalesBill.Find(id));
            _context.SaveChanges();

            return RedirectToAction("List", "SalesBill");
        }

        [HttpGet]
        public JsonResult GetLedgerMasterDetails(int id)
        {
            var ledgerMaster = _context.LedgerMaster.FirstOrDefault(x => x.LedgerMasterId == id);

            var salesBillFilter = _context.SalesBill.Where(x => x.ConsignorId == id);

            var balanceAmount = salesBillFilter.Count() == 0 ? 0 : salesBillFilter.OrderByDescending(x => x.SalesBillId).FirstOrDefault().BalanceAmount;

            var JsonResult = new
            {
                LedgerMasterData = ledgerMaster,
                PreviousBalance = balanceAmount
            };

            return Json(JsonResult);
        }

        [HttpGet]
        public JsonResult GetItemMasterDetails(int id)
        {
            var result = _context.ItemMaster.FirstOrDefault(x => x.ItemMasterId == id);

            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateSalesBillItems(SalesBillItems salesBillItems)
        {
            if (salesBillItems.ItemMasterId != 0)
            {
                salesBillItems.CreateDate = DateTime.Today;

                _context.SalesBillItems.Add(salesBillItems);
                _context.SaveChanges();
            }

            var SalesBillItemsFilter = _context.SalesBillItems.Where(x => x.SalesBillId == salesBillItems.SalesBillId);

            var salesBillItemsViewModels = from SalesBillItems in SalesBillItemsFilter
                                           join ItemMaster in _context.ItemMaster on SalesBillItems.ItemMasterId equals ItemMaster.ItemMasterId
                                           select new SalesBillItemsViewModel
                                           {
                                               SalesBillItemsId = SalesBillItems.SalesBillItemsId,
                                               ItemMasterName = ItemMaster.ItemMasterName,
                                               Description = SalesBillItems.Description,
                                               Qty = SalesBillItems.Qty,
                                               Weight = SalesBillItems.Weight,
                                               Amount = SalesBillItems.Amount
                                           };

            var totals = new
            {
                TotalWeight = SalesBillItemsFilter.Sum(x => x.Weight),
                TotalAmount = SalesBillItemsFilter.Sum(x => x.Amount)
            };

            var JsonResult = new
            {
                BillItems = salesBillItemsViewModels,
                Totals = totals
            };

            return Json(JsonResult);
        }

        [HttpPost]
        public JsonResult UpdateBillItems(SalesBillItems salesBillItems)
        {
            _context.Entry(salesBillItems).Property(x => x.Qty).IsModified = true;
            _context.Entry(salesBillItems).Property(x => x.Weight).IsModified = true;
            _context.Entry(salesBillItems).Property(x => x.Amount).IsModified = true;
            _context.SaveChanges();

            return Json("Success");
        }

        [HttpPost]
        public JsonResult DeleteBillItems(SalesBillItems salesBillItems)
        {
            _context.SalesBillItems.Remove(_context.SalesBillItems.Find(salesBillItems.SalesBillItemsId));
            _context.SaveChanges();

            return Json("Success");
        }
    }
}