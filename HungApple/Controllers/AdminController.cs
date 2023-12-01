using HungApple.Data;
using HungApple.Models;
using HungApple.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HungApple.Controllers
{
    public class AdminController : Controller
    {
        private readonly HungAppleContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public AdminController(HungAppleContext context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }

        //Trang chủ AdminPanel
        public IActionResult Index(int? page)
        {
            var lstSubscript = _context.NewsLetterSubscription.ToList();
            int pageSize = 10;
            int pageNumber = page ?? 1;
            int startIndex = (pageNumber - 1) * pageSize;
            var model = lstSubscript.Take(pageSize).Skip(startIndex).ToList();
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)lstSubscript.Count / pageSize);
            return View(model);
        }

        //Start Categories
        public async Task<IActionResult> CategoriesIndex()
        {
            return _context.Category != null ?
                        View(await _context.Category.ToListAsync()) :
                        Problem("Entity set 'HungAppleContext.Category'  is null.");
        }

        public async Task<IActionResult> CategoriesDetails(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public IActionResult CategoriesCreate()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoriesCreate([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                await Task.Delay(1500);
                return RedirectToAction(nameof(CategoriesIndex));
            }
            return View(category);
        }

        public async Task<IActionResult> CategoriesEdit(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoriesEdit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                await Task.Delay(1500);
                return RedirectToAction(nameof(CategoriesIndex));
            }
            return View(category);
        }

        public async Task<IActionResult> CategoriesDelete(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("CategoriesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoriesDelete(int id)
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'HungAppleContext.Category'  is null.");
            }
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            await Task.Delay(1500);
            return RedirectToAction(nameof(CategoriesIndex));
        }
        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //End Categories

        //Start Discounts
        // GET: Discounts
        public async Task<IActionResult> DiscountsIndex()
        {
            return _context.Discount != null ?
                        View(await _context.Discount.ToListAsync()) :
                        Problem("Entity set 'HungAppleContext.Discount'  is null.");
        }

        // GET: Discounts/Details/5
        public async Task<IActionResult> DiscountsDetails(int? id)
        {
            if (id == null || _context.Discount == null)
            {
                return NotFound();
            }

            var discount = await _context.Discount
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // GET: Discounts/Create
        public IActionResult DiscountsCreate()
        {
            return View();
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiscountsCreate([Bind("Id,Name,Amount,Code,StartDate,EndDate")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discount);
                await _context.SaveChangesAsync();
                await Task.Delay(1500);
                return RedirectToAction(nameof(DiscountsIndex));
            }
            return View(discount);
        }

        // GET: Discounts/Edit/5
        public async Task<IActionResult> DiscountsEdit(int? id)
        {
            if (id == null || _context.Discount == null)
            {
                return NotFound();
            }

            var discount = await _context.Discount.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiscountsEdit(int id, [Bind("Id,Name,Amount,Code,StartDate,EndDate")] Discount discount)
        {
            if (id != discount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountExists(discount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                await Task.Delay(1500);
                return RedirectToAction(nameof(DiscountsIndex));
            }
            return View(discount);
        }

        // GET: Discounts/Delete/5
        public async Task<IActionResult> DiscountsDelete(int? id)
        {
            if (id == null || _context.Discount == null)
            {
                return NotFound();
            }

            var discount = await _context.Discount
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("DiscountsDelete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DiscountsDeleteConfirmed(int id)
        {
            if (_context.Discount == null)
            {
                return Problem("Entity set 'HungAppleContext.Discount'  is null.");
            }
            var discount = await _context.Discount.FindAsync(id);
            if (discount != null)
            {
                _context.Discount.Remove(discount);
            }

            await _context.SaveChangesAsync();
            await Task.Delay(1500);
            return RedirectToAction(nameof(DiscountsIndex));
        }

        private bool DiscountExists(int id)
        {
            return (_context.Discount?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //End Discounts

        //Start Products
        // GET: Products
        public async Task<IActionResult> ProductsIndex(int pageNumber = 1)
        {
            var hungAppleContext = _context.Product.Include(p => p.Category).Include(p => p.Discount);
            var pageSize = 10; // Cập nhật kích thước trang là 10
            var total = hungAppleContext.Count();
            ViewBag.total = total;
            ViewBag.pageSize = pageSize;

            var page = total / pageSize;
            if (total % pageSize > 0)
            {
                page += 1;
            }
            ViewBag.page = page;
            ViewBag.pageNumber = pageNumber;
            var pagedProducts = await hungAppleContext.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return View(pagedProducts);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> ProductsDetails(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Discount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult ProductsCreate()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsCreate(Product product, IFormFile fileImage)
        {
            if (ModelState.IsValid)
            {
                var pathImage = string.Empty;
                if (fileImage != null)
                {
                    string filename = "";
                    filename = fileImage.FileName;
                    string folderName = DateTime.Now.ToString("yyMMdd");
                    string newPath = Path.Combine(folderName);
                    string SavePath = Path.Combine("wwwroot/Upload/Product", newPath, filename);
                    var fi = new FileInfo(SavePath);
                    if (!Directory.Exists(fi.DirectoryName))
                    {
                        Directory.CreateDirectory(fi.DirectoryName);
                    }
                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        fileImage.CopyTo(stream);
                    }
                    pathImage = folderName + "\\" + filename;
                }
                product.Image = pathImage;
                _context.Add(product);
                await _context.SaveChangesAsync();
                await Task.Delay(1500);
                return RedirectToAction(nameof(ProductsIndex));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Name", product.DiscountId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> ProductsEdit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Name", product.DiscountId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsEdit(int id, Product product, IFormFile fileImage)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            var entity = _context.Product.FirstOrDefault(x => x.Id == product.Id);
            if (entity == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            ViewData["DiscountId"] = new SelectList(_context.Discount, "Id", "Name", product.DiscountId);
            var pathImage = string.Empty;
            if (fileImage != null)
            {
                string filename = "";
                filename = fileImage.FileName;
                string folderName = DateTime.Now.ToString("yyMMdd");
                string newPath = Path.Combine(folderName);
                string SavePath = Path.Combine("wwwroot/Upload/Product", newPath, filename);
                var fi = new FileInfo(SavePath);
                if (!Directory.Exists(fi.DirectoryName))
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    fileImage.CopyTo(stream);
                }
                pathImage = folderName + "\\" + filename;
            }
            try
            {
                entity.Name = product.Name;
                entity.Description = product.Description;
                entity.Detail = product.Detail;
                entity.Price = product.Price;
                entity.PriceDiscount = product.PriceDiscount;
                entity.Image = (!string.IsNullOrEmpty(pathImage) ? pathImage : entity.Image);
                entity.Color = product.Color;
                entity.Quantity = product.Quantity;
                entity.IsNew = product.IsNew;
                entity.IsBestSeller = product.IsBestSeller;
                entity.CategoryId = product.CategoryId;
                entity.DiscountId = product.DiscountId;
                entity.IsSale = product.IsSale;
                _context.Update(entity);
                await _context.SaveChangesAsync();
                await Task.Delay(1500);
                return RedirectToAction(nameof(ProductsIndex));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    return View(product);
                }
            }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> ProductsDelete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Discount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("ProductsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductsDeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'HungAppleContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            await Task.Delay(1500);
            return RedirectToAction(nameof(ProductsIndex));
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //End Products

        //Start Users
        // GET: Users
        public async Task<IActionResult> UsersIndex()
        {
            var hungAppleContext = _context.Comment
               .Include(p => p.Product)
               .Include(p => p.User)
               .Join(_context.Product, c => c.ProductId, p => p.Id, (c, p) => new { c, p })
               .Join(_context.User, cc => cc.c.UserId, u => u.Id, (cc, u) => new { cc, u })
               .Select(m => new Comment
               {
                   Id = m.cc.c.Id,
                   Content = m.cc.c.Content,
                   Rating = m.cc.c.Rating,
                   DateCreated = m.cc.c.DateCreated,
                   UserName = m.u.Username,
                   ProductName = m.cc.p.Name
               }).OrderByDescending(x => x.Id);


            var user = _context.User.
                Join(_context.Provinces, u => u.ProvinceId, p => p.province_id, (u, p) => new { u, p })
                .Join(_context.Districts, uu => uu.u.DistrictId, d => d.district_id, (uu, d) => new { uu, d })
                .Join(_context.Wards, uuu => uuu.uu.u.WardId, w => w.ward_id, (uuu, w) => new { uuu, w }).Select(m => new User
                {
                    Id = m.uuu.uu.u.Id,
                    Username = m.uuu.uu.u.Username,
                    Password = m.uuu.uu.u.Password,
                    Email = m.uuu.uu.u.Email,
                    Phone = m.uuu.uu.u.Phone,
                    Role = m.uuu.uu.u.Role,
                    ResetPasswordToken = m.uuu.uu.u.ResetPasswordToken,
                    ResetPasswordExpiration = m.uuu.uu.u.ResetPasswordExpiration,
                    ProvinceId = m.uuu.uu.u.ProvinceId,
                    DistrictId = m.uuu.uu.u.DistrictId,
                    WardId = m.uuu.uu.u.WardId,
                    ProvinceName = m.uuu.uu.p.province_name,
                    DistrictName = m.uuu.d.district_name,
                    WardName = m.w.ward_name

                });
            return user != null ?
                        View(await user.ToListAsync()) :
                        Problem("Entity set 'HungAppleContext.User'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> UsersDetails(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult UsersCreate()
        {
            ViewDefault();
            return View();
        }
        public void ViewDefault()
        {
            ViewBag.ListProvinces = _context.Provinces.ToList();
            ViewBag.ListDistricts = _context.Districts.ToList();
            ViewBag.ListWards = _context.Wards.ToList();
        }
        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsersCreate(User user, IFormFile fileImage)
        {
            ViewDefault();
            var pathImage = string.Empty;
            if (fileImage != null)
            {
                string filename = "";
                filename = fileImage.FileName;
                string folderName = DateTime.Now.ToString("yyMMdd");
                string newPath = Path.Combine(folderName);
                string SavePath = Path.Combine("wwwroot/Upload/Avatar", newPath, filename);
                var fi = new FileInfo(SavePath);
                if (!Directory.Exists(fi.DirectoryName))
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    fileImage.CopyTo(stream);
                }
                pathImage = folderName + "\\" + filename;
            }
            user.ImagePath = pathImage;

            _context.Add(user);
            await _context.SaveChangesAsync();
            await Task.Delay(1500);
            return RedirectToAction(nameof(UsersIndex));
            //return View(user);
        }

        // GET: Users/Edit//
        public async Task<IActionResult> UsersEdit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }
            ViewDefault();
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsersEdit(int id, User user, IFormFile fileImage)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pathImage = string.Empty;
                    if (fileImage != null)
                    {
                        string filename = "";
                        filename = fileImage.FileName;
                        string folderName = DateTime.Now.ToString("yyMMdd");
                        string newPath = Path.Combine(folderName);
                        string SavePath = Path.Combine("wwwroot/Upload/Avatar", newPath, filename);
                        var fi = new FileInfo(SavePath);
                        if (!Directory.Exists(fi.DirectoryName))
                        {
                            Directory.CreateDirectory(fi.DirectoryName);
                        }
                        using (var stream = new FileStream(SavePath, FileMode.Create))
                        {
                            fileImage.CopyTo(stream);
                        }
                        pathImage = folderName + "\\" + filename;
                    }
                    user.ImagePath = (!string.IsNullOrEmpty(pathImage) ? pathImage : user.ImagePath);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                await Task.Delay(1500);
                return RedirectToAction(nameof(UsersIndex));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> UsersDelete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("UsersDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsersDeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'HungAppleContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            await Task.Delay(1500);
            return RedirectToAction(nameof(UsersIndex));
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> IndexComment(int pageNumber = 1)
        {
            var hungAppleContext = _context.Comment
                .Include(p => p.Product)
                .Include(p => p.User)
                .Join(_context.Product, c => c.ProductId, p => p.Id, (c, p) => new { c, p })
                .Join(_context.User, cc => cc.c.UserId, u => u.Id, (cc, u) => new { cc, u })
                .Select(m => new Comment
                {
                    Id = m.cc.c.Id,
                    Content = m.cc.c.Content,
                    Rating = m.cc.c.Rating,
                    DateCreated = m.cc.c.DateCreated,
                    UserName = m.u.Username,
                    ProductName = m.cc.p.Name
                }).OrderByDescending(x => x.Id);
            var pageSize = 10; // Cập nhật kích thước trang là 10
            var total = hungAppleContext.Count();
            ViewBag.total = total;
            ViewBag.pageSize = pageSize;

            var page = total / pageSize;
            if (total % pageSize > 0)
            {
                page += 1;
            }
            ViewBag.page = page;
            ViewBag.pageNumber = pageNumber;
            var pagedComments = await hungAppleContext.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return View(pagedComments);
        }

        public async Task<IActionResult> CommentDelete(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(p => p.Product)
                .Include(p => p.User)
                .Join(_context.Product, c => c.ProductId, p => p.Id, (c, p) => new { c, p })
                .Join(_context.User, cc => cc.c.UserId, u => u.Id, (cc, u) => new { cc, u })
                .Select(m => new Comment
                {
                    Id = m.cc.c.Id,
                    Content = m.cc.c.Content,
                    Rating = m.cc.c.Rating,
                    DateCreated = m.cc.c.DateCreated,
                    UserName = m.u.Username,
                    ProductName = m.cc.p.Name
                }).FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("CommentDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommentDeleteConfirmed(int id)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'HungAppleContext.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }

            await _context.SaveChangesAsync();
            await Task.Delay(1500);
            return RedirectToAction("IndexComment", "Admin");
        }
        public async Task<IActionResult> SubscriceDelete(int? id)
        {
            if (id == null || _context.NewsLetterSubscription == null)
            {
                return NotFound();
            }

            var newLetter =  _context.NewsLetterSubscription.FirstOrDefault(x => x.Id == id);
            if (newLetter == null)
            {
                return NotFound();
            }

            return View(newLetter);
        }


        // POST: NewsLetterSubscriptions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe(int Id)
        {

            var model = _context.NewsLetterSubscription
                               .FirstOrDefault(e => e.Id == Id);

            if (model != null)
            {

                _context.NewsLetterSubscription.Remove(model);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Admin");
        }

        private bool NewsLetterSubscriptionExists(int id)
        {
            return (_context.NewsLetterSubscription?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}