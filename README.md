This repository is a cloned copy of my android mobile applcaition for my final year project. This copy has removed the branches and changed the AWS credentials for security purposes, as such it dosn't work outright in it's current state. To get the code to function you will need to place your own AWS credentials in the locations secified below.

# FYProject
This is the multilingual allergy searching and translation application for my 2023 final year project for Sheffield Hallam University, using Apache 2.0 and MIT licenses

Unit tests:

Due to time constraints the unit test in the LibUnitTest Library are designed to only handle one user at a time, if multiple tests are run where a user is created (such as Dispaly method) or multiple tests that change values run at the same time, for example (TranslateAppDispalyValues method in translate test methods), then some test may fail, the easist fix for this is to run all filed tests, as each time you do this the number of fialing test decreases, unitl no more fail.

Android over IOS:

The ios file is currently unused and therfor can't be deployed. This file is kept in to allow for future development on the IOS platform when a devloper with the requried IOS hardware to test it, begin development. As such for testing and running the applcaiton pleas euse the android project file instead.

AWS credentails:

The base OCRLibrary and TransaltionLibrary's both use AWS credentails, if you wish to use these libraries please enter your own AWS crendientas for a user with the textract and the translate permission for each library respectivly. These values can be inputted in each files findconnectionmethods as hardcoded values.
