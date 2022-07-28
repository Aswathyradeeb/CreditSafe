using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eventsapp.WebAPI.Models
{
    public class PagedList
    {
        public IList Content { get; set; }
        public Int32 CurrentPage { get; set; }
        public Int32 PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalRecords / PageSize); }
        }
        public int FromRecord
        {
            get
            {
                if (CurrentPage > 1) { return (int)Math.Ceiling((decimal)PageSize * (CurrentPage - 1)); }
                return 1;
            }
        }
        public int ToRecord
        {
            get
            {
                if (Content.Count < PageSize) { return (int)Math.Ceiling((decimal)PageSize * (CurrentPage - 1)) + Content.Count; }
                return (int)Math.Ceiling((decimal)CurrentPage * PageSize);
            }
        }
    }

}