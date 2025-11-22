using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;

namespace MVVM_QuanLyQuyTrINH.Services
{
    public class WorkService
    {
        private readonly QLQuyTrinhLamViecContext db_context;

        public WorkService()
        {
            db_context = new QLQuyTrinhLamViecContext();
        }

        public List<CongViec> GetCongViecs()
        {
            return db_context.CongViecs.AsNoTracking()
                 .Include(cv => cv.MaDuAnNavigation)
                 .Include(cv => cv.MaNvphuTrachNavigation)
                     .ThenInclude(nv => nv.MaNvNavigation)
                 .OrderByDescending(cv => cv.NgayGiao)
                 .ToList();
        }
        public bool Add(CongViec congViec)
        {
            try
            {
                db_context.CongViecs.Add(congViec);
                db_context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            using (var transaction = db_context.Database.BeginTransaction())
            {
                try
                {
                    var task = db_context.CongViecs
                        .Include(c => c.LichSuCongViecs)
                        .Include(c => c.PhanCongs)
                        .FirstOrDefault(c => c.MaCv == id);
                    if (task == null) return false;
                    if (task.LichSuCongViecs != null)
                        db_context.LichSuCongViecs.RemoveRange(task.LichSuCongViecs);

                    if (task.PhanCongs != null)
                        db_context.PhanCongs.RemoveRange(task.PhanCongs);
                    db_context.CongViecs.Remove(task);
                    db_context.SaveChanges();
                    int maxId = 0;
                    if (db_context.CongViecs.Any())
                    {
                        maxId = db_context.CongViecs.Max(x => x.MaCv);
                    }
                    string sqlCmd = $"DBCC CHECKIDENT ('CongViec', RESEED, {maxId})";
                    db_context.Database.ExecuteSqlRaw(sqlCmd);
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
        public CongViec GetById(int id)
        {
            return db_context.CongViecs
                .Include(cv => cv.MaDuAnNavigation)
                .Include(cv => cv.MaNvphuTrachNavigation)
                .FirstOrDefault(cv => cv.MaCv == id);
        }
        public bool Update(CongViec congViec)
        {
            try
            {
                db_context.CongViecs.Update(congViec);
                db_context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
