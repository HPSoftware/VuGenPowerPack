using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using HP.LR.Vugen.Common;
using HP.LR.VuGen.BackEnd.StepManager.StepData;
using HP.LR.VuGen.ServiceCore.Data.StepService;
using HP.LR.VuGen.ServiceCore.Interfaces;
using HP.LR.VuGen.Snapshots.WebSnapshotBrowserControl.Interfaces;
using HP.LR.VuGen.Snapshots.WebSnapshotBrowserControl.ViewEditors;
using HP.Utt.UttCore;
using ICSharpCode.Core;
using ICSharpCode.Core.Services;
using HP.Utt.UttDialog;

namespace HttpXmlViewAddin
{
  public abstract class LrXmlStepCommand : UttBaseCommand
  {
    public static string GetCurrentStepResponseParameter()
    {
      IStepService stepService = ServiceManager.Instance.GetService<IStepService>();
      var step = stepService.CurrentStep;
      string result = step.GetExtensionProperty("dataposid");
      return result;
    }

    public static void SetCurrentStepResponseParameter(string value)
    {
      IStepService stepService = ServiceManager.Instance.GetService<IStepService>();
      var step = stepService.CurrentStep;
      step.SetExtensionProperty("dataposid", value);
    }


    protected string _xpath = "";
    protected string _elementName = "";
    protected string _value = "";

    private string ExtractElementName(string elementString)
    {
      XmlDocument doc = new XmlDocument();
      try
      {
        doc.LoadXml(elementString);
        return doc.DocumentElement.Name;
      }
      catch (Exception ex)
      {
        Log.VuGen.Error(string.Format("{0} Is not a valid XML document. Exception : {1}", elementString, ex.Message));
      }

      return "";
    }

    private string ExtractElementValue(string elementString)
    {
      XmlDocument doc = new XmlDocument();
      try
      {
        doc.LoadXml(elementString);
        return doc.DocumentElement.InnerText;
      }
      catch (Exception ex)
      {
        Log.VuGen.Error(string.Format("{0} Is not a valid XML document. Exception : {1}", elementString, ex.Message));
      }

      return "";
    }

    public override void Run()
    {
      if (!(Owner is ISelectableEditor))
        return;

      try
      {
        XmlEditor editor = Owner as XmlEditor;
        if (editor == null)
          return;

        _xpath = editor.SingleDirectionData.SelectedXPath.XPath;
        _elementName = ExtractElementName(editor.SingleDirectionData.SelectedText);
        _value = ExtractElementValue(editor.SingleDirectionData.SelectedText);

        var selectableEditor = Owner as ISelectableEditor;
        if (selectableEditor.Selected == null)
          return;

        var stepService = ServiceManager.Instance.GetService<IStepService>();
        if (stepService == null)
          return;

        var currentStep = stepService.CurrentStep;

        var parserStatus = stepService.GetParserStatus(currentStep.FunctionCall.Location.FilePath);
        if (parserStatus == false)
        {
          MessageService.ShowMessage("Cannot add step");
          return;
        }

        var functionCall = new FunctionCall();
        var signature = new FunctionCallSignature { Name = this.Name };

        string currentParameterName = GetCurrentStepResponseParameter();
        if (!string.IsNullOrEmpty(currentParameterName))
        {
          signature.Parameters.Add(new FunctionCallParameter(String.Format("XML={0}", currentParameterName),
                                                  ParameterType.ArgtypeString));

        }

        FillParameters(signature);

        functionCall.Signature = signature;
        functionCall.Location = new FunctionCallLocation(currentStep.FunctionCall.Location.FilePath, null, null);


        IStepModel stepModel = stepService.GenerateStep(functionCall);
        stepModel.ShowArguments(asyncResult =>
        {
          var result = (ShowArgumentsResult)asyncResult.AsyncState;
          if (result.IsModified)
          {
            //set the last parameter to False, so it will not move cursor to the newly added step
            stepService.AddStep(ref stepModel, currentStep,
                                Relative, false);

            if (string.IsNullOrEmpty(currentParameterName))
            {
              FunctionCallParameter userParameter = stepModel.FunctionCall.Signature.Parameters.Find(p =>
              {
                return p.Value.StartsWith("XML=", StringComparison.InvariantCultureIgnoreCase);
              });
              if (userParameter != null)
              {
                SetCurrentStepResponseParameter(userParameter.Value.Remove(0, 4));
              }
            }

            stepService.CurrentStep = currentStep;
          }
        });
      }
      catch (Exception ex)
      {
        Log.VuGen.Error(string.Format("Error occurred when adding the {2} step. (Type: '{0}', Exception: '{1}')", Owner.GetType(), ex, Name));
      }
    }

    protected abstract string Name
    {
      get;
    }

    protected abstract RelativeStep Relative
    {
      get;
    }

    protected abstract void FillParameters(FunctionCallSignature signature);


  }


  public class CreateLrXmlFindStepCommand : LrXmlStepCommand
  {

    protected override string Name
    {
      get
      {
        return "lr_xml_find";
      }
    }

    protected override void FillParameters(FunctionCallSignature signature)
    {

      signature.Parameters.Add(new FunctionCallParameter(string.Format("FastQuery={0}", _xpath), ParameterType.ArgtypeString));
      signature.Parameters.Add(new FunctionCallParameter(String.Format("Value={0}", _value), ParameterType.ArgtypeString));
    }

    protected override RelativeStep Relative
    {
      get { return RelativeStep.After; }
    }
  }

  public class CreateLrXmlExctarctStepCommand : LrXmlStepCommand
  {
    protected override string Name
    {
      get { return "lr_xml_extract"; }
    }

    protected override void FillParameters(FunctionCallSignature signature)
    {
      signature.Parameters.Add(new FunctionCallParameter(string.Format("Query={0}", _xpath), ParameterType.ArgtypeString));
      signature.Parameters.Add(new FunctionCallParameter(String.Format("XMLFragmentParam=ParamXml_{0}", _elementName), ParameterType.ArgtypeString));
    }

    protected override RelativeStep Relative
    {
      get { return RelativeStep.After; }
    }

  }

  public class CreateLrXmlGetValuesStepCommand : LrXmlStepCommand
  {
    protected override string Name
    {
      get { return "lr_xml_get_values"; }
    }

    protected override void FillParameters(FunctionCallSignature signature)
    {
      signature.Parameters.Add(new FunctionCallParameter(string.Format("FastQuery={0}", _xpath), ParameterType.ArgtypeString));
      signature.Parameters.Add(new FunctionCallParameter(String.Format("ValueParam=ParamValue_{0}", _elementName), ParameterType.ArgtypeString));
    }

    protected override RelativeStep Relative
    {
      get { return RelativeStep.After; }
    }

  }

}
