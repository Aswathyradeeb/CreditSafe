using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System.Web;
using System;
using OfficeOpenXml;
using System.Threading;
using OfficeOpenXml.Drawing.Chart;
using EventsApp.Domain.Enums;

namespace Eventsapp.Services
{
    public class KPIService : IKPIService
    {
        //private readonly ICurrentUser user;

        //public KPIService(ICurrentUser user)
        //{
        //    this.user = user;
        //}


        //public async Task<KPIGraphDTO> GetRegisteredUsers(int eventId)
        //{
        //    using (var dbContext = new eventsappEntities())
        //    {
        //        var roleId = this.user.UserInfo.Roles.FirstOrDefault().Id;
        //        var data = dbContext.fn_GetRegisteredUsers(eventId, this.user.UserInfo.Id, roleId).ToList(); 
        //        List<KPIGraphDTO> kpiGraphLst = new List<KPIGraphDTO>();

        //        KPIGraphDTO kpiGraphDto = new KPIGraphDTO();
        //        kpiGraphDto.Series = new List<SeriesSingleDTO>();
        //        kpiGraphDto.Labels = new List<LabelSingleDTO>();
        //        kpiGraphDto.Graph = new List<GraphDTO>();

        //        kpiGraphDto.ChartType = "pie";
        //        kpiGraphDto.NameAr = "Registered Users KPI";
        //        kpiGraphDto.NameEn = "Registered Users KPI";


        //        if (data.Count() > 0)
        //        {
        //            kpiGraphDto.Series.Add(new SeriesSingleDTO
        //            {
        //                NameEn = data.FirstOrDefault(x => x.EventId == eventId).EventEn,
        //                NameAr = data.FirstOrDefault(x => x.EventId == eventId).EventAr,
        //                Id = data.FirstOrDefault(x => x.EventId == eventId).EventId,
        //                Unit = new UnitDto { NameEn = "No. Of Users", NameAr = "No. Of Users" }
        //            });

        //            GraphDTO graph = new GraphDTO();
        //            graph.Name = data.FirstOrDefault(x => x.EventId == eventId).EventEn;
        //            graph.data = new List<List<string>>();

        //            foreach (var item in data.Where(x => x.EventId == eventId))
        //            {
        //                if (kpiGraphDto.Labels.Count(x => x.Id == item.RegistrationTypeId) == 0)
        //                    kpiGraphDto.Labels.Add(new LabelSingleDTO { Id = item.RegistrationTypeId, NameEn = item.RegistrationTypeEn.ToString(), NameAr = item.RegistrationTypeAr.ToString() });
        //                var dataitems = new List<string>();
        //                dataitems.Add(item.RegistrationTypeEn.ToString());
        //                dataitems.Add(item.NoOfUsersCount.Value.ToString());
        //                graph.data.Add(dataitems);
        //            }
        //            kpiGraphDto.Graph.Add(graph);
        //        }
        //        return kpiGraphDto;
        //    }
        //}


        //public async Task<KPIGraphDTO> GetSurveyResult(int eventId)
        //{
        //    using (var dbContext = new eventsappEntities())
        //    {
        //        var roleId = this.user.UserInfo.Roles.FirstOrDefault().Id;
        //        var data = dbContext.fn_GetSurveyResult(eventId, this.user.UserInfo.Id, roleId).ToList();

        //        List<KPIGraphDTO> kpiGraphLst = new List<KPIGraphDTO>();

        //        KPIGraphDTO kpiGraphDto = new KPIGraphDTO();
        //        kpiGraphDto.Series = new List<SeriesSingleDTO>();
        //        kpiGraphDto.Labels = new List<LabelSingleDTO>();
        //        kpiGraphDto.Graph = new List<GraphDTO>();

        //        kpiGraphDto.ChartType = "pie";
        //        kpiGraphDto.NameAr = "Registered Users KPI";
        //        kpiGraphDto.NameEn = "Registered Users KPI";


        //        if (data.Count() > 0)
        //        {
        //            foreach (var SurveyId in data.Select(x => x.SurveyId).Distinct())
        //            {
        //                kpiGraphDto.Series.Add(new SeriesSingleDTO
        //                {
        //                    NameEn = data.FirstOrDefault(x => x.SurveyId == SurveyId).SurveyEn,
        //                    NameAr = data.FirstOrDefault(x => x.SurveyId == SurveyId).SurveyAr,
        //                    Id = data.FirstOrDefault(x => x.SurveyId == SurveyId).SurveyId,
        //                    Unit = new UnitDto { NameEn = "Count", NameAr = "Count" }
        //                });

        //                GraphDTO graph = new GraphDTO();
        //                graph.Name = data.FirstOrDefault(x => x.SurveyId == SurveyId).SurveyEn;
        //                graph.data = new List<List<string>>();

        //                foreach (var item in data.Where(x => x.SurveyId == SurveyId))
        //                {
        //                    if (kpiGraphDto.Labels.Count(x => x.Id == item.SurveyOptionId) == 0)
        //                        kpiGraphDto.Labels.Add(new LabelSingleDTO { Id = item.SurveyOptionId, NameEn = item.SurveyOptionEn.ToString(), NameAr = item.SurveyOptionAr.ToString() });
        //                    var dataitems = new List<string>();
        //                    dataitems.Add(item.SurveyOptionEn.ToString());
        //                    dataitems.Add(item.Count.Value.ToString());
        //                    graph.data.Add(dataitems);
        //                }
        //                kpiGraphDto.Graph.Add(graph); 
        //            }

        //        }
        //        return kpiGraphDto;
        //    }
        //}

        //public async Task<KPIGraphDTO> GetUsersAttendance(int eventId)
        //{
        //    using (var dbContext = new eventsappEntities())
        //    {
        //        var roleId = this.user.UserInfo.Roles.FirstOrDefault().Id;
        //        var data = dbContext.fn_GetUsersAttendance(eventId, this.user.UserInfo.Id, roleId).ToList();

        //        List<KPIGraphDTO> kpiGraphLst = new List<KPIGraphDTO>();

        //        KPIGraphDTO kpiGraphDto = new KPIGraphDTO();
        //        kpiGraphDto.Series = new List<SeriesSingleDTO>();
        //        kpiGraphDto.Labels = new List<LabelSingleDTO>();
        //        kpiGraphDto.Graph = new List<GraphDTO>();

        //        kpiGraphDto.ChartType = "pie";
        //        kpiGraphDto.NameAr = "Registered Users KPI";
        //        kpiGraphDto.NameEn = "Registered Users KPI";


        //        if (data.Count() > 0)
        //        {
        //            foreach (var RegistrationTypeId in data.Select(x => x.RegistrationTypeId).Distinct())
        //            {
        //                kpiGraphDto.Series.Add(new SeriesSingleDTO
        //                {
        //                    NameEn = data.FirstOrDefault(x => x.RegistrationTypeId == RegistrationTypeId).RegistrationTypeEn,
        //                    NameAr = data.FirstOrDefault(x => x.RegistrationTypeId == RegistrationTypeId).RegistrationTypeAr,
        //                    Id = data.FirstOrDefault(x => x.RegistrationTypeId == RegistrationTypeId).RegistrationTypeId,
        //                    Unit = new UnitDto { NameEn = "Count", NameAr = "Count" }
        //                });

        //                GraphDTO graph = new GraphDTO();
        //                graph.Name = data.FirstOrDefault(x => x.RegistrationTypeId == RegistrationTypeId).RegistrationTypeEn;
        //                graph.data = new List<List<string>>();

        //                foreach (var item in data.Where(x => x.RegistrationTypeId == RegistrationTypeId))
        //                {
        //                    if (kpiGraphDto.Labels.Count(x => x.Id == item.TypeId) == 0)
        //                        kpiGraphDto.Labels.Add(new LabelSingleDTO { Id = item.TypeId, NameEn = item.TypeEn.ToString(), NameAr = item.TypeAr.ToString() });
        //                    var dataitems = new List<string>();
        //                    dataitems.Add(item.TypeEn.ToString());
        //                    dataitems.Add(item.Count.Value.ToString());
        //                    graph.data.Add(dataitems);
        //                }
        //                kpiGraphDto.Graph.Add(graph); 
        //            }

        //        }
        //        return kpiGraphDto;
        //    }
        //}

        //public async Task<byte[]> ExportExcel(List<GraphDTO> excelExportModel)
        //{
        //    using (ExcelPackage xp = new ExcelPackage())
        //    {
        //        ExcelWorksheet ws = xp.Workbook.Worksheets.Add("KPI Graph");
        //        //ws.Protection.IsProtected = true; //--------Protect whole sheet
        //        var currentThread = Thread.CurrentThread.CurrentUICulture;
        //        if (currentThread.Name == "ar-AE")
        //        {
        //            ws.View.RightToLeft = true;
        //        }
        //        int rowstart = 1;
        //        int colstart = 1;
        //        int rowend = rowstart;
        //        int colend = excelExportModel.FirstOrDefault().data.Count;
        //        ws.Cells[rowstart, colstart, rowstart, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[rowstart, colstart, rowstart, colend].Style.Font.Bold = true;
        //        ws.Cells[rowstart, colstart, rowstart, colend].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        ws.Cells[rowstart, colstart, rowstart, colend].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        //        for (int i = 2; i <= excelExportModel.Count + 1; i++)
        //        {
        //            ws.Cells[1, i].Value = excelExportModel[i - 2].Name;
        //            for (int k = 2; k <= excelExportModel[i - 2].data.Count + 1; k++)
        //            {
        //                ws.Cells[k, i].Value = Double.Parse(getValue(excelExportModel[i - 2].data[k - 2][1]));
        //                ws.Cells[k, i].Style.Numberformat.Format = "#,##0.00";
        //            }
        //        }
        //        for (int j = 2; j <= excelExportModel.FirstOrDefault().data.Count + 1; j++)
        //        {
        //            ws.Cells[j, 1].Value = excelExportModel.FirstOrDefault().data[j - 2][0];
        //        }

        //        ws.Cells[ws.Dimension.Address].AutoFitColumns();
        //        var myChart = ws.Drawings.AddChart("chart", eChartType.ColumnClustered);
        //        for (int i = 2; i <= excelExportModel.FirstOrDefault().data.Count + 1; i++)
        //        {

        //            var series = myChart.Series.Add(ws.Cells[i, 2, i, excelExportModel.Count + 1], ws.Cells[1, 2, 1, excelExportModel.Count + 1]);
        //            series.Header = ws.Cells[i, 1].Value != null ? ws.Cells[i, 1].Value.ToString() : "";
        //        }
        //        myChart.Border.Fill.Color = System.Drawing.Color.Green;
        //        // Define series for the chart
        //        // var series = myChart.Series.Add("C2: C4", "B2: B4");

        //        // Add to 6th row and to the 6th column
        //        myChart.SetPosition(excelExportModel.Count + 2, 0, excelExportModel.Count + 2, 0);
        //        //ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
        //        //   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
        //        //   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
        //        //   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        //ws.Cells[rowstart, colstart, rowend, colend].Style.WrapText = true;
        //        //ws.Cells[rowstart, colstart, rowend, colend].Worksheet.DefaultColWidth = 40;// = true;
        //        //ws.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        //        byte[] byteArray = xp.GetAsByteArray();
        //        return byteArray;
        //    }
        //}

        //public string getValue(Object obj)
        //{
        //    if (obj != null && obj.ToString() != "")
        //    {
        //        return obj.ToString();
        //    }
        //    else
        //    {
        //        return "0";
        //    }
        //}

    }
}
