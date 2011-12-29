using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using NetWebScript.Equivalents.Globalization;
using System.Globalization;

namespace NetWebScript.Equivalents
{
	[ScriptEquivalent(typeof(System.DateTime))]
	[ScriptAvailable]
	public sealed class DateTimeEquiv : IFormattable
	{
		private readonly Date date; // the underlaying timestamp is UTC based
		private readonly DateTimeKind kind;

		private DateTimeEquiv(Date date, DateTimeKind kind)
		{
			this.date = date;
			this.kind = kind;
		}

		public DateTimeEquiv(int year, int month, int day)
		{
			this.date = new Date(year, month - 1, day);
			this.kind = DateTimeKind.Unspecified;
		}

		public DateTimeEquiv(int year, int month, int day, int hour, int minute, int second)
			: this(year, month, day, hour, minute, second, DateTimeKind.Unspecified)
		{
		}

		public DateTimeEquiv(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
		{
			if (kind != DateTimeKind.Utc)
			{
				this.date = new Date(year, month-1, day, hour, minute, second);
			}
			else
			{
				this.date = new Date(Script.Date.UTC(year, month - 1, day, hour, minute, second));
			}
			this.kind = kind;
		}

		public DateTimeEquiv(int year, int month, int day, int hour, int minute, int second, int milisecond)
			: this(year, month, day, hour, minute, second, milisecond, DateTimeKind.Unspecified)
		{
		}

		public DateTimeEquiv(int year, int month, int day, int hour, int minute, int second, int milisecond, DateTimeKind kind)
		{
			if (kind != DateTimeKind.Utc)
			{
				this.date = new Date(year, month - 1, day, hour, minute, second, milisecond);
			}
			else
			{
				this.date = new Date(Script.Date.UTC(year, month - 1, day, hour, minute, second, milisecond));
			}
			this.kind = kind;
		}

		public static DateTimeEquiv UtcNow
		{
			get { return new DateTimeEquiv(new Date(), DateTimeKind.Utc); }
		}

		public static DateTimeEquiv Now
		{
			get { return new DateTimeEquiv(new Date(), DateTimeKind.Local); }
		}

		public DateTimeEquiv Date
		{
			get { return new DateTimeEquiv(new Date(Script.Date.UTC(date.GetUTCFullYear(), date.GetUTCMonth(), date.GetUTCDate())), kind); }
		}

		public int Day
		{
			get
			{
				if (kind == DateTimeKind.Utc)
				{
					return date.GetUTCDate();
				}
				return date.GetDate();
			}
		}


		public DayOfWeek DayOfWeek
		{
			get
			{
				if (kind == DateTimeKind.Utc)
				{
					return (DayOfWeek)date.GetUTCDay();
				}
				return (DayOfWeek)date.GetDay();
			}
		}

		public int Hour
		{
			get
			{
				if (kind == DateTimeKind.Utc)
				{
					return date.GetUTCHours();
				}
				return date.GetHours();
			}
		}

		public DateTimeKind Kind
		{
			get { return kind; }
		}

		public int Milisecond
		{
			get
			{
				if (kind == DateTimeKind.Utc)
				{
					return date.GetUTCMilliseconds();
				}
				return date.GetMilliseconds();
				
			}
		}

		public int Minute
		{
			get
			{
				if (kind == DateTimeKind.Utc)
				{
					return date.GetUTCMinutes();
				}
				return date.GetMinutes();
			}
		}

		public int Month
		{
			get
			{
				if (kind == DateTimeKind.Utc)
				{
					return date.GetUTCMonth() + 1;
				}
				return date.GetMonth() + 1;
			}
		}

		public int Second
		{
			get
			{
				if (kind == DateTimeKind.Utc)
				{
					return date.GetUTCSeconds();
				}
				return date.GetSeconds();
			}
		}


		public int Year
		{
			get
			{
				if (kind == DateTimeKind.Utc)
				{
					return date.GetUTCFullYear();
				}
				return date.GetFullYear();
			}
		}

		public DateTimeEquiv ToUniversalTime()
		{
			return new DateTimeEquiv(date, DateTimeKind.Utc);
		}

		public DateTimeEquiv ToLocalTime()
		{
			return new DateTimeEquiv(date, DateTimeKind.Local);
		}

		public DateTimeEquiv AddDays(double value)
		{
			return AddMiliseconds(value * 24 * 60 * 60 * 1000);
		}

		public DateTimeEquiv AddHours(double value)
		{
			return AddMiliseconds(value * 60 * 60 * 1000);
		}

		public DateTimeEquiv AddMinutes(double value)
		{
			return AddMiliseconds(value * 60 * 1000);
		}

		public DateTimeEquiv AddSeconds(double value)
		{
			return AddMiliseconds(value * 1000);
		}

		public DateTimeEquiv AddMiliseconds(double value)
		{
			return new DateTimeEquiv(new Date(date.GetTime() + value), kind);
		}

		public DateTimeEquiv Add(TimeSpan span)
		{
			return AddMiliseconds(span.TotalMilliseconds);
		}

		public DateTimeEquiv AddMonths(int value)
		{
			int newMonth = date.GetUTCMonth() + (value % 12);
			int newYear = date.GetUTCFullYear() + (int)Math.Truncate(value / 12.0);
			int newDay = date.GetUTCDate();

			int daysInMonth = DaysInMonth(newYear, newMonth + 1);
			if (newDay > daysInMonth)
			{
				newDay = daysInMonth;
			}
			return new DateTimeEquiv(new Date(Script.Date.UTC(newYear, newMonth, newDay, date.GetUTCHours(), date.GetUTCMinutes(), date.GetUTCSeconds(), date.GetUTCMilliseconds())), kind);
		}

		public DateTimeEquiv AddYears(int value)
		{
			return AddMonths(value * 12);
		}

		public static int DaysInMonth(int year, int month)
		{
			// We ask day '0' of following month (as javascript month is 0 based) wich is the
			// last day of month
			return new Date(year, month, 0).GetDate(); 
		}

		public override string ToString()
		{
			return DateTimeFormat.FormatDate(null, DateTimeFormatInfo.CurrentInfo, date, kind);
		}

		public string ToString(string format)
		{
			return DateTimeFormat.FormatDate(format, DateTimeFormatInfo.CurrentInfo, date, kind);
		}

		public string ToString(IFormatProvider formatProvider)
		{
			return DateTimeFormat.FormatDate(null, DateTimeFormatInfo.GetInstance(formatProvider), date, kind);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return DateTimeFormat.FormatDate(
				format,
				DateTimeFormatInfo.GetInstance(formatProvider),
				date,
				kind);
		}

		public bool Equals(DateTime other)
		{
			return EqualsPrivate(other); 
		}

		private bool EqualsPrivate(DateTimeEquiv other)
		{
			return other != null && other.kind == kind && other.date.GetTime() == date.GetTime(); 
		}

		public override bool Equals(object obj)
		{
			return EqualsPrivate(obj as DateTimeEquiv);
		}

		public override int GetHashCode()
		{
			return (int)date.GetTime();
		}

		[ScriptBody(Inline = "v")]
		public static implicit operator DateTimeEquiv(DateTime v)
		{
			return new DateTimeEquiv(new Date(v), v.Kind);
		}

		[ScriptBody(Inline = "v")]
		public static implicit operator DateTime(DateTimeEquiv v)
		{
			if (v != null)
			{
				if (v.Kind == DateTimeKind.Utc)
				{
					return v.date.ToDateTime().ToUniversalTime();
				}
				return v.date.ToDateTime();
			}
			return default(DateTime);
		}
	}
}
