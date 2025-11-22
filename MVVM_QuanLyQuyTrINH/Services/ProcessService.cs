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
        public List<DuAn> GetDuAnList()
        {
            using (var context = new QLQuyTrinhLamViecContext())
            {
                return context.DuAns.AsNoTracking()
                    .Include(da => da.MaTruongNhomNavigation)
                    .ThenInclude(nv => nv.MaNvNavigation)
                    .Include(da => da.CongViecs)
                    .Include(da => da.QuyTrinhs)
                    .OrderByDescending(da => da.NgayBatDau)
                    .ToList();
            }
        }
        public bool AddDuAn(DuAn newDuAn, List<QuyTrinh> cacBuoc)
        {
            using (var context = new QLQuyTrinhLamViecContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.DuAns.Add(newDuAn);
                        context.SaveChanges();
                        foreach (var buoc in cacBuoc)
                        {
                            buoc.MaDuAn = newDuAn.MaDuAn;
                            context.QuyTrinhs.Add(buoc);
                        }
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
    }
}
