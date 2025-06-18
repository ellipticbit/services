//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace EllipticBit.Services.Email
{
	public enum ParticipationStatus
	{
		NeedsAction,
		Accepted,
		Declined,
		Tentative,
		Delegated
	}

	public sealed class CalendarAttachment : IEmailAttachment
	{
		public string Id { get; }
		public byte[] Content => GetContent();
		public string FileName => "invite.ics";
		public string Type => "text/calendar; method=REQUEST";

		public string EventId { get; }

		public bool IsCanceled { get; } = false;

		public EmailAddress FromAddress { get; set; }
		public List<EmailAddress> ToAddresses { get; } = new List<EmailAddress>();

		public DateTime StartTime { get; set; } = DateTime.Now;
		public DateTime EndTime { get; set; } = DateTime.Now;
		public int AlarmMinutes { get; set; } = 0;
		public ParticipationStatus DefaultStatus { get; set; } = ParticipationStatus.NeedsAction;

		public string Summary { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;

		public CalendarAttachment(string eventId, bool cancel = false, string attachmentId = null) {
			this.Id = attachmentId;
			this.EventId = string.IsNullOrWhiteSpace(eventId) ? Guid.NewGuid().ToString() : eventId;
			this.IsCanceled = cancel;
		}

		private byte[] GetContent() {
			StringBuilder str = new StringBuilder(4096);

			str.AppendLine("BEGIN:VCALENDAR");
			str.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");
			str.AppendLine("VERSION:2.0");
			str.AppendLine("CALSCALE:GREGORIAN");
			str.AppendLine($"METHOD:{(IsCanceled ? "CANCEL" : "REQUEST")}");
			str.AppendLine("BEGIN:VEVENT");

			str.AppendLine($"UID:{EventId}");
			str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", StartTime.ToUniversalTime()));
			str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", EndTime.ToUniversalTime()));
			str.AppendLine($"DTSTAMP:{DateTime.Now.ToUniversalTime():yyyyMMddTHHmmss}");
			if (!string.IsNullOrWhiteSpace(Summary)) str.AppendLine(string.Format("LOCATION:{0}", Location));
			if (!string.IsNullOrWhiteSpace(Description)) str.AppendLine(string.Format("DESCRIPTION:{0}", Description));
			if (!string.IsNullOrWhiteSpace(Description)) str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", Description.Replace("\n", "<br>")));
			if (!string.IsNullOrWhiteSpace(Summary)) str.AppendLine(string.Format("SUMMARY:{0}", Summary));

			str.AppendLine($"ORGANIZER;CN=\"{FromAddress.Name}\":MAILTO:{FromAddress.Email}");
			foreach (var ta in ToAddresses) {
				str.AppendLine($"ATTENDEE;CUTYPE=INDIVIDUAL;PARTSTAT={(DefaultStatus == ParticipationStatus.NeedsAction ? "NEEDS-ACTION" : DefaultStatus.ToString().ToUpperInvariant())};RSVP=TRUE;CN=\"{ta.Name}\";mailto:{ta.Email}");
			}

			if (AlarmMinutes > 0) {
				str.AppendLine("BEGIN:VALARM");
				str.AppendLine($"TRIGGER:-PT{AlarmMinutes}M");
				str.AppendLine("ACTION:DISPLAY");
				str.AppendLine("DESCRIPTION:Reminder");
				str.AppendLine("END:VALARM");
			}

			str.AppendLine("END:VEVENT");
			str.AppendLine("END:VCALENDAR");

			return Encoding.UTF8.GetBytes(str.ToString());
		}
	}
}
