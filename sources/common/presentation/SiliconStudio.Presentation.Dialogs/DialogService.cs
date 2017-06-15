// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Presentation.Services;
using SiliconStudio.Presentation.Windows;
using MessageBoxButton = SiliconStudio.Presentation.Services.MessageBoxButton;
using MessageBoxImage = SiliconStudio.Presentation.Services.MessageBoxImage;
using MessageBoxResult = SiliconStudio.Presentation.Services.MessageBoxResult;

namespace SiliconStudio.Presentation.Dialogs
{
    public class DialogService : IDialogService
    {
        private Action onClosedAction;

        public DialogService([NotNull] IDispatcherService dispatcher, string applicationName)
        {
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));

            Dispatcher = dispatcher;
            ApplicationName = applicationName;
        }

        public string ApplicationName { get; }

        protected IDispatcherService Dispatcher { get; }

        public IFileOpenModalDialog CreateFileOpenModalDialog()
        {
            return new FileOpenModalDialog(Dispatcher);
        }

        public IFolderOpenModalDialog CreateFolderOpenModalDialog()
        {
            return new FolderOpenModalDialog(Dispatcher);
        }

        public IFileSaveModalDialog CreateFileSaveModalDialog()
        {
            return new FileSaveModalDialog(Dispatcher);
        }

        public Task<MessageBoxResult> MessageBox(string message, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.MessageBox(Dispatcher, message, ApplicationName, buttons, image);
        }

        public Task<int> MessageBox(string message, IEnumerable<DialogButtonInfo> buttons, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.MessageBox(Dispatcher, message, ApplicationName, buttons, image);
        }

        public Task<CheckedMessageBoxResult> CheckedMessageBox(string message, bool? isChecked, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.CheckedMessageBox(Dispatcher, message, ApplicationName, isChecked, button, image);
        }

        public Task<CheckedMessageBoxResult> CheckedMessageBox(string message, bool? isChecked, string checkboxMessage, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.CheckedMessageBox(Dispatcher, message, ApplicationName, isChecked, checkboxMessage, button, image);
        }

        public Task<CheckedMessageBoxResult> CheckedMessageBox(string message, bool? isChecked, string checkboxMessage, IEnumerable<DialogButtonInfo> buttons, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.CheckedMessageBox(Dispatcher, message, ApplicationName, isChecked, checkboxMessage, buttons, image);
        }

        public MessageBoxResult BlockingMessageBox(string message, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.BlockingMessageBox(Dispatcher, message, ApplicationName, buttons, image);
        }

        public int BlockingMessageBox(string message, IEnumerable<DialogButtonInfo> buttons, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.BlockingMessageBox(Dispatcher, message, ApplicationName, buttons, image);
        }

        public CheckedMessageBoxResult BlockingCheckedMessageBox(string message, bool? isChecked, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.BlockingCheckedMessageBox(Dispatcher, message, ApplicationName, isChecked, button, image);
        }

        public CheckedMessageBoxResult BlockingCheckedMessageBox(string message, bool? isChecked, string checkboxMessage, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.BlockingCheckedMessageBox(Dispatcher, message, ApplicationName, isChecked, checkboxMessage, button, image);
        }

        public CheckedMessageBoxResult BlockingCheckedMessageBox(string message, bool? isChecked, string checkboxMessage, IEnumerable<DialogButtonInfo> buttons, MessageBoxImage image = MessageBoxImage.None)
        {
            return DialogHelper.BlockingCheckedMessageBox(Dispatcher, message, ApplicationName, isChecked, checkboxMessage, buttons, image);
        }

        public async Task CloseMainWindow(Action onClosed)
        {
            var window = Application.Current.MainWindow;
            if (window != null)
            {
                var asyncClosable = window as IAsyncClosableWindow;
                if (asyncClosable != null)
                {
                    var closed = await asyncClosable.TryClose();
                    if (closed)
                    {
                        onClosed?.Invoke();
                    }
                }
                else
                {
                    onClosedAction = onClosed;
                    window.Closing -= MainWindowClosing;
                    window.Closing += MainWindowClosing;
                    window.Closed -= MainWindowClosed;
                    window.Closed += MainWindowClosed;
                    window.Close();
                }
            }
        }

        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            ((Window)sender).Closing -= MainWindowClosing;
            if (e.Cancel)
            {
                ((Window)sender).Closed -= MainWindowClosed;
            }
        }

        private void MainWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= MainWindowClosed;
            onClosedAction?.Invoke();
        }
    }
}
