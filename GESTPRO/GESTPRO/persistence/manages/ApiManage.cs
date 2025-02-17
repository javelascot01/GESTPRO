using GESTPRO.domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GESTPRO.persistence.manages
{
    public class ApiManage
    {
        private const string ApiKey = "n4cjR6uwsSsZudUQ5at3nHOeW78OYzTs";
        private const string BaseUrl = "https://calendarific.com/api/v2/holidays";

        public async Task<List<Holiday>> GetHolidaysAsync(string country, int year, int? month = null, int? day = null)
        {
            using (var client = new HttpClient())
            {
                string url = $"{BaseUrl}?api_key={ApiKey}&country={country}&year={year}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error al consultar la API: {response.StatusCode}");
                }

                string json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<CalendarificResponse>(json);

                if (apiResponse.meta.Code != 200)
                {
                    throw new HttpRequestException($"Error en la API: Código {apiResponse.meta.Code}");
                }

                // Mapear y filtrar los datos
                var filteredHolidays = apiResponse.response.Holidays.AsEnumerable();

                if (month.HasValue)
                {
                    filteredHolidays = filteredHolidays.Where(h => h.Date.DateTime.Month == month.Value);
                }

                if (day.HasValue)
                {
                    filteredHolidays = filteredHolidays.Where(h => h.Date.DateTime.Day == day.Value);
                }

                return filteredHolidays
                    .Select(h => new Holiday
                    {
                        Name = h.Name,
                        Description = string.Join(", ", h.Type),
                        Date = DateTime.Parse(h.Date.Iso)
                    })
                    .ToList();
            }
        }

        // Respuesta de la API
        private class CalendarificResponse
        {
            [JsonProperty("meta")]
            public Meta meta { get; set; }

            [JsonProperty("response")]
            public Response response { get; set; }

            public class Meta
            {
                [JsonProperty("code")]
                public int Code { get; set; }
            }

            public class Response
            {
                [JsonProperty("holidays")]
                public List<Holiday> Holidays { get; set; }
            }

            public class Holiday
            {
                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("date")]
                public HolidayDate Date { get; set; }

                [JsonProperty("type")]
                public List<string> Type { get; set; }
            }

            public class HolidayDate
            {
                [JsonProperty("iso")]
                public string Iso { get; set; }

                [JsonProperty("datetime")]
                public HolidayDateTime DateTime { get; set; }
            }

            public class HolidayDateTime
            {
                [JsonProperty("year")]
                public int Year { get; set; }

                [JsonProperty("month")]
                public int Month { get; set; }

                [JsonProperty("day")]
                public int Day { get; set; }
            }
        }
    }
}
