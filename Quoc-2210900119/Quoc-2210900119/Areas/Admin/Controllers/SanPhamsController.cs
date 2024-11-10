using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Quoc_2210900119.Models;

namespace Quoc_2210900119.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private CHBHTH2Entities1 db = new CHBHTH2Entities1();

        // GET: Admin/SanPhams
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var sanPhams = db.SanPhams.Include(s => s.LoaiHang).Include(s => s.NhaCungCap);

            // Tổng số sản phẩm
            int totalProducts = sanPhams.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Lấy sản phẩm cho trang hiện tại và sắp xếp theo MaSP giảm dần
            var paginatedProducts = sanPhams.OrderByDescending(s => s.MaSP) // Sắp xếp theo MaSP giảm dần
                                              .Skip((page - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToList();

            // Gán giá trị cho ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(paginatedProducts);
        }



        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(db.LoaiHangs, "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,GiaBan,Soluong,MoTa,MaLoai,MaNCC,AnhSP")] SanPham sanPham, HttpPostedFileBase FileAnh)
        {
             if (ModelState.IsValid)
            {
                // Kiểm tra nếu có tệp được tải lên
                if (FileAnh != null && FileAnh.ContentLength > 0)
                {
                    // Lấy tên tệp và tạo đường dẫn để lưu trữ
                    string fileName = Path.GetFileName(FileAnh.FileName);
                    string path = Path.Combine(Server.MapPath("~/Data/Image"), fileName);

                    // Lưu tệp ảnh vào thư mục "Content/Images"
                    FileAnh.SaveAs(path);

                    // Lưu đường dẫn ảnh vào thuộc tính AnhSP
                    sanPham.AnhSP = "/Data/Image" + fileName;
                }

                // Lưu thông tin sản phẩm vào cơ sở dữ liệu
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoai = new SelectList(db.LoaiHangs, "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Edit/5
        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaLoai = new SelectList(db.LoaiHangs, "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,GiaBan,Soluong,MoTa,MaLoai,MaNCC,AnhSP")] SanPham sanPham, HttpPostedFileBase fileAnh)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu có file ảnh được tải lên
                if (fileAnh != null && fileAnh.ContentLength > 0)
                {

                    // Đường dẫn lưu ảnh mới
                    string rootFolder = Server.MapPath("/Data/Image");
                    string pathImage = Path.Combine(rootFolder, Path.GetFileName(fileAnh.FileName));

                    // Lưu file ảnh lên server
                    fileAnh.SaveAs(pathImage);

                    // Cập nhật đường dẫn ảnh mới trong thuộc tính AnhSP
                    sanPham.AnhSP = "/Data/Image/" + Path.GetFileName(fileAnh.FileName);
                }

                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoai = new SelectList(db.LoaiHangs, "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(sanPham);
        }



        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // POST: Admin/SanPhams/DeleteSelected
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSelected(int[] selectedIds)
        {
            if (selectedIds != null && selectedIds.Length > 0)
            {
                foreach (var id in selectedIds)
                {
                    SanPham sanPham = db.SanPhams.Find(id);
                    if (sanPham != null)
                    {
                        db.SanPhams.Remove(sanPham);
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

       
    }
}
