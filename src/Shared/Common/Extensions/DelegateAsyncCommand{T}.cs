﻿namespace LiveScore.Common.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Prism.Commands;

    public class DelegateAsyncCommand<T> : DelegateCommandBase
    {
        private Func<T, Task> _commandTask { get; }

        private Func<T, bool> _canExecute { get; }

        private bool _allowMultipleExecution { get; }

        private Action<Exception> _exceptionHandler { get; }

        public DelegateAsyncCommand(
            Func<T, Task> commandTask,
            Func<T, bool> canExecute = null,
            bool allowMultipleExecution = false,
            Action<Exception> exceptionHandler = null)
        {
            _commandTask = commandTask;
            _canExecute = canExecute;
            _allowMultipleExecution = allowMultipleExecution;
            _exceptionHandler = exceptionHandler;
        }

        public bool CanExecute(T parameter) =>
        _canExecute?.Invoke(parameter) ?? CanExecuteAgain();

        public async Task ExecuteAsync(T parameter) =>
            await _commandTask.Invoke(parameter).ConfigureAwait(false);

        protected override bool CanExecute(object parameter) =>
            CanExecute((T)parameter);

        protected override void Execute(object parameter)
        {
            // Sanity Check
            if (!_allowMultipleExecution && IsExecuting)
            {
                return;
            }

            IsExecuting = true;

            try
            {
                ExecuteAsync((T)parameter).ContinueWith((p) => { });
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Invoke(ex);
            }
            finally
            {
                IsExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        private bool IsExecuting { get; set; }

        private bool CanExecuteAgain() =>
            _allowMultipleExecution || !IsExecuting;
    }
}