using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EllipticBit.Services.Dialog.WPF
{
	public partial class DialogHost : Grid
	{
		private class DialogState
		{
			public DialogWindowBase Dialog { get; }
			public Task<object> Awaiter { get; }

			public DialogState(DialogWindowBase dialog) {
				this.Dialog = dialog;
				this.Awaiter = dialog.WaitForCloseAsync();
			}
		}

		public delegate void DialogOpeningHandler(DialogHost sender, DialogWindowBase dialog);
		public delegate void DialogClosingHandler(DialogHost sender, DialogWindowBase dialog);

		public event DialogOpeningHandler DialogOpening;
		public event DialogClosingHandler DialogClosing;

		private ConcurrentQueue<DialogState> criticalQueue;
		private ConcurrentQueue<DialogState> normalQueue;
		private ConcurrentQueue<DialogState> lowQueue;

		public bool IsActive { get; private set; }

		internal DialogHost() {
			criticalQueue = new ConcurrentQueue<DialogState>();
			normalQueue = new ConcurrentQueue<DialogState>();
			lowQueue = new ConcurrentQueue<DialogState>();

			InitializeComponent();
		}

		internal Task<object> QueueDialog(DialogWindowBase dialog, DialogPriority priority) {
			dialog.Host = this;
			var state = new DialogState(dialog);

			if (priority == DialogPriority.Normal) {
				normalQueue.Enqueue(state);
			} else if (priority == DialogPriority.Critical) {
				criticalQueue.Enqueue(state);
			} else {
				lowQueue.Enqueue(state);
			}

			ProcessDialogQueue();

			return state.Awaiter;
		}

		internal void SetDialog(DialogWindowBase dialog) {
			if (dialog == null) {
				this.Visibility = System.Windows.Visibility.Collapsed;
				MessageContent.Content = null;
				IsActive = false;
				DialogClosing(this, MessageContent.Content as DialogWindowBase);
				ProcessDialogQueue();
				return;
			}

			if (IsActive) DialogClosing(this, MessageContent.Content as DialogWindowBase);
			MessageContent.Content = dialog;
			this.Visibility = System.Windows.Visibility.Visible;
			DialogOpening(this, dialog);
			IsActive = true;
		}

		private void ProcessDialogQueue() {
			if (IsActive) return;
			if (criticalQueue.TryDequeue(out DialogState critical)) {
				SetDialog(critical.Dialog);
			} else if (normalQueue.TryDequeue(out DialogState normal)) {
				SetDialog(normal.Dialog);
			} else if (lowQueue.TryDequeue(out DialogState low)) {
				SetDialog(low.Dialog);
			}
		}
	}
}