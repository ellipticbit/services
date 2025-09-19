//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
	public static class SystemExtensions
	{
		// Compiled regex for better performance
		private static readonly Regex UrlRegex = new Regex(@"^(https?|ftps?):\/\/(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}(?::(?:0|[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5]))?(?:\/(?:[-a-zA-Z0-9@%_\+.~#?&=]+\/?)*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		///     Validates a URL using a regular expression.
		/// </summary>
		/// <param name="url">The URL to validate.</param>
		/// <returns>true if the URL is valid; otherwise, false.</returns>
		public static bool IsValidUrl(this string url)
		{
			return UrlRegex.IsMatch(url);
		}

		public static DateTime StartOfWeek(this DateTime date) {
			return date.AddDays(-(int)date.DayOfWeek);
		}

		public static DateTimeOffset StartOfWeek(this DateTimeOffset date) {
			return date.AddDays(-(int)date.DayOfWeek);
		}

		public static bool IsLastOfMonth(this DateTime date, DayOfWeek weekDay)
		{
			if (date.DayOfWeek != weekDay) return false;

			return date.AddDays(7).Month != date.Month;
		}

		public static bool IsLastOfMonth(this DateTimeOffset date, DayOfWeek weekDay)
		{
			if (date.DayOfWeek != weekDay) return false;

			return date.AddDays(7).Month != date.Month;
		}

		public static int GetWeekNumber(this DateTime date)
		{
			return GetWeekNumber(date, CultureInfo.CurrentCulture);
		}

		public static int GetWeekNumber(this DateTime date, CultureInfo culture)
		{
			return culture.Calendar.GetWeekOfYear(date,
				culture.DateTimeFormat.CalendarWeekRule,
				culture.DateTimeFormat.FirstDayOfWeek);
		}

		public static int GetWeekNumber(this DateTimeOffset date)
		{
			return GetWeekNumber(date, CultureInfo.CurrentCulture);
		}

		public static int GetWeekNumber(this DateTimeOffset date, CultureInfo culture)
		{
			return culture.Calendar.GetWeekOfYear(date.DateTime,
				culture.DateTimeFormat.CalendarWeekRule,
				culture.DateTimeFormat.FirstDayOfWeek);
		}

		public static int GetWeekOfMonth(this DateTime date)
		{
			return GetWeekOfMonth(date, CultureInfo.CurrentCulture);
		}

		public static int GetWeekOfMonth(this DateTime date, CultureInfo culture)
		{
			return date.GetWeekNumber(culture) - new DateTime(date.Year, date.Month, 1).GetWeekNumber(culture) + 1;
		}

		public static int GetWeekOfMonth(this DateTimeOffset date)
		{
			return GetWeekOfMonth(date, CultureInfo.CurrentCulture);
		}

		public static int GetWeekOfMonth(this DateTimeOffset date, CultureInfo culture)
		{
			return date.GetWeekNumber(culture) - new DateTime(date.Year, date.Month, 1).GetWeekNumber(culture) + 1;
		}

		public static bool Contains(this string str, string substring, StringComparison comp) {
			if (substring == null) {
				throw new ArgumentNullException("substring", "substring cannot be null.");
			}
			else if (!Enum.IsDefined(typeof(StringComparison), comp)) {
				throw new ArgumentException("comp is not a member of StringComparison", "comp");
			}

			return str.IndexOf(substring, comp) >= 0;
		}

		public static string StringToBase64Utf8(this string str)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
		}

		public static string StringFromBase64Utf8(this string str)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(str));
		}

		public static string ArrayToBase64(this byte[] str)
		{
			return Convert.ToBase64String(str);
		}

		public static byte[] ArrayFromBase64(this string str)
		{
			return Convert.FromBase64String(str);
		}

		public static string StringToUrlBase64Utf8(this string str)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(str)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
		}

		public static string StringFromUrlBase64Utf8(this string str)
		{
			str += (str.Length % 4 == 2 ? "==" : str.Length % 4 == 3 ? "=" : string.Empty);
			return Encoding.UTF8.GetString(Convert.FromBase64String(str.Replace('-', '+').Replace('_', '/')));
		}

		public static string ArrayToUrlBase64(this byte[] str)
		{
			return Convert.ToBase64String(str).TrimEnd('=').Replace('+', '-').Replace('/', '_');
		}

		public static byte[] ArrayFromUrlBase64(this string str)
		{
			str += (str.Length % 4 == 2 ? "==" : str.Length % 4 == 3 ? "=" : string.Empty);
			return Convert.FromBase64String(str.Replace('-', '+').Replace('_', '/'));
		}

		public static string ToHexString(this byte[] bytes) {
			return BitConverter.ToString(bytes).Replace("-", string.Empty);
		}

		public static byte[] FromHexString(this string hex) {
			byte[] raw = new byte[hex.Length / 2];
			for (int i = 0; i < raw.Length; i++) {
				raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
			}
			return raw;
		}

		public static short? ToNullableInt16(this string s) {
			if (short.TryParse(s, out short i)) return i;
			return null;
		}

		public static int? ToNullableInt32(this string s) {
			if (int.TryParse(s, out int i)) return i;
			return null;
		}

		public static long? ToNullableInt64(this string s) {
			if (long.TryParse(s, out long i)) return i;
			return null;
		}

		public static ushort? ToNullableUInt16(this string s) {
			if (ushort.TryParse(s, out ushort i)) return i;
			return null;
		}

		public static uint? ToNullableUInt32(this string s) {
			if (uint.TryParse(s, out uint i)) return i;
			return null;
		}

		public static ulong? ToNullableUInt64(this string s) {
			if (ulong.TryParse(s, out ulong i)) return i;
			return null;
		}

		public static float? ToNullableFloat(this string s) {
			if (float.TryParse(s, out float i)) return i;
			return null;
		}

		public static double? ToNullableDouble(this string s) {
			if (double.TryParse(s, out double i)) return i;
			return null;
		}

		public static decimal? ToNullableDecimal(this string s) {
			if (decimal.TryParse(s, out decimal i)) return i;
			return null;
		}
	}
}
