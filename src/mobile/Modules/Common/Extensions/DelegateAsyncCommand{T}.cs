namespace Common.Extensions
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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AP.MobileToolkit.Commands.DelegateAsyncCommand`1"/> class.
        /// </summary>
        /// <param name="commandTask">Command task.</param>
        /// <param name="canExecute">Can execute.</param>
        /// <param name="allowMultipleExecution">If set to <c>true</c> allow multiple execution.</param>
        /// <param name="exceptionHandler">Exception handler.</param>
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

        /// <summary>
        /// Cans the execute.
        /// </summary>
        /// <returns><c>true</c>, if execute was caned, <c>false</c> otherwise.</returns>
        /// <param name="parameter">Parameter.</param>
        public bool CanExecute(T parameter) =>
        _canExecute?.Invoke(parameter) ?? CanExecuteAgain();

        /// <summary>
        /// Executes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="parameter">Parameter.</param>
        public async Task ExecuteAsync(T parameter) =>
            await _commandTask.Invoke(parameter).ConfigureAwait(false);

        /// <inheritDoc />
        protected override bool CanExecute(object parameter) =>
            CanExecute((T)parameter);

        /// <inheritDoc />
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
