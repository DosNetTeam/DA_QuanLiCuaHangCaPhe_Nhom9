//csharp DataAccess/KhoTruyVanMainForm.cs
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess;
using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess
{
    public class KhoTruyVanMainForm
    {
        // Try EF first; if any exception occurs use ADO repositories.

        public List<string> TaiLoaiSanPham()
        {
            try
            {
                using (var db = new DataSqlContext())
                {
                    var tatCaSanPham = db.SanPhams.ToList();
                    var cacLoaiSPTam = new List<string>();
                    foreach (var sp in tatCaSanPham)
                    {
                        if (!string.IsNullOrWhiteSpace(sp.LoaiSp)) cacLoaiSPTam.Add(sp.LoaiSp);
                    }
                    var ketQua = new List<string>();
                    foreach (var loai in cacLoaiSPTam) if (!ketQua.Contains(loai)) ketQua.Add(loai);
                    return ketQua;
                }
            }
            catch (Exception)
            {
                // Fallback ADO
                var all = SanPhamRepository.GetAllActive();
                var set = new HashSet<string>();
                foreach (var sp in all) if (!string.IsNullOrWhiteSpace(sp.LoaiSp)) set.Add(sp.LoaiSp);
                return new List<string>(set);
            }
        }

        public DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main.DuLieuSanPham LayDuLieuSanPham()
        {
            try
            {
                using (var db = new DataSqlContext())
                {
                    var tatCaSanPham_raw = db.SanPhams.ToList();
                    var tatCaNguyenLieu_raw = db.NguyenLieus.ToList();
                    var allDinhLuong = db.DinhLuongs.ToList();

                    var tatCaSanPham_filter = new List<SanPham>();
                    var allNguyenLieu_filter = new List<NguyenLieu>();

                    foreach (var sp in tatCaSanPham_raw)
                    {
                        if (sp.TrangThai == "Còn bán") tatCaSanPham_filter.Add(sp);
                    }

                    foreach (var nl in tatCaNguyenLieu_raw)
                    {
                        if (nl.TrangThai == "?ang kinh doanh") allNguyenLieu_filter.Add(nl);
                    }

                    return new DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main.DuLieuSanPham
                    {
                        TatCaSanPham = tatCaSanPham_filter,
                        AllDinhLuong = allDinhLuong,
                        AllNguyenLieu = allNguyenLieu_filter
                    };
                }
            }
            catch (Exception)
            {
                // Fallback ADO
                var allSp = SanPhamRepository.GetAllActive();
                var listSp = new List<SanPham>();
                foreach (var sp in allSp)
                {
                    if (sp.TrangThai == "Còn bán") listSp.Add(sp);
                }

                var allNl = NguyenLieuRepository.GetAllActive();
                var allDl = new List<DinhLuong>();
                // load DinhLuong via ADO
                string sql = "SELECT MaSP, MaNL, SoLuongCan FROM DinhLuong";
                var dlrows = AdoNetHelper.QueryList(sql, r =>
                {
                    var dl = new DinhLuong();
                    dl.MaSp = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                    dl.MaNl = r.IsDBNull(1) ? 0 : r.GetInt32(1);
                    dl.SoLuongCan = r.IsDBNull(2) ? 0m : r.GetDecimal(2);
                    return dl;
                });
                allDl = dlrows;

                return new DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main.DuLieuSanPham
                {
                    TatCaSanPham = listSp,
                    AllDinhLuong = allDl,
                    AllNguyenLieu = allNl
                };
            }
        }

    }
}

