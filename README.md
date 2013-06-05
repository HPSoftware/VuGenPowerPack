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





