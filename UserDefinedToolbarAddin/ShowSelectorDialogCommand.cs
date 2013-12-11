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
    private static string UsedCommandsListKey = "UsedCommandsListKey";

    public override void Run()
    {
      CustomDialog dialog = new HP.Utt.UttDialog.CustomDialog("UserDefinedToolbarAddin.SelectorDialog");
      dialog.Content = new CommandsSelector();
      List<string> usedCommands = dialog.PersistenceData.GetValue<List<string>>("", UsedCommandsListKey, new List<string>());
      List<SearchItem> items = SearchItemBuilder.BuildSearchItems();
      dialog.DataContext = new CommandsSelectorViewModel(items, usedCommands);
      dialog.MinWidth = 600;
      dialog.MinHeight = 400;
      dialog.MaxHeight = 600;
      dialog.AddOkCancelButtons(new Action<CustomDialog>(OnOkPressed));
      dialog.ShowDialog();
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
      dialog.DialogResult = CustomDialogResult.Ok;
      dialog.Close();
    }
  }
}
