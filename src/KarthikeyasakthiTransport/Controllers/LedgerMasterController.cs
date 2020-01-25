using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class LedgerMasterController : Controller
    {
        private DatabaseContext _context;

        public LedgerMasterController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult List(int id)
        {
            var list = from ledger in _context.LedgerMaster
                       join subGroup in _context.SubGroup on ledger.SubGroupId equals subGroup.SubGroupId

                       select new LedgerMaster
                       {
                           LedgerMasterId = ledger.LedgerMasterId,
                           LedgerMasterName = ledger.LedgerMasterName,
                           SubGroupId = ledger.SubGroupId,
                           SubGroupName = subGroup.SubGroupName
                       };

            switch (id)
            {
                case 1:
                    list = list.Where(x => x.SubGroupId == 1);
                    ViewBag.Title = Constants.Constants.LedgerTypeValue.SupplierMaster;
                    break;

                case 2:
                    list = list.Where(x => x.SubGroupId == 2);
                    ViewBag.Title = Constants.Constants.LedgerTypeValue.CustomerMaster;
                    break;

                default:
                    list = list.Where(x => x.SubGroupId > 2);
                    ViewBag.Title = Constants.Constants.LedgerTypeValue.LedgerHead;
                    break;
            }

            ViewBag.LedgerTypeId = id;

            return View(list);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var ledgerMasterCode = _context.LedgerMaster.Max(x => x.LedgerMasterId) + 1;

            var ledgerMaster = new LedgerMaster
            {
                LedgerMasterType = id,
                LedgerMasterId = ledgerMasterCode
            };

            return ReturnView(ledgerMaster, true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LedgerMaster ledgerMaster)
        {
            if (ModelState.IsValid)
            {
                ledgerMaster.LedgerMasterId = 0;
                bool isNameExists = IsNameExists(ledgerMaster);

                if (isNameExists)
                {
                    ModelState.AddModelError("LedgerMasterName", "Ledger name is already exists.");
                    return ReturnView(ledgerMaster, true);
                }

                if (!string.IsNullOrWhiteSpace(ledgerMaster.SubGroupName) && ledgerMaster.SubGroupId == 0)
                {
                    ModelState.AddModelError("SubGroupName", "Please select valid Sub Group");
                    return ReturnView(ledgerMaster, true);
                }

                _context.LedgerMaster.Add(ledgerMaster);
                _context.SaveChanges();

                return RedirectToAction("List", new { id = ledgerMaster.LedgerMasterType });
            }

            return ReturnView(ledgerMaster, true);
        }

        private IActionResult ReturnView(LedgerMaster ledgerMaster, bool create)
        {
            switch (ledgerMaster.LedgerMasterType)
            {
                case 1:
                    return create ? View("CreateSupplierMaster", ledgerMaster) : View("EditSupplierMaster", ledgerMaster);

                case 2:
                    return create ? View("CreateCustomerMaster", ledgerMaster) : View("EditCustomerMaster", ledgerMaster);

                default:
                    return create ? View("CreateLedgerHead", ledgerMaster) : View("EditLedgerHead", ledgerMaster);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var list = from ledger in _context.LedgerMaster
                       join subGroup in _context.SubGroup on ledger.SubGroupId equals subGroup.SubGroupId
                       where ledger.LedgerMasterId == id
                       select new LedgerMaster
                       {
                           LedgerMasterId = ledger.LedgerMasterId,
                           LedgerMasterName = ledger.LedgerMasterName,
                           LedgerMasterDescription = ledger.LedgerMasterDescription,
                           Division = ledger.Division,
                           SubGroupName = ledger.SubGroupName,
                           SubGroupId = ledger.SubGroupId,
                           LedgerMasterType = ledger.LedgerMasterType,
                           MarginPercentage = ledger.MarginPercentage,
                           SaleRate = ledger.SaleRate,
                           Address = ledger.Address,
                           City = ledger.City,
                           State = ledger.State,
                           PinCode = ledger.PinCode,
                           MobileNumber = ledger.MobileNumber,
                           EmailId = ledger.EmailId,
                           GSTNumber = ledger.GSTNumber,
                           PANNumber = ledger.PANNumber,
                           Debit = ledger.Debit,
                           Credit = ledger.Credit,
                           CreatedBy = ledger.CreatedBy,
                           CreateDate = ledger.CreateDate,
                       };

            return ReturnView(list.First(), false);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LedgerMaster ledgerMaster)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(ledgerMaster);

                if (isNameExists)
                {
                    ModelState.AddModelError("LedgerMasterName", "Ledger name is already exists.");
                    return ReturnView(ledgerMaster, false);
                }

                if (!string.IsNullOrWhiteSpace(ledgerMaster.SubGroupName) && ledgerMaster.SubGroupId == 0)
                {
                    ModelState.AddModelError("SubGroupName", "Please select valid Sub Group");
                    return ReturnView(ledgerMaster, false);
                }

                _context.Entry(ledgerMaster).Property(x => x.LedgerMasterName).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.LedgerMasterDescription).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.Division).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.SubGroupId).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.MarginPercentage).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.SaleRate).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.Address).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.City).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.State).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.PinCode).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.MobileNumber).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.EmailId).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.GSTNumber).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.PANNumber).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.Debit).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.Credit).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.CreatedBy).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.CreateDate).IsModified = true;

                _context.SaveChanges();

                return RedirectToAction("List", new { id = ledgerMaster.LedgerMasterType });
            }

            return ReturnView(ledgerMaster, false);
        }

        private bool IsNameExists(LedgerMaster ledgerMaster)
        {
            var result = _context.LedgerMaster.Where(x => x.LedgerMasterName.Equals(ledgerMaster.LedgerMasterName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            return result == null ? false : result.LedgerMasterId == ledgerMaster.LedgerMasterId ? false : true;
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var ledgerMaster = _context.LedgerMaster.Find(id);
            _context.LedgerMaster.Remove(ledgerMaster);
            _context.SaveChanges();

            return RedirectToAction("List", new { id = ledgerMaster.LedgerMasterType });
        }
    }
}