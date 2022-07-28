using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    public class KPIGraphDTO
    {
        public int Id
        {
            get
            {
                Random random = new Random();
                return random.Next(1, 3000);
            }
        }
        public string NameEn { get; set; }
        public string Source { get; set; }
        public string DescriptionEn { get; set; }
        public string NameAr { get; set; }
        public string DescriptionAr { get; set; }
        public string ChartType { get; set; }
        public Nullable<bool> Line { get; set; }
        public Nullable<bool> Map { get; set; }
        public Nullable<bool> Coloumn { get; set; }
        public Nullable<bool> Pie { get; set; }
        public List<string> Criteria { get; set; }
        public List<string> Types { get; set; }
        public List<SeriesSingleDTO> Series { get; set; }  //Application Numbers 
        public List<LabelSingleDTO> Labels { get; set; }
        public List<LabelSingleDTO> Events { get; set; }
        public List<string> IssueYear { get; set; } 
        public List<GraphDTO> Graph { get; set; }
        public List<IndicatorDTO> Indicator { get; set; }
        public ICollection<FavouriteDTO> Favourite { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}

public class GraphDTO
{
    public string Name { get; set; }
    public List<List<string>> data { get; set; }
    public object Tag { get; set; }
}
public class GraphModel
{
    public string Name { get; set; }
    public List<string> data { get; set; }
}
public class ExcelExportModel
{
    public List<GraphModel> graph { get; set; }
    public List<string> categories { get; set; }
}

 

//public class KPIGraphDTO
//{
//    public int Id { get; set; }
//    public string NameEn { get; set; }
//    public string Source { get; set; }
//    public string DescriptionEn { get; set; }
//    public string ChartType { get; set; }
//    public List<string> Criteria { get; set; }
//    public List<string> Labels { get; set; }
//    public List<string> Series { get; set; }
//    public List<List<string>> Data { get; set; }
//}