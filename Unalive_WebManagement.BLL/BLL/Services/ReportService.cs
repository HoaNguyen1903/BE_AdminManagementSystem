using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync()
        {
            var reports = await _reportRepository.GetAllAsync();
            return reports.Select(r => new ReportDto
            {
                ReportId = r.ReportId,
                SenderId = r.SenderId,
                AccusedId = r.AccusedId,
                Reason = r.Reason,
                AdditionalInfo = r.AdditionalInfo,
                SendDate = r.SendDate,
                Status = r.Status,
                ApprovedBy = r.ApprovedBy,
                ApprovedDate = r.ApprovedDate
            });
        }

        public async Task<ReportDto?> GetReportByIdAsync(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null) return null;

            return new ReportDto
            {
                ReportId = report.ReportId,
                SenderId = report.SenderId,
                AccusedId = report.AccusedId,
                Reason = report.Reason,
                AdditionalInfo = report.AdditionalInfo,
                SendDate = report.SendDate,
                Status = report.Status,
                ApprovedBy = report.ApprovedBy,
                ApprovedDate = report.ApprovedDate
            };
        }

        public async Task<bool> ApproveReportAsync(int reportId, int staffId)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report == null) return false;

            report.Status = "Approved";
            report.ApprovedBy = staffId;
            report.ApprovedDate = DateTime.Now;

            await _reportRepository.UpdateAsync(report);
            return true;
        }
    }
}
