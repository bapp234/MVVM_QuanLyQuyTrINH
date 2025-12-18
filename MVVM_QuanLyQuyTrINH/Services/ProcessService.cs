using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_QuanLyQuyTrINH.Services
{
    public class ProcessService
    {
        private readonly QLQuyTrinhLamViecContext context;

        public List<DuAn> GetDuAnList()
        {
            using (var context = new QLQuyTrinhLamViecContext())
            {
                return context.DuAns.AsNoTracking()
                    .Include(da => da.MaTruongNhomNavigation)
                    .ThenInclude(nv => nv.MaQlNavigation)
                    .Include(da => da.CongViecs)
                    .Include(da => da.QuyTrinhs)
                    .OrderByDescending(da => da.NgayBatDau)
                    .ToList();
            }
        }
        public bool AddDuAn(DuAn newDuAn, List<QuyTrinh> cacBuoc)
        {
            using (var context = new QLQuyTrinhLamViecContext())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.DuAns.Add(newDuAn);
                    context.SaveChanges();
                    if (newDuAn.MaDuAn <= 0)
                        throw new Exception("EF không tạo được MaDuAn (ID = 0)!");
                    foreach (var buoc in cacBuoc)
                    {
                        if (string.IsNullOrWhiteSpace(buoc.TenBuoc))
                            throw new Exception("Tên bước không được để trống");

                        buoc.MaDuAn = newDuAn.MaDuAn;
                        context.QuyTrinhs.Add(buoc);
                    }
                    context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    System.Windows.MessageBox.Show(
                        "Lỗi khi lưu quy trình!\n\n" +
                        ex.Message + "\n" +
                        (ex.InnerException?.Message ?? ""),
                        "Lỗi",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error
                    );

                    return false;
                }
            }
        }
        public bool DeleteDuAn(int maDuAn)
        {
            using (var context = new QLQuyTrinhLamViecContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var duAn= context.DuAns
                            .Include(da => da.CongViecs)
                            .Include(da => da.QuyTrinhs)
                            .FirstOrDefault(da => da.MaDuAn == maDuAn);
                        if (duAn == null)
                            return false;
                       if (duAn.CongViecs != null && duAn.CongViecs.Count > 0)
                        {
                            context.CongViecs.RemoveRange(duAn.CongViecs);
                        }
                       if (duAn.QuyTrinhs != null && duAn.QuyTrinhs.Count > 0)
                        {
                            context.QuyTrinhs.RemoveRange(duAn.QuyTrinhs);
                        }   

                        context.DuAns.Remove(duAn);
                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        public bool UpdateDuAnFull(DuAn duAnToUpdate, List<QuyTrinh> currentSteps, List<QuyTrinh> deletedSteps)
        {
            using (var context = new QLQuyTrinhLamViecContext())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var existingDuAn = context.DuAns.FirstOrDefault(d => d.MaDuAn == duAnToUpdate.MaDuAn);
                    if (existingDuAn == null) return false;

                    existingDuAn.TenDuAn = duAnToUpdate.TenDuAn;
                    existingDuAn.MaTruongNhom = duAnToUpdate.MaTruongNhom;
                    existingDuAn.NgayBatDau = duAnToUpdate.NgayBatDau;
                    existingDuAn.NgayKetThuc = duAnToUpdate.NgayKetThuc;
                    existingDuAn.MoTa = duAnToUpdate.MoTa;
                    existingDuAn.TrangThai = duAnToUpdate.TrangThai;
                    if (deletedSteps != null && deletedSteps.Count > 0)
                    {
                        var idsToDelete = deletedSteps.Where(s => s.MaBuoc > 0).Select(s => s.MaBuoc).ToList();
                        var stepsToDelete = context.QuyTrinhs.Where(q => idsToDelete.Contains(q.MaBuoc)).ToList();
                        context.QuyTrinhs.RemoveRange(stepsToDelete);
                    }
                    int stt = 1;
                    foreach (var step in currentSteps)
                    {
                        step.ThuTu = stt++; 
                        step.MaDuAn = duAnToUpdate.MaDuAn; 

                        if (step.MaBuoc == 0)
                        {
                            context.QuyTrinhs.Add(step);
                        }
                        else
                        {
                            var existingStep = context.QuyTrinhs.FirstOrDefault(q => q.MaBuoc == step.MaBuoc);
                            if (existingStep != null)
                            {
                                existingStep.TenBuoc = step.TenBuoc;
                                existingStep.Mota = step.Mota;
                                existingStep.NgayBatDau = step.NgayBatDau;
                                existingStep.NgayKetThuc = step.NgayKetThuc;
                                existingStep.ThuTu = step.ThuTu;
                            }
                        }
                    }

                    context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool AddBuoc(QuyTrinh quyTrinh)
        {
            try
            {
                using (var context = new QLQuyTrinhLamViecContext())
                {
                    context.QuyTrinhs.Add(quyTrinh);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
