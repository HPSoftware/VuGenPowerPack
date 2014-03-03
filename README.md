VuGenPowerPack
==============

This repository will contain all the Addins I am developing for VuGen

###Compilation Note
The compilation of this library requires .Net 4 or above. The provided solution file works with Visual Studio 2012 and above.
To compile the solution you must add *%LR_DIR%\bin* to the reference paths of all the projects.
There is a compiled version of all the addins in the *VuGenPowerPack* directory (separated by LoadRunner version) in the root of the repository. If you 
don't want to compile yourself you can copy the content of this directory to *%LR_DIR%\addins\extra\VuGenPowerPack*.

##HttpXmlView

This addin adds some missing functionality to the XmlView inside the Http snapshot.
Currently, Xml operations are not possible from the Xml view inside the Http snapshot. This addin adds the following
commands to the context menu of the Xml view:

* _Insert Xml Check..._ - Adds ```lr_xml_find``` step to your script with respect to the selected element. 
You must select a single element in the tree and its value will be used for the generated query.

* _Get Values..._ - Adds ```lr_xml_get_values``` step to your script with respect to the selected element. 
You must select a single element in the tree and its value will be used for the generated query.

* _Extract Xml..._ - Adds ```lr_xml_extract``` step to your script with respect to the selected element. It will
extract the selected element as a full Xml.

Additionally, a new search option was added inside the _Highlight_ menu - 

* _Highlight by Text_ - Clicking this menu item opens a dialog where you input the string to search. Confirming the dialog
highlights all the Xml elements with the input string as their value (a similar functionality existed in VuGen 11.03)



##XmlViewAddin

This addin demonstrates how to create an addin for VuGen. You can find the full instructions [here](https://github.com/HPSoftware/VuGenPowerPack/tree/master/XmlViewAddin).
The addin adds the ability to show a piece of Xml in a convenient dialog. The user may reformat the Xml to a nicer representation by
selecting the appropriate button in the dialog. The dialog can be launched by right clicking the editor and selecting the _"Open in Xml viewer"_ menu item.
Below is an example of the possible use of this addin.
![XmlViewAddin in action](/img/XmlViewAddin.png "XmlViewAddin in action")


##GeneralCommandsAddin

This addin contains some general commands that make VuGen easier to use:

* Export To Zip - Adds a command which allows you to export to zip directly above the script directory like in VuGen 11.04. You can find the command at File -> Manage Zip Files -> Export to Zip file (script directory)

* Go to step in Replay log - Adds a command which jumps to the line of the current step in the replay log (similar to the command in the editor context menu). You can find the command at Search -> Go to step in replay log (Control + Shift + J)


##UserDefinedToolbarAddin

Creates a secondary toolbar where the user can put commands from the main menu. The toolbar can load commands from any 
path in the addin tree so the user may add any command to the toolbar by adding the path of the command to the toolbar addin.

![UserDefinedToolbarAddin in action](/img/customtoolbar.gif "UserDefinedToolbarAddin in action")


##The Experimental Addins

The _experimental_ addins are located in a separate directory within the PowerPack. They are separated **not** because of
instability but because they affect the normal operation of VuGen even if the user doesn't take specific action in activating the
addin (such as clicking a button). Please use the addins in this section only if you need their functionality explicitly.

### TransactionWriterAddin

Developed mainly for BPM users, this addin adds pre-defined transaction names into the usr file during "Save", "SaveAs", and "Export" operations.
If there is a file called _dynamic_transactions.txt_ in the extra files, it will read the file line by line and add each line as a transaction name
in the usr file. This allows you to register transaction names which are not actual transactions within the script.

#License
© Copyright 2013 Hewlett-Packard Development Company, L.P.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
