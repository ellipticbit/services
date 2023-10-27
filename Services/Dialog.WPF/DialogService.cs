using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace EllipticBit.Services.Dialog.WPF
{
	public enum DialogPriority
	{
		Critical,
		Normal,
		Low
	}

	public class DialogService
	{
		private readonly MessageDialogSettings defaultMessageSettings;
		private readonly LoginDialogSettings defaultLoginSettings;
		private DialogHost _defaultHost;
		private ImmutableDictionary<Window, DialogHost> _windows;

		public DialogService(MessageDialogSettings defaultMessageSettings, LoginDialogSettings defaultLoginSettings) {
			_windows = ImmutableDictionary<Window, DialogHost>.Empty;
			this.defaultMessageSettings = defaultMessageSettings;
			this.defaultLoginSettings = defaultLoginSettings;
		}

		public DialogHost RegisterDialogWindow(Window window, bool defaultWindow = false) {
			Dispatcher.CurrentDispatcher.VerifyAccess();
			if (window == null) {
				throw new ArgumentNullException(nameof(window));
			}

			var dh = new DialogHost();
			dh.Visibility = Visibility.Collapsed;

			var wc = window.Content as UIElement;
			window.Content = null;
			var ng = new Grid();
			ng.Children.Add(wc);
			ng.Children.Add(dh);
			window.Content = ng;

			_windows = _windows.Add(window, dh);

			if (_defaultHost == null || defaultWindow) _defaultHost = dh;

			return dh;
		}

		public async Task<DialogResult> ShowMessageDialog(string title, string message, DialogButtons buttons = DialogButtons.Default, DialogPriority priority = DialogPriority.Normal, Window sendTo = null, MessageDialogSettings settings = null) {
			Dispatcher.CurrentDispatcher.VerifyAccess();
			if (sendTo != null && !_windows.ContainsKey(sendTo)) {
				throw new ArgumentOutOfRangeException(nameof(sendTo));
			}

			var dialog = new MessageDialog(title, message, buttons, settings ?? defaultMessageSettings);

			if (sendTo == null) {
				return (DialogResult)await _defaultHost.QueueDialog(dialog, priority).ConfigureAwait(true);
			} else {
				return (DialogResult)await _windows[sendTo].QueueDialog(dialog, priority).ConfigureAwait(true);
			}
		}

		public async Task<TResult> ShowContentDialog<TResult>(DialogWindowBase dialog, DialogPriority priority = DialogPriority.Normal, Window sendTo = null) {
			Dispatcher.CurrentDispatcher.VerifyAccess();
			if (sendTo != null && !_windows.ContainsKey(sendTo)) {
				throw new ArgumentOutOfRangeException(nameof(sendTo));
			}

			if (sendTo == null) {
				return (TResult)await _defaultHost.QueueDialog(dialog, priority).ConfigureAwait(true);
			} else {
				return (TResult)await _windows[sendTo].QueueDialog(dialog, priority).ConfigureAwait(true);
			}
		}

		public Task ShowContentDialog(DialogWindowBase dialog, DialogPriority priority = DialogPriority.Normal, Window sendTo = null) {
			Dispatcher.CurrentDispatcher.VerifyAccess();
			if (sendTo != null && !_windows.ContainsKey(sendTo)) {
				throw new ArgumentOutOfRangeException(nameof(sendTo));
			}

			if (sendTo == null) {
				return _defaultHost.QueueDialog(dialog, priority);
			} else {
				return _windows[sendTo].QueueDialog(dialog, priority);
			}
		}

		public async Task<TResult> ShowLoginDialog<TResult>(string title, DialogButtons buttons = DialogButtons.Default, Window sendTo = null, LoginDialogSettings settings = null) {
			Dispatcher.CurrentDispatcher.VerifyAccess();
			if (sendTo != null && !_windows.ContainsKey(sendTo)) {
				throw new ArgumentOutOfRangeException(nameof(sendTo));
			}

			var dialog = new LoginDialog(title, buttons, settings ?? defaultLoginSettings);

			if (sendTo == null) {
				return (TResult)await _defaultHost.QueueDialog(dialog, DialogPriority.Critical).ConfigureAwait(true);
			} else {
				return (TResult)await _windows[sendTo].QueueDialog(dialog, DialogPriority.Critical).ConfigureAwait(true);
			}
		}
	}
}
