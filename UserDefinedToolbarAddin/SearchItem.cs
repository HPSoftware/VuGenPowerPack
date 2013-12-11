using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;
using System.Windows.Input;

namespace UserDefinedToolbarAddin
{
  public class SearchItem
  {
    public string Id
    {
      get;
      set;
    }

    public List<string> PathChain
    {
      get;
      private set;
    }

    public Codon Codon
    {
      get;
      set;
    }

    public string Label
    {
      get;
      set;
    }

    public KeyGesture Keys
    {
      get;
      set;
    }

    public SearchItem()
    {
      PathChain = new List<string>();
    }

    public string DisplayString
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        foreach (string part in PathChain)
        {
          sb.Append(part);
          sb.Append("\u2192");
        }
        sb.Append(Label);
        return sb.ToString();
      }
    }

    public string Category
    {
      set;
      get;
    }

    public string Shortcut
    {
      get;
      set;
    }


    public bool IsEnabled
    {
      get
      {
        if (Codon.GetFailedAction(this) == ConditionFailedAction.Disable)
          return false;
        return true;

      }
    }

    public bool IsVisible
    {
      get
      {
        if (Codon.GetFailedAction(this) == ConditionFailedAction.Exclude)
          return false;
        return true;
      }
    }

    public string CommandTypeString
    {
      get;
      set;
    }

    /// <summary>
    /// If this command is set then CommandTypeString is ignored
    /// </summary>
    public System.Windows.Input.ICommand Command
    {
      get;
      set;
    }

    public object CommandParameter
    {
      get;
      set;
    }

    public Action Activator
    {
      get
      {
        if (Command != null)
          return () => { Command.Execute(CommandParameter); };

        if (String.IsNullOrWhiteSpace(CommandTypeString))
          return null;

        try
        {
          ICSharpCode.Core.ICommand command = Codon.AddIn.CreateObject(CommandTypeString) as ICSharpCode.Core.ICommand;
          if (command != null)
            return () => { command.Run(); };
        }
        catch
        {
          return null;
        }
        return null;
      }
    }

    public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;




  }
}
