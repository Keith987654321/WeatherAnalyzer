using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace WeatherAnalyzer.Helpers;

public class RelayCommand : ICommand
{
    private readonly Action? _execute;
    private readonly Func<Task>? _executeAsync;
    private readonly Func<bool>? _canExecute;

    private bool _isExecuting;

    public RelayCommand(
        Action execute,
        Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public RelayCommand(
        Func<Task> executeAsync,
        Func<bool>? canExecute = null)
    {
        _executeAsync = executeAsync;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        if (_isExecuting)
        {
            return false;
        }

        return _canExecute?.Invoke() ?? true;
    }

    public async void Execute(object? parameter)
    {
        if (_isExecuting)
        {
            return;
        }

        try
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();

            if (_execute is not null)
            {
                _execute();
            }
            else if (_executeAsync is not null)
            {
                await _executeAsync();
            }
        }
        finally
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(
            this,
            EventArgs.Empty);
    }
}