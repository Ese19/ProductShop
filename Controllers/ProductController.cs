using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetMvc.Data;
using NetMvc.Models;

namespace NetMvc.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly AppDbContext _appDbContext;

    public ProductController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        IEnumerable<Product> ProductsList = _appDbContext.Products;
        return View(ProductsList);
    }

    
    // Get
    public IActionResult Add()
    {
        return View();
    }


    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(Product model)
    {
        try
        {
            if(ModelState.IsValid)
            {
                _appDbContext.Add(model);
                _appDbContext.SaveChanges();
                TempData["Success"] = "Product added successfully";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(ex.ToString(), ex.Message);
            return View();
        }
        
    }

    //Get 
    public IActionResult Edit(int? id)
    {
        if(id == null || id == 0)
        {
            return NotFound();
        }
        var ProductFromDb = _appDbContext.Products.Find(id);
        // var productFromDbFirst = _appDbContext.Products.FirstOrDefault(x => x.Id == id);
        // var productFromDbSingle = _appDbContext.Products.SingleOrDefault(x => x.Id == id);

        if(ProductFromDb == null)
        {
            return NotFound();
        }
        return View(ProductFromDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product model)
    {
        try
        {
            if(ModelState.IsValid)
            {
                _appDbContext.Update(model);
                _appDbContext.SaveChanges();
                TempData["Success"] = "Product edited successfully";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(ex.ToString(), ex.Message);
            return View();
        }
    }

    // Get
    public IActionResult Delete(int? id)
    {
        if(id == null || id == 0)
        {
            return NotFound();
        }
        var ProductFromDb = _appDbContext.Products.Find(id);
        // var productFromDbFirst = _appDbContext.Products.FirstOrDefault(x => x.Id == id);
        // var productFromDbSingle = _appDbContext.Products.SingleOrDefault(x => x.Id == id);

        if(ProductFromDb == null)
        {
            return NotFound();
        }
        return View(ProductFromDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        try
        {
            var obj = _appDbContext.Products.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            _appDbContext.Remove(obj);
            _appDbContext.SaveChanges();
            TempData["Success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(ex.ToString(), ex.Message);
            return View();
        }
    }
}