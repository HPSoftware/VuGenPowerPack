VuGenPowerPack
==============

This repository will contain all the Addins I am developing for VuGen

###Compilation Note
To compile the solution you must add *%LR_DIR%\bin* to the reference paths of all the projects.
There is a compiled version in the *VuGenPowerPack* directory in the root of the repository. If you 
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



