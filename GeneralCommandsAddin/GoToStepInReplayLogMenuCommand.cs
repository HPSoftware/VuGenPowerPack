// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using HP.LR.VuGen.ServiceCore;
using HP.LR.VuGen.ServiceCore.Data.Debugger;
using HP.LR.VuGen.ServiceCore.Data.StepService;
using HP.LR.VuGen.ServiceCore.Interfaces;
using HP.Utt.InformationPanes;
using HP.Utt.UttCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GeneralCommandsAddin
{
  class GoToStepInReplayLogMenuCommand : UttBaseWpfCommand
  {
    public override void Run()
    {
      var stepService = VuGenServiceManager.GetService<IStepService>();
      var step = stepService.CurrentStep;
      if (step != null)
      {
        FindInReplayLog(step);
      }
    }

    private static string GenerateSearchPattern(int currentLine, string fileName)
    {
      string pattern = string.Format("{0}({1})", fileName, currentLine);
      pattern = "^" + Regex.Escape(pattern);
      return pattern;
    }

    public static void FindInReplayLog(IStepModel step)
    {
      if (step == null || step.FunctionCall == null || step.FunctionCall.Location == null)
        return;

      if (!UttOutputService.Instance.CategoryExists(OutputCategories.ReplayCategory))
        return;

      IFunctionCallLocation location = step.FunctionCall.Location;
      string fileName = Path.GetFileName(location.FilePath);
      string pattern = GenerateSearchPattern(location.Start.Item1, fileName);

      UttOutputService.Instance.BringPadToFront();
      UttOutputService.Instance.JumpToCategoryPosition(OutputCategories.ReplayCategory, 0, 0, false);
      UttOutputService.Instance.FindNext(pattern, false, false, true);

    }
  }
}
