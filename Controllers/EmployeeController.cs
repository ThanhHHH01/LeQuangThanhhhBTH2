
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeQuangThanhBTH2.Models;
using LeQuangThanhBTH2.Models.Process; 
using LeQuangThanhBTH2.Data;



namespace LeQuangThanhBTH2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbcontext _context;

        private ExcelProcess _excelProcess = new ExcelProcess();


        public EmployeeController(ApplicationDbcontext context)
        {
            _context = context;
        }

        // // GET: Employee
        // public async Task<IActionResult> Index()
        // {
        //       return _context.Employee != null ? 
        //                   View(await _context.Employee.ToListAsync()) :
        //                   Problem("Entity set 'ApplicationDbcontext.Employee'  is null.");
        // }

        // // GET: Employee/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null || _context.Employee == null)
        //     {
        //         return NotFound();
        //     }

        //     var employee = await _context.Employee
        //         .FirstOrDefaultAsync(m => m.ID == id);
        //     if (employee == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(employee);
        // }

        // // GET: Employee/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }
        public async Task<IActionResult>Index()
        {
            return View(await _context.Employee.ToListAsync());
        }

        public async Task<IActionResult> Upload()
        {
            return View();
        }
        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if(fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload!");
                }
                else
                {
                    var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                       await file.CopyToAsync(stream);
                    }
                }
            
            }
            return View();
        }
        public async Task<IActionResult> Create([Bind("ID,Name,Age")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Age")] Employee employee)
        {
            if (id != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'ApplicationDbcontext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employee?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}