using HP.Utt.UttCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDefinedToolbarAddin.Controls;
using HP.Utt.UttDialog;

namespace UserDefinedToolbarAddin
{
  public class ShowSelectorDialogCommand:UttBaseCommand
  {
    internal static string UsedCommandsListKey = "UsedCommandsListKey";

    public override void Run()
    {
      CustomDialog dialog = new HP.Utt.UttDialog.CustomDialog("UserDefinedToolbarAddin.SelectorDialog");
      dialog.Content = new CommandsSelector();
      List<string> usedCommands = dialog.PersistenceData.GetValue<List<string>>("", UsedCommandsListKey, new List<string>());
      List<SearchItem> items = SearchItemBuilder.BuildSearchItems();
      CommandsSelectorViewModel viewModel = new CommandsSelectorViewModel(items, usedCommands);
      dialog.DataContext = viewModel;
      dialog.MinWidth = dialog.MaxWidth = 800;
      dialog.MinHeight = dialog.MaxHeight =  400;
      dialog.ResizeMode = System.Windows.ResizeMode.NoResize;
      dialog.AddOkCancelButtons(new Action<CustomDialog>(OnOkPressed));
      dialog.AddButton("Clear", OnClearPressed);
      if (dialog.ShowDialog() == CustomDialogResult.Ok)
      {
        AutostartCommand.UpdateToolbar(viewModel.GetUsedSearchItems());
      }
    }

    private void OnClearPressed(CustomDialog dialog)
    {
      CommandsSelectorViewModel viewModel = dialog.DataContext as CommandsSelectorViewModel;
      if (viewModel != null)
      {
        viewModel.UsedItems.Clear();
      }

    }

    private void OnCancelPressed(CustomDialog dialog)
    {
      dialog.DialogResult = CustomDialogResult.Cancel;
    }

    private void OnOkPressed(CustomDialog dialog)
    {
      CommandsSelectorViewModel viewModel = dialog.DataContext as CommandsSelectorViewModel;
      if (viewModel != null)
      {
        dialog.PersistenceData.SetValue<List<string>>("", UsedCommandsListKey, viewModel.GetUsedCommandsList());
      }
      dialog.Save();
      dialog.DialogResult = CustomDialogResult.Ok;
      dialog.Close();

    }
  }
}
