// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserDefinedToolbarAddin.Controls
{
  /// <summary>
  /// Interaction logic for CommandsSelector.xaml
  /// </summary>
  public partial class CommandsSelector : UserControl
  {
    public CommandsSelector()
    {
      InitializeComponent();
    }

    public void UsedListItem_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      CommandsSelectorViewModel viewModel = this.DataContext as CommandsSelectorViewModel;
      if (viewModel != null)
      {
        if (viewModel.MoveLeftCommand.CanExecute(UsedItemsListBox.SelectedItem))
        {
          viewModel.MoveLeftCommand.Execute(UsedItemsListBox.SelectedItem);
        }
      }
    }

    public void UnusedListItem_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      CommandsSelectorViewModel viewModel = this.DataContext as CommandsSelectorViewModel;
      if (viewModel != null)
      {
        if (viewModel.MoveRightCommand.CanExecute(UnusedItemsListBox.SelectedItem))
        {
          viewModel.MoveRightCommand.Execute(UnusedItemsListBox.SelectedItem);
        }
      }
    }
  }
}
