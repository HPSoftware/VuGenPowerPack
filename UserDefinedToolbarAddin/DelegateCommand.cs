// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using HP.Utt.UttCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserDefinedToolbarAddin
{
  public class DelegateCommand<T> : UttBaseWpfCommand
    where T:class
  {
    private Func<T,bool> _canExecute = null;
    private Action<T> _execute = null;

    public DelegateCommand(Func<T,bool> canExecute, Action<T> execute)
    {
      _canExecute = canExecute;
      _execute = execute;
    }

    public override bool CanExecute(object parameter)
    {
      if (parameter is T && _canExecute != null)
      {
        return _canExecute(parameter as T);
      }

      return false;
    }

    public override void Execute(object parameter)
    {
      if (parameter is T && _execute != null)
      {
        _execute(parameter as T);
      }

    }

    public override void Run()
    {
      if (_execute != null)
      {
        _execute(null);
      }
    }
  }
}
