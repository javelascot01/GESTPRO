using GESTPRO.persistence.manages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTPRO.domain
{
    public class Holiday
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        ApiManage am = new ApiManage();
        public async Task<List<Holiday>> GetHolidays(string country, int year, int? month = null, int? day = null)
        {
            var holidays = await am.GetHolidaysAsync(country, year, month, day);

            return holidays;
        }
    }
}
