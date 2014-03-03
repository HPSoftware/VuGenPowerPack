// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using HP.LR.Vugen.Common;
using HP.LR.VuGen.ServiceCore;
using HP.LR.VuGen.ServiceCore.Data.ProjectSystem;
using HP.LR.VuGen.ServiceCore.Interfaces;
using HP.Utt.ProjectSystem;
using HP.Utt.UttCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionWriterAddin
{
  public class AutostartCommand : UttBaseCommand
  {

    public override void Run()
    {
      VuGenServiceManager.RunWhenInitialized(InnerRun);
    }

    private void InnerRun()
    {
      IVuGenProjectService projectService = VuGenServiceManager.GetService<IVuGenProjectService>();
      projectService.ScriptSaved += projectService_ScriptSaved;
      projectService.ScriptSavedAs += projectService_ScriptSavedAs;
      if (projectService is UttProjectService)
      (projectService as UttProjectService).ScriptExported+=AutostartCommand_ScriptExported;

    }

    void projectService_ScriptSavedAs(object sender, SaveAsEventArgs e)
    {
      AddTransactionNames(e.Script as IVuGenScript);
    }

    void AutostartCommand_ScriptExported(object sender, ExportEventArgs e)
    {
      AddTransactionNames(e.Script as IVuGenScript,Path.Combine(e.PersistenceToken.LocalPath,e.PersistenceToken.LoadData));
    }

    void projectService_ScriptSaved(object sender, HP.Utt.ProjectSystem.SaveEventArgs e)
    {
      AddTransactionNames(e.Script as IVuGenScript);
    }

    private static void AddTransactionNames(IVuGenScript script, string usrFileName=null)
    {
      try
      {
        if (script == null) return;
        List<IExtraFileScriptItem> extraFiles = script.GetExtraFiles();
        IExtraFileScriptItem dynamicTransacrions = null;
        foreach (IExtraFileScriptItem extraFile in extraFiles)
        {
          if (extraFile.FileName == "dynamic_transactions.txt") //This is not configurable. This is the file name!
          {
            dynamicTransacrions = extraFile;
            break;
          }
        }

        if (dynamicTransacrions == null) return;

        string fileContent = File.ReadAllText(dynamicTransacrions.FullFileName);
        string[] transactionNames = fileContent.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        IScriptDataObject scriptData = script.GenerateDataObject() as IScriptDataObject;
        if (scriptData == null) return;
        List<String> transactionsList = scriptData.SpecialSteps.Transactions;
        transactionsList.AddRange(transactionNames);
        IniUsrFile usr;
        if (usrFileName != null)
        {
          usr = new IniUsrFile(usrFileName);
        }
        else
        {
          usr = new IniUsrFile(script.FileName); 
        }
        usr.WriteTransactions(transactionsList);
        usr.Save();
      }
      catch
      {
        //Don't want to have adverse influence on the regular usage of VuGen. If this fails it does so silently. 
      }
    }
  }
}
