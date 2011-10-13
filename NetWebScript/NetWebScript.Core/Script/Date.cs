using System;
namespace NetWebScript.Script
{
    /// <summary>
    /// JavaScript / ECMAScript Date.
    /// </summary>
    [Imported, IgnoreNamespace]
    public sealed class Date
    {
        private readonly DateTime datetime;

        private static readonly DateTime UnixEpox = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private Date(DateTime datetime)
        {
            this.datetime = datetime;
        }

        public Date()
        {
            datetime = DateTime.Now;
        }

        public Date(double milliseconds)
        {
            datetime = UnixEpox.AddMilliseconds(milliseconds).ToLocalTime();
        }

        public Date(string date)
        {
            throw new NotImplementedException();
        }

        public Date(int year, int month, int date)
        {
            datetime = new DateTime(year, month, date, 0, 0, 0, DateTimeKind.Local);
        }

        public Date(int year, int month, int date, int hours)
        {
            datetime = new DateTime(year, month, date, hours, 0, 0, DateTimeKind.Local);
        }

        public Date(int year, int month, int date, int hours, int minutes)
        {
            datetime = new DateTime(year, month, date, hours, minutes, 0, DateTimeKind.Local);
        }

        public Date(int year, int month, int date, int hours, int minutes, int seconds)
        {
            datetime = new DateTime(year, month, date, hours, minutes, seconds, DateTimeKind.Local);
        }

        public Date(int year, int month, int date, int hours, int minutes, int seconds, int milliseconds)
        {
            datetime = new DateTime(year, month, date, hours, minutes, seconds, milliseconds, DateTimeKind.Local);
        }

        public int GetDate()
        {
            return datetime.Day;
        }

        public int GetDay()
        {
            return (int)datetime.DayOfWeek;
        }

        public int GetFullYear()
        {
            return datetime.Year;
        }

        public int GetHours()
        {
            return datetime.Hour;
        }

        public int GetMilliseconds()
        {
            return datetime.Millisecond;
        }

        public int GetMinutes()
        {
            return datetime.Minute;
        }

        public int GetMonth()
        {
            return datetime.Month;
        }

        public int GetSeconds()
        {
            return datetime.Second;
        }

        public double GetTime()
        {
            return GetTime(datetime);
        }

        public int GetTimezoneOffset()
        {
            throw new NotImplementedException();
        }

        public int GetUTCDate()
        {
            return datetime.ToUniversalTime().Day;
        }

        public int GetUTCDay()
        {
            return (int)datetime.ToUniversalTime().DayOfWeek;
        }

        public int GetUTCFullYear()
        {
            return datetime.ToUniversalTime().Year;
        }

        public int GetUTCHours()
        {
            return datetime.ToUniversalTime().Hour;
        }

        public int GetUTCMilliseconds()
        {
            return datetime.ToUniversalTime().Millisecond;
        }

        public int GetUTCMinutes()
        {
            return datetime.ToUniversalTime().Minute;
        }

        public int GetUTCMonth()
        {
            return datetime.ToUniversalTime().Month;
        }

        public int GetUTCSeconds()
        {
            return datetime.ToUniversalTime().Second;
        }

        public static bool operator ==(Date a, Date b)
        {
            return a.datetime == b.datetime;
        }

        public static bool operator >(Date a, Date b)
        {
            return a.datetime > b.datetime;
        }

        public static bool operator >=(Date a, Date b)
        {
            return a.datetime >= b.datetime;
        }

        public static bool operator !=(Date a, Date b)
        {
            return a.datetime != b.datetime;
        }

        public static bool operator <(Date a, Date b)
        {
            return a.datetime < b.datetime;
        }

        public static bool operator <=(Date a, Date b)
        {
            return a.datetime <= b.datetime;
        }

        public static double operator -(Date a, Date b)
        {
            return (double)((a.datetime - b.datetime).TotalMilliseconds);
        }

        public static int Parse(string value)
        {
            throw new NotImplementedException();
        }

        public void SetDate(int date)
        {
            throw new NotImplementedException();
        }

        public void SetFullYear(int year)
        {
            throw new NotImplementedException();
        }

        public void SetHours(int hours)
        {
            throw new NotImplementedException();
        }

        public void SetMilliseconds(int milliseconds)
        {
            throw new NotImplementedException();
        }

        public void SetMinutes(int minutes)
        {
            throw new NotImplementedException();
        }

        public void SetMonth(int month)
        {
            throw new NotImplementedException();
        }

        public void SetSeconds(int seconds)
        {
            throw new NotImplementedException();
        }

        public void SetTime(int milliseconds)
        {
            throw new NotImplementedException();
        }

        public void SetUTCDate(int date)
        {
            throw new NotImplementedException();
        }

        public void SetUTCFullYear(int year)
        {
            throw new NotImplementedException();
        }

        public void SetUTCHours(int hours)
        {
            throw new NotImplementedException();
        }

        public void SetUTCMilliseconds(int milliseconds)
        {
            throw new NotImplementedException();
        }

        public void SetUTCMinutes(int minutes)
        {
            throw new NotImplementedException();
        }

        public void SetUTCMonth(int month)
        {
            throw new NotImplementedException();
        }

        public void SetUTCSeconds(int seconds)
        {
            throw new NotImplementedException();
        }

        public void SetYear(int year)
        {
            throw new NotImplementedException();
        }

        public string ToDateString()
        {
            throw new NotImplementedException();
        }

        public string ToLocaleDateString()
        {
            throw new NotImplementedException();
        }

        public string ToLocaleTimeString()
        {
            throw new NotImplementedException();
        }

        public string ToTimeString()
        {
            throw new NotImplementedException();
        }

        public string ToUTCString()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        private static double GetTime(DateTime datetime)
        {
            return (double)(datetime - UnixEpox).TotalMilliseconds;
        }

        [PreserveCase]
        public static double UTC(int year, int month, int day)
        {
            return GetTime(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc));
        }

        [PreserveCase]
        public static double UTC(int year, int month, int day, int hours)
        {
            return GetTime(new DateTime(year, month, day, hours, 0, 0, DateTimeKind.Utc));
        }

        [PreserveCase]
        public static double UTC(int year, int month, int day, int hours, int minutes)
        {
            return GetTime(new DateTime(year, month, day, hours, minutes, 0, DateTimeKind.Utc));
        }

        [PreserveCase]
        public static double UTC(int year, int month, int day, int hours, int minutes, int seconds)
        {
            return GetTime(new DateTime(year, month, day, hours, minutes, seconds, DateTimeKind.Utc));
        }

        [PreserveCase]
        public static double UTC(int year, int month, int day, int hours, int minutes, int seconds, int milliseconds)
        {
            return GetTime(new DateTime(year, month, day, hours, minutes, seconds, milliseconds, DateTimeKind.Utc));
        }
    }
}

