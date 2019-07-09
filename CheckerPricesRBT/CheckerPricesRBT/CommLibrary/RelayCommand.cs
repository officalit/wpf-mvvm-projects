using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CheckerPricesRBT.CommLibrary
{
    #region Делегаты для методов WPF команд
    /// <summary>Делегат исполняющего метода команды</summary>
    /// <param name="parameter">Параметер команды</param>
    public delegate void ExecuteHandler(object parameter);
    /// <summary>Делегат проверяющего метода команды</summary>
    /// <param name="parameter">Параметер команды</param>
    public delegate bool CanExecuteHandler(object parameter);
    #endregion




    public class RelayCommand : ICommand
    {
        private readonly CanExecuteHandler _canExecute;
        private readonly ExecuteHandler _onExecute;
        private readonly EventHandler _requerySuggested;

        /// <summary>Событие извещающее об измении состояния команды</summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>Конструктор команды</summary>
        /// <param name="execute">Выполняемый метод команды</param>
        /// <param name="canExecute">Метод разрешающий выполнение команды</param>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute = null)
        {
            _onExecute = execute;
            _canExecute = canExecute;

            _requerySuggested = (o, e) => Invalidate();
            CommandManager.RequerySuggested += _requerySuggested;
        }

        public void Invalidate()
            => Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }), null);

        /// <summary>Вызов разрешающего метода команды</summary>
        /// <param name="parameter">Параметр команды</param>
        /// <returns>True - если выполнение команды разрешено</returns>
        public bool CanExecute(object parameter) => _canExecute == null ? true : _canExecute.Invoke(parameter);

        /// <summary>Вызов выполняющего метода команды</summary>
        /// <param name="parameter">Параметр команды</param>
        public void Execute(object parameter) => _onExecute?.Invoke(parameter);
    }

    /// <summary>
    /// Добавлено для асинхронного таска и канцел-токена
    /// </summary>


    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }

    public sealed class DelegateCommand : ICommand
    {
        private readonly Action _command;

        public DelegateCommand(Action command)
        {
            _command = command;
        }

        public void Execute(object parameter)
        {
            _command();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}
