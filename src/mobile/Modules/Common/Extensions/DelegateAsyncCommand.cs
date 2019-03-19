namespace Common.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Prism.Commands;

    public class DelegateAsyncCommand : DelegateCommandBase
    {
        private Func<Task> _commandTask { get; }
        private Func<bool> _canExecute { get; }
        private bool _allowMultipleExecution { get; }
        private Action<Exception> _exceptionHandler { get; }

        public DelegateAsyncCommand(
            Func<Task> commandTask,
            Func<bool> canExecute = null,
            bool allowMultipleExecution = false,
            Action<Exception> exceptionHandler = null)
        {
            _commandTask = commandTask;
            _canExecute = canExecute;
            _allowMultipleExecution = allowMultipleExecution;
            _exceptionHandler = exceptionHandler;
        }

        private bool IsExecuting { get; set; }

        private bool CanExecuteAgain() =>
            _allowMultipleExecution || !IsExecuting;

        private async Task ExecuteAsync() =>
            await _commandTask().ConfigureAwait(false);

        protected override bool CanExecute(object parameter) =>
            _canExecute?.Invoke() ?? CanExecuteAgain();

        protected override void Execute(object parameter)
        {
            if (!_allowMultipleExecution && IsExecuting)
            {
                return;
            }

            IsExecuting = true;

            try
            {
                ExecuteAsync().ContinueWith((o) => { });
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
    }
}